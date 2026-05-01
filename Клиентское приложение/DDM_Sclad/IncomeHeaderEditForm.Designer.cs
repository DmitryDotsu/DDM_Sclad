namespace DDM_Sclad
{
    partial class IncomeHeaderEditForm
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
            this.lblWarehouse = new System.Windows.Forms.Label();
            this.lblCounterparty = new System.Windows.Forms.Label();
            this.lblNumber = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboWarehouse = new System.Windows.Forms.ComboBox();
            this.cboCounterparty = new System.Windows.Forms.ComboBox();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();

            // lblWarehouse
            this.lblWarehouse.AutoSize = true;
            this.lblWarehouse.Location = new System.Drawing.Point(12, 15);
            this.lblWarehouse.Text = "Склад:";

            // cboWarehouse
            this.cboWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWarehouse.Location = new System.Drawing.Point(100, 12);
            this.cboWarehouse.Size = new System.Drawing.Size(250, 24);

            // lblCounterparty
            this.lblCounterparty.AutoSize = true;
            this.lblCounterparty.Location = new System.Drawing.Point(12, 48);
            this.lblCounterparty.Text = "Контрагент:";

            // cboCounterparty
            this.cboCounterparty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCounterparty.Location = new System.Drawing.Point(100, 45);
            this.cboCounterparty.Size = new System.Drawing.Size(250, 24);

            // lblNumber
            this.lblNumber.AutoSize = true;
            this.lblNumber.Location = new System.Drawing.Point(12, 81);
            this.lblNumber.Text = "Номер накладной:";

            // txtNumber
            this.txtNumber.Location = new System.Drawing.Point(120, 78);
            this.txtNumber.Size = new System.Drawing.Size(230, 22);

            // lblDate
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(12, 114);
            this.lblDate.Text = "Дата:";

            // dtDate
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDate.Location = new System.Drawing.Point(60, 111);
            this.dtDate.Size = new System.Drawing.Size(150, 22);

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(100, 155);
            this.btnSave.Size = new System.Drawing.Size(90, 35);
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(210, 155);
            this.btnCancel.Size = new System.Drawing.Size(90, 35);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            // IncomeHeaderEditForm
            this.ClientSize = new System.Drawing.Size(374, 210);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dtDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.txtNumber);
            this.Controls.Add(this.lblNumber);
            this.Controls.Add(this.cboCounterparty);
            this.Controls.Add(this.lblCounterparty);
            this.Controls.Add(this.cboWarehouse);
            this.Controls.Add(this.lblWarehouse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.Label lblCounterparty;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cboWarehouse;
        private System.Windows.Forms.ComboBox cboCounterparty;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.DateTimePicker dtDate;
    }
}