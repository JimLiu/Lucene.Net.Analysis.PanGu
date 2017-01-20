using System;
using System.Collections.Generic;
using System.Reflection;
#if NETCORE
using System.Runtime.Loader;
#endif
using System.Text;

namespace PanGu.Framework
{
    public class Instance
    {
        static public object CreateInstance(string typeName)
        {
            object obj = Assembly.GetEntryAssembly().CreateInstance(typeName); ;

            if (obj != null)
            {
                return obj;
            }

            //TODO: 目前还不知道以下代码怎么迁移
#if !NETCORE
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                obj = asm.CreateInstance(typeName);

                if (obj != null)
                {
                    return obj;
                }
            }
#endif

            return null;

        }

        static public object CreateInstance(Type type)
        {
            return type.GetTypeInfo().Assembly.CreateInstance(type.FullName);
        }

        static public object CreateInstance(Type type, string assemblyFile)
        {
#if NETCORE
            var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFile);
#else
            System.Reflection.Assembly asm;

            asm = System.Reflection.Assembly.LoadFrom(assemblyFile);
#endif

            return asm.CreateInstance(type.FullName);
        }

        static public Type GetType(string assemblyFile, string typeName)
        {
#if NETCORE
            var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFile);
#else
            System.Reflection.Assembly asm;

            asm = System.Reflection.Assembly.LoadFrom(assemblyFile);
#endif
            return asm.GetType(typeName);
        }

    }

}
