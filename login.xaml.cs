using System.Windows;
using MySql.Data.MySqlClient;
using System.Data;
using LibConnection;
using System;
using System.Windows.Forms;

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

                string log = conect.hashPassword(Llogin.Text);
                string pas = conect.hashPassword(Lpassword.Text);

                MySqlConnection sqlc = new MySqlConnection(conn);
                sqlc.Open(); 

                MySqlCommand tlogin = new MySqlCommand($"SELECT * FROM администрация WHERE Логин = @Ll AND Пароль = @Lp", sqlc);
                tlogin.Parameters.Add("Ll", MySqlDbType.VarChar).Value = log;
                tlogin.Parameters.Add("Lp", MySqlDbType.VarChar).Value = pas;

                adapter.SelectCommand = tlogin;
                adapter.Fill(dt);

                if (dt.Rows.Count > 0) 
                {
                    cons.logins(ref mes, log);
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                    System.Windows.Forms.MessageBox.Show(mes);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Неверный логин или пароль!");
                }
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

        private void butopenregistr_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();

        }
    }
}
