using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace DDM_Sclad
{
    public partial class ReportForm : Form
    {
        private string reportType;
        private string reportTitle;

        public ReportForm(string title, string type)
        {
            InitializeComponent();
            this.reportTitle = title;
            this.reportType = type;
            this.Text = title;
            SetupFilterControls();
        }

        private void InitializeComponent()
        {
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.panelFilters = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvReport
            // 
            this.dgvReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReport.Location = new System.Drawing.Point(12, 100);
            this.dgvReport.Size = new System.Drawing.Size(960, 400);
            this.dgvReport.ReadOnly = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Text = "Сформировать отчет";
            this.btnGenerate.Size = new System.Drawing.Size(150, 30);
            this.btnGenerate.Location = new System.Drawing.Point(12, 60);
            this.btnGenerate.Click += new EventHandler(this.btnGenerate_Click);
            // 
            // panelFilters
            // 
            this.panelFilters.Location = new System.Drawing.Point(12, 12);
            this.panelFilters.Size = new System.Drawing.Size(960, 45);
            // 
            // ReportForm
            // 
            this.ClientSize = new System.Drawing.Size(984, 540);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.panelFilters);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
        }

        private void SetupFilterControls()
        {
            if (reportType == "stock")
            {
                // Остатки на складах - фильтр по складу
                var lbl = new Label { Text = "Склад:", Location = new System.Drawing.Point(5, 10), Size = new System.Drawing.Size(50, 20) };
                var cbo = new ComboBox { Location = new System.Drawing.Point(60, 8), Size = new System.Drawing.Size(150, 21), DropDownStyle = ComboBoxStyle.DropDownList };
                var dt = DatabaseHelper.ExecuteQuery("SELECT id_склада, наименование FROM склады");
                cbo.DisplayMember = "наименование";
                cbo.ValueMember = "id_склада";
                cbo.DataSource = dt;
                cbo.SelectedIndex = -1;
                panelFilters.Controls.Add(lbl);
                panelFilters.Controls.Add(cbo);

                var lblSort = new Label { Text = "Сортировка:", Location = new System.Drawing.Point(230, 10), Size = new System.Drawing.Size(70, 20) };
                var cboSort = new ComboBox { Location = new System.Drawing.Point(305, 8), Size = new System.Drawing.Size(120, 21), DropDownStyle = ComboBoxStyle.DropDownList };
                cboSort.Items.AddRange(new[] { "По товару", "По остатку (возр.)", "По остатку (уб.)" });
                cboSort.SelectedIndex = 0;
                panelFilters.Controls.Add(lblSort);
                panelFilters.Controls.Add(cboSort);

                btnGenerate.Tag = new { cbo, cboSort };
            }
            else if (reportType == "profit")
            {
                // Прибыль от реализации - фильтр по датам
                var lblFrom = new Label { Text = "С даты:", Location = new System.Drawing.Point(5, 10), Size = new System.Drawing.Size(60, 20) };
                var dtFrom = new DateTimePicker { Location = new System.Drawing.Point(70, 8), Size = new System.Drawing.Size(120, 21), Format = DateTimePickerFormat.Short };
                dtFrom.Value = DateTime.Now.AddMonths(-1);

                var lblTo = new Label { Text = "По дату:", Location = new System.Drawing.Point(200, 10), Size = new System.Drawing.Size(60, 20) };
                var dtTo = new DateTimePicker { Location = new System.Drawing.Point(265, 8), Size = new System.Drawing.Size(120, 21), Format = DateTimePickerFormat.Short };
                dtTo.Value = DateTime.Now;

                var lblSort = new Label { Text = "Сортировка:", Location = new System.Drawing.Point(400, 10), Size = new System.Drawing.Size(70, 20) };
                var cboSort = new ComboBox { Location = new System.Drawing.Point(475, 8), Size = new System.Drawing.Size(150, 21), DropDownStyle = ComboBoxStyle.DropDownList };
                cboSort.Items.AddRange(new[] { "По дате", "По прибыли (возр.)", "По прибыли (уб.)" });
                cboSort.SelectedIndex = 0;

                panelFilters.Controls.Add(lblFrom);
                panelFilters.Controls.Add(dtFrom);
                panelFilters.Controls.Add(lblTo);
                panelFilters.Controls.Add(dtTo);
                panelFilters.Controls.Add(lblSort);
                panelFilters.Controls.Add(cboSort);

                btnGenerate.Tag = new { dtFrom, dtTo, cboSort };
            }
            else if (reportType == "movement")
            {
                // Движение товаров - фильтр по товару и датам
                var lblProduct = new Label { Text = "Товар:", Location = new System.Drawing.Point(5, 10), Size = new System.Drawing.Size(50, 20) };
                var cboProduct = new ComboBox { Location = new System.Drawing.Point(60, 8), Size = new System.Drawing.Size(150, 21), DropDownStyle = ComboBoxStyle.DropDownList };
                var dt = DatabaseHelper.ExecuteQuery("SELECT id_товара, название FROM товары ORDER BY название");
                cboProduct.DisplayMember = "название";
                cboProduct.ValueMember = "id_товара";
                cboProduct.DataSource = dt;
                cboProduct.SelectedIndex = -1;

                var lblFrom = new Label { Text = "С даты:", Location = new System.Drawing.Point(230, 10), Size = new System.Drawing.Size(60, 20) };
                var dtFrom = new DateTimePicker { Location = new System.Drawing.Point(295, 8), Size = new System.Drawing.Size(100, 21), Format = DateTimePickerFormat.Short };
                dtFrom.Value = DateTime.Now.AddMonths(-3);

                var lblTo = new Label { Text = "По дату:", Location = new System.Drawing.Point(410, 10), Size = new System.Drawing.Size(60, 20) };
                var dtTo = new DateTimePicker { Location = new System.Drawing.Point(475, 8), Size = new System.Drawing.Size(100, 21), Format = DateTimePickerFormat.Short };
                dtTo.Value = DateTime.Now;

                panelFilters.Controls.Add(lblProduct);
                panelFilters.Controls.Add(cboProduct);
                panelFilters.Controls.Add(lblFrom);
                panelFilters.Controls.Add(dtFrom);
                panelFilters.Controls.Add(lblTo);
                panelFilters.Controls.Add(dtTo);

                btnGenerate.Tag = new { cboProduct, dtFrom, dtTo };
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "";

                if (reportType == "stock")
                {
                    var controls = btnGenerate.Tag;
                    var warehouseId = ((dynamic)controls).cbo.SelectedValue;
                    var sortType = ((dynamic)controls).cboSort.SelectedIndex;

                    query = @"SELECT т.название AS Товар, т.категория AS Категория,
                                     COALESCE(SUM(п.количество), 0) - COALESCE(SUM(р.количество), 0) AS Остаток,
                                     COALESCE(SUM(п.сумма), 0) - COALESCE(SUM(р.сумма), 0) AS Стоимость_остатка
                              FROM товары т
                              LEFT JOIN ТЧ_накладная_прихода п ON т.id_товара = п.id_товара
                              LEFT JOIN приход_товара пдок ON п.id_прихода = пдок.id_прихода
                              LEFT JOIN ТЧ_накладная_расхода р ON т.id_товара = р.id_товара
                              LEFT JOIN расход_товара рдок ON р.id_расхода = рдок.id_расхода";

                    if (warehouseId != null && Convert.ToInt32(warehouseId) > 0)
                    {
                        query += " WHERE (пдок.id_склада = @warehouse OR рдок.id_склада = @warehouse)";
                    }

                    query += " GROUP BY т.id_товара, т.название, т.категория HAVING COALESCE(SUM(п.количество), 0) - COALESCE(SUM(р.количество), 0) > 0";

                    if (sortType == 1) query += " ORDER BY Остаток ASC";
                    else if (sortType == 2) query += " ORDER BY Остаток DESC";
                    else query += " ORDER BY Товар";

                    var parameters = new System.Collections.Generic.List<NpgsqlParameter>();
                    if (warehouseId != null && Convert.ToInt32(warehouseId) > 0)
                        parameters.Add(new NpgsqlParameter("@warehouse", warehouseId));

                    var dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());

                    // Добавляем итоговую строку
                    decimal totalStock = 0, totalValue = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        totalStock += Convert.ToDecimal(row["Остаток"]);
                        totalValue += Convert.ToDecimal(row["Стоимость_остатка"]);
                    }
                    dt.Rows.Add(new object[] { "ИТОГО:", "", totalStock, totalValue });

                    dgvReport.DataSource = dt;
                }
                else if (reportType == "profit")
                {
                    var controls = btnGenerate.Tag;
                    var dateFrom = ((dynamic)controls).dtFrom.Value;
                    var dateTo = ((dynamic)controls).dtTo.Value;
                    var sortType = ((dynamic)controls).cboSort.SelectedIndex;

                    query = @"
                        SELECT т.название AS Товар,
                               SUM(тчр.количество) AS Количество_проданных,
                               SUM(тчр.сумма) AS Выручка,
                               SUM(тчп.сумма) / NULLIF(SUM(тчп.количество), 0) AS Средняя_цена_закупа,
                               SUM(тчр.сумма) - (SUM(тчр.количество) * (SUM(тчп.сумма) / NULLIF(SUM(тчп.количество), 0))) AS Прибыль
                        FROM товары т
                        JOIN ТЧ_накладная_расхода тчр ON т.id_товара = тчр.id_товара
                        JOIN расход_товара рд on тчр.id_расхода = рд.id_расхода
                        LEFT JOIN ТЧ_накладная_прихода тчп ON т.id_товара = тчп.id_товара
                        LEFT JOIN приход_товара пд ON тчп.id_прихода = пд.id_прихода
                        WHERE рд.дата_расхода BETWEEN @dateFrom AND @dateTo
                        GROUP BY т.id_товара, т.название";

                    if (sortType == 1) query += " ORDER BY Прибыль ASC";
                    else if (sortType == 2) query += " ORDER BY Прибыль DESC";
                    else query += " ORDER BY Товар";

                    var dt = DatabaseHelper.ExecuteQuery(query, new[] {
                        new NpgsqlParameter("@dateFrom", dateFrom),
                        new NpgsqlParameter("@dateTo", dateTo)
                    });

                    // Добавляем итоговые данные
                    decimal totalRevenue = 0, totalProfit = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        totalRevenue += Convert.ToDecimal(row["Выручка"]);
                        totalProfit += Convert.ToDecimal(row["Прибыль"]);
                    }
                    dt.Rows.Add(new object[] { "ИТОГО:", "", totalRevenue, "", totalProfit });

                    dgvReport.DataSource = dt;
                }
                else if (reportType == "movement")
                {
                    var controls = btnGenerate.Tag;
                    var productId = ((dynamic)controls).cboProduct.SelectedValue;
                    var dateFrom = ((dynamic)controls).dtFrom.Value;
                    var dateTo = ((dynamic)controls).dtTo.Value;

                    query = @"
                        SELECT * FROM (
                            SELECT пд.дата_прихода AS Дата, 'ПРИХОД' AS Тип, 
                                   с.наименование AS Склад, к.название AS Контрагент,
                                   тч.количество, тч.цена, тч.сумма
                            FROM ТЧ_накладная_прихода тч
                            JOIN приход_товара пд ON тч.id_прихода = пд.id_прихода
                            JOIN склады с ON пд.id_склада = с.id_склада
                            JOIN контрагенты к ON пд.id_контрагента = к.id_контрагента
                            WHERE тч.id_товара = @productId AND пд.дата_прихода BETWEEN @dateFrom AND @dateTo
                            
                            UNION ALL
                            
                            SELECT рд.дата_расхода AS Дата, 'РАСХОД' AS Тип,
                                   с.наименование AS Склад, к.название AS Контрагент,
                                   тч.количество, тч.цена, тч.сумма
                            FROM ТЧ_накладная_расхода тч
                            JOIN расход_товара рд ON тч.id_расхода = рд.id_расхода
                            JOIN склады с ON рд.id_склада = с.id_склада
                            JOIN контрагенты к ON рд.id_контрагента = к.id_контрагента
                            WHERE тч.id_товара = @productId AND рд.дата_расхода BETWEEN @dateFrom AND @dateTo
                        ) AS movement
                        ORDER BY Дата";

                    var dt = DatabaseHelper.ExecuteQuery(query, new[] {
                        new NpgsqlParameter("@productId", productId),
                        new NpgsqlParameter("@dateFrom", dateFrom),
                        new NpgsqlParameter("@dateTo", dateTo)
                    });

                    dgvReport.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка формирования отчета: {ex.Message}");
            }
        }

        private DataGridView dgvReport;
        private Button btnGenerate;
        private Panel panelFilters;
    }
}