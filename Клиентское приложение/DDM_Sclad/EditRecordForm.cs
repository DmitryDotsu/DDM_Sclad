using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DDM_Sclad
{
    public partial class EditRecordForm : Form
    {
        private string tableName;
        private string[] fields;
        private DataRow editingRow;
        private Control[] inputControls;
        private Label[] lblFields;
        private bool isEdit;

        public EditRecordForm(string tableName, string[] fields, DataRow row)
        {
            InitializeComponent();
            this.tableName = tableName;
            this.fields = fields;
            this.editingRow = row;
            this.isEdit = (row != null);
            this.Text = isEdit ? $"Редактирование {tableName}" : $"Добавление в {tableName}";
            CreateFields();
            if (isEdit) LoadData();
        }

        private void InitializeComponent()
        {
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelFields = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Text = "Сохранить";
            this.btnSave.Location = new System.Drawing.Point(100, 300);
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Location = new System.Drawing.Point(220, 300);
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            // 
            // panelFields
            // 
            this.panelFields.AutoScroll = true;
            this.panelFields.Location = new System.Drawing.Point(12, 12);
            this.panelFields.Size = new System.Drawing.Size(400, 280);
            // 
            // EditRecordForm
            // 
            this.ClientSize = new System.Drawing.Size(434, 350);
            this.Controls.Add(this.panelFields);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }

        private void CreateFields()
        {
            inputControls = new Control[fields.Length];
            lblFields = new Label[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                lblFields[i] = new Label
                {
                    Text = fields[i] + ":",
                    Location = new System.Drawing.Point(10, 10 + i * 35),
                    Size = new System.Drawing.Size(120, 20)
                };

                string fieldName = fields[i];
                bool isBoolean = false;

                // Проверка по имени поля (для русских названий из запроса)
                if (fieldName == "Поставщик" || fieldName == "Покупатель"
                    || fieldName == "является_поставщиком" || fieldName == "является_покупателем")
                {
                    isBoolean = true;
                }

                if (isBoolean)
                {
                    CheckBox chkBox = new CheckBox
                    {
                        Location = new System.Drawing.Point(140, 7 + i * 35),
                        Size = new System.Drawing.Size(100, 20),
                        Checked = false,
                        Text = ""
                    };
                    inputControls[i] = chkBox;
                }
                else
                {
                    TextBox txtBox = new TextBox
                    {
                        Location = new System.Drawing.Point(140, 7 + i * 35),
                        Size = new System.Drawing.Size(200, 20)
                    };
                    inputControls[i] = txtBox;
                }

                panelFields.Controls.Add(lblFields[i]);
                panelFields.Controls.Add(inputControls[i]);
            }
        }

        private void LoadData()
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (editingRow.Table.Columns.Contains(fields[i]) && editingRow[fields[i]] != DBNull.Value)
                {
                    if (inputControls[i] is CheckBox chkBox)
                    {
                        chkBox.Checked = Convert.ToBoolean(editingRow[fields[i]]);
                    }
                    else if (inputControls[i] is TextBox txtBox)
                    {
                        txtBox.Text = editingRow[fields[i]].ToString();
                    }
                }
                else
                {
                    // Если значение NULL - очищаем поле
                    if (inputControls[i] is TextBox txtBox)
                    {
                        txtBox.Text = "";
                    }
                    else if (inputControls[i] is CheckBox chkBox)
                    {
                        chkBox.Checked = false;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isEdit)
                {
                    string idField = "Код";
                    string setClause = string.Join(", ",
                        fields.Select(f => $"{GetRealFieldName(f)} = @{f.Replace(" ", "_")}"));
                    string query = $"UPDATE {tableName} SET {setClause} WHERE {GetRealFieldName(idField)} = @id";

                    var parameters = new NpgsqlParameter[fields.Length + 1];
                    for (int i = 0; i < fields.Length; i++)
                    {
                        object value = GetParameterValue(fields[i], inputControls[i]);
                        parameters[i] = new NpgsqlParameter($"@{fields[i].Replace(" ", "_")}", value ?? DBNull.Value);
                    }
                    parameters[fields.Length] = new NpgsqlParameter("@id", editingRow[0]);

                    DatabaseHelper.ExecuteNonQuery(query, parameters);
                }
                else
                {
                    string fieldsList = string.Join(", ", fields.Select(f => GetRealFieldName(f)));
                    string valuesList = string.Join(", ", fields.Select(f => $"@{f.Replace(" ", "_")}"));
                    string query = $"INSERT INTO {tableName} ({fieldsList}) VALUES ({valuesList})";

                    var parameters = new NpgsqlParameter[fields.Length];
                    for (int i = 0; i < fields.Length; i++)
                    {
                        object value = GetParameterValue(fields[i], inputControls[i]);
                        parameters[i] = new NpgsqlParameter($"@{fields[i].Replace(" ", "_")}", value ?? DBNull.Value);
                    }

                    DatabaseHelper.ExecuteNonQuery(query, parameters);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Npgsql.PostgresException ex)
            {
                string userMessage = GetUserFriendlyErrorMessage(ex);
                MessageBox.Show(userMessage, "Ошибка ввода данных",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private object GetParameterValue(string fieldName, Control control)
        {
            object value;

            if (control is CheckBox chkBox)
            {
                value = chkBox.Checked;
            }
            else if (control is TextBox txtBox)
            {
                string textValue = txtBox.Text;

                // Пустая строка = NULL
                if (string.IsNullOrWhiteSpace(textValue))
                    return DBNull.Value;

                // Определяем тип поля по имени
                if (fieldName == "оклад" || fieldName == "цена" || fieldName == "сумма")
                {
                    // Числовые поля
                    if (decimal.TryParse(textValue, out decimal decimalValue))
                        value = decimalValue;
                    else
                        throw new Exception($"Поле '{fieldName}' должно содержать число");
                }
                else if (fieldName == "количество")
                {
                    // Целочисленные поля
                    if (int.TryParse(textValue, out int intValue))
                        value = intValue;
                    else
                        throw new Exception($"Поле '{fieldName}' должно содержать целое число");
                }
                else
                {
                    // Текстовые поля
                    value = textValue;
                }
            }
            else
            {
                value = "";
            }

            return value;
        }
        private string GetUserFriendlyErrorMessage(Npgsql.PostgresException ex)
        {
            if (ex.ConstraintName == "контрагенты_ийн_check")
            {
                return "Некорректный формат ИНН.\n\n" +
                       "ИНН должен содержать:\n" +
                       "• 10 цифр для юридических лиц\n" +
                       "• 12 цифр для физических лиц\n\n" +
                       "Пожалуйста, проверьте введённое значение.";
            }

            if (ex.ConstraintName == "контрагенты_инн_key")
            {
                return "Контрагент с таким ИНН уже существует.\n\n" +
                       "Пожалуйста, введите другой ИНН.";
            }

            return $"Ошибка базы данных:\n\n{ex.MessageText}\n\n" +
                   $"Пожалуйста, проверьте введённые данные и попробуйте снова.";
        }
        // Метод для перевода отображаемых имен в реальные имена колонок БД
        private string GetRealFieldName(string fieldName)
        {
            if (fieldName == "Поставщик")
                return "является_поставщиком";
            if (fieldName == "Покупатель")
                return "является_покупателем";
            if (fieldName == "Код")
                return GetIdFieldName(tableName);
            if (fieldName == "Склад")
                return "id_склада";
            if (fieldName == "Должность")
                return "id_должности";
            if (fieldName == "название" || fieldName == "Название")
                return "название";
            if (fieldName == "оклад" || fieldName == "Оклад")
                return "оклад";
            if (fieldName == "обязанности" || fieldName == "Обязанности")
                return "обязанности";
            if (fieldName == "инн" || fieldName == "Инн")
                return "инн";
            if (fieldName == "телефон" || fieldName == "Телефон")
                return "телефон";
            if (fieldName == "email" || fieldName == "Email")
                return "email";
            if (fieldName == "адрес" || fieldName == "Адрес")
                return "адрес";
            if (fieldName == "фио")
                return "фио";
            if (fieldName == "дата_приема")
                return "дата_приема";

            return fieldName;
        }

        // Вспомогательный метод для получения ID поля
        private string GetIdFieldName(string tableName)
        {
            if (tableName == "должности")
                return "id_должности";
            if (tableName == "контрагенты")
                return "id_контрагента";
            if (tableName == "склады")
                return "id_склада";
            if (tableName == "персонал")
                return "id_сотрудника";
            if (tableName == "товары")
                return "id_товара";
            return "id";
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