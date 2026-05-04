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
    public partial class AdminDashboard : Form
    {
        Controller controllerObj;
        int adminID;
        string adminUsername;
        public AdminDashboard(int id, string username)
        {
            InitializeComponent();
            controllerObj = new Controller();
          adminID = id;
           adminUsername = username;
           // lblWelcome.Text = "Welcome, " + controllerObj.GetAdminName(username);

            // Initialize all tabs
            LoadBranches();
            LoadMenu();
            LoadBranchesForInventory();
            InitializeEmployeeTab();
        }

        // ============ BRANCH MANAGEMENT ============

        private void LoadBranches()
        {
            DataTable dt = controllerObj.GetAllBranches();
            if (dt != null)
            {
                dgvBranches.DataSource = dt;
                dgvBranches.ReadOnly = true;
                dgvBranches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an employee to delete", "Selection Required");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this employee?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                int employeeID = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["EmployeeID"].Value);
                int result = controllerObj.DeleteEmployee(employeeID);

                if (result > 0)
                {
                    MessageBox.Show("Employee deleted successfully!", "Success");
                    ClearEmployeeFields();
                    LoadEmployees();
                }
                else if (result == -1)
                {
                    MessageBox.Show("Cannot delete employee: They have orders assigned to them.\nEmployees with order history cannot be deleted.", "Error");
                }
                else
                {
                    MessageBox.Show("Failed to delete employee", "Error");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadEmployees();
            ClearEmployeeFields();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reports reportsForm = new Reports();
            reportsForm.Show();
            this.Hide();
        }

        private void btnAddBranch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter branch name", "Validation Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter branch location", "Validation Error");
                return;
            }
            int result = controllerObj.AddBranch(
                txtName.Text.Trim(),
                txtLocation.Text.Trim(),
                txtBranchPhone.Text.Trim(),
                adminID  // ← ADD THIS!
            );

            if (result > 0)
            {
                MessageBox.Show("Branch added successfully!", "Success");
                ClearBranchFields();
                LoadBranches();
                LoadBranchesForInventory();
                LoadBranchesForEmployees();
            }
            else
            {
                MessageBox.Show("Failed to add branch", "Error");
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdateBranch_Click(object sender, EventArgs e)
        {
            if (dgvBranches.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a branch to update", "Selection Required");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter branch name", "Validation Error");
                return;
            }

            int branchID = Convert.ToInt32(dgvBranches.SelectedRows[0].Cells["BranchID"].Value);

            int result = controllerObj.UpdateBranch(
                branchID,
                txtName.Text.Trim(),
                txtLocation.Text.Trim(),
                txtBranchPhone.Text.Trim(),
                adminID  ///////////////

            );

            if (result > 0)
            {
                MessageBox.Show("Branch updated successfully!", "Success");
                ClearBranchFields();
                LoadBranches();
            }
            else
            {
                MessageBox.Show("Failed to update branch", "Error");
            }
        }
        private void dgvBranches_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        
            if (dgvBranches.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvBranches.SelectedRows[0];
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtLocation.Text = row.Cells["Location"].Value.ToString();
                txtBranchPhone.Text = row.Cells["Phone"].Value.ToString();
            }
        }

        private void btnDeleteBranch_Click(object sender, EventArgs e)
        {
            if (dgvBranches.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a branch to delete", "Selection Required");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "WARNING: This will delete:\n" +
                "• The branch\n" +
                "• All employees at this branch\n" +
                "• All inventory at this branch\n\n" +
                "Are you sure you want to continue?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                int branchID = Convert.ToInt32(dgvBranches.SelectedRows[0].Cells["BranchID"].Value);
                int result = controllerObj.DeleteBranch(branchID);

                if (result > 0)
                {
                    MessageBox.Show("Branch and all related data deleted successfully!", "Success");
                    ClearBranchFields();
                    LoadBranches();
                    LoadBranchesForInventory();
                    LoadBranchesForEmployees(); // Refresh employee dropdown
                }
                else if (result == -3)
                {
                    MessageBox.Show("Cannot delete branch: It has order history.\n" +
                                  "Branches with orders cannot be deleted for record-keeping purposes.", "Error");
                }
                else
                {
                    MessageBox.Show("Failed to delete branch.", "Error");
                }
            }
        }

        private void btnRefreshBranches_Click(object sender, EventArgs e)
        {
            LoadBranches();
            ClearBranchFields();
        }
        private void ClearBranchFields()
        {
            txtName.Clear();
            txtLocation.Clear();
            txtBranchPhone.Clear();
        }
        // ============ MENU MANAGEMENT ============

        private void LoadMenu()
        {
            DataTable dt = controllerObj.GetAllMenuItems();
            if (dt != null)
            {
                dgvMenu.DataSource = dt;
                dgvMenu.ReadOnly = true;
                dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text))
            {
                MessageBox.Show("Please enter item name", "Validation Error");
                return;
            }

            if (cboCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category", "Validation Error");
                return;
            }

            if (!decimal.TryParse(txtItemPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price", "Validation Error");
                return;
            }


            int result = controllerObj.AddMenuItem(
                txtItemName.Text.Trim(),
                cboCategory.SelectedItem.ToString(),
                price,
                   adminID
            );

            if (result > 0)
            {
                MessageBox.Show("Menu item added successfully!", "Success");
                ClearMenuFields();
                LoadMenu();
            }
            else
            {
                MessageBox.Show("Failed to add menu item", "Error");
            }
        }

        private void btnUpdateItem_Click(object sender, EventArgs e)
        {

            if (dgvMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a menu item to update", "Selection Required");
                return;
            }

            if (cboCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category", "Validation Error");
                return;
            }

            if (!decimal.TryParse(txtItemPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price", "Validation Error");
                return;
            }

            int itemID = Convert.ToInt32(dgvMenu.SelectedRows[0].Cells["ItemID"].Value);


            int result = controllerObj.UpdateMenuItem(
                itemID,
                txtItemName.Text.Trim(),
                cboCategory.SelectedItem.ToString(),
                price,
                   adminID
            );

            if (result > 0)
            {
                MessageBox.Show("Menu item updated successfully!", "Success");
                ClearMenuFields();
                LoadMenu();
            }
            else
            {
                MessageBox.Show("Failed to update menu item", "Error");
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (dgvMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a menu item to delete", "Selection Required");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this menu item?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                int itemID = Convert.ToInt32(dgvMenu.SelectedRows[0].Cells["ItemID"].Value);
                int result = controllerObj.DeleteMenuItem(itemID);

                if (result > 0)
                {
                    MessageBox.Show("Menu item deleted successfully!", "Success");
                    ClearMenuFields();
                    LoadMenu();
                }
                else
                {
                    MessageBox.Show("Failed to delete menu item. It may be used in orders.", "Error");
                }
            }
        }
        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMenu.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvMenu.SelectedRows[0];
                txtItemName.Text = row.Cells["ItemName"].Value.ToString();
                cboCategory.SelectedItem = row.Cells["Category"].Value.ToString();
                txtItemPrice.Text = row.Cells["Price"].Value.ToString();
            }
        }
       

        private void ClearMenuFields()
        {
            txtItemName.Clear();
            cboCategory.SelectedIndex = -1;
            txtItemPrice.Clear();
        }

        private void btnRefreshMenu_Click_1(object sender, EventArgs e)
        {
            LoadMenu();
            ClearMenuFields();
        }
        // ============ INVENTORY MANAGEMENT ============

        private void LoadBranchesForInventory()
        {
            DataTable dt = controllerObj.GetAllBranches();
            if (dt != null)
            {
                cboBranches.DataSource = dt;
                cboBranches.DisplayMember = "Location";
                cboBranches.ValueMember = "BranchID";

                if (cboBranches.Items.Count > 0)
                {
                    cboBranches.SelectedIndex = 0;
                    LoadInventory();
                }
            }
        }
   
        private void LoadInventory()
        {
            if (cboBranches.SelectedItem == null || cboBranches.SelectedIndex < 0)
                return;

            try
            {
                DataRowView selectedRow = (DataRowView)cboBranches.SelectedItem;
                int branchID = Convert.ToInt32(selectedRow["BranchID"]);
                DataTable dt = controllerObj.GetInventoryByBranch(branchID);

                if (dt != null)
                {
                    
                    if (dt.Columns.Contains("Quantity"))
                    {
                        dt.Columns["Quantity"].ReadOnly = false;
                    }

                    dgvInventory.DataSource = dt;

                    // Make only Quantity column editable in DataGridView
                    foreach (DataGridViewColumn col in dgvInventory.Columns)
                    {
                        col.ReadOnly = (col.Name != "Quantity");
                    }

                    dgvInventory.SelectionMode = DataGridViewSelectionMode.CellSelect;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory: " + ex.Message, "Error");
            }
        }

        private void cboBranches_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboBranches.SelectedIndex >= 0)
            {
                LoadInventory();
            }
        }

        private void btnSaveInventory_Click(object sender, EventArgs e)
        {
            if (cboBranches.SelectedValue == null) return;

            int branchID = Convert.ToInt32(cboBranches.SelectedValue);
            int successCount = 0;
            int errorCount = 0;

            foreach (DataGridViewRow row in dgvInventory.Rows)
            {
                if (row.IsNewRow) continue;

                int itemID = Convert.ToInt32(row.Cells["ItemID"].Value);
                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);

                int result = controllerObj.SetInventoryQuantity(branchID, itemID, quantity);
                if (result > 0)
                    successCount++;
                else
                    errorCount++;
            }

            MessageBox.Show(
                $"Inventory updated!\nSuccess: {successCount}\nFailed: {errorCount}",
                "Update Complete"
            );

            LoadInventory();
        }

        private void btnRefreshInventory_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }

        // ============ EMPLOYEE MANAGEMENT ============

        private void LoadEmployees()
        {
            DataTable dt = controllerObj.GetAllEmployees();
            if (dt != null)
            {
                dgvEmployees.DataSource = dt;
                dgvEmployees.ReadOnly = true;
                dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }
        private void LoadBranchesForEmployees()
        {
            DataTable dt = controllerObj.GetAllBranches();
            if (dt != null)
            {
                cboEmpBranch.DataSource = dt;
                cboEmpBranch.DisplayMember = "Location";
                cboEmpBranch.ValueMember = "BranchID";
            }
        }
        private void InitializeEmployeeTab()
        {
            LoadBranchesForEmployees();
            LoadEmployees();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmpFName.Text) ||
              string.IsNullOrWhiteSpace(txtEmpLName.Text) ||
              string.IsNullOrWhiteSpace(txtEmpUsername.Text) ||
              string.IsNullOrWhiteSpace(txtEmpPassword.Text))
            {
                MessageBox.Show("Please fill all required fields", "Validation Error");
                return;
            }

            if (cboEmployeeType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select employee type (Cashier or Barista)", "Validation Error");
                return;
            }

            if (cboEmpBranch.SelectedItem == null)
            {
                MessageBox.Show("Please select a branch", "Validation Error");
                return;
            }

            try
            {
                DataRowView selectedBranch = (DataRowView)cboEmpBranch.SelectedItem;
                int branchID = Convert.ToInt32(selectedBranch["BranchID"]);

                int result = controllerObj.AddEmployee(
                    txtEmpFName.Text.Trim(),
                    txtEmpLName.Text.Trim(),
                    txtEmpUsername.Text.Trim(),
                    txtEmpPassword.Text,
                    branchID,
                    cboEmployeeType.SelectedItem.ToString()
                );

                if (result > 0)
                {
                    MessageBox.Show("Employee added successfully!", "Success");
                    ClearEmployeeFields();
                    LoadEmployees();
                }
                else if (result == -1)
                {
                    MessageBox.Show("Username already exists! Please choose a different username.", "Error");
                }
                else
                {
                    MessageBox.Show("Failed to add employee", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }

        private void btnUpdateEmployee_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an employee to update", "Selection Required");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmpFName.Text) ||
                string.IsNullOrWhiteSpace(txtEmpLName.Text))
            {
                MessageBox.Show("Please fill required fields", "Validation Error");
                return;
            }

            if (cboEmployeeType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select employee type", "Validation Error");
                return;
            }

            if (cboEmpBranch.SelectedItem == null)
            {
                MessageBox.Show("Please select a branch", "Validation Error");
                return;
            }

            int employeeID = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["EmployeeID"].Value);
            DataRowView selectedBranch = (DataRowView)cboEmpBranch.SelectedItem;
            int branchID = Convert.ToInt32(selectedBranch["BranchID"]);

            int result = controllerObj.UpdateEmployee(
                employeeID,
                txtEmpFName.Text.Trim(),
                txtEmpLName.Text.Trim(),
                branchID,
                cboEmployeeType.SelectedItem.ToString()
            );

            if (result > 0)
            {
                MessageBox.Show("Employee updated successfully!", "Success");
                ClearEmployeeFields();
                LoadEmployees();
            }
            else
            {
                MessageBox.Show("Failed to update employee", "Error");
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow row = dgvEmployees.SelectedRows[0];

                    string fullName = row.Cells["EmployeeName"].Value.ToString();
                    string[] nameParts = fullName.Split(' ');

                    txtEmpFName.Text = nameParts.Length > 0 ? nameParts[0] : "";
                    txtEmpLName.Text = nameParts.Length > 1 ? nameParts[1] : "";
                    txtEmpUsername.Text = row.Cells["Username"].Value.ToString();
                    cboEmployeeType.SelectedItem = row.Cells["EmployeeType"].Value.ToString();

                    int branchID = Convert.ToInt32(row.Cells["BranchID"].Value);
                    for (int i = 0; i < cboEmpBranch.Items.Count; i++)
                    {
                        DataRowView item = (DataRowView)cboEmpBranch.Items[i];
                        if (Convert.ToInt32(item["BranchID"]) == branchID)
                        {
                            cboEmpBranch.SelectedIndex = i;
                            break;
                        }
                    }

                    txtEmpPassword.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading employee data: " + ex.Message, "Error");
                }
            }
        }

        private void ClearEmployeeFields()
        {
            txtEmpFName.Clear();
            txtEmpLName.Clear();
            txtEmpUsername.Clear();
            txtEmpPassword.Clear();
            cboEmployeeType.SelectedIndex = -1;
            if (cboEmpBranch.Items.Count > 0)
                cboEmpBranch.SelectedIndex = 0;
        }

        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
