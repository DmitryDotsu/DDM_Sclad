namespace DDM_Sclad
{
    partial class MainForm
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
            this.btnWarehouses = new System.Windows.Forms.Button();
            this.btnEmployees = new System.Windows.Forms.Button();
            this.btnPositions = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.btnCounterparties = new System.Windows.Forms.Button();
            this.btnIncomeDoc = new System.Windows.Forms.Button();
            this.btnExpenseDoc = new System.Windows.Forms.Button();
            this.btnReportStock = new System.Windows.Forms.Button();
            this.btnReportProfit = new System.Windows.Forms.Button();
            this.btnReportMovement = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnWarehouses
            // 
            this.btnWarehouses.Location = new System.Drawing.Point(518, 74);
            this.btnWarehouses.Name = "btnWarehouses";
            this.btnWarehouses.Size = new System.Drawing.Size(200, 40);
            this.btnWarehouses.TabIndex = 1;
            this.btnWarehouses.Text = "Склады";
            this.btnWarehouses.Click += new System.EventHandler(this.btnWarehouses_Click);
            // 
            // btnEmployees
            // 
            this.btnEmployees.Location = new System.Drawing.Point(50, 74);
            this.btnEmployees.Name = "btnEmployees";
            this.btnEmployees.Size = new System.Drawing.Size(200, 40);
            this.btnEmployees.TabIndex = 3;
            this.btnEmployees.Text = "Персонал";
            this.btnEmployees.Click += new System.EventHandler(this.btnEmployees_Click);
            // 
            // btnPositions
            // 
            this.btnPositions.Location = new System.Drawing.Point(50, 120);
            this.btnPositions.Name = "btnPositions";
            this.btnPositions.Size = new System.Drawing.Size(200, 40);
            this.btnPositions.TabIndex = 0;
            this.btnPositions.Text = "Должности";
            this.btnPositions.Click += new System.EventHandler(this.btnPositions_Click);
            // 
            // btnProducts
            // 
            this.btnProducts.Location = new System.Drawing.Point(287, 74);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(200, 40);
            this.btnProducts.TabIndex = 4;
            this.btnProducts.Text = "Товары";
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            // 
            // btnCounterparties
            // 
            this.btnCounterparties.Location = new System.Drawing.Point(518, 120);
            this.btnCounterparties.Name = "btnCounterparties";
            this.btnCounterparties.Size = new System.Drawing.Size(200, 40);
            this.btnCounterparties.TabIndex = 5;
            this.btnCounterparties.Text = "Контрагенты";
            this.btnCounterparties.Click += new System.EventHandler(this.btnCounterparties_Click);
            // 
            // btnIncomeDoc
            // 
            this.btnIncomeDoc.Location = new System.Drawing.Point(287, 120);
            this.btnIncomeDoc.Name = "btnIncomeDoc";
            this.btnIncomeDoc.Size = new System.Drawing.Size(200, 40);
            this.btnIncomeDoc.TabIndex = 6;
            this.btnIncomeDoc.Text = "Приход товара";
            this.btnIncomeDoc.Click += new System.EventHandler(this.btnIncomeDoc_Click);
            // 
            // btnExpenseDoc
            // 
            this.btnExpenseDoc.Location = new System.Drawing.Point(287, 166);
            this.btnExpenseDoc.Name = "btnExpenseDoc";
            this.btnExpenseDoc.Size = new System.Drawing.Size(200, 40);
            this.btnExpenseDoc.TabIndex = 7;
            this.btnExpenseDoc.Text = "Расход товара";
            this.btnExpenseDoc.Click += new System.EventHandler(this.btnExpenseDoc_Click);
            // 
            // btnReportStock
            // 
            this.btnReportStock.Location = new System.Drawing.Point(518, 249);
            this.btnReportStock.Name = "btnReportStock";
            this.btnReportStock.Size = new System.Drawing.Size(200, 40);
            this.btnReportStock.TabIndex = 8;
            this.btnReportStock.Text = "Отчет: Остатки на складах";
            this.btnReportStock.Click += new System.EventHandler(this.btnReportStock_Click);
            // 
            // btnReportProfit
            // 
            this.btnReportProfit.Location = new System.Drawing.Point(50, 249);
            this.btnReportProfit.Name = "btnReportProfit";
            this.btnReportProfit.Size = new System.Drawing.Size(200, 40);
            this.btnReportProfit.TabIndex = 9;
            this.btnReportProfit.Text = "Отчет: Прибыль от продаж";
            this.btnReportProfit.Click += new System.EventHandler(this.btnReportProfit_Click);
            // 
            // btnReportMovement
            // 
            this.btnReportMovement.Location = new System.Drawing.Point(290, 249);
            this.btnReportMovement.Name = "btnReportMovement";
            this.btnReportMovement.Size = new System.Drawing.Size(200, 40);
            this.btnReportMovement.TabIndex = 10;
            this.btnReportMovement.Text = "Отчет: Движение товаров";
            this.btnReportMovement.Click += new System.EventHandler(this.btnReportMovement_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(70, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(369, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Система автоматизации склада";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(764, 340);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnWarehouses);
            this.Controls.Add(this.btnEmployees);
            this.Controls.Add(this.btnPositions);
            this.Controls.Add(this.btnProducts);
            this.Controls.Add(this.btnCounterparties);
            this.Controls.Add(this.btnIncomeDoc);
            this.Controls.Add(this.btnExpenseDoc);
            this.Controls.Add(this.btnReportStock);
            this.Controls.Add(this.btnReportProfit);
            this.Controls.Add(this.btnReportMovement);
            this.Name = "MainForm";
            this.Text = "Учет товаров на складе";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnWarehouses;
        private System.Windows.Forms.Button btnEmployees;
        private System.Windows.Forms.Button btnPositions;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Button btnCounterparties;
        private System.Windows.Forms.Button btnIncomeDoc;
        private System.Windows.Forms.Button btnExpenseDoc;
        private System.Windows.Forms.Button btnReportStock;
        private System.Windows.Forms.Button btnReportProfit;
        private System.Windows.Forms.Button btnReportMovement;
        private System.Windows.Forms.Label lblTitle;
    }
}