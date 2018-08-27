using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Xml;

namespace Common.Lib.Utility
{
    public static class ActiveDirectoryHelper
    {
        //Better solution may be this project
        //http://linqtoad.codeplex.com/

        //Most Examples come from here:
        //http://www.codeproject.com/Articles/18102/Howto-Almost-Everything-In-Active-Directory-via-C#45
        //also
        //http://msdn.microsoft.com/en-gb/library/ms180915.aspx

        //http://msdn.microsoft.com/en-us/library/bb299745

        private const string DOMAIN = "raven.ravenind.net";
        private const string DOMAIN_CONTAINER = "DC=raven,DC=ravenind,DC=net";
        private const string PRIVILEGED_USER = "ad_srvc";
        private static string PrivilegedPass { 
            get
            {
                byte[] encodedDataAsBytes = Convert.FromBase64String(PRIVILEGEDD_PASS_BASE_64);
                return Encoding.Unicode.GetString(encodedDataAsBytes);
            }
        }

        private const string PRIVILEGEDD_PASS_BASE_64 = "UgBhAHYAZQBuADEA";

        public static string ExtractUserName(string path)
        {
            string[] userPath = path.Split(new char[] { '\\' });
            return userPath.Any() ? userPath[userPath.Length - 1] : path;
        }

        /// <summary>
        /// Gets the AD user groups.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns></returns>
        public static List<string> GetADGroupsForUser(string loginName)
        {
            DirectorySearcher search = new DirectorySearcher { Filter = String.Format("(SAMAccountName={0})", loginName) };
            search.PropertiesToLoad.Add("memberOf");
            SearchResult result = search.FindOne();

            List<string> UserList = new List<string>();
            if (result != null)
            {
                for (int counter = 0; counter < result.Properties["memberOf"].Count; counter++)
                {
                    UserList.Add(((string)result.Properties["memberOf"][counter]).Split(',')[0].Split('=')[1]);
                }
            }

            return UserList;
        }

        /// <summary>
        /// Doeses the user exist in AD.
        /// </summary>
        /// <param name="loginName">Expected format is Raven/nmg</param>
        /// <returns></returns>
        public static bool DoesUserExistInAD(string loginName)
        {
            string userName = ExtractUserName(loginName);
            DirectorySearcher search = new DirectorySearcher { Filter = String.Format("(SAMAccountName={0})", userName) };
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Does the user exist in the specified AD group.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        public static bool DoesUserExistInADGroup(string loginName, string groupName)
        {
            string userName = ExtractUserName(loginName);
            List<string> groups = GetADGroupsForUser(userName);
            return groups.Any(p => p == groupName);
        }

        /// <summary>
        /// Does the user exist in AD groups.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="groupNames">The group names.</param>
        /// <returns></returns>
        public static bool DoesUserExistInADGroups(string loginName, List<string> groupNames)
        {
            string userName = ExtractUserName(loginName);
            List<string> groups = GetADGroupsForUser(userName);

            return groupNames.Any(name => groups.Any(p => p == name));
        }

        /// <summary>
        /// Gets the AD group user names.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        public static List<string> GetADUsersFromGroupName(string groupName)
        {
            List<string> allUsers = new List<string>();

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            using (var group = GroupPrincipal.FindByIdentity(context, IdentityType.SamAccountName, groupName))
            {
                if (group != null)
                    allUsers.AddRange(group.GetMembers(false).Select(p => p.SamAccountName));
            }

            return allUsers;
        }

        /// <summary>
        /// Gets all AD domain users.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllADDomainUserNames()
        {
            List<string> allUsers = new List<string>();

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            {
                foreach (var groupName in GetAllADDomainGroups())
                {
                    using (var group = GroupPrincipal.FindByIdentity(context, IdentityType.SamAccountName, groupName))
                    {
                        if (group != null)
                        {
                            try
                            {
                                var members = group.GetMembers(true);
                                allUsers.AddRange(members.Select(p => groupName + "\\" + p.SamAccountName));
                            }
                            catch (PrincipalOperationException ex)
                            {
                                //Ignore this error and move on.
                                if (!ex.Message.Contains("1332"))
                                    throw;
                            }
                        }
                    }
                }
            }

            return allUsers;
        }

        //       PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

        //GroupPrincipal qbeUser = new GroupPrincipal(ctx);

        //Principal userOrGroup = qbeUser as Principal;
        //userOrGroup.Name = "*";

        //PrincipalSearcher searcher = new PrincipalSearcher(userOrGroup);

        //List<string> AllGroups = new List<string>();

        //// enumerate the results - you need to check what kind of principal you get back
        //foreach (Principal found in searcher.FindAll())
        //{
        //    // is it a UserPrincipal - do what you need to do with that...
        //    if (found is UserPrincipal)
        //    {
        //        //  ......
        //    }
        //    else if (found is GroupPrincipal)
        //    {
        //        AllGroups.Add(found.Name);

        //        //GroupPrincipal gp = found as GroupPrincipal;

        //        //var data = gp.GetMembers();

        //        // if it's a group - do whatever you need to do with a group....
        //    }
        //}

        ////return AllGroups;

        /// <summary>
        /// Gets all AD domain groups.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">No groups found</exception>
        public static List<string> GetAllADDomainGroups()
        {
            List<string> groups = new List<string>();

            using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + DOMAIN))
            using (DirectorySearcher search = new DirectorySearcher(entry) { Filter = "(&(objectClass=group))", SearchScope = SearchScope.Subtree })
            {
                SearchResultCollection results = search.FindAll();

                // Enumerate groups 
                if (results.Count != 0)
                {
                    groups.AddRange(from SearchResult objResult in results select objResult.GetDirectoryEntry() into objGroupEntry select objGroupEntry.Name.Replace("CN=", ""));
                }

                entry.RefreshCache();
                entry.Close();
                groups.Sort();
            }
            
            return groups;
        }

        /// <summary>
        /// Gets the AD user details by login.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns></returns>
        public static string GetADUserDetailsByLogin(string loginName)
        {
            string userName = ExtractUserName(loginName);
            DirectorySearcher search = new DirectorySearcher { Filter = String.Format("(SAMAccountName={0})", userName) };
            //search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            using (var ms = new MemoryStream())
            using (var writer = new XmlTextWriter(ms, new UTF8Encoding(false)) { Formatting = Formatting.Indented })
            {

                writer.WriteStartDocument();
                writer.WriteStartElement("root");

                ResultPropertyCollection propertiesCollection = result.Properties;
                foreach (string myKey in propertiesCollection.PropertyNames)
                {
                    foreach (Object myCollection in propertiesCollection[myKey])
                    {
                        writer.WriteElementString(myKey, myCollection.ToString());
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Determines whether [is user locked] [the specified login name].
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns>
        ///   <c>true</c> if [is user locked] [the specified login name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUserLocked(string loginName)
        {
            string userName = ExtractUserName(loginName);

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
            {
                return user.IsAccountLockedOut();
            }
        }

        /// <summary>
        /// Unlocks the user.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        public static void UnlockUser(string loginName)
        {
            string userName = ExtractUserName(loginName);

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
            {
                if (user.IsAccountLockedOut())
                    user.UnlockAccount();
            }
        }

        /// <summary>
        /// Locks the user.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        public static void LockUser(string loginName)
        {
            string userName = ExtractUserName(loginName);
            using (DirectorySearcher search = new DirectorySearcher { Filter = String.Format("(SAMAccountName={0})", userName) })
            {
                SearchResult result = search.FindOne();
                DirectoryEntry user = result.GetDirectoryEntry();
                string path = user.Path;
                string badPassword = "SomeBadPassword";
                int maxLoginAttempts = 5;

                for (int i = 0; i < maxLoginAttempts; i++)
                {
                    try
                    {
                        new DirectoryEntry(path, userName, badPassword).RefreshCache();
                    }
                    catch (Exception)
                    {

                    }
                }
                user.Close();
            }
        }

        /// <summary>
        /// Disables the specified login name.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        public static void DisableUser(string loginName)
        {
            string userName = ExtractUserName(loginName);

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
            {
                user.Enabled = false;
                user.Save();
            }
        }

        /// <summary>
        /// Enables the specified login name.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        public static void EnableUser(string loginName)
        {
            string userName = ExtractUserName(loginName);

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
            {
                user.Enabled = true;
                user.Save();
            }
        }

        /// <summary>
        /// Determines whether [is user disabled] [the specified login name].
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns>
        ///   <c>true</c> if [is user disabled] [the specified login name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUserDisabled(string loginName)
        {
            string userName = ExtractUserName(loginName);

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
            {
                return !(bool)user.Enabled;
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="deafultPassword">The deafult password.</param>
        /// <param name="forcePasswordChangeOnNextLogin">if set to <c>true</c> [force passwrod change on next login].</param>
        public static void ResetUserPassword(string loginName, string deafultPassword, bool forcePasswordChangeOnNextLogin)
        {
            string userName = ExtractUserName(loginName);

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
            {
                user.SetPassword(deafultPassword);

                if (forcePasswordChangeOnNextLogin)
                {
                    //this will force them to change password on next login.
                    user.ExpirePasswordNow();
                }
                user.Save();
            }
        }


        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        public static void ChangeUserPassword(string loginName, string oldPassword, string newPassword)
        {
            string userName = ExtractUserName(loginName);

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
            {
                user.ChangePassword(oldPassword, newPassword);
                user.Save();
            }
        }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool ValidateUserCredentials(string loginName, string password)
        {
            string userName = ExtractUserName(loginName);

            using (var context = new PrincipalContext(ContextType.Domain, DOMAIN, DOMAIN_CONTAINER, PRIVILEGED_USER, PrivilegedPass))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
            {
                return context.ValidateCredentials(userName, password, ContextOptions.Negotiate);
            }
        }

        //NOTE: Not Tested!
        /// <summary>
        /// Resets the password.
        /// CONST   HEX
        ///-------------------------------
        ///        SCRIPT 0x0001
        ///        ACCOUNTDISABLE 0x0002
        ///        HOMEDIR_REQUIRED 0x0008
        ///        LOCKOUT 0x0010
        ///        PASSWD_NOTREQD 0x0020
        ///        PASSWD_CANT_CHANGE 0x0040
        ///        ENCRYPTED_TEXT_PWD_ALLOWED 0x0080
        ///        TEMP_DUPLICATE_ACCOUNT 0x0100
        ///        NORMAL_ACCOUNT 0x0200
        ///        INTERDOMAIN_TRUST_ACCOUNT 0x0800
        ///        WORKSTATION_TRUST_ACCOUNT 0x1000
        ///        SERVER_TRUST_ACCOUNT 0x2000
        ///        DONT_EXPIRE_PASSWORD 0x10000
        ///        MNS_LOGON_ACCOUNT 0x20000
        ///        SMARTCARD_REQUIRED 0x40000
        ///        TRUSTED_FOR_DELEGATION 0x80000
        ///        NOT_DELEGATED 0x100000
        ///        USE_DES_KEY_ONLY 0x200000
        ///        DONT_REQ_PREAUTH 0x400000
        ///        PASSWORD_EXPIRED 0x800000
        ///        TRUSTED_TO_AUTH_FOR_DELEGATION 0x1000000
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        public static void SetUserPermissions(string loginName, string password)
        {
            string userName = ExtractUserName(loginName);
            using (DirectorySearcher search = new DirectorySearcher { Filter = String.Format("(SAMAccountName={0})", userName) })
            {
                SearchResult result = search.FindOne();
                DirectoryEntry dEntry = result.GetDirectoryEntry();

                int val = (int)dEntry.Properties["userAccountControl"].Value;
                //newUser is DirectoryEntry object

                dEntry.Properties["userAccountControl"].Value = val | 0x10000; //0x10000 is dont expire password.

                dEntry.CommitChanges(); //may not be needed but adding it anyways

                dEntry.Close();
            }
        }

        //NOTE: Not Tested!
        public static void AddUser(string firstName, string lastName, string domain, string login, string password)
        {
            string fullName = firstName + " " + lastName;
            //CN=Nathan Gilster,OU=IS,OU=Users,OU=Downtown,DC=raven,DC=ravenind,DC=net
            DirectoryEntry container = new DirectoryEntry(string.Format("LDAP://CN={0},OU=IS,OU=Users,OU=Downtown,DC=raven,DC=ravenind,DC=net", fullName));
            // create a user directory entry in the container
            DirectoryEntry newUser = container.Children.Add("cn=" + login, "user");

            // add the samAccountName mandatory attribute
            newUser.Properties["sAMAccountName"].Value = login;

            // add any optional attributes
            newUser.Properties["givenName"].Value = firstName;
            newUser.Properties["sn"].Value = lastName;
            newUser.Properties["displayName"].Value = fullName;

            // save to the directory
            newUser.CommitChanges();

            // set a password for the user account
            // using Invoke method and IadsUser.SetPassword
            newUser.Invoke("SetPassword", new object[] { password });

            // require that the password must be changed on next logon
            newUser.Properties["pwdLastSet"].Value = 0;

            // enable the user account
            // newUser.InvokeSet("AccountDisabled", new object[]{false});
            // or use ADS_UF_NORMAL_ACCOUNT (512) to effectively unset the
            // disabled bit
            newUser.Properties["userAccountControl"].Value = 512;

            // save to the directory
            newUser.CommitChanges();
        }

        /// <summary>
        /// Gets the windows user name.
        /// </summary>
        /// <param name="employeeName">The name of the person to search for.</param>
        /// <returns></returns>
        public static string GetUserNameFromUserString(string employeeName)
        {
            DirectorySearcher search = new DirectorySearcher { Filter = String.Format("(cn={0})", employeeName) };
            search.PropertiesToLoad.Add("SAMAccountName");
            SearchResult result = search.FindOne();

            if (result != null)
                return (string)result.Properties["SAMAccountName"][0];

            return "";
        }
        

        /// <summary>
        /// Method that allows filtering of the data from AD that comes back in a dynamic
        /// that will allow retrieval of the fields.
        /// </summary>
        /// <param name="loginName">The current login name</param>
        /// <param name="fields">A list of field names to pull back from Active Directory.</param>
        /// <returns>A dynamic that underneath is an Expando object.</returns>
        public static dynamic GetFilteredUserDetails(string loginName, List<string> fields )
        {
            dynamic filteredADUserInfo = new ExpandoObject();

            //Expando doesn't directly expose IDictionary so use trickery so we can dynamically
            //add. See here for more details: http://msdn.microsoft.com/en-us/magazine/ff796227.aspx
            var dictionaryProp = filteredADUserInfo as IDictionary<string, object>;

            string userName = ExtractUserName(loginName);
            DirectorySearcher search = new DirectorySearcher { Filter = String.Format("(SAMAccountName={0})", userName) };
            
            if(fields != null)
            {
                search.PropertiesToLoad.AddRange(fields.ToArray());

                //Fill our properties in case some of the fields aren't filled in for
                //a particular user.
                foreach (var propName in fields)
                {
                    dictionaryProp[propName.Replace("-", "_")] = null;
                }
            }
            
            SearchResult result = search.FindOne();

            if(result != null)
            {
                ResultPropertyCollection propertiesCollection = result.Properties;

                foreach (string myKey in propertiesCollection.PropertyNames)
                {
                    foreach (Object myCollection in propertiesCollection[myKey])
                    {
                        //Remove any dash chars, invalid C# property names.
                        dictionaryProp[myKey.Replace("-","_")] = myCollection;
                    }
                }
            }

            return filteredADUserInfo;
        }

    }
}
