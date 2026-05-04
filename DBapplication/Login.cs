using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBapplication
{
    public partial class Login : Form
    {
        DBManager db = new DBManager();
        public Login()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please, enter username and password.");
                return;
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please, enter the missing fields.");
                return;
            }

            string adminQuery = $"SELECT COUNT(*) FROM Admin WHERE Username='{username}' AND Password='{password}'";
            int adminCount = Convert.ToInt32(db.ExecuteScalar(adminQuery));

            if (adminCount > 0)
            {
                string getAdminIDQuery = $"SELECT AdminID FROM Admin WHERE Username='{username}'";
                int adminID = Convert.ToInt32(db.ExecuteScalar(getAdminIDQuery));

                AdminDashboard adminForm = new AdminDashboard(adminID, username);
                adminForm.Show();
                this.Hide();
                return;
            }

            string empCheck = $"SELECT EmployeeID FROM Employee WHERE Username='{username}' AND Password='{password}'";
            object result = db.ExecuteScalar(empCheck);

            if (result != null)
            {
                int empID = Convert.ToInt32(result);

                int isCashier = Convert.ToInt32(db.ExecuteScalar($"SELECT COUNT(*) FROM Cashier WHERE EmployeeID={empID}"));

                if (isCashier > 0)
                {
                    Cashier c = new Cashier(empID);
                    c.Show();
                }
                else
                {
                    Kitchen k = new Kitchen();
                    k.Show();
                }
                this.Hide();
                return;
            }


            string customerQuery = $"SELECT COUNT(*) FROM Customer WHERE Username='{username}' AND Password='{password}'";
            int customerCount = Convert.ToInt32(db.ExecuteScalar(customerQuery));

            if (customerCount > 0)
            {
                string getNameQuery = $"SELECT Name FROM Customer WHERE Username='{username}'";
                string name = db.ExecuteScalar(getNameQuery).ToString();
                Customer customer = new Customer(username, name);
                customer.Show();
                this.Hide();
                return;
            }
            

            MessageBox.Show("Invalid username or password.");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            registerForm.Show();
            this.Hide();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangePassword changePassForm = new ChangePassword();  
            changePassForm.Show();  
            this.Hide();

        }
    }
}
