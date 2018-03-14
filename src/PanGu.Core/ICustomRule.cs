using System;
using System.Collections.Generic;
using System.Text;
using PanGu.Framework;

namespace PanGu
{
    /// <summary>
    /// 用户自定义规则接口
    /// </summary>
    public interface ICustomRule
    {
        string Text {get; set;}
        void AfterSegment(SuperLinkedList<WordInfo> result);
    }

    internal class CustomRule
    {
        private static Dictionary<string, Type> _AsmFilePathDict = new Dictionary<string, Type>();

        private static object _LockObj = new object();

        internal static ICustomRule GetCustomRule(string assemblyFilePath, string classFullName)
        {
            lock (_LockObj)
            {
                if (string.IsNullOrEmpty(assemblyFilePath) || string.IsNullOrEmpty(classFullName))
                {
                    return null;
                }

                Type type;
                string key = assemblyFilePath.ToLower().Trim();
                if (!_AsmFilePathDict.TryGetValue(key, out type))
                {
                    type = Instance.GetType(assemblyFilePath, classFullName);
                    _AsmFilePathDict.Add(key, type);
                }

                if (type == null)
                {
                    return null;
                }

                return Instance.CreateInstance(type) as ICustomRule;
            }
        }
    }
}
