using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu.Setting
{
    class SettingLoader
    {
        private void Load(string fileName)
        {
            PanGuSettings.Load(fileName);
        }

        public SettingLoader(string fileName)
        {
            Load(fileName);
        }

        public SettingLoader()
        {
            string fileName = PanGu.Framework.Path.GetAssemblyPath() + "PanGu.xml";
            Load(fileName);
        }
    }
}
