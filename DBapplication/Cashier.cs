using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace DBapplication
{
    public partial class Cashier : Form
    {
        DBManager db = new DBManager();
        DataTable cartTable = new DataTable();
        int cashierID;

        public Cashier(int empID)
        {
            InitializeComponent();
            cashierID = empID;
            object nameObj = db.ExecuteScalar($"SELECT FName FROM Employee WHERE EmployeeID = {empID}");
            if (nameObj != null)
            {
                lblCashierName.Text = $"Welcome, {nameObj.ToString()}!";
            }
            InitializeCart();
            LoadBranches();
            LoadCustomers();
            LoadMenu(); 
            cmbPayment.SelectedIndex = 0;
        }
        private void InitializeCart()
        {
            cartTable.Columns.Add("ItemID", typeof(int));
            cartTable.Columns.Add("ItemName", typeof(string));
            cartTable.Columns.Add("Price", typeof(decimal));
            cartTable.Columns.Add("Quantity", typeof(int));
            cartTable.Columns.Add("Total", typeof(decimal));
            gridCart.DataSource = cartTable;
        }
        private void LoadBranches()
        {
            string query = "SELECT BranchID, Name FROM Branch";
            DataTable dt = db.ExecuteReader(query);
            cmbBranch.DataSource = dt;
            cmbBranch.DisplayMember = "Name";
            cmbBranch.ValueMember = "BranchID";
        }
        private void LoadMenu()
        {
            if (cmbBranch.SelectedValue == null) return;

            int branchID = Convert.ToInt32(cmbBranch.SelectedValue);
            string query = $"SELECT MenuItem.ItemID, MenuItem.ItemName, MenuItem.Price, AvailableAt.Quantity " +
                           $"FROM MenuItem " +
                           $"JOIN AvailableAt ON MenuItem.ItemID = AvailableAt.ItemID " +
                           $"WHERE AvailableAt.BranchID = {branchID} AND AvailableAt.Quantity > 0";

            DataTable dt = db.ExecuteReader(query);
            gridMenu.DataSource = dt;
        }
        private void LoadCustomers()
        {
            string query = "SELECT CustomerID, Name FROM Customer";
            DataTable dt = db.ExecuteReader(query);

            cmbCustomers.DataSource = dt;
            cmbCustomers.DisplayMember = "Name";
            cmbCustomers.ValueMember = "CustomerID";
        }

        private void gridMenu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(gridMenu.Rows[e.RowIndex].Cells["ItemID"].Value);
            string name = gridMenu.Rows[e.RowIndex].Cells["ItemName"].Value.ToString();
            decimal price = Convert.ToDecimal(gridMenu.Rows[e.RowIndex].Cells["Price"].Value);

            cartTable.Rows.Add(id, name, price, 1, price);

            UpdateTotal();
        }
        private void UpdateTotal()
        {
            decimal sum = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                sum += Convert.ToDecimal(row["Total"]);
            }
            lblTotal.Text = sum.ToString("0.00");
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0) return;
            if (cmbCustomers.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer first!");
                return;
            }
            if(cmbBranch.SelectedValue == null)
            {
                MessageBox.Show("Please select a branch first!");
                return;
            }
            if (string.IsNullOrEmpty(cmbPayment.Text))
            {
                MessageBox.Show("Please select a payment method (Cash/Credit)!");
                return;
            }

            int customerID = Convert.ToInt32(cmbCustomers.SelectedValue);
            int branchID = Convert.ToInt32(cmbBranch.SelectedValue);
            decimal subTotal = Convert.ToDecimal(lblTotal.Text);

            decimal discount = 0;
            int pointsUsed = 0;

            if (chkUsePoints.Checked)
            {
                object pointObj = db.ExecuteScalar($"SELECT LoyaltyBalance FROM Customer WHERE CustomerID={customerID}");
                int currentPoints = (pointObj != null) ? Convert.ToInt32(pointObj) : 0;

                decimal maxDiscountPossible = (decimal)currentPoints / 100;

                if (maxDiscountPossible >= subTotal)
                {
                    discount = subTotal;
                    pointsUsed = (int)(subTotal * 100);
                }
                else
                {
                    discount = maxDiscountPossible;
                    pointsUsed = currentPoints;
                }
            }

            decimal finalTotal = subTotal - discount;
            string paymentMethod = cmbPayment.Text;

            if (finalTotal == 0 && pointsUsed > 0)
            {
                paymentMethod = "Loyalty";
            }

            string orderQ = "INSERT INTO Orders (OrderDate, Status, SubTotal, Discount, TotalAmount, PaymentMethod, CustomerID, BranchID, CashierID, BaristaID) " +
                 $"VALUES (GETDATE(), 'Pending', {subTotal}, {discount}, {finalTotal}, '{paymentMethod}', {customerID}, {branchID}, {cashierID}, NULL); " +
                 "SELECT SCOPE_IDENTITY();";

            object res = db.ExecuteScalar(orderQ);

            if (res != null)
            {
                int orderID = Convert.ToInt32(res);

                foreach (DataRow row in cartTable.Rows)
                {
                    int itemID = (int)row["ItemID"];
                    decimal unitPrice = Convert.ToDecimal(row["Price"]);
                    int quantity = Convert.ToInt32(row["Quantity"]);
                    decimal lineTotal = unitPrice * quantity;

                    string detailQ = "INSERT INTO OrderDetails (OrderID, ItemID, Quantity, UnitPrice, LineTotal)" +
                                     $"VALUES ({orderID}, {itemID}, {quantity}, {unitPrice}, {lineTotal})";
                    db.ExecuteNonQuery(detailQ);

                    db.ExecuteNonQuery($"UPDATE AvailableAt SET Quantity = Quantity - {quantity} WHERE ItemID={itemID} AND BranchID={branchID}");
                }

                int pointsEarned = (int)(finalTotal / 10);
                string updatePointsQ = $"UPDATE Customer SET LoyaltyBalance = LoyaltyBalance - {pointsUsed} + {pointsEarned} WHERE CustomerID={customerID}";
                db.ExecuteNonQuery(updatePointsQ);

                MessageBox.Show($"Order Placed at {cmbBranch.Text}!\n" +
                                $"Paid: ${finalTotal} via {paymentMethod}\n" +
                                $"Points Used: {pointsUsed}\n" +
                                $"Points Earned: {pointsEarned}");

                cartTable.Rows.Clear();
                lblTotal.Text = "0.00";
                chkUsePoints.Checked = false;
                LoadMenu(); 
            }
        }
        

        private void cmbBranch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cartTable.Rows.Clear();
            lblTotal.Text = "0.00";
            LoadMenu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }

        private void Cashier_Load(object sender, EventArgs e)
        {

        }
    }
}
