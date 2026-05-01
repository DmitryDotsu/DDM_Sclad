using System;
using System.Windows.Forms;

namespace DDM_Sclad
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();  // Этот метод находится в MainForm.Designer.cs
        }
        /*
        private void btnWarehouses_Click(object sender, EventArgs e)
        {
            var form = new TableForm("склады",
                "SELECT id_склада AS Код, наименование, адрес, завсклад FROM склады",
                new[] { "наименование", "адрес", "завсклад" });
            form.ShowDialog();
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            var form = new TableForm("персонал",
                @"SELECT п.id_сотрудника AS Код, п.фио, д.название AS Должность, 
                 д.оклад, д.обязанности,
                 с.наименование AS Склад, п.инн, п.телефон, п.email, п.дата_приема
          FROM персонал п
          JOIN должности д ON п.id_должности = д.id_должности
          JOIN склады с ON п.id_склада = с.id_склада",
                new[] { "фио", "инн", "телефон", "email", "дата_приема", "id_должности", "id_склада" });
            form.ShowDialog();
        }
        private void btnPositions_Click(object sender, EventArgs e)
        {
            var form = new TableForm("должности",
                "SELECT id_должности AS Код, название, оклад, обязанности FROM должности",
                new[] { "название", "оклад", "обязанности" });
            form.ShowDialog();
        }
        private void btnProducts_Click(object sender, EventArgs e)
        {
            var form = new TableForm("товары",
                "SELECT id_товара AS Код, название, категория, ед_измерения FROM товары",
                new[] { "название", "категория", "ед_измерения" });
            form.ShowDialog();
        }

        private void btnCounterparties_Click(object sender, EventArgs e)
        {
            var form = new TableForm("контрагенты",
                "SELECT id_контрагента AS Код, название, инн, телефон, email, адрес, является_поставщиком AS Поставщик, является_покупателем AS Покупатель FROM контрагенты",
                new[] { "название", "инн", "телефон", "email", "адрес", "Поставщик", "Покупатель" });
            form.ShowDialog();
        } */
        private void btnWarehouses_Click(object sender, EventArgs e)
        {
            var form = new DynamicTableForm("склады");
            form.ShowDialog();
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            var form = new DynamicTableForm("персонал");
            form.ShowDialog();
        }

        private void btnPositions_Click(object sender, EventArgs e)
        {
            var form = new DynamicTableForm("должности");
            form.ShowDialog();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            var form = new DynamicTableForm("товары");
            form.ShowDialog();
        }

        private void btnCounterparties_Click(object sender, EventArgs e)
        {
            var form = new DynamicTableForm("контрагенты");
            form.ShowDialog();
        }
        private void btnIncomeDoc_Click(object sender, EventArgs e)
        {
            var form = new IncomeDocForm();
            form.ShowDialog();
        }

        private void btnExpenseDoc_Click(object sender, EventArgs e)
        {
            var form = new ExpenseDocForm();
            form.ShowDialog();
        }

        private void btnReportStock_Click(object sender, EventArgs e)
        {
            var form = new ReportForm("Остатки на складах", "stock");
            form.ShowDialog();
        }

        private void btnReportProfit_Click(object sender, EventArgs e)
        {
            var form = new ReportForm("Прибыль от реализации", "profit");
            form.ShowDialog();
        }

        private void btnReportMovement_Click(object sender, EventArgs e)
        {
            var form = new ReportForm("Движение товаров", "movement");
            form.ShowDialog();
        }
    }
}