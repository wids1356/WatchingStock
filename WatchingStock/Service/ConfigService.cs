using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using WatchingStock;

namespace WatchingStock.Service
{
    public class ConfigService
    {
        private const string PREFIX_KEY = "{0}_{1}";

        private const string configFile = "config.ini";
        private const string SECTION_BUSINESS = "business";
        private const string KEY_STOCK = "stock";
        private const string SECTION_UI = "ui";
        private const string KEY_STOCK_SHOWN_TIME = "stock_shown_time";

    
        private static Dictionary<string, string> configs = new Dictionary<string, string>();

        private static string Read(string section, string key)
        {
            StringBuilder sbTemp = new StringBuilder(1000);
            string filePath = Directory.GetCurrentDirectory() + "\\" + configFile;
            WndAPI.GetPrivateProfileString(section, key, "", sbTemp, 1000, filePath);
            return sbTemp.ToString().Trim();
        }

        private static void Write(string section, string key, string value)
        {
            string filePath = Directory.GetCurrentDirectory() + "\\" + configFile;
            WndAPI.WritePrivateProfileString(section, key, value, filePath);
        }

        /// <summary>
        /// 股票配置
        /// </summary>
        public static string[] Stocks
        {
            get
            {
                string value = GetProperty(SECTION_BUSINESS, KEY_STOCK);
                if ("" == value) return new string[0];
                return value.Split(',');
            }
            set
            {
               string joinValue = string.Join(",", value);
                SetProperty(SECTION_BUSINESS, KEY_STOCK, joinValue);
            }
        }

        /// <summary>
        /// 股票显示时间
        /// </summary>
        public static int StockShownTime
        {
            get
            {
                string value = GetProperty(SECTION_UI, KEY_STOCK_SHOWN_TIME);
                if ("" == value || !int.TryParse(value, out _))
                {
                    SetProperty(SECTION_UI, KEY_STOCK_SHOWN_TIME, 10.ToString());
                    return 10;
                }
                return Convert.ToInt32(value);
            }
            set
            {
                SetProperty(SECTION_UI, KEY_STOCK_SHOWN_TIME, value.ToString());
            }
        }


        private static string GetProperty(string section, string key)
        {
            string configKey = string.Format(PREFIX_KEY, section, key);
            if (!configs.ContainsKey(configKey))
            {
                string value = Read(section, key);
                if ("" == value)
                {
                    Write(section, key, "");
                }
                return value;
            }
            else
            {
                return configs[configKey];
            }
        }

        private static void SetProperty(string section, string key, string value)
        {
            string configKey = string.Format(PREFIX_KEY, section, key);
            if (null == value) value = "";
            Write(section, key, value);
            if (!configs.ContainsKey(configKey))
            {
                configs.Add(configKey, value);
            }
            else
            {
                configs[configKey] = value;
            }
            Write(section, key, value);
        }

        public static void Reset()
        {
            configs.Clear();
            File.Delete(configFile);
        }
    }
}
