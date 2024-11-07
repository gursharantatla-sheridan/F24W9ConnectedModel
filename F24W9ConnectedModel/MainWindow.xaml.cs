using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace F24W9ConnectedModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // read the connection string from the app.config file
        string connStr = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "select EmployeeID, FirstName, LastName, City, Country from Employees";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                DataTable tbl = new DataTable();
                tbl.Load(reader);
                grdEmployees.ItemsSource = tbl.DefaultView;
            }
            //conn.Close();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // string concatenation
                //string query = "select EmployeeID, FirstName, LastName, City, Country from Employees where FirstName='" + txtFirstname.Text + "'";

                // parameterized query
                string query = "select EmployeeID, FirstName, LastName, City, Country from Employees where FirstName=@fn";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fn", txtFirstname.Text);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                DataTable tbl = new DataTable();
                tbl.Load(reader);
                grdEmployees.ItemsSource = tbl.DefaultView;
            }
        }

        private void btnCount_Click(object sender, RoutedEventArgs e)
        {
            string query = "select count(*) from Employees";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                int numRows = (int)cmd.ExecuteScalar();

                MessageBox.Show("Number of rows = " + numRows);
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            string query = "insert into Employees(FirstName, LastName) values(@fn, @ln)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fn", txtFirstname.Text);
                cmd.Parameters.AddWithValue("@ln", txtLastname.Text);

                conn.Open();

                cmd.ExecuteNonQuery();

                LoadData();
                MessageBox.Show("New employee added");
            }
        }
    }
}