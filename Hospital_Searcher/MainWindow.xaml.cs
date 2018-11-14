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
        private int? _currentEmployeeId;

        public MainWindow()
        {
            InitializeComponent();
            ClearTextboxes();
            btn_Search.Content = "Szukaj";
            lbl_Address.Content = "Address";
            lbl_email.Content = "Email";
            lbl_FirstName.Content = "FirstName";
            lbl_LastName.Content = "LastName";
            lbl_Salary.Content = "Salary";
            lbl_Status.Content = "Użyj wyszukiwarki, żeby znaleźć pracownika";
            lbl_Telephone.Content = "Telefon";
            btn_Save.Visibility = Visibility.Hidden;
            btn_Delete.IsEnabled = false;
            this.LastNameList();
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
            if (this.rcb_Search.SelectedValue != null)
            {
                bool isLoaded = false;
                _currentEmployeeId = 0;
                string CS = @"data source=DESKTOP-E9EAAOK\SQLEXPRESS01;database=HospitalsEmployees;integrated security=SSPI";
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select top 1 ID, FirstName, LastName, Address, Telephone, Email, Salary from tblEmployee where LastName='" + this.rcb_Search.SelectedValue.ToString() + "'", con);
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
                    btn_Delete.IsEnabled = true;
                }
            }
            else
            {
                lbl_Status.Content = "Nie znaleziono pracownika";
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
                if (TotalRowsAffected == 1)
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

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            string CS = @"data source=DESKTOP-E9EAAOK\SQLEXPRESS01;database=HospitalsEmployees;integrated security=SSPI";
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("procEmployeeInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName", tb_FirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", tb_LastName.Text);
                cmd.Parameters.AddWithValue("@Address", tb_Address.Text);
                cmd.Parameters.AddWithValue("@Telephone", tb_Telephone.Text);
                cmd.Parameters.AddWithValue("@Email", tb_Email.Text);
                cmd.Parameters.AddWithValue("@Salary", tb_Salary.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                lbl_Status.Content = "Dodano nowego pracownika";
                lbl_Status.Foreground = Brushes.Green;
            }
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            string CS = @"data source=DESKTOP-E9EAAOK\SQLEXPRESS01;database=HospitalsEmployees;integrated security=SSPI";
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("procEmployeeDelete", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", this._currentEmployeeId);

                con.Open();
                cmd.ExecuteNonQuery();
                lbl_Status.Content = "Usunięto pracownika";
                lbl_Status.Foreground = Brushes.Red;
            }

            tb_FirstName.Text = null;
            tb_LastName.Text = null;
            tb_Address.Text = null;
            tb_Telephone.Text = null;
            tb_Email.Text = null;
            tb_Salary.Text = null;
            _currentEmployeeId = null;
            btn_Delete.IsEnabled = false;
        }
        public void LastNameList()
        {
            List<string> Lista = new List<string>();
            string CS = @"data source=DESKTOP-E9EAAOK\SQLEXPRESS01;database=HospitalsEmployees;integrated security=SSPI";
            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("procEmployeeList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    Lista.Add("Wybierz");
                    while (rdr.Read())
                    {
                        string lastName = rdr["LastName"] as string;
                        Lista.Add(lastName);
                    }
                    
                    this.rcb_Search.ItemsSource = Lista;
                    this.rcb_Search.SelectedIndex = 0;
                }
                
                cmd.ExecuteNonQuery();
                
            }
        }
    }
}
