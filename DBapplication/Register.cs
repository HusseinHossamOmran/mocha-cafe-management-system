using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBapplication
{
    public partial class Register : Form
    {
        DBManager db = new DBManager();

        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            string username = textBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            string email = textBox4.Text.Trim();
            string confirmpass = textBox5.Text.Trim();

            if (string.IsNullOrEmpty(name)|| string.IsNullOrEmpty(email) ||string.IsNullOrEmpty(username)||string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please, enter the missing fields.");
                return;
            }
            if (password != confirmpass)
            {
                MessageBox.Show("Please, confirm a password"); return;
            }
            if (!email.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address with '@'.");
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.");
                return;
            }
            string checkUsernameQuery = $"SELECT COUNT(*) FROM Customer WHERE Username='{username}'";
            int existsUsername = Convert.ToInt32(db.ExecuteScalar(checkUsernameQuery));
            if (existsUsername > 0)
            {
                MessageBox.Show("username already exists.");
                return;
            }
            string checkEmailQuery = $"SELECT COUNT(*) FROM Customer WHERE Email='{email}'";
            int existsEmail = Convert.ToInt32(db.ExecuteScalar(checkEmailQuery));
            if (existsEmail > 0)
            {
                MessageBox.Show("this email is for another account");
                return;
            }
            string insertQuery = $"INSERT INTO Customer (Name, Email, Username, Password, LoyaltyBalance) VALUES ('{name}', '{email}', '{username}', '{password}', 0)";
            int insert= db.ExecuteNonQuery(insertQuery); //return 1 if row is inserted (affected)

            if (insert > 0)
            {
                MessageBox.Show("Registration successful!");
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
                this.Close();
            }
            else
            {
                MessageBox.Show("Registration failed.");
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
            this.Close();

        }
    }
}
