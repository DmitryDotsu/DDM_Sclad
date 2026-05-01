using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace DDM_Sclad
{
    public partial class IncomeDetailEditForm : Form
    {
        private int headerId;
        private DataRow editingRow;
        private bool isEdit;

        public IncomeDetailEditForm(int headerId, DataRow row)
        {
            InitializeComponent();
            this.headerId = headerId;
            this.editingRow = row;
            this.isEdit = (row != null);
            Text = isEdit ? "Редактирование строки" : "Добавление товара";
            LoadProductData();
            if (isEdit) LoadData();
        }

        private void LoadProductData()
        {
            var products = DatabaseHelper.ExecuteQuery("SELECT id_товара, название FROM товары ORDER BY название");
            cboProduct.DataSource = products;
            cboProduct.DisplayMember = "название";
            cboProduct.ValueMember = "id_товара";
        }

        private void LoadData()
        {
            if (editingRow != null)
            {
                txtPrice.Text = editingRow["цена"].ToString();
                txtQuantity.Text = editingRow["количество"].ToString();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Введите корректную цену");
                    return;
                }
                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Введите корректное количество");
                    return;
                }

                if (isEdit)
                {
                    string query = @"UPDATE ТЧ_накладная_прихода 
                                    SET id_товара = @product, цена = @price, количество = @quantity
                                    WHERE id_записи = @id";
                    DatabaseHelper.ExecuteNonQuery(query, new[] {
                        new NpgsqlParameter("@product", cboProduct.SelectedValue),
                        new NpgsqlParameter("@price", price),
                        new NpgsqlParameter("@quantity", quantity),
                        new NpgsqlParameter("@id", editingRow["Код"])
                    });
                }
                else
                {
                    string query = @"INSERT INTO ТЧ_накладная_прихода (id_прихода, id_товара, цена, количество) 
                                    VALUES (@header, @product, @price, @quantity)";
                    DatabaseHelper.ExecuteNonQuery(query, new[] {
                        new NpgsqlParameter("@header", headerId),
                        new NpgsqlParameter("@product", cboProduct.SelectedValue),
                        new NpgsqlParameter("@price", price),
                        new NpgsqlParameter("@quantity", quantity)
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