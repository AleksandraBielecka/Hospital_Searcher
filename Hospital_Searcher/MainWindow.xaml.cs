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
        private int _currentEmployeeId;

        public MainWindow()
        {
            InitializeComponent();
            ClearTextboxes();
            btn_Search.Content = "Szukaj";
            tb_Search.Text = "";
            lbl_Address.Content = "Address";
            lbl_email.Content = "Email";
            lbl_FirstName.Content = "FirstName";
            lbl_LastName.Content = "LastName";
            lbl_Salary.Content = "Salary";
            lbl_Status.Content = "Użyj wyszukiwarki, żeby znaleźć pracownika";
            lbl_Telephone.Content = "Content";
            btn_Save.Visibility = Visibility.Hidden;
        }

        private void ClearTextboxes()
        {
            tb_FirstName.Text = "";
            tb_LastName.Text = "";
            tb_Address.Text = "";
            tb_Telephone.Text = "";
            tb_Email.Text = "";
            tb_Salary.Text = "";
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            bool isLoaded = false;
            _currentEmployeeId = 0;
            string CS = @"data source=DESKTOP-E9EAAOK\SQLEXPRESS01;database=HospitalsEmployees;integrated security=SSPI";
            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select top 1 ID, FirstName, LastName, Address, Telephone, Email, Salary from tblEmployee where LastName='" + tb_Search.Text + "'", con);
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
                        _currentEmployeeId = Convert.ToInt32(rdr["ID"]);
                        isLoaded = true;
                    }

                }
            }
            if (isLoaded)
            {
                lbl_Status.Content = "Pobrano z bazy danych pracownika";
                lbl_Status.Foreground = Brushes.Green;
                btn_Save.Content = "Zaktualizuj";
                btn_Save.IsEnabled = true;
                btn_Save.Visibility = Visibility.Visible;
            }
            else
            {
                lbl_Status.Content = "Nie znaleziono pracownika o nazwisku " + tb_Search.Text;
                lbl_Status.Foreground = Brushes.Red;
                btn_Save.IsEnabled = false;
                ClearTextboxes();
                btn_Save.Visibility = Visibility.Hidden;
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string CS = @"data source=DESKTOP-E9EAAOK\SQLEXPRESS01;database=HospitalsEmployees;integrated security=SSPI";
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("UPDATE tblEmployee SET FirstName = '" + tb_FirstName.Text + "', LastName = '" + tb_LastName.Text + "', Address = '" + tb_Address.Text + "', Telephone = '" + tb_Telephone.Text + "', Email = '" + tb_Email.Text + "', Salary = " + tb_Salary.Text + " WHERE ID = " + _currentEmployeeId.ToString(), con);
                con.Open(); 
                int TotalRowsAffected = cmd.ExecuteNonQuery();
                if (TotalRowsAffected==1)
                {
                    lbl_Status.Content = "Zaktualizowano dane pracownika";
                    lbl_Status.Foreground = Brushes.Green;
                }
                else
                {
                    lbl_Status.Content = "Nie zaktualizowano danych pracownika";
                    lbl_Status.Foreground = Brushes.Red;
                }

            }
        }
    }
}
