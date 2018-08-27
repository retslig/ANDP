using System;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Common.Lib.MVC.Helpers
{  

    /// <summary>
    /// Builds a function that returns a JavaScript object that represents your .net data model.
    /// It works by using reflection to find all the public properties in the class.  It will properties that 
    /// are a nested class, the routine will follow the inheritance chain down. 
    /// Limitations
    /// -------------
    /// 1 - All models must re
    /// Swiped from http://buildjavascriptmodel.codeplex.com/
    /// </summary>                
    public class ModelToJavascript
    {
        #region Constants and Fields

        /// <summary>
        /// This is the value that is assigned to each property that represents
        /// a value type in the Datamodel.
        /// </summary>
        //private const string AssignedValue = " ko.observable()";
        private const string JAVASCRIPT_PROPERTY_TYPE = " ko.observable()";
        private const string JAVASCRIPT_ARRAY_TYPE = "ko.observableArray()";
        private static readonly String[] IgnoreTypes = new[] { "System.String", "System.DateTime" };

        private static readonly int IgnoreTypesLen = IgnoreTypes.Length;

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds JavaScript data model from modelType.  The generated function will be assigned
        /// to variable that has the same name as the modelType
        /// </summary>
        /// <param name="modelType">.Net class to build JavaScript data model for</param>
        /// <returns>String contains the generated Json Datamodel</returns>
        public static string Build(Type modelType)
        {
            return Build(modelType, modelType.Name);
        }

        /// <summary>
        ///  Builds JavaScript data model from modelType. 
        /// </summary>
        /// <param name="modelType">.Net class to build JavaScript data model for</param>
        /// <param name="targetName">Name to be assigned to function that returns the data model.</param>
        /// <returns>String contains the generated Json Datamodel</returns>
        public static string Build(Type modelType, string targetName)
        {
            if (targetName == null)
            {
                targetName = modelType.Name;
            }
            PropertyInfo[] propertyInfos = modelType.GetProperties();
            return InternalFormat(targetName, propertyInfos, 1, modelType.FullName);
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Internal, Recursive function that does the actual conversion
        /// </summary>
        /// <param name="targetName">Name to be given to the function that generates the data model</param>
        /// <param name="propsParam">List of Properties to generate for</param>
        /// <param name="level">Current Recursion Level</param>
        /// <param name="typeName">Name of the type that data model is being generated for</param>
        /// <returns>String contains the generated Json Datamodel</returns>
        private static string InternalFormat(string targetName, PropertyInfo[] propsParam, int level, string typeName)
        {
            if (propsParam == null)
            {
                throw new ArgumentNullException("propsParam");
            }
            // to prevent infinite run-away, terminate recursion at 10 levels.
            if (level > 10)
            {
                throw new OverflowException("Excessive Recursion.  Please review data model type.");
            }

            const string eol = "\r\n";
            const string spaces = " ";
            const string dashes = "//---------------------------------------------------------------" + eol;
            var dashesLength = dashes.Length - 2;
            var padding = spaces.PadLeft((level + 1) * 4);
            var sb = new StringBuilder();
            if (level == 1)
            {
                sb.Append(eol + dashes);
                sb.Append("//      Begin Auto Generated code -------------------------------" + eol);
                var s = "//      For Model Type - " + typeName;
                if (s.Length <= dashesLength)
                {
                    s = s + "   " + dashes.Substring(2);
                    s = s.Substring(0, dashes.Length) + eol;
                }
                else
                {
                    s = s + eol;
                }
                sb.Append(s);
                sb.Append(dashes);

                sb.AppendFormat("var {0} = function() {{ {1} ", targetName, eol);
                sb.AppendLine("var self = this;");
                //sb.Append("    return {" + eol);
            }
            else
            {
                sb.AppendFormat("{0}{1} " + eol, spaces.PadLeft((level) * 4), string.Concat("self.",targetName,"= {"));
            }


            var len = propsParam.Length;
            for (var i = 0; i < len; i++)
            {
                PropertyInfo propertyInfo = propsParam[i];

                var types = propertyInfo.PropertyType.GetInterfaces();
                bool isList = types.Any(t => t.Name == "IList");

                string javascriptPropType = isList ? JAVASCRIPT_ARRAY_TYPE : JAVASCRIPT_PROPERTY_TYPE;

                var propType = propertyInfo.PropertyType.FullName;
                var ignoreType = false;
                for (var j = 0; j < IgnoreTypesLen && !ignoreType; j++)
                    ignoreType = propType.StartsWith(IgnoreTypes[j]);

                PropertyInfo[] p = propertyInfo.PropertyType.GetProperties();
                
                //Make sure we don't serialize the public properties of collection types in addition
                //to the other types we should ignore.
                if (p.Length > 0 && !ignoreType && !isList)
                {
                    string d = InternalFormat(propertyInfo.Name, p, level + 1, "");
                    sb.Append(d);
                }
                else
                {

                    if(level == 1)
                    {
                        sb.AppendFormat("{0}{1} = {2}", padding, "self." + propertyInfo.Name, javascriptPropType);
                        sb.Append(";");
                    }
                    else
                    {
                        sb.AppendFormat("{0}{1} : {2}", padding, propertyInfo.Name, javascriptPropType);
                        if(i + 1 < len)
                        {
                            sb.Append(",");
                        }
                    }
                                
                    sb.AppendLine();
                }
            }

            sb.Append(padding + "}");
            if (level != 1)
            {
                sb.Append(";");
            }
            else
            {                
                sb.AppendLine();                
                sb.Append(dashes);
                sb.Append("//      End Auto Generated code ---------------------------------" + eol);
                sb.Append(dashes);
            }
            sb.AppendLine();

            return sb.ToString();
        }

        #endregion
    }
}
