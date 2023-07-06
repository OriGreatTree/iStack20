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
    /// Логика взаимодействия для NWorker.xaml
    /// </summary>
    public partial class NWorker : Window
    {
        public NWorker()
        {
            InitializeComponent();
        }

        conect cons = new conect();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        DataTable dt = new DataTable();
        string conn = "server= localhost;user= root;database= istack24;port= 3306;password= root;";

        private void ButSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime testik = Convert.ToDateTime(birthday.Text);
                string bir = testik.ToString("yyyy-MM-dd");
                MySqlConnection sqlc = new MySqlConnection(conn);
                sqlc.Open();
                MySqlCommand update = new MySqlCommand($"INSERT INTO работники (Фамилия_работника, Имя_работника, Отчество_работника, Дата_рождения, Стаж)" +
                     $" VALUES (@secname, @firname, @hzname, @birthday, @staz)", sqlc);
                update.Parameters.Add("@secname", MySqlDbType.VarChar).Value = SecNameCli.Text;
                update.Parameters.Add("@firname", MySqlDbType.VarChar).Value = FirstNameCli.Text;
                update.Parameters.Add("@hzname", MySqlDbType.VarChar).Value = HZNameCli.Text;
                update.Parameters.Add("@birthday", MySqlDbType.DateTime).Value = bir;
                update.Parameters.Add("@staz", MySqlDbType.Int32).Value = Convert.ToInt32(Staz.Text);
                update.ExecuteNonQuery();
                sqlc.Close();
                System.Windows.Forms.MessageBox.Show("Работник добавлен");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Возникла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
