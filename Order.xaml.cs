using System;
using System.Windows;
using System.Windows.Controls;
using LibConnection;
using MySql.Data.MySqlClient;
using System.Data;
using iExcelWORK;
using OfficeOpenXml;

namespace iStack20
{
    public partial class Order : Window
    {
        public Order()
        {
            InitializeComponent();
        }

        conect cons = new conect();
        MySqlDataAdapter adapter;
        DataTable dt = new DataTable();
        string conn = "server= localhost;user= root;database= istack24;port= 3306;password= root;";

        private void ButExcel_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FirstDate.Text) && string.IsNullOrEmpty(SecondDate.Text))
            {
                MessageBox.Show("Введите промежуток!!!");
            }
            else
            {
                int k = 0;
                DateTime testik = Convert.ToDateTime(FirstDate.Text);
                string first = testik.ToString("yyyy-MM-dd");
                testik = Convert.ToDateTime(SecondDate.Text);
                string second = testik.ToString("yyyy-MM-dd");

                MySqlConnection sqlc = new MySqlConnection(conn);
                sqlc.Open();
                MySqlCommand coma = new MySqlCommand($"SELECT Тип_заказа, Дата_оформления, Дата_выполнения, Цена_заказа FROM Заказ WHERE Дата_выполнения BETWEEN @first AND @second", sqlc);
                coma.Parameters.Add("@first", MySqlDbType.DateTime).Value = first;
                coma.Parameters.Add("@second", MySqlDbType.DateTime).Value = second;
                adapter = new MySqlDataAdapter(coma);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dt = ds.Tables[0];

                MySqlCommand sum = new MySqlCommand($"SELECT Цена_заказа FROM Заказ WHERE Дата_выполнения BETWEEN @first AND @second", sqlc);
                sum.Parameters.Add("@first", MySqlDbType.DateTime).Value = first;
                sum.Parameters.Add("@second", MySqlDbType.DateTime).Value = second;
                MySqlDataReader sqlread = sum.ExecuteReader();
                while (sqlread.Read())
                {
                    k = k + Convert.ToInt32(sqlread["Цена_заказа"]);
                }
                sqlc.Close();
                iew iew = new iew();
                iew.CreatOrder(dt, first, second, k);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cons.selectcon("Тип_заказа, Дата_оформления, Дата_выполнения, Цена_заказа", $"заказ", ref adapter, ref dt);
            DataGrid.ItemsSource = dt.DefaultView;
        }

        private void butopen_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FirstDate.Text) && string.IsNullOrEmpty(SecondDate.Text))
            {
                MessageBox.Show("Введите промежуток!!!");
            }
            else
            {
                int k = 0;
                DateTime testik = Convert.ToDateTime(FirstDate.Text);
                DateTime testik2 = Convert.ToDateTime(SecondDate.Text);
                string first = testik.ToString("yyyy-MM-dd");
                string second = testik2.ToString("yyyy-MM-dd");
                MySqlConnection sqlc = new MySqlConnection(conn);
                sqlc.Open();
                MySqlCommand coma = new MySqlCommand($"SELECT Тип_заказа, Дата_оформления, Дата_выполнения, Цена_заказа FROM Заказ WHERE Дата_выполнения BETWEEN @first AND @second", sqlc);
                coma.Parameters.Add("@first", MySqlDbType.DateTime).Value = first;
                coma.Parameters.Add("@second", MySqlDbType.DateTime).Value = second;
                cons.selectconparametr(coma, ref adapter, ref dt);
                DataGrid.ItemsSource = dt.DefaultView;
                MySqlCommand sum = new MySqlCommand($"SELECT Цена_заказа FROM Заказ WHERE Дата_выполнения BETWEEN @first AND @second", sqlc);
                sum.Parameters.Add("@first", MySqlDbType.DateTime).Value = first;
                sum.Parameters.Add("@second", MySqlDbType.DateTime).Value = second;
                MySqlDataReader sqlread = sum.ExecuteReader();
                while (sqlread.Read())
                {
                    k = k + Convert.ToInt32(sqlread["Цена_заказа"]);
                    reportt.Text = Convert.ToString($"Прибыль за данный период составляет: {k}");
                }
                sqlc.Close();
            }   
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
