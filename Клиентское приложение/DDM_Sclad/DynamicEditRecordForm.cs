using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace DDM_Sclad
{
    public partial class DynamicEditRecordForm : Form
    {
        private string tableName;
        private DataRow editingRow;
        private bool isEdit;
        private DataTable tableSchema;
        private string primaryKey;
        private Control[] inputControls;
        private Label[] lblFields;
        private string[] fields;

        public DynamicEditRecordForm(string tableName, DataRow row)
        {
            InitializeComponent();
            this.tableName = tableName;
            this.editingRow = row;
            this.isEdit = (row != null);
            this.Text = isEdit ? $"Редактирование {tableName}" : $"Добавление в {tableName}";

            LoadSchemaAndCreateFields();
        }

        private void InitializeComponent()
        {
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelFields = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            this.btnSave.Text = "Сохранить";
            this.btnSave.Location = new System.Drawing.Point(100, 300);
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.Text = "Отмена";
            this.btnCancel.Location = new System.Drawing.Point(220, 300);
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.panelFields.AutoScroll = true;
            this.panelFields.Location = new System.Drawing.Point(12, 12);
            this.panelFields.Size = new System.Drawing.Size(400, 280);

            this.ClientSize = new System.Drawing.Size(434, 350);
            this.Controls.Add(this.panelFields);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }

        private void LoadSchemaAndCreateFields()
        {
            tableSchema = DatabaseHelper.GetTableSchema(tableName);
            primaryKey = DatabaseHelper.GetPrimaryKey(tableName);

            var fieldList = new System.Collections.Generic.List<string>();

            foreach (DataRow col in tableSchema.Rows)
            {
                string columnName = col["column_name"].ToString();
                string dataType = col["data_type"].ToString();

                // Пропускаем первичный ключ
                if (columnName == primaryKey)
                    continue;

                fieldList.Add(columnName);
            }

            fields = fieldList.ToArray();
            inputControls = new Control[fields.Length];
            lblFields = new Label[fields.Length];

            int rowIndex = 0;

            foreach (DataRow col in tableSchema.Rows)
            {
                string columnName = col["column_name"].ToString();
                string dataType = col["data_type"].ToString();

                if (columnName == primaryKey)
                    continue;

                string headerText = GetRussianHeaderName(columnName);

                lblFields[rowIndex] = new Label
                {
                    Text = headerText + ":",
                    Location = new System.Drawing.Point(10, 10 + rowIndex * 35),
                    Size = new System.Drawing.Size(120, 20)
                };

                // Проверяем, является ли поле внешним ключом
                if (IsForeignKey(columnName))
                {
                    // Создаём ComboBox для выбора из справочника
                    ComboBox cbo = new ComboBox
                    {
                        Location = new System.Drawing.Point(140, 7 + rowIndex * 35),
                        Size = new System.Drawing.Size(200, 21),
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };

                    // Загружаем данные для справочника
                    DataTable lookupData = DatabaseHelper.GetLookupData(columnName);
                    cbo.DataSource = lookupData;
                    cbo.DisplayMember = lookupData.Columns[1].ColumnName; // наименование
                    cbo.ValueMember = lookupData.Columns[0].ColumnName;   // id
                    cbo.SelectedIndex = -1;

                    inputControls[rowIndex] = cbo;
                }
                else if (dataType == "boolean")
                {
                    CheckBox chkBox = new CheckBox
                    {
                        Location = new System.Drawing.Point(140, 7 + rowIndex * 35),
                        Size = new System.Drawing.Size(100, 20),
                        Checked = false
                    };
                    inputControls[rowIndex] = chkBox;
                }
                else if (dataType == "numeric" || dataType == "integer" || dataType == "real" || dataType == "double precision")
                {
                    NumericUpDown numBox = new NumericUpDown
                    {
                        Location = new System.Drawing.Point(140, 7 + rowIndex * 35),
                        Size = new System.Drawing.Size(150, 20),
                        Minimum = decimal.MinValue,
                        Maximum = decimal.MaxValue,
                        ThousandsSeparator = true
                    };

                    if (dataType == "integer")
                        numBox.DecimalPlaces = 0;
                    else
                        numBox.DecimalPlaces = 2;

                    inputControls[rowIndex] = numBox;
                }
                else if (dataType == "date")
                {
                    DateTimePicker dtPicker = new DateTimePicker
                    {
                        Location = new System.Drawing.Point(140, 7 + rowIndex * 35),
                        Size = new System.Drawing.Size(150, 20),
                        Format = DateTimePickerFormat.Short
                    };
                    inputControls[rowIndex] = dtPicker;
                }
                else
                {
                    TextBox txtBox = new TextBox
                    {
                        Location = new System.Drawing.Point(140, 7 + rowIndex * 35),
                        Size = new System.Drawing.Size(200, 20)
                    };
                    inputControls[rowIndex] = txtBox;
                }

                panelFields.Controls.Add(lblFields[rowIndex]);
                panelFields.Controls.Add(inputControls[rowIndex]);

                rowIndex++;
            }

            if (isEdit)
                LoadData();
        }

        // Проверка, является ли поле внешним ключом
        private bool IsForeignKey(string columnName)
        {
            // Список полей, которые являются внешними ключами
            string[] foreignKeys = { "id_должности", "id_склада", "id_товара", "id_контрагента", "id_прихода", "id_расхода", "id_завсклада" };

            foreach (string fk in foreignKeys)
            {
                if (columnName == fk)
                    return true;
            }
            return false;
        }

        private string GetRussianHeaderName(string columnName)
        {
            var names = new System.Collections.Generic.Dictionary<string, string>
            {
                { "название", "Название" },
                { "оклад", "Оклад" },
                { "обязанности", "Обязанности" },
                { "наименование", "Наименование" },
                { "адрес", "Адрес" },
                { "завсклад", "Зав. складом" },
                { "фио", "ФИО" },
                { "инн", "ИНН" },
                { "телефон", "Телефон" },
                { "дата_приема", "Дата приема" },
                { "категория", "Категория" },
                { "ед_измерения", "Ед. измерения" },
                { "email", "Email" },
                { "является_поставщиком", "Поставщик" },
                { "является_покупателем", "Покупатель" },
                { "цена", "Цена" },
                { "количество", "Количество" },
                { "номер_накладной", "Номер накладной" },
                { "дата_прихода", "Дата прихода" },
                { "дата_расхода", "Дата расхода" }
            };

            return names.ContainsKey(columnName) ? names[columnName] : columnName;
        }

        private void LoadData()
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (editingRow.Table.Columns.Contains(fields[i]) && editingRow[fields[i]] != DBNull.Value)
                {
                    object value = editingRow[fields[i]];

                    if (inputControls[i] is CheckBox chkBox)
                    {
                        chkBox.Checked = Convert.ToBoolean(value);
                    }
                    else if (inputControls[i] is NumericUpDown numBox)
                    {
                        numBox.Value = Convert.ToDecimal(value);
                    }
                    else if (inputControls[i] is DateTimePicker dtPicker)
                    {
                        dtPicker.Value = Convert.ToDateTime(value);
                    }
                    else if (inputControls[i] is ComboBox cbo)
                    {
                        // Устанавливаем выбранное значение в ComboBox
                        cbo.SelectedValue = value;
                    }
                    else if (inputControls[i] is TextBox txtBox)
                    {
                        txtBox.Text = value.ToString();
                    }
                }
            }
        }

        private object GetParameterValue(Control control, string fieldName)
        {
            if (control is CheckBox chkBox)
                return chkBox.Checked;

            if (control is NumericUpDown numBox)
                return numBox.Value;

            if (control is DateTimePicker dtPicker)
                return dtPicker.Value;

            if (control is ComboBox cbo)
            {
                if (cbo.SelectedValue != null && cbo.SelectedValue is int)
                    return cbo.SelectedValue;
                return DBNull.Value;
            }

            if (control is TextBox txtBox)
            {
                if (string.IsNullOrWhiteSpace(txtBox.Text))
                    return DBNull.Value;
                return txtBox.Text;
            }

            return DBNull.Value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isEdit)
                {
                    var setClauses = new System.Collections.Generic.List<string>();
                    var parameters = new System.Collections.Generic.List<NpgsqlParameter>();

                    for (int i = 0; i < fields.Length; i++)
                    {
                        setClauses.Add($"{fields[i]} = @p{i}");
                        var value = GetParameterValue(inputControls[i], fields[i]);
                        parameters.Add(new NpgsqlParameter($"@p{i}", value ?? DBNull.Value));
                    }

                    string query = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE {primaryKey} = @id";
                    parameters.Add(new NpgsqlParameter("@id", editingRow[primaryKey]));

                    DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());
                }
                else
                {
                    var fieldNames = new System.Collections.Generic.List<string>();
                    var paramNames = new System.Collections.Generic.List<string>();
                    var parameters = new System.Collections.Generic.List<NpgsqlParameter>();

                    for (int i = 0; i < fields.Length; i++)
                    {
                        fieldNames.Add(fields[i]);
                        paramNames.Add($"@p{i}");
                        var value = GetParameterValue(inputControls[i], fields[i]);
                        parameters.Add(new NpgsqlParameter($"@p{i}", value ?? DBNull.Value));
                    }

                    string query = $"INSERT INTO {tableName} ({string.Join(", ", fieldNames)}) VALUES ({string.Join(", ", paramNames)})";
                    DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Npgsql.PostgresException ex)
            {
                MessageBox.Show($"Ошибка базы данных: {ex.MessageText}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private Button btnSave, btnCancel;
        private Panel panelFields;
    }
}