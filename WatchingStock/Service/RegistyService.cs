using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WatchingStock.Service
{
    public class RegistyService
    {
        public static object GetCurrentUser(string path, string key)
        {
            // win 11时注册表获取任务栏是否居中
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(path);
            return regkey.GetValue(key);
        }
    }
}
