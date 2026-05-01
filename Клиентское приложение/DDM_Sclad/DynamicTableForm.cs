using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DDM_Sclad
{
    public partial class DynamicTableForm : Form
    {
        private string tableName;
        private string primaryKey;
        private DataTable tableSchema;
        private DataTable dataTable;
        private BindingSource bindingSource;
        private string currentSortColumn = "";
        private string currentSortDirection = "ASC";

        public DynamicTableForm(string tableName)
        {
            InitializeComponent();
            this.tableName = tableName;
            this.Text = $"Работа с таблицей: {tableName}";

            // Загружаем структуру таблицы
            LoadSchema();

            // Настройка клавиш
            this.KeyPreview = true;
            this.KeyDown += DynamicTableForm_KeyDown;

            // Загружаем данные
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

            // dgvData
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
            this.dgvData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvData_DataBindingComplete);

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.Text = "Добавить";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // btnEdit
            this.btnEdit.Location = new System.Drawing.Point(106, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(212, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(318, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // lblStatus
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 510);

            // panelButtons
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Location = new System.Drawing.Point(12, 12);
            this.panelButtons.Size = new System.Drawing.Size(960, 32);

            // DynamicTableForm
            this.ClientSize = new System.Drawing.Size(984, 540);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblStatus);
            this.Name = "DynamicTableForm";

            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadSchema()
        {
            tableSchema = DatabaseHelper.GetTableSchema(tableName);
            primaryKey = DatabaseHelper.GetPrimaryKey(tableName);

            // Устанавливаем сортировку по первичному ключу по умолчанию
            if (string.IsNullOrEmpty(currentSortColumn))
                currentSortColumn = primaryKey;
        }

        private void LoadData()
        {
            dataTable = DatabaseHelper.GetAllData(tableName, currentSortColumn, currentSortDirection);
            bindingSource = new BindingSource { DataSource = dataTable };
            dgvData.DataSource = bindingSource;

            // Настраиваем заголовки колонок
            foreach (DataGridViewColumn col in dgvData.Columns)
            {
                string columnName = col.Name;
                col.HeaderText = GetRussianHeaderName(columnName);
            }

            UpdateStatus();
        }

        private string GetRussianHeaderName(string columnName)
        {
            var names = new System.Collections.Generic.Dictionary<string, string>
            {
                { "id_должности", "Код" },
                { "название", "Название" },
                { "оклад", "Оклад" },
                { "обязанности", "Обязанности" },
                { "id_склада", "Код склада" },
                { "наименование", "Наименование" },
                { "адрес", "Адрес" },
                { "завсклад", "Зав. складом" },
                { "фио", "ФИО" },
                { "инн", "ИНН" },
                { "телефон", "Телефон" },
                { "дата_приема", "Дата приема" },
                { "id_сотрудника", "Код" },
                { "id_товара", "Код" },
                { "категория", "Категория" },
                { "ед_измерения", "Ед. измерения" },
                { "id_контрагента", "Код" },
                { "email", "Email" },
                { "является_поставщиком", "Поставщик" },
                { "является_покупателем", "Покупатель" },
                { "цена", "Цена" },
                { "количество", "Количество" },
                { "сумма", "Сумма" },
                { "номер_накладной", "Номер накладной" },
                { "дата_прихода", "Дата прихода" },
                { "дата_расхода", "Дата расхода" }
            };

            return names.ContainsKey(columnName) ? names[columnName] : columnName;
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

        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void dgvData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && dataTable != null && dataTable.Rows.Count > 0)
            {
                btnEdit_Click(sender, e);
            }
        }

        private void dgvData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Показываем индикатор сортировки
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
                currentId = Convert.ToInt32(currentRow[primaryKey]);
            }

            LoadData();

            if (currentId != -1 && dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataTable.Rows[i][primaryKey]) == currentId)
                    {
                        bindingSource.Position = i;
                        break;
                    }
                }
                UpdateStatus();
            }
        }

        private void DynamicTableForm_KeyDown(object sender, KeyEventArgs e)
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            int currentId = -1;
            if (bindingSource != null && bindingSource.Current != null)
            {
                DataRowView currentRow = (DataRowView)bindingSource.Current;
                currentId = Convert.ToInt32(currentRow[primaryKey]);
            }

            LoadData();

            if (currentId != -1 && dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataTable.Rows[i][primaryKey]) == currentId)
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
            var form = new DynamicEditRecordForm(tableName, null);
            if (form.ShowDialog() == DialogResult.OK)
            {
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
            DataRow row = currentRowView.Row;

            var form = new DynamicEditRecordForm(tableName, row);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();

                // Восстанавливаем позицию
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataTable.Rows[i][primaryKey]) == Convert.ToInt32(row[primaryKey]))
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
                    int currentId = Convert.ToInt32(currentRowView[primaryKey]);
                    int currentPosition = bindingSource.Position;

                    string query = $"DELETE FROM {tableName} WHERE {primaryKey} = @id";
                    DatabaseHelper.ExecuteNonQuery(query, new[] { new NpgsqlParameter("@id", currentId) });

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

        private DataGridView dgvData;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;
        private Label lblStatus;
        private Panel panelButtons;
    }
}