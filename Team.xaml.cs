using System.Windows;
using System.Windows.Controls;
using LibConnection;
using MySql.Data.MySqlClient;
using System.Data;

namespace iStack20
{
    public partial class Team : Window
    {
        public Team()
        {
            InitializeComponent();
        }

        conect cons = new conect();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        DataTable dt = new DataTable();
        string conn = "server= localhost;user= root;database= istack24;port= 3306;password= root;";
        int sc = 0;
        string idr;

        private void butopen_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection sqlc = new MySqlConnection(conn);
            sqlc.Open();
            if (string.IsNullOrEmpty(findworker.Text))
            {
                string namet = "работники";
                cons.con(namet, ref adapter, ref dt);
                DataGrid.ItemsSource = dt.DefaultView;
            }
            else
            {
                cons.selectcon("*", $"работники WHERE Фамилия_работника = '{findworker.Text}'", ref adapter, ref dt);
                DataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void butclose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cons.con("работники", ref adapter, ref dt);
            DataGrid.ItemsSource = dt.DefaultView;
            cons.con("команда", ref adapter, ref dt);
            DataGrid2.ItemsSource = dt.DefaultView;
        }

        private void butupdate_Click(object sender, RoutedEventArgs e)
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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)DataGrid.SelectedItems[0];
                idr = row["ID_Работника"].ToString();
                MessageBox.Show("Выберите команду");
                sc++;
            }
            catch (System.ArgumentOutOfRangeException)
            {

            }
            catch (System.InvalidCastException)
            {

            }
        }

        private void DataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MySqlConnection sqlc = new MySqlConnection(conn);
                DataRowView row = (DataRowView)DataGrid2.SelectedItems[0];
                string idt = row["ID_Команды"].ToString();
                if (sc == 1)
                {
                    sqlc.Open();
                    MySqlCommand update = new MySqlCommand($"UPDATE работники SET ID_Команды = {idt} WHERE ID_Работника = {idr}", sqlc);
                    update.ExecuteNonQuery();
                    cons.con("работники", ref adapter, ref dt);
                    DataGrid.ItemsSource = dt.DefaultView;
                    sqlc.Close();
                    MessageBox.Show("Команда обновлена!");
                    sc = 0;
                }
                else
                {
                    sqlc.Open();
                    cons.selectcon("*", $"работники WHERE ID_Команды = {idt}", ref adapter, ref dt);
                    DataGrid2.ItemsSource = dt.DefaultView;
                    sqlc.Close();
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {

            }
            catch (System.InvalidCastException)
            {

            }
        }

        private void butback_Click(object sender, RoutedEventArgs e)
        {
            cons.con("команда", ref adapter, ref dt);
            DataGrid2.ItemsSource = dt.DefaultView;
        }
    }
}
