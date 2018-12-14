using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.Common
{
    public abstract class BaseORM
    {
        public abstract bool Connect(string connectionString);

        //public string ApplyTemplate(TableModel table, ICommand template)
        //{
        //    return template.ApplyTemplate(table);
        //}

        public static bool InterfaceFilter(Type typeObj, Object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }

        public List<IProcedureTransformation> AvailableProcTemplates()
        {
            List<IProcedureTransformation> lstRet = new List<IProcedureTransformation>();

            const string qualifiedInterfaceName = "SWBrasil.ORM.Common.IProcedureTransformation";
            var interfaceFilter = new TypeFilter(InterfaceFilter);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);

            var di = new DirectoryInfo(ORM.Default.TemplatesPath);
            foreach (FileInfo file in di.GetFiles("*.dll"))
            {
                try
                {
                    var nextAssembly = Assembly.UnsafeLoadFrom(file.FullName);

                    foreach (var type in nextAssembly.GetTypes())
                    {
                        var myInterfaces = type.FindInterfaces(interfaceFilter, qualifiedInterfaceName);
                        if (myInterfaces.Length > 0)
                        {
                            for( int i=0; i < myInterfaces.Length; i++ )
                                lstRet.Add((IProcedureTransformation)Activator.CreateInstance(type));

                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    // Not a .net assembly  - ignore
                }
            }

            return lstRet;
        }

        public List<ICommand> AvailableTableTemplates(string path)
        {
            List<ICommand> lstRet = new List<ICommand>();

            const string qualifiedInterfaceName = "SWBrasil.ORM.Common.ICommand";
            var interfaceFilter = new TypeFilter(InterfaceFilter);
            //AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);

            var di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles("*.CommandTemplate.dll"))
            {
                try
                {
                    var nextAssembly = Assembly.UnsafeLoadFrom(file.FullName);

                    foreach (var type in nextAssembly.GetTypes())
                    {
                        var myInterfaces = type.FindInterfaces(interfaceFilter, qualifiedInterfaceName);
                        if (myInterfaces.Length > 0)
                        {
                            for (int i = 0; i < myInterfaces.Length; i++)
                                lstRet.Add((ICommand)Activator.CreateInstance(type));

                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    // Not a .net assembly  - ignore
                }
            }

            return lstRet;
        }


        public List<IProject> AvailableProjectTemplates(string path)
        {
            List<IProject> lstRet = new List<IProject>();

            const string qualifiedInterfaceName = "SWBrasil.ORM.Common.IProject";
            var interfaceFilter = new TypeFilter(InterfaceFilter);
            //AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);

            var di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles("*.CommandTemplate.dll"))
            {
                try
                {
                    var nextAssembly = Assembly.UnsafeLoadFrom(file.FullName);

                    foreach (var type in nextAssembly.GetTypes())
                    {
                        var myInterfaces = type.FindInterfaces(interfaceFilter, qualifiedInterfaceName);
                        if (myInterfaces.Length > 0)
                        {
                            for (int i = 0; i < myInterfaces.Length; i++)
                                lstRet.Add((IProject)Activator.CreateInstance(type));

                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    // Not a .net assembly  - ignore
                }
            }

            return lstRet;
        }

        private Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return System.Reflection.Assembly.ReflectionOnlyLoad(args.Name);
        }



        public abstract string ConvertDataType(string dataBaseType, bool nullable);

        public List<TableModel> Tables { get; set; }

        public List<ProcModel> Procedures { get; set; }
    }
}
