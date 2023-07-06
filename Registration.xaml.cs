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
using System.Windows.Forms;

namespace iStack20
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        conect cons = new conect();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        DataTable dt = new DataTable();
        string conn = "server= localhost;user= root;database= istack24;port= 3306;password= root;";
        string key = "F38575EE8AF23BA6D923C0D98EE767FC"; //Дельтаплан

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pswd = LpasswordHide.Password;
                string key2 = conect.hashPassword(pswd);
                if (key == key2)
                {
                    string log = conect.hashPassword(LoginReg.Text);
                    string pas = conect.hashPassword(PasswordReg.Text);
                    DateTime testik = Convert.ToDateTime(birthday.Text);
                    string bir = testik.ToString("yyyy-MM-dd");
                    MySqlConnection sqlc = new MySqlConnection(conn);
                    sqlc.Open();
                    MySqlCommand update = new MySqlCommand($"INSERT INTO администрация (Фамилия_администратора, Имя_администратора, Отчество_администратора, Дата_рождения, Должность, Логин, Пароль)" +
                         $" VALUES (@secname, @firname, @hzname, @birthday, @dol, @login, @password)", sqlc);
                    update.Parameters.Add("@secname", MySqlDbType.VarChar).Value = SecNameCli.Text;
                    update.Parameters.Add("@firname", MySqlDbType.VarChar).Value = FirstNameCli.Text;
                    update.Parameters.Add("@hzname", MySqlDbType.VarChar).Value = HZNameCli.Text;
                    update.Parameters.Add("@birthday", MySqlDbType.DateTime).Value = bir;
                    update.Parameters.Add("@dol", MySqlDbType.VarChar).Value = dolz.Text;
                    update.Parameters.Add("@login", MySqlDbType.VarChar).Value = log;
                    update.Parameters.Add("@password", MySqlDbType.VarChar).Value = pas;
                    update.ExecuteNonQuery();
                    sqlc.Close();
                    System.Windows.Forms.MessageBox.Show("Сотрудник добавлен");
                    this.Close();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Код подтверждения отсутствует!");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
