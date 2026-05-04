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
    public partial class Kitchen : Form
    {
        DBManager db = new DBManager();
        public Kitchen()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadOrders();
        }
        private void LoadOrders()
        {
            string query = "SELECT OrderID, Status, OrderDate, SubTotal FROM Orders WHERE Status != 'Completed'";
            DataTable dt = db.ExecuteReader(query);
            gridOrders.DataSource = dt;
        }

        private void btnReady_Click(object sender, EventArgs e)
        {
            UpdateOrderStatus("Ready");
        }

        private void btnPreparing_Click(object sender, EventArgs e)
        {
            UpdateOrderStatus("Preparing");
        }

        private void btnPickup_Click(object sender, EventArgs e)
        {
            UpdateOrderStatus("Completed");
        }
        private void UpdateOrderStatus(string newStatus)
        {
            if (gridOrders.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(gridOrders.SelectedRows[0].Cells["OrderID"].Value);
                string currentStatus = gridOrders.SelectedRows[0].Cells["Status"].Value.ToString();

                if (newStatus == "Preparing" && currentStatus != "Pending")
                {
                    MessageBox.Show("This order is already being prepared or is ready!");
                    return;
                }
                if(newStatus == "Ready" && currentStatus != "Preparing")
                {
                    MessageBox.Show("This order is not being prepared yet!");
                    return;
                }
                if(newStatus == "Completed" && currentStatus != "Ready")
                {
                    MessageBox.Show("This order is not ready for pickup yet!");
                    return;
                }
                string query = $"UPDATE Orders SET Status='{newStatus}' WHERE OrderID={id}";
                db.ExecuteNonQuery(query);
                LoadOrders();
            }
            else
            {
                MessageBox.Show("Please select an order first!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }
    }
}
