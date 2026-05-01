using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DDM_Sclad
{
    public partial class TableForm : Form
    {
        private string tableName;
        private string selectQuery;
        private string[] editableFields;
        private DataTable dataTable;
        private BindingSource bindingSource;

        // Переменные для хранения состояния сортировки
        private string currentSortColumn = "Код";
        private string currentSortDirection = "ASC";

        public TableForm(string tableName, string selectQuery, string[] editableFields)
        {
            InitializeComponent();
            this.tableName = tableName;
            this.selectQuery = selectQuery;
            this.editableFields = editableFields;
            this.Text = $"Работа с таблицей: {tableName}";

            // Настройка обработки клавиш для всей формы
            this.KeyPreview = true;
            this.KeyDown += TableForm_KeyDown;

            // Подписываемся на событие завершения привязки данных
            this.dgvData.DataBindingComplete += dgvData_DataBindingComplete;

            LoadData();
        }

        private void InitializeComponent()
        {
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvData.Location = new System.Drawing.Point(12, 50);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(960, 450);
            this.dgvData.TabIndex = 0;
            this.dgvData.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_CellMouseDoubleClick);
            this.dgvData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_ColumnHeaderMouseClick);
            this.dgvData.SelectionChanged += new System.EventHandler(this.dgvData_SelectionChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(106, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(212, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(318, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 510);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 3;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Location = new System.Drawing.Point(12, 12);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(960, 32);
            this.panelButtons.TabIndex = 1;
            // 
            // TableForm
            // 
            this.ClientSize = new System.Drawing.Size(984, 540);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblStatus);
            this.Name = "TableForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Обработка глобальных клавиш
        private void TableForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnEdit_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                btnDelete_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Insert)
            {
                btnAdd_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                btnRefresh_Click(sender, e);
            }
        }

        // Обновление статуса при клике на строку
        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        // Дабл клик для редактирования
        private void dgvData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && dataTable != null && dataTable.Rows.Count > 0)
            {
                btnEdit_Click(sender, e);
            }
        }

        // Событие завершения привязки данных - здесь рисуем индикатор сортировки
        private void dgvData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ShowSortIcon();
        }

        private void LoadData()
        {
            string query = selectQuery;

            // Добавляем сортировку
            string realColumnName = GetRealColumnNameForSort(currentSortColumn);
            if (!string.IsNullOrEmpty(realColumnName))
            {
                query += $" ORDER BY {realColumnName} {currentSortDirection}";
            }

            dataTable = DatabaseHelper.ExecuteQuery(query);
            bindingSource = new BindingSource { DataSource = dataTable };
            dgvData.DataSource = bindingSource;

            UpdateStatus();
        }

        // Показывает индикатор сортировки (треугольник) на заголовке колонки
        private void ShowSortIcon()
        {
            if (dgvData.Columns.Contains(currentSortColumn))
            {
                foreach (DataGridViewColumn col in dgvData.Columns)
                {
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
                }

                dgvData.Columns[currentSortColumn].HeaderCell.SortGlyphDirection =
                    currentSortDirection == "ASC" ? SortOrder.Ascending : SortOrder.Descending;
            }
        }

        // Преобразует отображаемое имя колонки в реальное имя поля в БД
        private string GetRealColumnNameForSort(string displayColumnName)
        {
            if (displayColumnName == "Код")
            {
                if (tableName == "должности") return "id_должности";
                if (tableName == "контрагенты") return "id_контрагента";
                if (tableName == "склады") return "id_склада";
                if (tableName == "персонал") return "id_сотрудника";
                if (tableName == "товары") return "id_товара";
                if (tableName == "приход_товара") return "id_прихода";
                if (tableName == "расход_товара") return "id_расхода";
                return "id";
            }

            if (displayColumnName == "Должность") return "id_должности";
            if (displayColumnName == "Склад") return "id_склада";
            if (displayColumnName == "Поставщик") return "является_поставщиком";
            if (displayColumnName == "Покупатель") return "является_покупателем";

            return displayColumnName;
        }

        private void UpdateStatus()
        {
            if (bindingSource != null && dataTable != null && dataTable.Rows.Count > 0)
            {
                int currentRow = bindingSource.Position + 1;
                lblStatus.Text = $"Запись {currentRow} из {dataTable.Rows.Count} | Сортировка: {currentSortColumn} ({currentSortDirection})";
            }
            else
            {
                lblStatus.Text = "Нет записей";
            }
        }

        // Обработчик клика по заголовку колонки для сортировки
        private void dgvData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string clickedColumn = dgvData.Columns[e.ColumnIndex].Name;

            if (currentSortColumn == clickedColumn)
            {
                currentSortDirection = currentSortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                currentSortColumn = clickedColumn;
                currentSortDirection = "ASC";
            }

            int currentId = -1;
            if (bindingSource != null && bindingSource.Current != null)
            {
                DataRowView currentRow = (DataRowView)bindingSource.Current;
                currentId = Convert.ToInt32(currentRow[0]);
            }

            LoadData();

            if (currentId != -1 && dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataTable.Rows[i][0]) == currentId)
                    {
                        bindingSource.Position = i;
                        break;
                    }
                }
                UpdateStatus();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            int currentId = -1;
            if (bindingSource != null && bindingSource.Current != null)
            {
                DataRowView currentRow = (DataRowView)bindingSource.Current;
                currentId = Convert.ToInt32(currentRow[0]);
            }

            LoadData();

            if (currentId != -1 && dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataTable.Rows[i][0]) == currentId)
                    {
                        bindingSource.Position = i;
                        break;
                    }
                }
                UpdateStatus();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new EditRecordForm(tableName, editableFields, null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                string savedSortColumn = currentSortColumn;
                string savedSortDirection = currentSortDirection;

                LoadData();

                currentSortColumn = savedSortColumn;
                currentSortDirection = savedSortDirection;

                LoadData();

                if (dataTable.Rows.Count > 0)
                    bindingSource.Position = dataTable.Rows.Count - 1;
                UpdateStatus();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Нет записей для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRowView currentRowView = (DataRowView)bindingSource.Current;
            int currentId = Convert.ToInt32(currentRowView[0]);
            DataRow row = currentRowView.Row;

            var form = new EditRecordForm(tableName, editableFields, row);
            if (form.ShowDialog() == DialogResult.OK)
            {
                string savedSortColumn = currentSortColumn;
                string savedSortDirection = currentSortDirection;

                LoadData();

                currentSortColumn = savedSortColumn;
                currentSortDirection = savedSortDirection;

                LoadData();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataTable.Rows[i][0]) == currentId)
                    {
                        bindingSource.Position = i;
                        break;
                    }
                }
                UpdateStatus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataTable == null || dataTable.Rows.Count == 0) return;

            if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DataRowView currentRowView = (DataRowView)bindingSource.Current;
                    int currentId = Convert.ToInt32(currentRowView[0]);
                    int currentPosition = bindingSource.Position;

                    string idField = GetRealIdFieldName(tableName);
                    string query = $"DELETE FROM {tableName} WHERE {idField} = @id";
                    DatabaseHelper.ExecuteNonQuery(query, new[] { new NpgsqlParameter("@id", currentId) });

                    string savedSortColumn = currentSortColumn;
                    string savedSortDirection = currentSortDirection;

                    LoadData();

                    currentSortColumn = savedSortColumn;
                    currentSortDirection = savedSortDirection;

                    LoadData();

                    if (dataTable.Rows.Count > 0)
                    {
                        if (currentPosition >= dataTable.Rows.Count)
                            bindingSource.Position = dataTable.Rows.Count - 1;
                        else
                            bindingSource.Position = currentPosition;
                    }
                    UpdateStatus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetRealIdFieldName(string tableName)
        {
            if (tableName == "должности") return "id_должности";
            if (tableName == "контрагенты") return "id_контрагента";
            if (tableName == "склады") return "id_склада";
            if (tableName == "персонал") return "id_сотрудника";
            if (tableName == "товары") return "id_товара";
            if (tableName == "приход_товара") return "id_прихода";
            if (tableName == "расход_товара") return "id_расхода";
            if (tableName == "ТЧ_накладная_прихода") return "id_записи";
            if (tableName == "ТЧ_накладная_расхода") return "id_записи";

            return "id";
        }

        private DataGridView dgvData;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;
        private Label lblStatus;
        private Panel panelButtons;
    }
}