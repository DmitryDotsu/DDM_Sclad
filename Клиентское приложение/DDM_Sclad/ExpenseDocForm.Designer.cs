namespace DDM_Sclad
{
    partial class ExpenseDocForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvHeaders = new System.Windows.Forms.DataGridView();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.btnAddHeader = new System.Windows.Forms.Button();
            this.btnEditHeader = new System.Windows.Forms.Button();
            this.btnDeleteHeader = new System.Windows.Forms.Button();
            this.btnAddDetail = new System.Windows.Forms.Button();
            this.btnEditDetail = new System.Windows.Forms.Button();
            this.btnDeleteDetail = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblDetail = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvHeaders);
            this.groupBox1.Controls.Add(this.btnAddHeader);
            this.groupBox1.Controls.Add(this.btnEditHeader);
            this.groupBox1.Controls.Add(this.btnDeleteHeader);
            this.groupBox1.Location = new System.Drawing.Point(12, 40);
            this.groupBox1.Size = new System.Drawing.Size(800, 250);
            this.groupBox1.Text = "Шапка документа расхода";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvDetails);
            this.groupBox2.Controls.Add(this.btnAddDetail);
            this.groupBox2.Controls.Add(this.btnEditDetail);
            this.groupBox2.Controls.Add(this.btnDeleteDetail);
            this.groupBox2.Location = new System.Drawing.Point(12, 320);
            this.groupBox2.Size = new System.Drawing.Size(800, 250);
            this.groupBox2.Text = "Товары в накладной (ТЧ)";
            // 
            // dgvHeaders
            // 
            this.dgvHeaders.Location = new System.Drawing.Point(6, 22);
            this.dgvHeaders.Size = new System.Drawing.Size(788, 180);
            this.dgvHeaders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHeaders.MultiSelect = false;
            this.dgvHeaders.SelectionChanged += new System.EventHandler(this.dgvHeaders_SelectionChanged);
            // 
            // dgvDetails
            // 
            this.dgvDetails.Location = new System.Drawing.Point(6, 22);
            this.dgvDetails.Size = new System.Drawing.Size(788, 180);
            this.dgvDetails.ReadOnly = true;
            // 
            // btnAddHeader
            // 
            this.btnAddHeader.Text = "Добавить";
            this.btnAddHeader.Location = new System.Drawing.Point(6, 208);
            this.btnAddHeader.Size = new System.Drawing.Size(90, 30);
            this.btnAddHeader.Click += new System.EventHandler(this.btnAddHeader_Click);
            // 
            // btnEditHeader
            // 
            this.btnEditHeader.Text = "Изменить";
            this.btnEditHeader.Location = new System.Drawing.Point(102, 208);
            this.btnEditHeader.Size = new System.Drawing.Size(90, 30);
            this.btnEditHeader.Click += new System.EventHandler(this.btnEditHeader_Click);
            // 
            // btnDeleteHeader
            // 
            this.btnDeleteHeader.Text = "Удалить";
            this.btnDeleteHeader.Location = new System.Drawing.Point(198, 208);
            this.btnDeleteHeader.Size = new System.Drawing.Size(90, 30);
            this.btnDeleteHeader.Click += new System.EventHandler(this.btnDeleteHeader_Click);
            // 
            // btnAddDetail
            // 
            this.btnAddDetail.Text = "Добавить";
            this.btnAddDetail.Location = new System.Drawing.Point(6, 208);
            this.btnAddDetail.Size = new System.Drawing.Size(90, 30);
            this.btnAddDetail.Click += new System.EventHandler(this.btnAddDetail_Click);
            // 
            // btnEditDetail
            // 
            this.btnEditDetail.Text = "Изменить";
            this.btnEditDetail.Location = new System.Drawing.Point(102, 208);
            this.btnEditDetail.Size = new System.Drawing.Size(90, 30);
            this.btnEditDetail.Click += new System.EventHandler(this.btnEditDetail_Click);
            // 
            // btnDeleteDetail
            // 
            this.btnDeleteDetail.Text = "Удалить";
            this.btnDeleteDetail.Location = new System.Drawing.Point(198, 208);
            this.btnDeleteDetail.Size = new System.Drawing.Size(90, 30);
            this.btnDeleteDetail.Click += new System.EventHandler(this.btnDeleteDetail_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Location = new System.Drawing.Point(700, 12);
            this.btnRefresh.Size = new System.Drawing.Size(100, 25);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblHeader.Location = new System.Drawing.Point(12, 15);
            this.lblHeader.Text = "Документы расхода";
            // 
            // lblDetail
            // 
            this.lblDetail.AutoSize = true;
            this.lblDetail.Location = new System.Drawing.Point(300, 305);
            this.lblDetail.Text = "Строки выбранного документа";
            // 
            // ExpenseDocForm
            // 
            this.ClientSize = new System.Drawing.Size(824, 581);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.lblDetail);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Text = "Расход товара";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dgvHeaders;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.Button btnAddHeader;
        private System.Windows.Forms.Button btnEditHeader;
        private System.Windows.Forms.Button btnDeleteHeader;
        private System.Windows.Forms.Button btnAddDetail;
        private System.Windows.Forms.Button btnEditDetail;
        private System.Windows.Forms.Button btnDeleteDetail;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}