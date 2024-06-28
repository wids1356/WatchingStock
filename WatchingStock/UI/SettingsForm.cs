using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WatchingStock.Service;
using WatchingStock.Entity;

namespace WatchingStock.UI
{
    public partial class SettingsForm : Form
    {
        private TaskbarControl taskbarControl;

        public SettingsForm(TaskbarControl taskbarControl)
        {
            InitializeComponent();
            this.taskbarControl = taskbarControl;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            StockEntity[] stocks = StockService.Stocks;
            foreach (StockEntity stock in stocks)
            {
                ListViewItem item = new ListViewItem();
                item.Text = stock.StockCode;
                item.SubItems.Add(stock.StockName);
                this.listViewStock.Items.Add(item);
            }
            this.txtStockShownTime.Value = ConfigService.StockShownTime;
            ResizeListViewTitle();
        }

        private void listViewStock_Resize(object sender, EventArgs e)
        {
            ResizeListViewTitle();
        }

        public void Loading()
        {
            this.listViewStock.Enabled = false;
            this.txtStockCode.Enabled = false;
            this.btnAdd.Enabled = false;
            this.btnRemove.Enabled = false;
            this.txtStockShownTime.Enabled = false;
        }

        public void Loaded()
        {
            this.listViewStock.Enabled = true;
            this.txtStockCode.Enabled = true;
            this.btnAdd.Enabled = true;
            this.btnRemove.Enabled = true;
            this.txtStockShownTime.Enabled = true;
        }

        /// <summary>
        /// 重新计算股票列表尺寸
        /// </summary>
        private void ResizeListViewTitle()
        {
            int size = this.listViewStock.Columns.Count;
            int width = 0;
            for (int i = 0; i < size - 1; i++)
            {
                width += this.listViewStock.Columns[i].Width;
            }
            this.listViewStock.Columns[this.listViewStock.Columns.Count - 1].Width = this.listViewStock.Width - width;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Loading();
            string stockCode = this.txtStockCode.Text;
            if (StockService.Exists(stockCode))
            {
                MessageBox.Show(this, "该股票已经添加过");
            } else
            {
                string stockName = StockService.GetStockName(stockCode);
                if (null != stockName)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = stockCode;
                    item.SubItems.Add(stockName);
                    this.listViewStock.Items.Add(item);
                    StockService.AddStock(stockCode, stockName);
                    this.txtStockCode.Text = "";
                }
                else
                {
                    MessageBox.Show(this, "无效的股票代码");
                }
                
            }
           
            this.Loaded();
        }

        private void listViewStock_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (this.listViewStock.SelectedItems.Count > 0)
            {
                this.btnRemove.Enabled = true;
            }
            else
            {
                this.btnRemove.Enabled = false;
            }
        }

        private void btnCommonSave_Click(object sender, EventArgs e)
        {
            this.Loading();
            int time = Convert.ToInt32(this.txtStockShownTime.Value);
            ConfigService.StockShownTime = time;
            this.taskbarControl.StockShownTime = time;
            this.Loaded();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.listViewStock.SelectedItems.Count > 0)
            {
                this.Loading();
                string stockCode = this.listViewStock.SelectedItems[0].Text;
                this.listViewStock.Items.RemoveAt(this.listViewStock.SelectedIndices[0]);
                StockService.RemoveStock(stockCode);
                this.btnRemove.Enabled = false;
                this.Loaded();
            }
        }

    }
}
