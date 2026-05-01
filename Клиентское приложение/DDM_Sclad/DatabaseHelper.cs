using Npgsql;
using System;
using System.Data;

namespace DDM_Sclad
{
    public static class DatabaseHelper
    {
        //параметры подключения
        private static string connectionString = DatabaseConfig.ConnectionString;
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        public static DataTable ExecuteQuery(string query, NpgsqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static int ExecuteNonQuery(string query, NpgsqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string query, NpgsqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }
        
// Получить структуру таблицы
        public static DataTable GetTableSchema(string tableName)
        {
            string query = @"
        SELECT 
            column_name,
            data_type,
            is_nullable,
            column_default
        FROM information_schema.columns 
        WHERE table_name = @tableName 
        AND table_schema = 'public'
        ORDER BY ordinal_position";

            return ExecuteQuery(query, new[] { new NpgsqlParameter("@tableName", tableName) });
        }

        // Получить первичный ключ таблицы
        public static string GetPrimaryKey(string tableName)
        {
            string query = @"
        SELECT a.attname
        FROM pg_index i
        JOIN pg_attribute a ON a.attrelid = i.indrelid AND a.attnum = ANY(i.indkey)
        WHERE i.indrelid = @tableName::regclass
        AND i.indisprimary";

            var result = ExecuteScalar(query, new[] { new NpgsqlParameter("@tableName", tableName) });
            return result?.ToString() ?? "id";
        }

        // Получить все данные из таблицы
        public static DataTable GetAllData(string tableName, string sortColumn = null, string sortDirection = "ASC")
        {
            string query = $"SELECT * FROM {tableName}";
            if (!string.IsNullOrEmpty(sortColumn))
                query += $" ORDER BY {sortColumn} {sortDirection}";

            return ExecuteQuery(query);
        }
        // Получить справочник для внешнего ключа
        public static DataTable GetLookupData(string foreignTable, string displayColumn = "наименование", string valueColumn = "id")
        {
            string tableName = foreignTable;
            string displayCol = displayColumn;
            string valueCol = valueColumn;

            // Для персонала: id_должности -> таблица должности
            if (foreignTable == "id_должности")
            {
                tableName = "должности";
                displayCol = "название";
                valueCol = "id_должности";
            }
            else if (foreignTable == "id_склада")
            {
                tableName = "склады";
                displayCol = "наименование";
                valueCol = "id_склада";
            }
            else if (foreignTable == "id_товара")
            {
                tableName = "товары";
                displayCol = "название";
                valueCol = "id_товара";
            }
            else if (foreignTable == "id_контрагента")
            {
                tableName = "контрагенты";
                displayCol = "название";
                valueCol = "id_контрагента";
            }
            else if (foreignTable == "id_завсклада")
            {
                tableName = "персонал";
                displayCol = "фио";
                valueCol = "id_сотрудника";
            }

            string query = $"SELECT {valueCol}, {displayCol} FROM {tableName} ORDER BY {displayCol}";
            return ExecuteQuery(query);
        }
    }
}