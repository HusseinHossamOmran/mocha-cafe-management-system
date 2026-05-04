using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBapplication
{
    public partial class Customer : Form
    {
        DBManager db = new DBManager();
        private string username;      
        private string customerName;
        private int customerID;
        public Customer(string user, string name)
        {
            InitializeComponent();
            username = user;
            customerName = name;
        }
        private void LoadMenuItems()
        {
            string query = "SELECT ItemName, Category, Price FROM MenuItem";
            DataTable dt = db.ExecuteReader(query);

            if (dt == null|| dt.Rows.Count == 0)
            {
                MessageBox.Show("No menu items found.");
                return;
            }
            dataGridView2.AutoGenerateColumns = true;
            dataGridView2.DataSource = dt;
           

        }
        private void LoadOrders()
        {
            string query = "SELECT OrderDate, Status, PaymentMethod, TotalAmount FROM Orders";
            DataTable dt = db.ExecuteReader(query);
            if (dt == null|| dt.Rows.Count == 0)
            {
                MessageBox.Show("No orders found.");
                return;
            }
            dataGridView1.AutoGenerateColumns = true; 
            dataGridView1.DataSource = dt;
        }
        private void LoadLoyaltyBalance()
        {
            string query = $"SELECT LoyaltyBalance FROM Customer WHERE Username='{username}'";
            int balance = Convert.ToInt32(db.ExecuteScalar(query) ?? 0);

            if (balance != null)
            {
                label4.Text = $"Loyalty Points: {balance}";
                if (balance >= 1000)
                {
                    label5.Text = "Tier: Gold";
                }
                else if (balance >= 500)
                {
                    label5.Text = "Tier: Silver";
                }
                else
                {
                    label5.Text = "Tier: Bronze"; 
                }

            }
            else
            {
                label4.Text = "Loyalty Points: 0";
                label5.Text = "Tier: Bronze";
            }
        }


        private void Customer_Load(object sender, EventArgs e)
        {
            label1.Text = $"Welcome Back, {customerName}!";
            string query = $"SELECT CustomerID FROM Customer WHERE Username = '{username}'";
            object ID = db.ExecuteScalar(query);

            if (ID != null)
            {
                customerID = Convert.ToInt32(ID);
            }
            else
            {
                MessageBox.Show("Unable to find customer ID.");
            }

            LoadMenuItems();
            LoadOrders();
            LoadLoyaltyBalance();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (customerID > 0)
            {
                Feedback feedbackForm = new Feedback(customerID);
                feedbackForm.Show();
            }
            else
            {
                MessageBox.Show("Unable to find customer ID.");
            }


        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
