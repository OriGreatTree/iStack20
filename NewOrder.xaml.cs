using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Win32;
using LibConnection;
using System.Windows.Forms;
using System.IO;

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
            MySqlCommand fioadm = new MySqlCommand($"SELECT Фамилия_администратора FROM администрация WHERE ID_Администратора = {conect.stat}", sqlc);
            administrator.Text = Convert.ToString(fioadm.ExecuteScalar());
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
                System.Windows.Forms.MessageBox.Show("Изорбражение отсутствует");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butcount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int costus = 0;
                cons.countcost(ref costus, Uslug.Text, items.Text);
                CostBox.Text = Convert.ToString(costus);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void butdel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Вы хотите удалить заказ?", "Внимание", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    norder = nameorder.Text;
                    nameorder.Items.Clear();
                    MySqlConnection sqlc = new MySqlConnection(conn);
                    sqlc.Open();

                    MySqlCommand iz = new MySqlCommand($"SELECT ID_Заказа FROM заказ WHERE Тип_заказа = '{norder}'", sqlc);
                    int iza = Convert.ToInt32(iz.ExecuteScalar());

                    MySqlCommand sostav = new MySqlCommand($"DELETE FROM Состав_заказа WHERE ID_Заказа = @idzak", sqlc);
                    sostav.Parameters.Add("@idzak", MySqlDbType.Int32).Value = iza;
                    sostav.ExecuteNonQuery();

                    MySqlCommand status = new MySqlCommand($"DELETE FROM Статус_заказа WHERE ID_Заказа = @idzak", sqlc);
                    status.Parameters.Add("@idzak", MySqlDbType.Int32).Value = iza;
                    status.ExecuteNonQuery();

                    MySqlCommand deletez = new MySqlCommand($"DELETE FROM заказ WHERE Тип_заказа = @namezak", sqlc);
                    deletez.Parameters.Add("@namezak", MySqlDbType.VarChar).Value = norder;
                    deletez.ExecuteNonQuery();

                    MySqlCommand command = new MySqlCommand($"SELECT ID_Клиента FROM клиент WHERE Фамилия_клиента = '{nameclient.Text}'", sqlc);
                    int idcli = Convert.ToInt32(command.ExecuteScalar());
                    MySqlDataReader sqlread = null;
                    MySqlCommand command1 = new MySqlCommand($"SELECT Тип_заказа FROM заказ WHERE ID_Клиента = {idcli}", sqlc);
                    sqlread = command1.ExecuteReader();
                    while (sqlread.Read())
                    {
                        nameorder.Items.Add(Convert.ToString(sqlread["Тип_заказа"]));
                    }
                    sqlc.Close();
                    System.Windows.Forms.MessageBox.Show("Заказ удален");
                }
            }
            catch (Exception ex) 
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenImages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SchemsPath = Path.Combine(Environment.CurrentDirectory, "Schems");
                Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                {
                    InitialDirectory = SchemsPath,
                    Title = "Выбор схемы",

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
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void butopen_Copy3_Click(object sender, RoutedEventArgs e)
        {   try
            {
                DateTime testik = Convert.ToDateTime(dateEnd.Text);
                string end = testik.ToString("yyyy-MM-dd");
                testik = Convert.ToDateTime(dateBegin.Text);
                string begin = testik.ToString("yyyy-MM-dd");
                string complete = null;
                if (string.IsNullOrEmpty(dateComplete.Text))
                {

                }
                else
                {
                    testik = Convert.ToDateTime(dateComplete.Text);
                    complete = testik.ToString("yyyy-MM-dd");
                }

                MySqlConnection sqlc = new MySqlConnection(conn);
                sqlc.Open();

                MySqlCommand iс = new MySqlCommand($"SELECT ID_Клиента FROM клиент WHERE Фамилия_клиента = '{nameclient.Text}'", sqlc);
                MySqlCommand iadmin = new MySqlCommand($"SELECT ID_Администратора FROM администрация WHERE Фамилия_администратора= '{administrator.Text}'", sqlc);

                int idc = Convert.ToInt32(iс.ExecuteScalar());
                int ida = Convert.ToInt32(iadmin.ExecuteScalar());

                MySqlCommand tr_y = new MySqlCommand($"SELECT Тип_Заказа FROM заказ WHERE Тип_Заказа = '{nameorder.Text}'", sqlc);
                string tr_yy = Convert.ToString(tr_y.ExecuteScalar());
                NameTeam = NamesTeams.Text;
                item = items.Text; uslugs = Uslug.Text;
                admin = administrator.Text; costB = CostBox.Text;
                nimage = nameimage.Text; norder = nameorder.Text;
                if (string.IsNullOrEmpty(tr_yy))
                {
                    cons.orders(NameTeam, norder, begin, end, complete, costB, nimage, uslugs, item, idc, ida);
                    System.Windows.Forms.MessageBox.Show("Заказ создан!");
                }
                else
                {
                    cons.orders(1, NameTeam, norder, begin, end, complete, costB, nimage, uslugs, item, idc, ida);
                    cons.orders(1, NameTeam, norder, begin, end, complete, costB, nimage, uslugs, item, idc, ida);
                    System.Windows.Forms.MessageBox.Show("Заказ обновлен!");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Отсутствует информация подлежащая загрузке в базу данных");
            }
            else
            {
                NamesTeams.Items.Clear();
                MySqlConnection sqlc = new MySqlConnection(conn);
                sqlc.Open();
                cons.upd(ref adapter, ref dt);
                DataGrid.ItemsSource = dt.DefaultView;
                System.Windows.Forms.MessageBox.Show("Обновление прошло успешно!");
                MySqlDataReader sqlread = null;
                MySqlCommand command = new MySqlCommand("SELECT Название_Команды FROM команда", sqlc);
                sqlread = command.ExecuteReader();
                while (sqlread.Read())
                {
                    NamesTeams.Items.Add(Convert.ToString(sqlread["Название_Команды"]));
                }
                sqlc.Close();
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
            try
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
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void butnewclientopern_Click(object sender, RoutedEventArgs e)
        {
            NewClient nc = new NewClient();
            nc.Show();
        }
    }
}
