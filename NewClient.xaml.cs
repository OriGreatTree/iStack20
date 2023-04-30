using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LibConnection;
using MySql.Data.MySqlClient;
using System.Data;

namespace iStack20
{
    /// <summary>
    /// Логика взаимодействия для NewClient.xaml
    /// </summary>
    public partial class NewClient : Window
    {
        public NewClient()
        {
            InitializeComponent();
        }

        conect cons = new conect();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        DataTable dt = new DataTable();
        string conn = "server= localhost;user= root;database= istack24;port= 3306;password= root;";

        private void ButSave_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection sqlc = new MySqlConnection(conn);
            sqlc.Open();
            MySqlCommand update = new MySqlCommand($"INSERT INTO клиент (Фамилия_клиента, Имя_клиента, Отчество_клиента, Телефон, Название_фирмы, Адрес)" +
                 $" VALUES (@secname, @firname, @hzname, @numbers, @firma, @adress)", sqlc);
            update.Parameters.Add("@secname", MySqlDbType.VarChar).Value = SecNameCli.Text;
            update.Parameters.Add("@firname", MySqlDbType.VarChar).Value = FirstNameCli.Text;
            update.Parameters.Add("@hzname", MySqlDbType.VarChar).Value = HZNameCli.Text;
            update.Parameters.Add("@numbers", MySqlDbType.VarChar).Value = NumberCli.Text;
            update.Parameters.Add("@firma", MySqlDbType.VarChar).Value = FirmCli.Text;
            update.Parameters.Add("@adress", MySqlDbType.VarChar).Value = AdressCli.Text;
            update.ExecuteNonQuery();
            sqlc.Close();
            MessageBox.Show("Клиент добавлен");
        }

        private void ButClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
