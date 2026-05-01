using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace DDM_Sclad
{
    public partial class ExpenseDetailEditForm : Form
    {
        private int headerId;
        private DataRow editingRow;
        private ComboBox cboProduct;
        private TextBox txtPrice;
        private TextBox txtQuantity;
        private Button btnSave;
        private Button btnCancel;
        private bool isEdit;

        public ExpenseDetailEditForm(int headerId, DataRow row)
        {
            InitializeComponent();
            this.headerId = headerId;
            this.editingRow = row;
            this.isEdit = (row != null);
            Text = isEdit ? "Редактирование строки расхода" : "Добавление товара в расход";
            LoadProductData();
            if (isEdit) LoadData();
        }

        private void InitializeComponent()
        {
            this.lblProduct = new Label();
            this.lblPrice = new Label();
            this.lblQuantity = new Label();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.cboProduct = new ComboBox();
            this.txtPrice = new TextBox();
            this.txtQuantity = new TextBox();
            this.SuspendLayout();

            // lblProduct
            this.lblProduct.Text = "Товар:";
            this.lblProduct.Location = new System.Drawing.Point(12, 15);
            this.lblProduct.Size = new System.Drawing.Size(60, 20);

            // cboProduct
            this.cboProduct.Location = new System.Drawing.Point(80, 12);
            this.cboProduct.Size = new System.Drawing.Size(250, 21);
            this.cboProduct.DropDownStyle = ComboBoxStyle.DropDownList;

            // lblPrice
            this.lblPrice.Text = "Цена:";
            this.lblPrice.Location = new System.Drawing.Point(12, 48);
            this.lblPrice.Size = new System.Drawing.Size(50, 20);

            // txtPrice
            this.txtPrice.Location = new System.Drawing.Point(70, 45);
            this.txtPrice.Size = new System.Drawing.Size(100, 20);

            // lblQuantity
            this.lblQuantity.Text = "Количество:";
            this.lblQuantity.Location = new System.Drawing.Point(185, 48);
            this.lblQuantity.Size = new System.Drawing.Size(70, 20);

            // txtQuantity
            this.txtQuantity.Location = new System.Drawing.Point(260, 45);
            this.txtQuantity.Size = new System.Drawing.Size(100, 20);

            // btnSave
            this.btnSave.Text = "Сохранить";
            this.btnSave.Location = new System.Drawing.Point(100, 85);
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.Click += new EventHandler(BtnSave_Click);

            // btnCancel
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Location = new System.Drawing.Point(210, 85);
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.Click += new EventHandler(BtnCancel_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(394, 140);
            this.Controls.AddRange(new Control[] { lblProduct, cboProduct, lblPrice, txtPrice,
                lblQuantity, txtQuantity, btnSave, btnCancel });
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblProduct;
        private Label lblPrice;
        private Label lblQuantity;

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
                if (editingRow["цена"] != DBNull.Value)
                    txtPrice.Text = editingRow["цена"].ToString();
                if (editingRow["количество"] != DBNull.Value)
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
                    string query = @"UPDATE ТЧ_накладная_расхода 
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
                    string query = @"INSERT INTO ТЧ_накладная_расхода (id_расхода, id_товара, цена, количество) 
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