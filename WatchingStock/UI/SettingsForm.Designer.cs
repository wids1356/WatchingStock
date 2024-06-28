
namespace WatchingStock.UI
{
    partial class SettingsForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageStock = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtStockCode = new System.Windows.Forms.TextBox();
            this.listViewStock = new System.Windows.Forms.ListView();
            this.chStockCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStockName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageUI = new System.Windows.Forms.TabPage();
            this.btnCommonSave = new System.Windows.Forms.Button();
            this.groupBoxUI = new System.Windows.Forms.GroupBox();
            this.txtStockShownTime = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPageStock.SuspendLayout();
            this.tabPageUI.SuspendLayout();
            this.groupBoxUI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStockShownTime)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageStock);
            this.tabControl.Controls.Add(this.tabPageUI);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(808, 514);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageStock
            // 
            this.tabPageStock.Controls.Add(this.label1);
            this.tabPageStock.Controls.Add(this.btnRemove);
            this.tabPageStock.Controls.Add(this.btnAdd);
            this.tabPageStock.Controls.Add(this.txtStockCode);
            this.tabPageStock.Controls.Add(this.listViewStock);
            this.tabPageStock.Location = new System.Drawing.Point(4, 28);
            this.tabPageStock.Name = "tabPageStock";
            this.tabPageStock.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStock.Size = new System.Drawing.Size(800, 482);
            this.tabPageStock.TabIndex = 0;
            this.tabPageStock.Text = "股票";
            this.tabPageStock.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(583, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "输入股票代码：";
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(583, 133);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(208, 40);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "移除";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(583, 87);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(208, 40);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "新增";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtStockCode
            // 
            this.txtStockCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStockCode.Location = new System.Drawing.Point(583, 42);
            this.txtStockCode.Name = "txtStockCode";
            this.txtStockCode.Size = new System.Drawing.Size(208, 28);
            this.txtStockCode.TabIndex = 2;
            // 
            // listViewStock
            // 
            this.listViewStock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewStock.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chStockCode,
            this.chStockName});
            this.listViewStock.FullRowSelect = true;
            this.listViewStock.HideSelection = false;
            this.listViewStock.Location = new System.Drawing.Point(3, 3);
            this.listViewStock.Name = "listViewStock";
            this.listViewStock.Size = new System.Drawing.Size(571, 476);
            this.listViewStock.TabIndex = 0;
            this.listViewStock.UseCompatibleStateImageBehavior = false;
            this.listViewStock.View = System.Windows.Forms.View.Details;
            this.listViewStock.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewStock_ItemSelectionChanged);
            this.listViewStock.Resize += new System.EventHandler(this.listViewStock_Resize);
            // 
            // chStockCode
            // 
            this.chStockCode.Text = "股票代码";
            this.chStockCode.Width = 131;
            // 
            // chStockName
            // 
            this.chStockName.Text = "股票名称";
            this.chStockName.Width = 256;
            // 
            // tabPageUI
            // 
            this.tabPageUI.Controls.Add(this.btnCommonSave);
            this.tabPageUI.Controls.Add(this.groupBoxUI);
            this.tabPageUI.Location = new System.Drawing.Point(4, 28);
            this.tabPageUI.Name = "tabPageUI";
            this.tabPageUI.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUI.Size = new System.Drawing.Size(800, 482);
            this.tabPageUI.TabIndex = 1;
            this.tabPageUI.Text = "常规";
            this.tabPageUI.UseVisualStyleBackColor = true;
            // 
            // btnCommonSave
            // 
            this.btnCommonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCommonSave.Location = new System.Drawing.Point(584, 434);
            this.btnCommonSave.Name = "btnCommonSave";
            this.btnCommonSave.Size = new System.Drawing.Size(208, 40);
            this.btnCommonSave.TabIndex = 4;
            this.btnCommonSave.Text = "保存";
            this.btnCommonSave.UseVisualStyleBackColor = true;
            this.btnCommonSave.Click += new System.EventHandler(this.btnCommonSave_Click);
            // 
            // groupBoxUI
            // 
            this.groupBoxUI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUI.Controls.Add(this.txtStockShownTime);
            this.groupBoxUI.Controls.Add(this.label2);
            this.groupBoxUI.Location = new System.Drawing.Point(6, 6);
            this.groupBoxUI.Name = "groupBoxUI";
            this.groupBoxUI.Size = new System.Drawing.Size(786, 100);
            this.groupBoxUI.TabIndex = 0;
            this.groupBoxUI.TabStop = false;
            this.groupBoxUI.Text = "界面";
            // 
            // txtStockShownTime
            // 
            this.txtStockShownTime.Location = new System.Drawing.Point(10, 54);
            this.txtStockShownTime.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtStockShownTime.Name = "txtStockShownTime";
            this.txtStockShownTime.Size = new System.Drawing.Size(189, 28);
            this.txtStockShownTime.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "股票显示时长（单位：秒）";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 514);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "配置";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageStock.ResumeLayout(false);
            this.tabPageStock.PerformLayout();
            this.tabPageUI.ResumeLayout(false);
            this.groupBoxUI.ResumeLayout(false);
            this.groupBoxUI.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStockShownTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageStock;
        private System.Windows.Forms.TabPage tabPageUI;
        private System.Windows.Forms.ListView listViewStock;
        private System.Windows.Forms.ColumnHeader chStockCode;
        private System.Windows.Forms.ColumnHeader chStockName;
        private System.Windows.Forms.TextBox txtStockCode;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxUI;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown txtStockShownTime;
        private System.Windows.Forms.Button btnCommonSave;
    }
}

