using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace DDM_Sclad
{
    public partial class IncomeHeaderEditForm : Form
    {
        private DataRow editingRow;
        private bool isEdit;

        public IncomeHeaderEditForm(DataRow row)
        {
            InitializeComponent();
            isEdit = (row != null);
            editingRow = row;
            Text = isEdit ? "Редактирование документа прихода" : "Новый документ прихода";
            LoadComboData();
            if (isEdit) LoadData();
        }

        private void LoadComboData()
        {
            var warehouses = DatabaseHelper.ExecuteQuery("SELECT id_склада, наименование FROM склады");
            cboWarehouse.DataSource = warehouses;
            cboWarehouse.DisplayMember = "наименование";
            cboWarehouse.ValueMember = "id_склада";

            var counterparties = DatabaseHelper.ExecuteQuery("SELECT id_контрагента, название FROM контрагенты WHERE является_поставщиком = true");
            cboCounterparty.DataSource = counterparties;
            cboCounterparty.DisplayMember = "название";
            cboCounterparty.ValueMember = "id_контрагента";
        }

        private void LoadData()
        {
            if (editingRow != null)
            {
                txtNumber.Text = editingRow["Номер"].ToString();
                dtDate.Value = Convert.ToDateTime(editingRow["Дата"]);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isEdit)
                {
                    string query = @"UPDATE приход_товара 
                                    SET id_склада = @warehouse, id_контрагента = @counterparty, 
                                        номер_накладной = @number, дата_прихода = @date
                                    WHERE id_прихода = @id";
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
                    string query = @"INSERT INTO приход_товара (id_склада, id_контрагента, номер_накладной, дата_прихода) 
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