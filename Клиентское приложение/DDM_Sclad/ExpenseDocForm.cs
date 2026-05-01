using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace DDM_Sclad
{
    public partial class ExpenseDocForm : Form
    {
        private DataTable headerTable;
        private DataTable detailTable;
        private BindingSource headerSource;
        private BindingSource detailSource;
        private int currentHeaderId = -1;

        public ExpenseDocForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string headerQuery = @"SELECT рд.id_расхода AS Код, с.наименование AS Склад, 
                                          к.название AS Контрагент, рд.номер_накладной AS Номер,
                                          рд.дата_расхода AS Дата
                                   FROM расход_товара рд
                                   JOIN склады с ON рд.id_склада = с.id_склада
                                   JOIN контрагенты к ON рд.id_контрагента = к.id_контрагента
                                   ORDER BY рд.дата_расхода DESC";

            headerTable = DatabaseHelper.ExecuteQuery(headerQuery);
            headerSource = new BindingSource { DataSource = headerTable };
            dgvHeaders.DataSource = headerSource;
        }

        private void LoadDetails()
        {
            if (currentHeaderId == -1)
            {
                detailTable = null;
                dgvDetails.DataSource = null;
                return;
            }

            string detailQuery = @"SELECT тч.id_записи AS Код, т.название AS Товар, 
                                          тч.цена, тч.количество, тч.сумма
                                   FROM ТЧ_накладная_расхода тч
                                   JOIN товары т ON тч.id_товара = т.id_товара
                                   WHERE тч.id_расхода = @id";

            detailTable = DatabaseHelper.ExecuteQuery(detailQuery,
                new[] { new NpgsqlParameter("@id", currentHeaderId) });
            detailSource = new BindingSource { DataSource = detailTable };
            dgvDetails.DataSource = detailSource;
        }

        private void dgvHeaders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHeaders.SelectedRows.Count > 0)
            {
                currentHeaderId = Convert.ToInt32(dgvHeaders.SelectedRows[0].Cells["Код"].Value);
                LoadDetails();
            }
        }

        private void btnAddHeader_Click(object sender, EventArgs e)
        {
            // Создайте форму редактирования шапки расхода
            MessageBox.Show("Функция добавления документа расхода");
        }

        private void btnEditHeader_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функция редактирования документа расхода");
        }

        private void btnDeleteHeader_Click(object sender, EventArgs e)
        {
            if (dgvHeaders.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Удалить документ и все его строки?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvHeaders.SelectedRows[0].Cells["Код"].Value);
                DatabaseHelper.ExecuteNonQuery("DELETE FROM расход_товара WHERE id_расхода = @id",
                    new[] { new NpgsqlParameter("@id", id) });
                LoadData();
                if (currentHeaderId == id) currentHeaderId = -1;
                LoadDetails();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            if (currentHeaderId != -1) LoadDetails();
        }
        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            if (currentHeaderId == -1)
            {
                MessageBox.Show("Сначала выберите документ");
                return;
            }
            MessageBox.Show("Функция добавления строки расхода");
        }

        private void btnEditDetail_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count == 0) return;
            MessageBox.Show("Функция редактирования строки расхода");
        }

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Удалить строку?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvDetails.SelectedRows[0].Cells["Код"].Value);
                DatabaseHelper.ExecuteNonQuery("DELETE FROM ТЧ_накладная_расхода WHERE id_записи = @id",
                    new[] { new NpgsqlParameter("@id", id) });
                LoadDetails();
            }
        }
    }
}