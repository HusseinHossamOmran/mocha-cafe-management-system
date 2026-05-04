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
    public partial class ChangePassword : Form
    {
        DBManager db = new DBManager();

        public ChangePassword()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string confirmpass = textBox3.Text.Trim();
            if (password != confirmpass)
            {
                MessageBox.Show("Please, confirm a password"); return;
            }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmpass))
            {
                MessageBox.Show("Please, enter the missing fields");
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.");
                return;
            }
            string adminQuery = $"SELECT COUNT(*) FROM Admin WHERE Username='{username}'";
            int adminCount = Convert.ToInt32(db.ExecuteScalar(adminQuery));

            if (adminCount > 0)
            {
                string adminupdateQuery = $"UPDATE Admin SET Password='{password}' WHERE Username='{username}'";
                int adminupdate = db.ExecuteNonQuery(adminupdateQuery);
                if (adminupdate > 0)
                {
                    MessageBox.Show("Admin Password updated successfully!");
                    Login loginForm = new Login();
                    loginForm.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Admin Password update failed.");
                }
                return;

            }
            string checkUsernameQuery = $"SELECT COUNT(*) FROM Customer WHERE Username='{username}'";
            int existsUsername = Convert.ToInt32(db.ExecuteScalar(checkUsernameQuery));
            if (existsUsername == 0)
            {
                MessageBox.Show("username does not exist.");
                return;
            }
            string updateQuery = $"UPDATE Customer SET Password='{password}' WHERE Username='{username}'";
            int update = db.ExecuteNonQuery(updateQuery);

            if (update > 0)
            {
                MessageBox.Show("Password updated successfully!");
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Password update failed.");
            }


        }

    }
}
