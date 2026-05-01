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
            dgvHeaders.AllowUserToAddRows = false;
            dgvDetails.AllowUserToAddRows = false;
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
            if (dgvHeaders.SelectedRows.Count > 0 && dgvHeaders.SelectedRows[0].DataBoundItem != null)
            {
                DataRowView rowView = (DataRowView)dgvHeaders.SelectedRows[0].DataBoundItem;
                if (rowView != null && rowView["Код"] != DBNull.Value)
                {
                    currentHeaderId = Convert.ToInt32(rowView["Код"]);
                    LoadDetails();
                }
            }
        }

        private void btnAddHeader_Click(object sender, EventArgs e)
        {
            var form = new ExpenseHeaderEditForm(null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnEditHeader_Click(object sender, EventArgs e)
        {
            if (dgvHeaders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите документ для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRowView rowView = (DataRowView)dgvHeaders.SelectedRows[0].DataBoundItem;
            if (rowView == null || rowView["Код"] == DBNull.Value) return;

            var form = new ExpenseHeaderEditForm(rowView.Row);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnDeleteHeader_Click(object sender, EventArgs e)
        {
            if (dgvHeaders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите документ для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRowView rowView = (DataRowView)dgvHeaders.SelectedRows[0].DataBoundItem;
            if (rowView == null || rowView["Код"] == DBNull.Value) return;

            if (MessageBox.Show("Удалить документ и все его строки?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(rowView["Код"]);
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
                MessageBox.Show("Сначала выберите документ", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var form = new ExpenseDetailEditForm(currentHeaderId, null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadDetails();
        }

        private void btnEditDetail_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRowView rowView = (DataRowView)dgvDetails.SelectedRows[0].DataBoundItem;
            if (rowView == null || rowView["Код"] == DBNull.Value) return;

            var form = new ExpenseDetailEditForm(currentHeaderId, rowView.Row);
            if (form.ShowDialog() == DialogResult.OK)
                LoadDetails();
        }

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRowView rowView = (DataRowView)dgvDetails.SelectedRows[0].DataBoundItem;
            if (rowView == null || rowView["Код"] == DBNull.Value) return;

            if (MessageBox.Show("Удалить строку?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(rowView["Код"]);
                DatabaseHelper.ExecuteNonQuery("DELETE FROM ТЧ_накладная_расхода WHERE id_записи = @id",
                    new[] { new NpgsqlParameter("@id", id) });
                LoadDetails();
            }
        }
    }
}