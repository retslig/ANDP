using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Common.Lib.Utility
{
    public static class OdbcHelper
    {
        public enum Architecture
        {
            Unknown = 0,
            Bit32 = 32,
            Bit64 = 64
        }

        //HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\ODBC\ODBC.INI\as400
        //HKEY_LOCAL_MACHINE\SOFTWARE\ODBC\ODBC.INI\as400

        private const string ODBC_INI_REG_PATH_32_BIT = "SOFTWARE\\ODBC\\ODBC.INI\\";
        private const string ODBCINST_INI_REG_PATH_32_BIT = "SOFTWARE\\ODBC\\ODBCINST.INI\\";
        private const string ODBC_INI_REG_PATH_64_BIT = "SOFTWARE\\Wow6432Node\\ODBC\\ODBC.INI\\";
        private const string ODBCINST_INI_REG_PATH_64_BIT = "SOFTWARE\\Wow6432Node\\ODBC\\ODBCINST.INI\\";
        private static string _odbcPath;
        private static string _odbcIniPath;

        /// <summary>
        /// Autoes the detect operating system architecture.
        /// </summary>
        /// <returns></returns>
        public static Architecture AutoDetectOperatingSystemArchitecture()
        {
            switch (IntPtr.Size)
            {
                case 4:
                    return Architecture.Bit32;
                case 8:
                    return Architecture.Bit64;
                default:
                    return Architecture.Unknown;
            }
        }

        /// <summary>
        /// Auto detects process architecture.
        /// </summary>
        /// <returns></returns>
        public static Architecture AutoDetectProcessArchitecture()
        {
            return Environment.Is64BitProcess ? Architecture.Bit64 : Architecture.Bit32;
        }

        /// <summary>
        /// Creates a new DSN entry with the specified values. If the DSN exists, the values are updated.
        /// </summary>
        /// <param name="dsnName">Name of the DSN for use by client applications</param>
        /// <param name="description">Description of the DSN that appears in the ODBC control panel applet</param>
        /// <param name="server">Network name or IP address of database server</param>
        /// <param name="driverName">Name of the driver to use</param>
        /// <param name="trustedConnection">True to use NT authentication, false to require applications to supply username/password in the connection string</param>
        /// <param name="database">Name of the datbase to connect to</param>
        /// <param name="is64Bit"> </param>
        public static void CreateDSN(string dsnName, string description, string server, string driverName,
                                     bool trustedConnection, string database, bool is64Bit = false)
        {
            _odbcIniPath = (is64Bit ? ODBCINST_INI_REG_PATH_64_BIT : ODBCINST_INI_REG_PATH_32_BIT) + driverName;
            _odbcPath = (is64Bit ? ODBC_INI_REG_PATH_64_BIT : ODBC_INI_REG_PATH_32_BIT);
            
            // Lookup driver path from driver name
            var driverKey = Registry.LocalMachine.CreateSubKey(_odbcIniPath);
            if (driverKey == null)
                throw new Exception(string.Format("ODBC Registry key for driver '{0}' does not exist", driverName));
            string driverPath = driverKey.GetValue("Driver").ToString();

            // Add value to odbc data sources
            var datasourcesKey = Registry.LocalMachine.CreateSubKey(_odbcPath + "ODBC Data Sources");
            if (datasourcesKey == null) throw new Exception("ODBC Registry key for datasources does not exist");
            datasourcesKey.SetValue(dsnName, driverName);

            // Create new key in odbc.ini with dsn name and add values
            var dsnKey = Registry.LocalMachine.CreateSubKey(_odbcPath + dsnName);
            if (dsnKey == null) throw new Exception("ODBC Registry key for DSN was not created");
            dsnKey.SetValue("Database", database);
            dsnKey.SetValue("Description", description);
            dsnKey.SetValue("Driver", driverPath);
            dsnKey.SetValue("LastUser", Environment.UserName);
            dsnKey.SetValue("Server", server);
            dsnKey.SetValue("Database", database);
            dsnKey.SetValue("Trusted_Connection", trustedConnection ? "Yes" : "No");
        }

        /// <summary>
        /// Removes a DSN entry
        /// </summary>
        /// <param name="dsnName">Name of the DSN to remove.</param>
        /// <param name="is64Bit"> </param>
        public static void RemoveDSN(string dsnName, bool is64Bit = false)
        {
            _odbcPath = (is64Bit ? ODBC_INI_REG_PATH_64_BIT : ODBC_INI_REG_PATH_32_BIT) + dsnName;
            // Remove DSN key
            Registry.LocalMachine.DeleteSubKeyTree(_odbcPath);

            // Remove DSN name from values list in ODBC Data Sources key
            var datasourcesKey = Registry.LocalMachine.CreateSubKey(ODBC_INI_REG_PATH_32_BIT + "ODBC Data Sources");
            if (datasourcesKey == null) throw new Exception("ODBC Registry key for datasources does not exist");
            datasourcesKey.DeleteValue(dsnName);
        }


        /// <summary>
        /// Retrieves the DSN values.
        /// </summary>
        /// <param name="dsnName">Name of the DSN.</param>
        /// <param name="is64Bit"> </param>
        /// <returns></returns>
        public static Dictionary<string, object> RetrieveDSNValues(string dsnName, bool is64Bit = false)
        {
            RegistryKey root = Registry.LocalMachine.CreateSubKey(ODBC_INI_REG_PATH_32_BIT + dsnName);
            return root.GetValueNames().ToDictionary(valueName => valueName, root.GetValue);
        }

        /// <summary>
        /// Retrieves the DSN specific value.
        /// </summary>
        /// <param name="dsnName">Name of the DSN.</param>
        /// <param name="name">The name.</param>
        /// <param name="is64Bit"> </param>
        /// <returns></returns>
        public static object RetrieveDSNSpecificValue(string dsnName, string name, bool is64Bit = false)
        {
            _odbcPath = (is64Bit ? ODBC_INI_REG_PATH_64_BIT : ODBC_INI_REG_PATH_32_BIT) + dsnName;
            RegistryKey root = Registry.LocalMachine.CreateSubKey(_odbcPath);
            return root.GetValue(name);
        }

        ///<summary>
        /// Checks the registry to see if a DSN exists with the specified name
        ///</summary>
        ///<param name="dsnName"></param>
        ///<param name="is64Bit"> </param>
        ///<returns></returns>
        public static bool DSNExists(string dsnName, bool is64Bit = false)
        {
            _odbcPath = (is64Bit ? ODBC_INI_REG_PATH_64_BIT : ODBC_INI_REG_PATH_32_BIT) + dsnName;
            RegistryKey root = Registry.LocalMachine.CreateSubKey(_odbcPath);
            return root != null;
        }

        /// <summary>
        /// Updates the DSN value.
        /// </summary>
        /// <param name="dsnName">Name of the DSN.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="is64Bit">if set to <c>true</c> [is64 bit].</param>
        public static void UpdateDSNValue(string dsnName, string name, string value, bool is64Bit = false)
        {
            _odbcPath = (is64Bit ? ODBC_INI_REG_PATH_64_BIT : ODBC_INI_REG_PATH_32_BIT) + dsnName;
            RegistryKey root = Registry.LocalMachine.CreateSubKey(_odbcPath);
            root.SetValue(name, value);
        }

        ///<summary>
        /// Returns an array of driver names installed on the system
        ///</summary>
        ///<returns></returns>
        public static string[] RetrieveInstalledDrivers(bool is64Bit = false)
        {
            _odbcIniPath = (is64Bit ? ODBCINST_INI_REG_PATH_64_BIT : ODBCINST_INI_REG_PATH_32_BIT);
            var driversKey = Registry.LocalMachine.CreateSubKey(_odbcIniPath + "ODBC Drivers");
            if (driversKey == null) throw new Exception("ODBC Registry key for drivers does not exist");

            var driverNames = driversKey.GetValueNames();

            return driverNames.Where(driverName => driverName != "(Default)").ToArray();
        }
    }
}
