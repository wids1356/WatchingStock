using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchingStock.Entity
{
    public class StockEntity
    {       
        public static StockEntity Empty
        {
            get
            {
                StockEntity stock = new StockEntity();
                stock.StockCode = "--------";
                stock.StockName = "加载中";
                stock.TradePrice = "0.00";
                stock.TradeRate = "0.00%";
                stock.TradeOffset = "0.00";
                return stock;
            }
        }


        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string TradePrice { get; set; }
        public string TradeRate { get; set; }
        public string TradeOffset { get; set; }

    }
}
