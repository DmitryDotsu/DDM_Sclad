using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace DDM_Sclad
{
    public partial class ExpenseHeaderEditForm : Form
    {
        private DataRow editingRow;
        private bool isEdit;
        private ComboBox cboWarehouse;
        private ComboBox cboCounterparty;
        private TextBox txtNumber;
        private DateTimePicker dtDate;
        private Button btnSave;
        private Button btnCancel;

        public ExpenseHeaderEditForm(DataRow row)
        {
            InitializeComponent();
            isEdit = (row != null);
            editingRow = row;
            Text = isEdit ? "Редактирование документа расхода" : "Новый документ расхода";
            LoadComboData();
            if (isEdit) LoadData();
        }

        private void InitializeComponent()
        {
            this.lblWarehouse = new Label();
            this.lblCounterparty = new Label();
            this.lblNumber = new Label();
            this.lblDate = new Label();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.cboWarehouse = new ComboBox();
            this.cboCounterparty = new ComboBox();
            this.txtNumber = new TextBox();
            this.dtDate = new DateTimePicker();
            this.SuspendLayout();

            // lblWarehouse
            this.lblWarehouse.Text = "Склад:";
            this.lblWarehouse.Location = new System.Drawing.Point(12, 15);
            this.lblWarehouse.Size = new System.Drawing.Size(80, 20);

            // cboWarehouse
            this.cboWarehouse.Location = new System.Drawing.Point(100, 12);
            this.cboWarehouse.Size = new System.Drawing.Size(250, 21);
            this.cboWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;

            // lblCounterparty
            this.lblCounterparty.Text = "Контрагент (покупатель):";
            this.lblCounterparty.Location = new System.Drawing.Point(12, 48);
            this.lblCounterparty.Size = new System.Drawing.Size(130, 20);

            // cboCounterparty
            this.cboCounterparty.Location = new System.Drawing.Point(150, 45);
            this.cboCounterparty.Size = new System.Drawing.Size(250, 21);
            this.cboCounterparty.DropDownStyle = ComboBoxStyle.DropDownList;

            // lblNumber
            this.lblNumber.Text = "Номер накладной:";
            this.lblNumber.Location = new System.Drawing.Point(12, 78);
            this.lblNumber.Size = new System.Drawing.Size(100, 20);

            // txtNumber
            this.txtNumber.Location = new System.Drawing.Point(120, 75);
            this.txtNumber.Size = new System.Drawing.Size(230, 20);

            // lblDate
            this.lblDate.Text = "Дата:";
            this.lblDate.Location = new System.Drawing.Point(12, 108);
            this.lblDate.Size = new System.Drawing.Size(40, 20);

            // dtDate
            this.dtDate.Location = new System.Drawing.Point(60, 105);
            this.dtDate.Size = new System.Drawing.Size(150, 20);
            this.dtDate.Format = DateTimePickerFormat.Short;

            // btnSave
            this.btnSave.Text = "Сохранить";
            this.btnSave.Location = new System.Drawing.Point(100, 145);
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);

            // btnCancel
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Location = new System.Drawing.Point(210, 145);
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(430, 195);
            this.Controls.AddRange(new Control[] { lblWarehouse, cboWarehouse, lblCounterparty,
                cboCounterparty, lblNumber, txtNumber, lblDate, dtDate, btnSave, btnCancel });
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblWarehouse;
        private Label lblCounterparty;
        private Label lblNumber;
        private Label lblDate;

        private void LoadComboData()
        {
            var warehouses = DatabaseHelper.ExecuteQuery("SELECT id_склада, наименование FROM склады");
            cboWarehouse.DataSource = warehouses;
            cboWarehouse.DisplayMember = "наименование";
            cboWarehouse.ValueMember = "id_склада";

            var counterparties = DatabaseHelper.ExecuteQuery("SELECT id_контрагента, название FROM контрагенты WHERE является_покупателем = true");
            cboCounterparty.DataSource = counterparties;
            cboCounterparty.DisplayMember = "название";
            cboCounterparty.ValueMember = "id_контрагента";
        }

        private void LoadData()
        {
            if (editingRow != null)
            {
                if (editingRow["Номер"] != DBNull.Value)
                    txtNumber.Text = editingRow["Номер"].ToString();
                if (editingRow["Дата"] != DBNull.Value)
                    dtDate.Value = Convert.ToDateTime(editingRow["Дата"]);
                if (editingRow["Склад"] != DBNull.Value)
                {
                    // Установка выбранного склада
                    for (int i = 0; i < cboWarehouse.Items.Count; i++)
                    {
                        DataRowView item = (DataRowView)cboWarehouse.Items[i];
                        if (item["наименование"].ToString() == editingRow["Склад"].ToString())
                        {
                            cboWarehouse.SelectedIndex = i;
                            break;
                        }
                    }
                }
                if (editingRow["Контрагент"] != DBNull.Value)
                {
                    for (int i = 0; i < cboCounterparty.Items.Count; i++)
                    {
                        DataRowView item = (DataRowView)cboCounterparty.Items[i];
                        if (item["название"].ToString() == editingRow["Контрагент"].ToString())
                        {
                            cboCounterparty.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isEdit)
                {
                    string query = @"UPDATE расход_товара 
                                    SET id_склада = @warehouse, id_контрагента = @counterparty, 
                                        номер_накладной = @number, дата_расхода = @date
                                    WHERE id_расхода = @id";
                    DatabaseHelper.ExecuteNonQuery(query, new[] {
                        new NpgsqlParameter("@warehouse", cboWarehouse.SelectedValue),
                        new NpgsqlParameter("@counterparty", cboCounterparty.SelectedValue),
                        new NpgsqlParameter("@number", txtNumber.Text),
                        new NpgsqlParameter("@date", dtDate.Value),
                        new NpgsqlParameter("@id", editingRow["Код"])
                    });
                }
                else
                {
                    string query = @"INSERT INTO расход_товара (id_склада, id_контрагента, номер_накладной, дата_расхода) 
                                    VALUES (@warehouse, @counterparty, @number, @date)";
                    DatabaseHelper.ExecuteNonQuery(query, new[] {
                        new NpgsqlParameter("@warehouse", cboWarehouse.SelectedValue),
                        new NpgsqlParameter("@counterparty", cboCounterparty.SelectedValue),
                        new NpgsqlParameter("@number", txtNumber.Text),
                        new NpgsqlParameter("@date", dtDate.Value)
                    });
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}