using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using WatchingStock.Service;
using WatchingStock.Entity;
using WatchingStock.Properties;

namespace WatchingStock.UI
{
    public partial class TaskbarControl : Form
    {
        // 任务栏方向
        private TaskbarDirection taskbarDirection;
        //是否win11任务栏
        private bool isWin11TaskBar = false;
        //任务栏小组件按钮是否显示
        private bool isTaskBarWidgetsBtnShow = false;
        //任务栏是否居中
        private bool isTaskBarCenter = false;
        private Rectangle rcTaskBar = new Rectangle();
        private Rectangle rcNotify = new Rectangle();
        private Rectangle rcBar = new Rectangle();
        private Rectangle rcMin = new Rectangle();
        // 文本字体
        private Font stockFont = new Font(new FontFamily("微软雅黑"), 14);
        private Font tradeFont = new Font(new FontFamily("微软雅黑"), 13);
        private int taskbarWidgetWidth = 160;
        // 空间在任务栏中高度的百分比
        private double heightScale = 0.7;
        // 图片缩放比例
        private double imageScale = 0.7;
        // 图标与文本之间的空白
        private int iconTxtSpace = 5;
        // 文本之间空白间隔
        private int txtSpace = 10;
        // 文本行之间的空白
        private int txtlineSpace = 0;
        // 上涨颜色
        private Color upColor = Color.FromArgb(255, 216, 30, 6);
        // 保持颜色
        private Color stayColor = Color.FromArgb(255, 255, 255, 255);
        // 下跌颜色
        private Color downColor = Color.FromArgb(255, 16, 178, 83);
        // 股票显示时间，超过时间则切换
        private long stockShownTime = ConfigService.StockShownTime;

        public long StockShownTime { set { this.stockShownTime = value; } }

        private Image iconImage;
        private Color txtColor;
        private Rectangle rectIcon = new Rectangle();
        private Rectangle rectTxtStockCode = new Rectangle();
        private Rectangle rectTxtStockName = new Rectangle();
        private Rectangle rectTxtTradePrice = new Rectangle();
        private Rectangle rectTxtTradeRate = new Rectangle();
        private Rectangle rectTxtTradeOffset = new Rectangle();
        private double screenScaleX = 1;
        private double screenScaleY = 1;

        private int shownStockIndex = -1;
        private long stockChangeTime = 0;
        private StockEntity currStock = StockEntity.Empty;
        private Image IconUp { get { return Resources.stock_up; } }
        private Image IconStay { get { return Resources.stock_stay; } }
        private Image IconDown { get { return Resources.stock_down; } }

        public TaskbarControl()
        {
            WndAPI.SetProcessDpiAwareness(WndAPI.PROCESS_SYSTEM_DPI_AWARE);
            InitializeComponent();

            //防止因为计算机休眠等问题重新计算
            PutControlInTaskBar();

            IntPtr hdc = WndAPI.GetDC(this.Handle);
            int dpiX = WndAPI.GetDeviceCaps(hdc, WndAPI.LOGPIXELSX);
            switch (dpiX)
            {
                case 96: this.screenScaleX = 1; break;
                case 120: this.screenScaleX = 1.25; break;
                case 144: this.screenScaleX = 1.5; break;
                case 192: this.screenScaleX = 2; break;
            }
            int dpiY = WndAPI.GetDeviceCaps(hdc, WndAPI.LOGPIXELSY);
            switch (dpiX)
            {
                case 96: this.screenScaleY = 1; break;
                case 120: this.screenScaleY = 1.25; break;
                case 144: this.screenScaleY = 1.5; break;
                case 192: this.screenScaleY = 2; break;
            }
            WndAPI.ReleaseDC(this.Handle,  hdc);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = WndAPI.WS_EX_LAYERED | WndAPI.WS_EX_TRANSPARENT | WndAPI.WS_EX_TOPMOST;
                /*cp.Style += WndAPI.WS_POPUP;*/
                cp.Parent = WndAPI.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
                return cp;
            }
        }

        private void TaskbarControl_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;

            //计算面板位置
            ComputeControlPos();

            this.StatusStay();
            StockService.Start();
            this.Invalidate();
        }

        /// <summary>
        /// 面板放入任务栏
        /// </summary>
        private void PutControlInTaskBar()
        {
            IntPtr hTaskBar = WndAPI.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
            IntPtr hNotify = WndAPI.FindWindowEx(hTaskBar, IntPtr.Zero, "TrayNotifyWnd", null);
            IntPtr hBar = WndAPI.FindWindowEx(hTaskBar, IntPtr.Zero, "ReBarWindow32", null);
            IntPtr hMin = WndAPI.FindWindowEx(hBar, IntPtr.Zero, "MSTaskSwWClass", null);

            CheckTaskBarStatus();

            WndAPI.GetWindowRect(hTaskBar, ref rcTaskBar);
            WndAPI.GetWindowRect(hNotify, ref rcNotify);
            WndAPI.GetWindowRect(hBar, ref rcBar);
            WndAPI.GetWindowRect(hMin, ref rcMin);

            WndAPI.SetParent(this.Handle, hTaskBar);
            //WndAPI.MoveWindow(this.Handle, 1950, 12, 2089 - 1950, 60 - 12, true);

            /*      int exStyle = WndAPI.GetWindowLong(this.Handle, WndAPI.GWL_EXSTYLE);
                  WndAPI.SetWindowLong(this.Handle, WndAPI.GWL_EXSTYLE, exStyle | WndAPI.WS_EX_LAYERED);
                  WndAPI.SetLayeredWindowAttributes(this.Handle, 1, 0, WndAPI.LWA_COLORKEY);*/
        }

        /// <summary>
        ///  加载任务栏状态
        /// </summary>
        private void CheckTaskBarStatus()
        {
            IntPtr hTaskBar = WndAPI.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
            isWin11TaskBar = (WndAPI.FindWindowEx(hTaskBar, IntPtr.Zero, "Windows.UI.Composition.DesktopWindowContentBridge", null) != IntPtr.Zero);
            if (isWin11TaskBar)
            {
                // win 11时注册表获取任务栏是否居中
                object value = RegistyService.GetCurrentUser("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "TaskbarAl");
                isTaskBarCenter = null == value || Convert.ToInt32(value) != 0;

                // win 11时注册表获取小组件按钮是否显示
               value = RegistyService.GetCurrentUser("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "TaskbarAl");
                isTaskBarWidgetsBtnShow = null == value || Convert.ToInt32(value) != 0;
            }
        }

        /// <summary>
        /// 计算面板坐标
        /// </summary>
        private void ComputeControlPos()
        {
           // 计算显示区域
            Rectangle workarea = Screen.PrimaryScreen.WorkingArea;
            Rectangle fullarea = Screen.PrimaryScreen.Bounds;
            int taskbarHeight; //任务栏高度
            Size controlSize = new Size();
            Point controlPos = new Point();

            if (workarea.Width == fullarea.Width)
            {
                //任务栏横置
                taskbarDirection = TaskbarDirection.Widthwise;
                taskbarHeight = fullarea.Height - workarea.Height;
                controlSize.Height = (int)(taskbarHeight * heightScale);
                rectIcon.X = (int)(controlSize.Height * (1 - imageScale) / 2);
                rectIcon.Y = (int)(controlSize.Height * (1 - imageScale) / 2);
                rectIcon.Width = (int)(controlSize.Height * imageScale);
                rectIcon.Height = (int)(controlSize.Height * imageScale);

                //计算文本的绘画长款
                using (Graphics g = Graphics.FromHwnd(this.Handle))
                {
                    rectTxtStockCode.Size = ComputeTextFont(currStock.StockCode, stockFont, workarea.Width, g);
                    rectTxtStockName.Size = ComputeTextFont(currStock.StockName, stockFont, workarea.Width, g);
                    rectTxtTradePrice.Size = ComputeTextFont(currStock.TradePrice, tradeFont, workarea.Width, g);
                    rectTxtTradeRate.Size = ComputeTextFont(currStock.TradeRate, tradeFont, workarea.Width, g);
                    rectTxtTradeOffset.Size = ComputeTextFont(currStock.TradeOffset, tradeFont, workarea.Width, g);
                }
                // 代码
                rectTxtStockCode.X = rectIcon.Right + iconTxtSpace;
                rectTxtStockCode.Y = 0;

                rectTxtStockName.X = rectTxtStockCode.Right + txtSpace;
                rectTxtStockName.Y = 0;

                rectTxtTradePrice.X = rectIcon.Right + iconTxtSpace;
                rectTxtTradePrice.Y = rectTxtStockCode.Bottom + txtlineSpace;

                rectTxtTradeRate.X = rectTxtTradePrice.Right + txtSpace;
                rectTxtTradeRate.Y = rectTxtTradePrice.Top;

                rectTxtTradeOffset.X = rectTxtTradeRate.Right + txtSpace;
                rectTxtTradeOffset.Y = rectTxtTradePrice.Top;

                controlSize.Width = Math.Max(rectTxtStockName.Right, rectTxtTradeOffset.Right);
            }
            else
            {
                //任务栏纵置
                taskbarDirection = TaskbarDirection.Longitudinal;
                taskbarHeight = fullarea.Width - workarea.Width;
                controlSize.Width = (int)(taskbarHeight * heightScale);
                rectIcon.X = (int)(controlSize.Height * (1 - imageScale) / 2);
                rectIcon.Y = (int)(controlSize.Height * (1 - imageScale) / 2);
                rectIcon.Width = (int)(controlSize.Height * imageScale);
                rectIcon.Height = (int)(controlSize.Height * imageScale);

                //计算文本的绘画长款
                using (Graphics g = Graphics.FromHwnd(this.Handle))
                {
                    rectTxtStockCode.Size = ComputeTextFont(currStock.StockCode, stockFont, workarea.Width, g);
                    rectTxtStockName.Size = ComputeTextFont(currStock.StockName, stockFont, workarea.Width, g);
                    rectTxtTradePrice.Size = ComputeTextFont(currStock.TradePrice, tradeFont, workarea.Width, g);
                    rectTxtTradeRate.Size = ComputeTextFont(currStock.TradeRate, tradeFont, workarea.Width, g);
                    rectTxtTradeOffset.Size = ComputeTextFont(currStock.TradeOffset, tradeFont, workarea.Width, g);
                }
                // 代码
                rectTxtStockCode.X = 0;
                rectTxtStockCode.Y = rectIcon.Bottom + iconTxtSpace;

                rectTxtStockName.X = 0;
                rectTxtStockName.Y = rectTxtStockCode.Bottom + txtSpace;

                rectTxtTradePrice.X = 0;
                rectTxtTradePrice.Y = rectTxtStockName.Bottom + txtSpace;

                rectTxtTradeRate.X = 0;
                rectTxtTradeRate.Y = rectTxtTradePrice.Bottom + txtSpace;

                rectTxtTradeOffset.X = 0;
                rectTxtTradeOffset.Y = rectTxtTradeRate.Bottom + txtSpace;

                controlSize.Height = rectTxtTradeOffset.Bottom - rectIcon.Top;
            }

            //计算控件位置
            if (isWin11TaskBar && isTaskBarCenter)
            {
                //如果win11居中则放在开始按钮左边
                if (TaskbarDirection.Widthwise == taskbarDirection)
                {
                    //横置
                    //controlPos.X = rcMin.Left - controlSize.Width;
                    controlPos.X = isTaskBarWidgetsBtnShow ?   (int)(taskbarWidgetWidth * screenScaleX) : 0;
                    controlPos.Y = (taskbarHeight - controlSize.Height) / 2;
                } 
                else
                {
                    //纵置
                    controlPos.X = (taskbarHeight - controlSize.Width) / 2;
                    //controlPos.Y = rcMin.Top - controlSize.Height;
                    controlPos.Y = isTaskBarWidgetsBtnShow ? (int)(taskbarWidgetWidth * screenScaleY) : 0;
                }
            }
            else
            {
                //放在通知栏左边
                if (TaskbarDirection.Widthwise == taskbarDirection)
                {
                    //横置
                    controlPos.X = rcNotify.Left - controlSize.Width;
                    controlPos.Y = (taskbarHeight - controlSize.Height) / 2;
                }
                else
                {
                    //纵置
                    controlPos.X = (taskbarHeight - controlSize.Width) / 2;
                    controlPos.Y = rcNotify.Top - controlSize.Height;
                }
            }
            this.Size = new Size(controlSize.Width + 1, controlSize.Height + 1);
            this.Location = controlPos;
        }

        private void StatusDown()
        {
            this.iconImage = this.IconDown;
            this.txtColor = this.downColor;
        }

        private void StatusUp()
        {
            this.iconImage = this.IconUp;
            this.txtColor = this.upColor;
        }

        private void StatusStay()
        {
            this.iconImage = this.IconStay;
            this.txtColor = this.stayColor;
        }


        /// <summary>
        /// 计算文本长度
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="maxWdith">一行最大宽度</param>
        /// <param name="g">绘图对象</param>
        /// <returns></returns>
        private Size ComputeTextFont(string text, Font txtFont, int maxWdith, Graphics g)
        {
            Size size = Size.Empty;
           /* StringFormat sf = StringFormat.GenericTypographic;
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;*/
            SizeF strSize = g.MeasureString(text, txtFont, maxWdith);
            size.Width = Convert.ToInt32(Math.Ceiling(strSize.Width));
            size.Height = Convert.ToInt32(Math.Ceiling(strSize.Height));
            return size;
        }

        private void switchTimer_Tick(object sender, EventArgs e)
        {

            if (StockService.Count > 0)
            {
                long now = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
                if (now - stockChangeTime > stockShownTime)
                {
                    stockChangeTime = now;
                    if (this.shownStockIndex == StockService.Count - 1)
                    {
                        this.shownStockIndex = 0;
                    }
                    else
                    {
                        this.shownStockIndex++;
                    }
                    //获取股票信息
                    this.currStock = StockService.Get(this.shownStockIndex);
                }
                double offset = Convert.ToDouble(currStock.TradeOffset);
                if (offset > 0)
                {
                    this.StatusUp();
                }
                else if (offset < 0)
                {
                    this.StatusDown();
                }
                else
                {
                    this.StatusStay();
                }
            }

            //防止因为计算机休眠等问题重新计算
            //PutControlInTaskBar();

            //计算面板位置
            ComputeControlPos();

            this.Invalidate();
        }

        private void TaskbarControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(this.iconImage, rectIcon);
            Brush txtBrush = new SolidBrush(this.txtColor);
            g.DrawString(currStock.StockCode, stockFont, Brushes.White, rectTxtStockCode);
            g.DrawString(currStock.StockName, stockFont, Brushes.White, rectTxtStockName);
            g.DrawString(currStock.TradePrice, tradeFont, txtBrush, rectTxtTradePrice);
            g.DrawString(currStock.TradeRate, tradeFont, txtBrush, rectTxtTradeRate);
            g.DrawString(currStock.TradeOffset, tradeFont, txtBrush, rectTxtTradeOffset);
            txtBrush.Dispose();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = !this.Visible;
        }

        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripMenuItem_Settings_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(this);
            settingsForm.Show();
        }
    }
}
