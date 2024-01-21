using System.Data.SqlClient;
using System.Globalization;
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

namespace WpfApp5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly string stringConnect = "Data Source=SHOHBOZ;Initial Catalog=AllUsers;Integrated Security=True";
        public readonly SqlConnection connection;
        private List<Names> names;
        public MainWindow()
        {
            InitializeComponent();
            connection = new SqlConnection(stringConnect);


        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {


            connection.Open();

            string query = $"Insert into Users1 (FirstName, LastName) values ('{txbFirstName.Text}','{txbLastName.Text}');";


            SqlCommand cmd = new SqlCommand(query, connection);

            var result = cmd.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show($"Result:{result.ToString()}");


            txbFirstName.Clear();
            txbLastName.Clear();

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            txbFirstName.Text = (dataGridNames.SelectedItem as Names).FirstName;
            txbLastName.Text = (dataGridNames.SelectedItem as Names).LastName;

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string query = $"Delete from Users1 where id={(dataGridNames.SelectedItem as Names).id} ";
            // as Names toifasiga ugiradi qiymatni 
            // qiymat null bulib qolsa xatoliksiz urniga null quyib utib ketadi
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            connection.Open();

            var result = sqlCommand.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show($"delete {result} items");


        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string result = txbFirstName.Text;
            string query = $"Select *from Users1 where firstname LIKE '%{result}%'";
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            connection.Open();
            var users = new List<Names>();
            var dataResult = sqlCommand.ExecuteReader();

            while (dataResult.Read())
            {
                var user = new Names();
                user.id = dataResult.IsDBNull(0) ? 0 : dataResult.GetInt32(0);
                user.FirstName = dataResult.IsDBNull(1) ? "" : dataResult.GetString(1);
                user.LastName = dataResult.IsDBNull(2) ? " " : dataResult.GetString(2);

                users.Add(user);


            }
            dataGridNames.ItemsSource = null;
            dataGridNames.ItemsSource = users;
            connection.Close();

        }

        private void btnShowData_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();

            string query = "Select *from Users1;";
            names = new List<Names>();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {


                var resultData = cmd.ExecuteReader();

                while (resultData.Read())
                {
                    var name = new Names();
                    name.id = resultData.GetInt32(0);
                    name.FirstName = resultData.IsDBNull(1) ? "null" : resultData.GetString(1);
                    name.LastName = resultData.IsDBNull(2) ? "null" : resultData.GetString(2);

                    names.Add(name);

                }
                connection.Close();
            }

            dataGridNames.ItemsSource = null;
            dataGridNames.ItemsSource = names;


        }
    }
}