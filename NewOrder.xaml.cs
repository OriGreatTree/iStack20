using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Win32;
using LibConnection;

namespace iStack20
{
    public partial class NewOrder : Window
    {
        public NewOrder()
        {
            InitializeComponent();
        }

        conect cons = new conect();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        DataTable dt = new DataTable();
        string conn = "server= localhost;user= root;database= istack24;port= 3306;password= root;";
        string begin, complete, end, NameTeam, item, uslugs, admin, costB, nimage, norder;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MySqlConnection sqlc = new MySqlConnection(conn);

            sqlc.Open();
            MySqlDataReader sqlread = null;
            MySqlCommand command = new MySqlCommand("SELECT Название_Команды FROM команда", sqlc);
            sqlread = command.ExecuteReader();
            while (sqlread.Read())
            {
                NamesTeams.Items.Add(Convert.ToString(sqlread["Название_Команды"]));
            }
            sqlc.Close();

            sqlc.Open();
            command = new MySqlCommand("SELECT Название_услуги FROM услуги", sqlc);
            sqlread = command.ExecuteReader();
            while (sqlread.Read())
            {
                Uslug.Items.Add(Convert.ToString(sqlread["Название_услуги"]));
            }
            sqlc.Close();

            sqlc.Open();
            command = new MySqlCommand("SELECT Название_товара FROM товар", sqlc);
            sqlread = command.ExecuteReader();
            while (sqlread.Read())
            {
                items.Items.Add(Convert.ToString(sqlread["Название_товара"]));
            }
            sqlc.Close();

            cons.con("клиент", 2, ref adapter, ref dt);
            DataGrid.ItemsSource = dt.DefaultView;
        }

        private void nameorder_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                begin = dateBegin.Text; complete = dateComplete.Text;
                end = dateEnd.Text; NameTeam = NamesTeams.Text;
                item = items.Text; uslugs = Uslug.Text;
                admin = administrator.Text; costB = CostBox.Text;
                nimage = nameimage.Text; norder = nameorder.Text;
                
                cons.showorder(ref end, ref begin, ref complete, ref NameTeam, ref item, ref uslugs, ref admin, ref costB, ref nimage, norder);

                dateBegin.Text = begin; dateComplete.Text = complete;
                dateEnd.Text = end; NamesTeams.Text = NameTeam;
                items.Text = item; Uslug.Text = uslugs;
                administrator.Text = admin; CostBox.Text = costB;
                nameimage.Text = nimage; nameorder.Text = norder;
                if (string.IsNullOrEmpty(nimage)) { }
                else
                    image.Source = BitmapFrame.Create(new Uri(nimage));

            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
            
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Изорбражение отсутствует");
            }
        }

        private void OpenImages_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == true)
            {
                nameimage.Text = openFileDialog1.FileName;
                image.Source = BitmapFrame.Create(new Uri(nameimage.Text));
            }
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void butopen_Copy3_Click(object sender, RoutedEventArgs e)
        {   
            DateTime testik = Convert.ToDateTime(dateEnd.Text);
            string end = testik.ToString("yyyy-MM-dd");
            testik = Convert.ToDateTime(dateBegin.Text);
            string begin = testik.ToString("yyyy-MM-dd");
            testik = Convert.ToDateTime(dateComplete.Text);
            string complete = testik.ToString("yyyy-MM-dd");

            MySqlConnection sqlc = new MySqlConnection(conn);
            sqlc.Open();

            MySqlCommand iс = new MySqlCommand($"SELECT ID_Клиента FROM клиент WHERE Фамилия_клиента = '{nameclient.Text}'", sqlc);
            MySqlCommand iadmin = new MySqlCommand($"SELECT ID_Администратора FROM администрация WHERE Фамилия_администратора= '{administrator.Text}'", sqlc);

            int idc = Convert.ToInt32(iс.ExecuteScalar());
            int ida = Convert.ToInt32(iadmin.ExecuteScalar());

            MySqlCommand tr_y = new MySqlCommand($"SELECT Тип_Заказа FROM заказ WHERE Тип_Заказа = '{nameorder.Text}'", sqlc);
            string tr_yy = Convert.ToString(tr_y.ExecuteScalar());
            if (string.IsNullOrEmpty(tr_yy))
            {
                cons.orders(NamesTeams.Text, nameorder.Text, begin, end, complete, CostBox.Text, nameimage.Text, Uslug.Text, items.Text, idc, ida);
                MessageBox.Show("Заказ создан!");
            }
            else
            {
                cons.orders(1, NamesTeams.Text, nameorder.Text, begin, end, complete, CostBox.Text, nameimage.Text, Uslug.Text, items.Text, idc, ida);
                MessageBox.Show("Заказ обновлен!");
            }
            
        }

        private void teamred_Click(object sender, RoutedEventArgs e)
        {
            Team tm = new Team();
            tm.Show();
        }

        private void butfind_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection sqlc = new MySqlConnection(conn);
            sqlc.Open();
            if (string.IsNullOrEmpty(findclient.Text))
            {
                string namet = "клиент";
                cons.con(namet, ref adapter, ref dt);
                DataGrid.ItemsSource = dt.DefaultView;
            }
            else
            {
                cons.selectcon("*", $"клиент WHERE Фамилия_клиента  = '{findclient.Text}'", ref adapter, ref dt);
                DataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void butupdatefind_Click(object sender, RoutedEventArgs e)
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
                nameclient.Text = row["Фамилия_клиента"].ToString();
            }
            catch (System.ArgumentOutOfRangeException)
            {

            }
            catch (System.InvalidCastException)
            {

            }
        }

        private void nameclient_SelectionChanged(object sender, RoutedEventArgs e)
        {
            nameorder.Items.Clear();

            MySqlConnection sqlc = new MySqlConnection(conn);
            sqlc.Open();

            MySqlCommand command = new MySqlCommand($"SELECT ID_Клиента FROM клиент WHERE Фамилия_клиента = '{nameclient.Text}'", sqlc);
            int idz = Convert.ToInt32(command.ExecuteScalar());

            MySqlDataReader sqlread = null;
            MySqlCommand command1 = new MySqlCommand($"SELECT Тип_заказа FROM заказ WHERE ID_Клиента = {idz}", sqlc);
            sqlread = command1.ExecuteReader();
            while (sqlread.Read())
            {
                nameorder.Items.Add(Convert.ToString(sqlread["Тип_заказа"]));
            }
            sqlc.Close();
        }

        private void butnewclientopern_Click(object sender, RoutedEventArgs e)
        {
            NewClient nc = new NewClient();
            nc.Show();
        }
    }
}
