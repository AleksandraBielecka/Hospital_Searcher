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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Hospital_Searcher
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            bool isLoaded = false;
            string CS = @"data source=DESKTOP-E9EAAOK\SQLEXPRESS01;database=HospitalsEmployees;integrated security=SSPI";
            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select top 1 FirstName, LastName, Address, Telephone, Email, Salary from tblEmployee where LastName='" + tb_Search.Text+"'", con);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var firstName = rdr["FirstName"] as string;
                        var lastName = rdr["LastName"] as string;
                        var address = rdr["Address"] as string;
                        var telephone = rdr["Telephone"] as string;
                        var email = rdr["Email"] as string;
                        var salary = Convert.ToInt32(rdr["Salary"]);
                        tb_FirstName.Text = firstName;
                        tb_LastName.Text = lastName;
                        tb_Address.Text = address;
                        tb_Telephone.Text = telephone;
                        tb_Email.Text = email;
                        tb_Salary.Text = salary.ToString();
                        isLoaded = true;
                    }

                }
            }
            if (isLoaded)
            {
                lbl_Status.Content = "Pobrano z bazy danych pracownika.";
                lbl_Status.Foreground = Brushes.Green;
                btn_Save.Content = "Zaktualizuj";
                btn_Save.IsEnabled = true;
            }
            else
            {
                lbl_Status.Content = "Nie znaleziono pracownika o nazwisku " + tb_Search.Text;
                lbl_Status.Foreground = Brushes.Red;
                btn_Save.IsEnabled = false;
            }
        }
    }
}
