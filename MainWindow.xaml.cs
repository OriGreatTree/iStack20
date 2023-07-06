using System;
using System.Windows;
using System.Windows.Controls;
using LibConnection;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

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
                switch (conect.log)
                {
                    case 1:
                        butwork.Visibility = Visibility.Hidden;
                        break;
                    case 2:
                        butupdate.Visibility = Visibility.Hidden;

                        break;
                    case 3:
                        butupdate.Visibility = Visibility.Hidden;
                        butwork.Visibility = Visibility.Hidden;
                        break;
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
                System.Windows.Forms.MessageBox.Show("Сервер не включен!!!");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butopen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (conect.log)
                {
                    case 1:
                        cons.con(namets.Text, ref adapter, ref dt);
                        DataGrid.ItemsSource = dt.DefaultView;
                        break;
                    case 2:
                        cons.con(namets.Text, conect.log, ref adapter, ref dt);
                        DataGrid.ItemsSource = dt.DefaultView;
                        break;
                    case 3:
                        cons.con(namets.Text, conect.log, ref adapter, ref dt);
                        DataGrid.ItemsSource = dt.DefaultView;
                        break;
                    case 128:
                        cons.con(namets.Text, ref adapter, ref dt);
                        DataGrid.ItemsSource = dt.DefaultView;
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            //if (conect.log == 2)
            //{
            //    cons.con(namets.Text, conect.log, ref adapter, ref dt);
            //    DataGrid.ItemsSource = dt.DefaultView;
            //}
            //else
            //{
            //    cons.con(namets.Text, ref adapter, ref dt);
            //    DataGrid.ItemsSource = dt.DefaultView;
            //}
        }

        private void butupdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dt == null)
                {
                    System.Windows.Forms.MessageBox.Show("Таблица не выбрана");
                }
                else
                {
                    cons.upd(ref adapter, ref dt);
                    DataGrid.ItemsSource = dt.DefaultView;
                    System.Windows.Forms.MessageBox.Show("Изменение успешно сохранено!");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        
    }
}
