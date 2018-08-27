using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;
using Microsoft.CSharp;

namespace Common.Lib.Infastructure
{
    public static class DynamicCodeService
    {
        private static Assembly _assembly;

        public static void CompileSourceCodeDom(string sourceCode, string[] referencedAssemblies)
        {
            CodeDomProvider cpd = new CSharpCodeProvider();
            var cp = new CompilerParameters();
            cp.ReferencedAssemblies.AddRange(referencedAssemblies);
            cp.GenerateExecutable = false;
            var compilerResults = cpd.CompileAssemblyFromSource(cp, sourceCode);
            
            //Handle Errors
            if (compilerResults.Errors.HasErrors)
                throw new Exception("Compiler Errors");

            if (compilerResults.Errors.HasErrors)
            {
                // *** Create Error String
                string errorMsg = compilerResults.Errors.Count + " Errors:";
                for (int x = 0; x < compilerResults.Errors.Count; x++)
                    errorMsg = errorMsg + "\r\nLine: " + compilerResults.Errors[x].Line + " - " +
                        compilerResults.Errors[x].ErrorText;

                throw new Exception("Compiler Errors: " + errorMsg);
            }

            _assembly =  compilerResults.CompiledAssembly;
        }

        public static object ExecuteMethodFromAssembly(string className, string methodName, object[] methodParameters)
        {
            var type = _assembly.GetType(className);
            var method = type.GetMethod(methodName);
            object createdInstance = _assembly.CreateInstance(className);
            return method.Invoke(createdInstance, BindingFlags.InvokeMethod, null, methodParameters, CultureInfo.CurrentCulture);
        }

        public static object RetrievePropertyValueFromAssembly(string className, string propertyName)
        {
            var type = _assembly.GetType(className);
            var property = type.GetProperty(propertyName);
            object createdInstance = _assembly.CreateInstance(className);
            return property.GetValue(createdInstance, null);
        }
    }
}


//class Foo
//{
//    public void Print()
//    {
//        System.Console.WriteLine(""Hello benohead.com !"");
//    }
//}

        //public Assembly CompileSourceCodeDom(string sourceCode, IEnumerable<string> referencedAssemblies)
        //{
        //    CodeDomProvider cpd = new CSharpCodeProvider();
        //    var cp = new CompilerParameters();
        //    cp.ReferencedAssemblies.Add("System.dll");
        //    cp.GenerateExecutable = false;
        //    var compilerResults = cpd.CompileAssemblyFromSource(cp, sourceCode);
            
        //    //Handle Erros
        //    if (compilerResults.Errors.HasErrors)
        //        throw new Exception();

        //    return compilerResults.CompiledAssembly;
        //}

        ////The other steps can then be implemented like this:
        //public void ExecuteFromAssembly(Assembly assembly)
        //{
        //    Type fooType = assembly.GetType("Foo");
        //    MethodInfo printMethod = fooType.GetMethod("Print");
        //    object foo = assembly.CreateInstance("Foo");
        //    printMethod.Invoke(foo, BindingFlags.InvokeMethod, null, null, CultureInfo.CurrentCulture);
        //}