using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;

namespace Common.Lib.Utility
{
    public static class PrinterHelper
    {
        //<?xml version="1.0"?>
        //<PrinterSettings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
        //  <Name>\\SFDPRINT2.raven.ravenind.net\P14401</Name>
        //  <ServerName>\\SFDPRINT2.raven.ravenind.net</ServerName>
        //  <ShareName>P14401</ShareName>
        //  <Comment>IT HP LJ4345mfp</Comment>
        //  <Default>true</Default>
        //</PrinterSettings>
        public class PrinterSettings
        {
            public string Name { get; set; }
            public string ServerName { get; set; }
            public string DeviceId { get; set; }
            public string ShareName { get; set; }
            public string Comment { get; set; }
            public bool Default { get; set; }
        }

        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi,
            ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter,
                                              IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi,
            ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level,
                                                  [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        /// <summary>
        /// Sends the file to printer.
        /// </summary>
        /// <param name="filePathAndName">Name of the file path and Name of File.</param>
        /// <param name="printerName">Name of the printer with Path. E.I. \\SFDPRINT2.raven.ravenind.net\P14401</param>
        public static void SendFileToPrinter(string filePathAndName, string printerName)
        {
            FileInfo file = new FileInfo(filePathAndName);
            file.CopyTo(printerName);
        }

        /// <summary>
        /// Gets all printers that have drivers installed on the calling machine.
        /// </summary>
        /// <returns></returns>
        public static List<PrinterSettings> GetAllPrinters()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Printer");
            ManagementObjectSearcher mos = new ManagementObjectSearcher(query);
            List<PrinterSettings> printers = new List<PrinterSettings>();

            foreach (ManagementObject mo in mos.Get())
            {
                PrinterSettings printer = new PrinterSettings();
                foreach (PropertyData property in mo.Properties)
                {
                    if (property.Name == "Name")
                        printer.Name = property.Value == null ? "" : property.Value.ToString();

                    if (property.Name == "ServerName")
                        printer.ServerName = property.Value == null ? "" : property.Value.ToString();

                    if (property.Name == "DeviceId")
                        printer.DeviceId = property.Value == null ? "" : property.Value.ToString();

                    if (property.Name == "ShareName")
                        printer.ShareName = property.Value == null ? "" : property.Value.ToString();

                    if (property.Name == "Comment")
                        printer.Comment = property.Value == null ? "" : property.Value.ToString();

                    if (property.Name == "Default")
                        printer.Default = (bool)property.Value;
                }
                printers.Add(printer);
            }

            return printers;
        }      

        /// <summary>
        /// When the function is given a printer name and an unmanaged array
        /// of bytes, the function sends those bytes to the print queue.
        /// </summary>
        /// <param name="szPrinterName">Name of the sz printer.</param>
        /// <param name="pBytes">The p bytes.</param>
        /// <param name="dwCount">The dw count.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        /// <summary>
        /// Sends the raw file to printer.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        /// <param name="filePathAndName">Name of the file path and.</param>
        /// <returns></returns>
        public static bool SendRawFileToPrinter(string printerName, string filePathAndName)
        {
            // Open the file.
            FileStream fs = new FileStream(filePathAndName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // Send the unmanaged bytes to the printer.            bSuccess = SendBytesToPrinter(printerName, pUnmanagedBytes, nLength);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }

        /// <summary>
        /// Sends the raw string to printer.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        /// <param name="stringContent">Content of the string.</param>
        /// <returns></returns>
        public static bool SendRawStringToPrinter(string printerName, string stringContent)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = stringContent.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(stringContent);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(printerName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }
}
