using System;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common.MetaSphere.Services
{
    public abstract class MetaSphereShUtilities
    {
        //public readonly static String ORIGIN_HOST = "?clientVersion=9.0";
        //public readonly static String IGNORE_SEQUENCE_NUMBER = "&ignoreSequenceNumber=true";

        /**
         * Check the result information returned by an Sh operation, and throw an
         * exception including result details if it does not indicate success.
         *
         * @param resultCode
         *                    The result code returned by the operation.
         * @param extendedResult
         *                    The extended result information returned by the
         *                    operation.
         * @param userData    The user data that was included in the operation.
         */
        public void CheckResultCode(int resultCode, tExtendedResult extendedResult, XmlElement userData)
        {
            //2001 is success and 5001 means doesn't exist
            //https://drive.google.com/open?id=0B7rht-P1r85vc29oRnFRQmpseFE
            if (resultCode != 2001 && resultCode != 5001)
            {
                //-----------------------------------------------------------------------
                // Request was unsuccessful, so return error information.
                //-----------------------------------------------------------------------
                StringBuilder error = new StringBuilder();

                error.Append("The Sh operation was unsuccessful.\n");
                error.Append("Result code: ");
                error.Append(resultCode);
                error.Append("\nExtended result code: ");
                error.Append(extendedResult.ExtendedResultCode);
                error.Append("\n\"" + extendedResult.ExtendedResultDetail + "\"\n");

                //-----------------------------------------------------------------------
                // Include each of the sub-results.
                //-----------------------------------------------------------------------
                foreach (tSubResult subResult in extendedResult.ExtendedSubResults)
                {
                    error.Append("\nSub-result code: ");
                    error.Append(subResult.SubResultCode);
                    error.Append("\n\"" + subResult.SubResultDetail + "\"\n");
                    error.Append("Source: " + getSourceItem(subResult, userData) + "\n");
                }

                throw new RequestFailedException(error.ToString());
            }
        }

        /**
         * Get the name of the item identified by the SubResultSource in a particular
         * SubResult.
         *
         * @returns           The name of the source item that caused a problem.
         *
         * @param subResult   The result containing the source string.
         * @param userData    The user data that was sent in.  (not used in this
         *                    implementation)
         */
        public virtual String getSourceItem(tSubResult subResult, XmlElement userData)
        {
            //-------------------------------------------------------------------------
            // This default method simply manipulates the SubResultSource as a string.
            // Typed applications would do this, but untyped applications have the
            // option of applying the SubResultSource directly as an XPath string: see
            // ShUntypedUtilities, where this is overloaded, for an example.
            //-------------------------------------------------------------------------
            String finalItem = "<none>";
            String source = subResult.SubResultSource;

            if ((source != null) && (source.Length > 0))
            {
                //-----------------------------------------------------------------------
                // Each part of the source has a namespace prefix, e.g. "u:".  Get the
                // last part of the source, without the namespace prefix.
                //-----------------------------------------------------------------------
                finalItem = source.Substring(source.LastIndexOf(':') + 1);
            }

            return finalItem;
        }

        /// <summary>
        /// Provide a string representation of a particular enumeration value.
        /// </summary>
        /// <param name="e">The enumeration value to convert.</param>
        /// <returns>The XmlEnumAttribute for the enum if defined, or the name of the attribute if not.</returns>
        public String ConvertToString(Enum e)
        {
            // Get the information about this particular value.
            var t = e.GetType();
            var info = t.GetField(e.ToString("G"));

            // Use the name of the enum by default.
            var displayString = e.ToString("G");

            if (info.IsDefined(typeof(XmlEnumAttribute), false))
            {
                // An XmlEnumAttribute is defined - use that instead.
                object[] o = info.GetCustomAttributes(typeof(XmlEnumAttribute), false);
                var att = (XmlEnumAttribute)o[0];
                displayString = att.Name;
            }

            return displayString;
        }

        /// <summary>
        /// Creates a UserIdentity string from a set of identifies by URL-encoding the individual identifiers and then concatenating them with / separators.
        /// </summary>
        /// <param name="identifiers">The identifiers of the requested object.</param>
        /// <returns>A string suitable for use as a UserIdentity.</returns>
        public string GetUserIdentity(String[] identifiers)
        {
            var userIdentityBuilder = new StringBuilder();

            // URL-encode each identifier, and add it to the UserIdentity.
            foreach (String identifier in identifiers)
            {
                var encodedIdent = System.Web.HttpUtility.UrlEncode(identifier, Encoding.UTF8);
                userIdentityBuilder.Append(encodedIdent);
                userIdentityBuilder.Append("/");
            }

            // Remove the final separator.
            userIdentityBuilder.Remove(userIdentityBuilder.Length - 1, 1);

            return userIdentityBuilder.ToString();
        }
    }

    //-----------------------------------------------------------------------------
    // Parameter
    //
    // A class for dealing with command line parameters that may include spaces.
    //-----------------------------------------------------------------------------
    public class Parameter
    {
        private String value;
        private bool touched = false;

        public Parameter(String defaultValue)
        {
            value = defaultValue;
        }

        public String toString()
        {
            return value;
        }

        public void append(String word)
        {
            if (touched)
            {
                value += " " + word;
            }
            else
            {
                value = word;
                touched = true;
            }
        }
    }

    //-----------------------------------------------------------------------------
    // MetaSwitchShInterfaceException
    //
    // An umbrella exception for problems hit by SOAP example applications.
    //-----------------------------------------------------------------------------
    public class MetaSwitchShInterfaceException : Exception
    {
        public MetaSwitchShInterfaceException(string s)
            : base(s)
        {
        }
    }

    //-----------------------------------------------------------------------------
    // RequestFailedException
    //
    // Indicates that a SOAP request was not successful.
    //-----------------------------------------------------------------------------
    public class RequestFailedException : MetaSwitchShInterfaceException
    {
        public RequestFailedException(string s)
            : base(s)
        {
        }
    }

    //-----------------------------------------------------------------------------
    // WrongParametersException
    //
    // Indicates that invalid parameters were passed on the command line to one of
    // the example applications.
    //-----------------------------------------------------------------------------
    public class WrongParametersException : MetaSwitchShInterfaceException
    {
        public WrongParametersException(string s)
            : base(s)
        {
        }
    }
}

