using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu.Framework
{
    public class Instance
    {
        static public object CreateInstance(string typeName)
        {
            object obj = null;
            obj = System.Reflection.Assembly.GetCallingAssembly().CreateInstance(typeName);

            if (obj != null)
            {
                return obj;
            }

            foreach (System.Reflection.Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                obj = asm.CreateInstance(typeName);

                if (obj != null)
                {
                    return obj;
                }
            }

            return null;

        }

        static public object CreateInstance(Type type)
        {
            return type.Assembly.CreateInstance(type.FullName);

        }

        static public object CreateInstance(Type type, string assemblyFile)
        {
            System.Reflection.Assembly asm;

            asm = System.Reflection.Assembly.LoadFrom(assemblyFile);

            return asm.CreateInstance(type.FullName);
        }

        static public Type GetType(string assemblyFile, string typeName)
        {
            System.Reflection.Assembly asm;

            asm = System.Reflection.Assembly.LoadFrom(assemblyFile);

            return asm.GetType(typeName);
        }

    }

}
