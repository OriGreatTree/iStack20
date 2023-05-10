using System.Windows;
using MySql.Data.MySqlClient;
using System.Data;
using LibConnection;
using System;

namespace iStack20
{
    public partial class login : Window
    {
        public login()
        {
            InitializeComponent();
        }

        conect cons = new conect();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        DataTable dt = new DataTable();
        string conn = "server= localhost;user= root;database= istack24;port= 3306;password= root;";

        int idadm;

        private void butopen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string mes = null;

                if (LpasswordHide.Visibility == Visibility.Visible) 
                {
                    string psw = LpasswordHide.Password;
                    Lpassword.Text = psw;
                }

                MySqlConnection sqlc = new MySqlConnection(conn);
                sqlc.Open(); 

                MySqlCommand tlogin = new MySqlCommand($"SELECT * FROM администрация WHERE Логин = @Ll AND Пароль = @Lp", sqlc);
                tlogin.Parameters.Add("Ll", MySqlDbType.VarChar).Value = Llogin.Text;
                tlogin.Parameters.Add("Lp", MySqlDbType.VarChar).Value = Lpassword.Text;

                adapter.SelectCommand = tlogin;
                adapter.Fill(dt);

                if (dt.Rows.Count > 0) 
                {
                    cons.logins(ref mes, Llogin.Text);
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();  
                    MessageBox.Show(mes);
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Сервер не включен!!!");
            }
        }

        private void butshow_Click(object sender, RoutedEventArgs e)
        {
            if (LpasswordHide.Visibility == Visibility.Visible)
            {
                string psw = LpasswordHide.Password;
                Lpassword.Text = psw;
                LpasswordHide.Visibility = Visibility.Hidden;
            }
            else
            {
                string psw = Lpassword.Text;
                LpasswordHide.Password = psw;
                LpasswordHide.Visibility = Visibility.Visible;
            } 
        }
    }
}
