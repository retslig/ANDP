using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using Common.Lib.Common.Enums;

namespace Common.Lib.Utility
{
    /// <summary>
    /// This class is used to handle object to/from flat file operations.
    /// Requires a model that has property names the same as the heading
    /// in the flat file. Quick way to build the model from the database is to use
    /// DB2BusinessEntityGenerator found in CSD DevTools area.
    /// </summary>
    public static class FlatFileHelper
    {

        /// <summary>
        /// Gets a list of objects from a Flat File.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataStream">The data stream.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static List<T> FromFlatFile<T>(Stream dataStream, string delimiter = ",") where T: class,new()
        {
            using(StreamReader sr = new StreamReader(dataStream))
            {
                return FromFlatFile<T>(sr.ReadToEnd(),delimiter);
            }
        }

        /// <summary>
        /// Gets a  list of objects from the given flat file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static List<T> FromFlatFile<T>(string data, string delimiter = ",") where T : class,new()
        {
            List<T> dataObjects = new List<T>();

            Type curType = typeof (T);
            PropertyInfo[] classProperties = curType.GetProperties();
            
            //Read header to build source mapping.
            using(StringReader sr = new StringReader(data))
            {
                string header = sr.ReadLine();

                if(!string.IsNullOrEmpty(header))
                {
                    string[] columnNames = header.Split(new [] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

                    //Parse the data row by row.
                    string line = sr.ReadLine();
                    while(!string.IsNullOrEmpty(line))
                    {
                        T newInstance = new T();

                        string[] dataValues = line.Split(new[] { delimiter },StringSplitOptions.None);

                        for (int idx = 0; idx < columnNames.Length; idx++ )
                        {
                            string colName = columnNames[idx];

                            PropertyInfo propInfo = classProperties.SingleOrDefault(prop => prop.Name == colName);

                            if(propInfo != null)
                            {
                                propInfo.SetValue(newInstance,Convert.ChangeType(dataValues[idx],propInfo.PropertyType),null);
                            }
                        }

                        dataObjects.Add(newInstance);

                        line = sr.ReadLine();
                    }
                }
            }

            return dataObjects;
        }

        public static void ToFlatFile<T>(IEnumerable<T> objectlist, string fileName, string separator = ",", bool writeheader = false)
        {
            Type t = typeof(T);
            PropertyInfo[] fields = t.GetProperties();
            StringBuilder csvdata = new StringBuilder();

            try
            {
                if (writeheader)
                {
                    csvdata.AppendLine(String.Join(separator, fields.Select(f => f.Name).ToArray()));
                }

                using (var writer = new StreamWriter(fileName))
                {
                    foreach (var o in objectlist)
                    {
                        csvdata.AppendLine(ToDelimitedFields(fields, o, separator));
                    }

                    writer.Write(csvdata.ToString());
                    writer.Close();
                }
            }
            catch(Exception e)
            {
                LogWriter.WriteLogEntry(null, "ExtractCodJournalEntries ToCsv()", LogLevelType.Error, e);
            }
        }

        public static string ToDelimitedFields(PropertyInfo[] fields, object o, string separator = ",")
        {
            StringBuilder line = new StringBuilder();

            foreach (var f in fields)
            {
                if (line.Length > 0)
                {
                    line.Append(separator);
                }

                var x = f.GetValue(o,null);

                if (x != null)
                {
                    line.Append(x.ToString());
                }
            }
            return line.ToString();
        }
    }
 }

