using System;
using System.Windows;
using System.Windows.Controls;
using LibConnection;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace iStack20
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        conect cons = new conect();
        MySqlDataAdapter adapter;
        DataTable dt = new DataTable();
        string conn = "server= localhost;user= root;database= istack24;port= 3306;password= root;";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (conect.log == 2)
                {
                    butupdate.Visibility = Visibility.Hidden;
                }
                MySqlConnection sqlc = new MySqlConnection(conn);
                sqlc.Open();
                MySqlDataReader sqlread = null;
                MySqlCommand fioadm = new MySqlCommand($"SELECT Имя_администратора FROM администрация WHERE ID_Администратора = {conect.stat}", sqlc);
                MySqlCommand dolz = new MySqlCommand($"SELECT Должность FROM администрация WHERE ID_Администратора = {conect.stat}", sqlc);
                Fi.Text = Convert.ToString(fioadm.ExecuteScalar());
                Fi.Text += " " + Convert.ToString(dolz.ExecuteScalar());

                //dol.Text = Convert.ToString(dolz.ExecuteScalar());
                MySqlCommand command = new MySqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = DATABASE() ", sqlc);
                sqlread = command.ExecuteReader();
                while (sqlread.Read())
                {
                    namets.Items.Add(Convert.ToString(sqlread["TABLE_NAME"]));
                }
            sqlc.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Сервер не включен!!!");
            }
        }

        private void butopen_Click(object sender, RoutedEventArgs e)
        {
            if (conect.log == 2)
            {
                cons.con(namets.Text, conect.log, ref adapter, ref dt);
                DataGrid.ItemsSource = dt.DefaultView;
            }
            else
            {
                cons.con(namets.Text, ref adapter, ref dt);
                DataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void butupdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (adapter == null)
                {
                    MessageBox.Show("Отсутствует информация подлежащая загрузке в базу данных");
                }
                else
                {
                    cons.upd(ref adapter, ref dt);
                    DataGrid.ItemsSource = dt.DefaultView;
                    MessageBox.Show("Изменение успешно сохранено!");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка в таблице!");
                string namet = namets.Text;
                cons.con(namet, ref adapter, ref dt);
                DataGrid.ItemsSource = dt.DefaultView;
            }  
        }

        private void butzak_Click(object sender, RoutedEventArgs e)
        {
            NewOrder nor = new NewOrder();
            nor.Show();
        }

        private void butautor_Click(object sender, RoutedEventArgs e)
        {
            login log = new login();
            log.Show();
            this.Close();
        }

        private void butwork_Click(object sender, RoutedEventArgs e)
        {
            Order od = new Order();
            od.Show();
        }

        private void butteam_Click(object sender, RoutedEventArgs e)
        {
            Team tm = new Team();
            tm.Show();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void namets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void butsp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\Users\Mich9\source\repos\iStack20\istck.chm");
        }
    }
}
