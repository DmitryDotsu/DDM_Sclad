using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace DDM_Sclad
{
    public partial class IncomeDocForm : Form
    {
        private DataTable headerTable;
        private DataTable detailTable;
        private BindingSource headerSource;
        private BindingSource detailSource;
        private int currentHeaderId = -1;

        public IncomeDocForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string headerQuery = @"SELECT пп.id_прихода AS Код, с.наименование AS Склад, 
                                          к.название AS Контрагент, пп.номер_накладной AS Номер,
                                          пп.дата_прихода AS Дата
                                   FROM приход_товара пп
                                   JOIN склады с ON пп.id_склада = с.id_склада
                                   JOIN контрагенты к ON пп.id_контрагента = к.id_контрагента
                                   ORDER BY пп.дата_прихода DESC";

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
                                   FROM ТЧ_накладная_прихода тч
                                   JOIN товары т ON тч.id_товара = т.id_товара
                                   WHERE тч.id_прихода = @id";

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
            var form = new IncomeHeaderEditForm(null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnEditHeader_Click(object sender, EventArgs e)
        {
            if (dgvHeaders.SelectedRows.Count == 0) return;
            DataRowView rowView = (DataRowView)headerSource.Current;
            var form = new IncomeHeaderEditForm(rowView.Row);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnDeleteHeader_Click(object sender, EventArgs e)
        {
            if (dgvHeaders.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Удалить документ и все его строки?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvHeaders.SelectedRows[0].Cells["Код"].Value);
                DatabaseHelper.ExecuteNonQuery("DELETE FROM приход_товара WHERE id_прихода = @id",
                    new[] { new NpgsqlParameter("@id", id) });
                LoadData();
                if (currentHeaderId == id) currentHeaderId = -1;
                LoadDetails();
            }
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            if (currentHeaderId == -1)
            {
                MessageBox.Show("Сначала выберите документ");
                return;
            }
            var form = new IncomeDetailEditForm(currentHeaderId, null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadDetails();
        }

        private void btnEditDetail_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count == 0) return;
            DataRowView rowView = (DataRowView)detailSource.Current;
            var form = new IncomeDetailEditForm(currentHeaderId, rowView.Row);
            if (form.ShowDialog() == DialogResult.OK)
                LoadDetails();
        }

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Удалить строку?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvDetails.SelectedRows[0].Cells["Код"].Value);
                DatabaseHelper.ExecuteNonQuery("DELETE FROM ТЧ_накладная_прихода WHERE id_записи = @id",
                    new[] { new NpgsqlParameter("@id", id) });
                LoadDetails();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            if (currentHeaderId != -1) LoadDetails();
        }
    }
}