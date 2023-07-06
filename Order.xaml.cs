using System;
using System.Windows;
using System.Windows.Controls;
using LibConnection;
using MySql.Data.MySqlClient;
using System.Data;
using iExcelWORK;
using OfficeOpenXml;
using System.Windows.Forms;

namespace iStack20
{
    public partial class Order : Window
    {
        public Order()
        {
            InitializeComponent();
        }

        iew iw = new iew();
        conect cons = new conect();
        MySqlDataAdapter adapter; MySqlDataAdapter adapter2;
        DataTable dt = new DataTable(); DataTable dt2 = new DataTable(); DataTable dt3 = new DataTable(); DataTable dt4 = new DataTable();
        string conn = "server= localhost;user= root;database= istack24;port= 3306;password= root;";

        private void butopen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(FirstDate.Text) && string.IsNullOrEmpty(SecondDate.Text))
                {
                    System.Windows.Forms.MessageBox.Show("Введите промежуток!!!");
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
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Не выбрана дата");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void ButExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(FirstDate.Text) && string.IsNullOrEmpty(SecondDate.Text))
                {
                    System.Windows.Forms.MessageBox.Show("Введите промежуток!!!");
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

                    MySqlCommand coma2 = new MySqlCommand($"SELECT Фамилия_администратора, count(заказ.ID_Администратора) AS 'Кол-во_заказов' FROM администрация JOIN заказ ON заказ.ID_Администратора = администрация.ID_Администратора WHERE Дата_выполнения BETWEEN @first AND @second GROUP BY 1 ORDER BY count(заказ.ID_Администратора) DESC ", sqlc);
                    coma2.Parameters.Add("@first", MySqlDbType.DateTime).Value = first;
                    coma2.Parameters.Add("@second", MySqlDbType.DateTime).Value = second;
                    adapter = new MySqlDataAdapter(coma2);
                    ds = new DataSet();
                    adapter.Fill(ds);
                    dt2 = ds.Tables[0];

                    MySqlCommand coma3 = new MySqlCommand($"SELECT услуги.Название_услуги, count(состав_заказа.ID_Услуги) FROM состав_заказа JOIN услуги ON состав_заказа.ID_Услуги = услуги.ID_Услуги JOIN заказ ON состав_заказа.ID_Заказа = заказ.ID_Заказа WHERE Дата_выполнения BETWEEN @first AND @second GROUP BY 1 ORDER BY count(состав_заказа.ID_Услуги) DESC", sqlc);
                    coma3.Parameters.Add("@first", MySqlDbType.DateTime).Value = first;
                    coma3.Parameters.Add("@second", MySqlDbType.DateTime).Value = second;
                    adapter = new MySqlDataAdapter(coma3);
                    ds = new DataSet();
                    adapter.Fill(ds);
                    dt3 = ds.Tables[0];

                    MySqlCommand coma4 = new MySqlCommand($"SELECT товар.Название_товара, count(состав_заказа.ID_Товара) FROM состав_заказа JOIN товар ON состав_заказа.ID_Товара = товар.ID_Товара JOIN заказ ON состав_заказа.ID_Заказа = заказ.ID_Заказа WHERE Дата_выполнения BETWEEN @first AND @second GROUP BY 1 ORDER BY count(состав_заказа.ID_Товара) DESC", sqlc);
                    coma4.Parameters.Add("@first", MySqlDbType.DateTime).Value = first;
                    coma4.Parameters.Add("@second", MySqlDbType.DateTime).Value = second;
                    adapter = new MySqlDataAdapter(coma4);
                    ds = new DataSet();
                    adapter.Fill(ds);
                    dt4 = ds.Tables[0];

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
                    iew.CreatOrder(dt, dt2, dt3, dt4, first, second, k);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cons.selectcon("Тип_заказа, Дата_оформления, Дата_выполнения, Цена_заказа", "заказ", ref adapter, ref dt);
            DataGrid.ItemsSource = dt.DefaultView;
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
