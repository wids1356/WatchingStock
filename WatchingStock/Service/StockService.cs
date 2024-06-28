using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using WatchingStock.Entity;
using System.Net.Http;

namespace WatchingStock.Service
{
    public class StockService
    {
        private static Timer timer = new Timer();

        // 实时 https://qt.gtimg.cn/q=sh600036
        // 日k https://proxy.finance.qq.com/ifzqgtimg/appstock/app/newkline/newkline?_var=kline_day&param=sh600036,day,,,320,&r=0.24738653723234094

        private static List<string> stockCodes = new List<string>();
        private static Dictionary<string, StockEntity> stocks = new Dictionary<string, StockEntity>();


        public static void Start()
        {
            stockCodes.Clear();
            string[] storedStocks = ConfigService.Stocks;
            if (storedStocks.Length > 0)
            {
                foreach (string stockConfig in storedStocks)
                {
                    string[] data = stockConfig.Split('|');
                    AddStock(data[0], data[1]);
                }
            }

            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Interval = 30000;
            timer.AutoReset = true;
            timer.Start();
            RefreshStock();
        }

        public static void AddStock(string code, string name)
        {
            StockEntity stock = CreateStock(code, name);
            stocks.Add(code, stock);
            stockCodes.Add(code);
            SaveStock();
        }

        public static void RemoveStock(string code)
        {
            stocks.Remove(code);
            stockCodes.Remove(code);
            SaveStock();
        }

        /// <summary>
        /// 存储股票配置
        /// </summary>
        private static void SaveStock()
        {
            string[] array = new string[stockCodes.Count];
            for (int i = 0; i < stockCodes.Count; i++)
            {
                string code = stockCodes[i];
                array[i] = code + "|" + stocks[code].StockName;
            }
            ConfigService.Stocks = array;
        }

         /// <summary>
         /// 股票是否存在
         /// </summary>
         /// <param name="code">股票代码</param>
         /// <returns></returns>
        public static bool Exists(string code)
        {
            return stocks.ContainsKey(code);
        }
        /// <summary>
        /// 股票数量
        /// </summary>
        public static int Count
        {
            get { return stockCodes.Count; }
        }

        /// <summary>
        /// 通过股票代码获取
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <returns>股票信息</returns>
        public static StockEntity Get(string code)
        {
            if (stocks.ContainsKey(code))
            {
                return stocks[code];
            }
            return CreateStock("--------", "加载中");
        }


        /// <summary>
        /// 通过股票代码获取
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <returns>股票信息</returns>
        public static StockEntity Get(int index)
        {
            if (index < stockCodes.Count)
            {
                return stocks[stockCodes[index]];
            }
            return CreateStock("--------", "加载中");
        }

        public static StockEntity[] Stocks
        {
            get {
                StockEntity[] array = new StockEntity[stockCodes.Count];
                for(int i = 0; i < stockCodes.Count; i++)
                {
                    array[i] = stocks[stockCodes[i]];
                }
                return array;
            }
        }

        private static StockEntity CreateStock(string code, string name)
        {
            StockEntity stock = new StockEntity();
            stock.StockCode = code;
            stock.StockName = name;
            stock.TradePrice = "0.00";
            stock.TradeRate = "0.00%";
            stock.TradeOffset = "0.00";
            return stock;
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RefreshStock();
        }

        private static void RefreshStock()
        {
            if (stockCodes.Count > 0)
            {
                try
                {
                    string url = string.Format("https://qt.gtimg.cn/q={0}", String.Join(",", stockCodes));
                    HttpClient httpClient = new HttpClient();
                    Task<string> response = httpClient.GetStringAsync(url);
                    string result = response.Result;
                    if (null != result && result.Length > 0)
                    {
                        string[] dataList = result.Split('\n');
                        foreach (string data in dataList)
                        {
                            if (data.StartsWith("v_"))
                            {
                                string[] stockData = data.Substring(data.IndexOf("\"") + 1).Replace("\";", "").Split('~');
                                string stockCode = data.Substring(0, data.IndexOf("=")).Replace("v_", "");
                                string tradePrice = stockData[3];
                                string tradeOffset = stockData[31];
                                string tradeRate = stockData[32];
                                if (stocks.ContainsKey(stockCode))
                                {
                                    StockEntity stock = stocks[stockCode];
                                    stock.TradePrice = tradePrice;
                                    stock.TradeRate = tradeRate + "%";
                                    stock.TradeOffset = tradeOffset;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("出现错误：{0}", ex.Message);
                }
            }
        }

        public static string GetStockName(string stockCode)
        {
            try
            {
                string url = string.Format("https://qt.gtimg.cn/q={0}", stockCode);
                HttpClient httpClient = new HttpClient();
                Task<string> response = httpClient.GetStringAsync(url);
                string result = response.Result;
                if (result.StartsWith("v_"+stockCode+"=\""))
                {
                    result = result.Substring(result.IndexOf("\"") + 1).Replace("\";", "");
                    return result.Split('~')[1];
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
