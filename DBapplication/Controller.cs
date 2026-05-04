using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBapplication
{
    public class Controller
    {
        DBManager dbMan;

        public Controller()
        {
            dbMan = new DBManager();
        }

        public void TerminateConnection()
        {
            dbMan.CloseConnection();
        }

        // ============================================
        // ADMIN AUTHENTICATION
        // ============================================

        public int AdminLogin(string username, string password)
        {

            //try
            //{
            //    MessageBox.Show("AdminLogin called with username: '" + username + "'", "DEBUG: Method Start");

            //    string query = "SELECT COUNT(*) FROM Admin WHERE Username = '" + username + "'";
            //    MessageBox.Show("Executing query:\n" + query, "DEBUG: Query");

            //    object countResult = dbMan.ExecuteScalar(query);

            //    string resultStr = countResult == null ? "NULL" :
            //                      countResult == DBNull.Value ? "DBNull" :
            //                      countResult.ToString();

            //    MessageBox.Show("ExecuteScalar returned: " + resultStr + "\nType: " + (countResult?.GetType().Name ?? "null"), "DEBUG: Result");

            //    if (countResult == null || countResult == DBNull.Value)
            //    {
            //        MessageBox.Show("Result is NULL or DBNull!", "DEBUG: NULL Check");
            //        return 0;
            //    }

            //    int count = Convert.ToInt32(countResult);
            //    MessageBox.Show("Count converted to: " + count, "DEBUG: Count");

            //    if (count == 0)
            //    {
            //        MessageBox.Show("No user found!", "DEBUG: No User");
            //        return 0;
            //    }

            //    // Get password
            //    string passQuery = "SELECT Password FROM Admin WHERE Username = '" + username + "'";
            //    MessageBox.Show("Getting password with query:\n" + passQuery, "DEBUG: Password Query");

            //    object passResult = dbMan.ExecuteScalar(passQuery);

            //    if (passResult == null || passResult == DBNull.Value)
            //    {
            //        MessageBox.Show("Password is NULL!", "DEBUG: NULL Password");
            //        return 0;
            //    }

            //    string storedPass = passResult.ToString();
            //    MessageBox.Show(
            //        "Stored: '" + storedPass + "' (len:" + storedPass.Length + ")\n" +
            //        "Entered: '" + password + "' (len:" + password.Length + ")\n" +
            //        "Match: " + (storedPass == password),
            //        "DEBUG: Password Compare"
            //    );

            //    if (storedPass == password)
            //    {
            //        MessageBox.Show("PASSWORDS MATCH - Login SUCCESS!", "DEBUG: Success");
            //        return 1;
            //    }
            //    else
            //    {
            //        MessageBox.Show("PASSWORDS DON'T MATCH!", "DEBUG: Fail");
            //        return -1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("EXCEPTION: " + ex.Message + "\n\nStack:\n" + ex.StackTrace, "DEBUG: Exception");
            //    return 0;
            //}


            string query = "SELECT COUNT(*) FROM Admin WHERE Username = '" + username + "'";
            object countResult = dbMan.ExecuteScalar(query);

            if (countResult == null || countResult == DBNull.Value)
                return 0;

            int count = Convert.ToInt32(countResult);

            if (count == 0)
                return 0; // Username doesn't exist

            string passQuery = "SELECT Password FROM Admin WHERE Username = '" + username + "'";
            object result = dbMan.ExecuteScalar(passQuery);

            if (result == null || result == DBNull.Value)
                return 0;

            string storedPass = result.ToString();

            if (storedPass == password)
                return 1; // Login successful
            else
                return -1; // Wrong password
        }

        public int GetAdminID(string username)
        {
            string query = "SELECT AdminID FROM Admin WHERE Username = '" + username + "'";
            object result = dbMan.ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public string GetAdminName(string username)
        {
            // CORRECTED: Uses FName + LName instead of Name
            string query = "SELECT FName + ' ' + LName FROM Admin WHERE Username = '" + username + "'";
            object result = dbMan.ExecuteScalar(query);
            return result != null ? result.ToString() : "Admin";
        }

        ////claraaaa//

        // BRANCH MANAGEMENT


        public DataTable GetAllBranches()
        {
            string query = "SELECT BranchID, Name, Location, Phone FROM Branch ORDER BY BranchID";
            return dbMan.ExecuteReader(query);
        }

        public int AddBranch(string name, string location, string phone, int adminID)
        {
            string query = "INSERT INTO Branch (Name, Location, Phone, AdminID) VALUES ('" +
                           name + "', '" + location + "', '" + phone + "', " + adminID + ")";
            return dbMan.ExecuteNonQuery(query);
        }
        public int UpdateBranch(int branchID, string name, string location, string phone, int adminID)
        {
            string query = "UPDATE Branch SET Name = '" + name + "', Location = '" + location + "', " +
                           "Phone = '" + phone + "', AdminID = " + adminID + " " +
                           "WHERE BranchID = " + branchID;
            return dbMan.ExecuteNonQuery(query);
        }

        public int DeleteBranch(int branchID)
        {
            try
            {
                // Check if branch has orders - CANNOT delete if there are orders
                string checkOrders = "SELECT COUNT(*) FROM [Order] WHERE BranchID = " + branchID;
                object ordCount = dbMan.ExecuteScalar(checkOrders);
                int orderCount = ordCount != null ? Convert.ToInt32(ordCount) : 0;

                if (orderCount > 0)
                {
                    // Can't delete - has order history
                    return -3; // Special code for "has orders"
                }

                // Delete employees at this branch FIRST
                string deleteEmployees = "DELETE FROM Employee WHERE BranchID = " + branchID;
                dbMan.ExecuteNonQuery(deleteEmployees);

                // Delete inventory at this branch
                string deleteInventory = "DELETE FROM AvailableAt WHERE BranchID = " + branchID;
                dbMan.ExecuteNonQuery(deleteInventory);

                // Finally delete the branch itself
                string query = "DELETE FROM Branch WHERE BranchID = " + branchID;
                return dbMan.ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting branch: " + ex.Message, "Error");
                return 0;
            }
        }


        public DataTable GetBranchByID(int branchID)
        {
            // CORRECTED: Removed Name field
            string query = "SELECT BranchID, Location, Phone FROM Branch WHERE BranchID = " + branchID;
            return dbMan.ExecuteReader(query);
        }

     
        // MENU ITEM MANAGEMENT
     

        public DataTable GetAllMenuItems()
        {

            string query = "SELECT ItemID, ItemName, Category, Price FROM MenuItem ORDER BY ItemName";
            return dbMan.ExecuteReader(query);
        }

        public int AddMenuItem(string itemName, string category, decimal price, int adminID)
        {
            string query = "INSERT INTO MenuItem (ItemName, Category, Price, AdminID) " +
                           "VALUES ('" + itemName + "', '" + category + "', " + price + ", " + adminID + ")";
            return dbMan.ExecuteNonQuery(query);
        }

        public int UpdateMenuItem(int itemID, string itemName, string category, decimal price, int adminID)
        {
            string query = "UPDATE MenuItem SET ItemName = '" + itemName + "', " +
                           "Category = '" + category + "', Price = " + price + ", " +
                           "AdminID = " + adminID + " " +
                           "WHERE ItemID = " + itemID;
            return dbMan.ExecuteNonQuery(query);
        }

        public int DeleteMenuItem(int itemID)
        {
            // Check if item is in any inventory
            string checkQuery = "SELECT COUNT(*) FROM AvailableAt WHERE ItemID = " + itemID;
            object countResult = dbMan.ExecuteScalar(checkQuery);
            int inventoryCount = countResult != null ? Convert.ToInt32(countResult) : 0;

            if (inventoryCount > 0)
            {
                // Delete from inventory first
                string deleteInventory = "DELETE FROM AvailableAt WHERE ItemID = " + itemID;
                dbMan.ExecuteNonQuery(deleteInventory);
            }

            string query = "DELETE FROM MenuItem WHERE ItemID = " + itemID;
            return dbMan.ExecuteNonQuery(query);
        }

        public DataTable GetMenuItemByID(int itemID)
        {
            string query = "SELECT ItemID, ItemName, Price FROM MenuItem WHERE ItemID = " + itemID;
            return dbMan.ExecuteReader(query);
        }

     
        // INVENTORY MANAGEMENT (AvailableAt)
       

        public DataTable GetInventoryByBranch(int branchID)
        {
            string query = "SELECT M.ItemID, M.ItemName, M.Price, ISNULL(A.Quantity, 0) AS Quantity " +
                           "FROM MenuItem M " +
                           "LEFT JOIN AvailableAt A ON M.ItemID = A.ItemID AND A.BranchID = " + branchID + " " +
                           "ORDER BY M.ItemName";
            return dbMan.ExecuteReader(query);
        }

        public DataTable GetAllInventory()
        {
            string query = "SELECT B.BranchID, B.Location, M.ItemID, M.ItemName, M.Price, " +
                           "ISNULL(A.Quantity, 0) AS Quantity " +
                           "FROM Branch B " +
                           "CROSS JOIN MenuItem M " +
                           "LEFT JOIN AvailableAt A ON B.BranchID = A.BranchID AND M.ItemID = A.ItemID " +
                           "ORDER BY B.BranchID, M.ItemName";
            return dbMan.ExecuteReader(query);
        }

        public int SetInventoryQuantity(int branchID, int itemID, int quantity)
        {
            // Check if record exists
            string checkQuery = "SELECT COUNT(*) FROM AvailableAt " +
                               "WHERE BranchID = " + branchID + " AND ItemID = " + itemID;
            object countResult = dbMan.ExecuteScalar(checkQuery);
            int exists = countResult != null ? Convert.ToInt32(countResult) : 0;

            if (exists > 0)
            {
                if (quantity > 0)
                {
                    // Update existing record
                    string updateQuery = "UPDATE AvailableAt SET Quantity = " + quantity + " " +
                                       "WHERE BranchID = " + branchID + " AND ItemID = " + itemID;
                    return dbMan.ExecuteNonQuery(updateQuery);
                }
                else
                {
                    // Delete if quantity is 0
                    string deleteQuery = "DELETE FROM AvailableAt " +
                                       "WHERE BranchID = " + branchID + " AND ItemID = " + itemID;
                    return dbMan.ExecuteNonQuery(deleteQuery);
                }
            }
            else
            {
                if (quantity > 0)
                {
                    // Insert new record
                    string insertQuery = "INSERT INTO AvailableAt (BranchID, ItemID, Quantity) " +
                                       "VALUES (" + branchID + ", " + itemID + ", " + quantity + ")";
                    return dbMan.ExecuteNonQuery(insertQuery);
                }
                return 0;
            }
        }

        public int GetItemQuantityAtBranch(int branchID, int itemID)
        {
            string query = "SELECT ISNULL(Quantity, 0) FROM AvailableAt " +
                          "WHERE BranchID = " + branchID + " AND ItemID = " + itemID;
            object result = dbMan.ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public DataTable GetOutOfStockItems(int branchID)
        {
            string query = "SELECT M.ItemID, M.ItemName, M.Price " +
                          "FROM MenuItem M " +
                          "LEFT JOIN AvailableAt A ON M.ItemID = A.ItemID AND A.BranchID = " + branchID + " " +
                          "WHERE ISNULL(A.Quantity, 0) = 0 " +
                          "ORDER BY M.ItemName";
            return dbMan.ExecuteReader(query);
        }

        public DataTable GetLowStockItems(int branchID, int threshold)
        {
            string query = "SELECT M.ItemID, M.ItemName, M.Price, A.Quantity " +
                          "FROM MenuItem M " +
                          "INNER JOIN AvailableAt A ON M.ItemID = A.ItemID " +
                          "WHERE A.BranchID = " + branchID + " AND A.Quantity > 0 AND A.Quantity < " + threshold + " " +
                          "ORDER BY A.Quantity ASC";
            return dbMan.ExecuteReader(query);
        }

      
        public DataTable GetBranchInventory(int branchID)
        {
            return GetInventoryByBranch(branchID);
        }

        public int UpdateInventory(int branchID, int itemID, int quantity)
        {
            return SetInventoryQuantity(branchID, itemID, quantity);
        }

        // EMPLOYEE MANAGEMENT





        public DataTable GetAllEmployees()
        {
            string query = @"
        SELECT E.EmployeeID, 
               E.FName + ' ' + E.LName AS EmployeeName, 
               E.Username, 
               CASE 
                   WHEN C.EmployeeID IS NOT NULL THEN 'Cashier'
                   WHEN B.EmployeeID IS NOT NULL THEN 'Barista'
                   ELSE 'Unknown'
               END AS EmployeeType,
               BR.Name AS BranchName, 
               E.BranchID 
        FROM Employee E 
        LEFT JOIN Cashier C ON E.EmployeeID = C.EmployeeID
        LEFT JOIN Barista B ON E.EmployeeID = B.EmployeeID
        INNER JOIN Branch BR ON E.BranchID = BR.BranchID 
        ORDER BY E.EmployeeID";
            return dbMan.ExecuteReader(query);
        }

        public int AddEmployee(string fName, string lName, string username, string password,
                         int branchID, string employeeType)
        {
            // Check if username exists
            string checkQuery = "SELECT COUNT(*) FROM Employee WHERE Username = '" + username + "'";
            object checkResult = dbMan.ExecuteScalar(checkQuery);
            if (checkResult != null && Convert.ToInt32(checkResult) > 0)
                return -1;

            //  Insert into Employee table 
            string query = "INSERT INTO Employee (FName, LName, Username, Password, BranchID) " +
                           "OUTPUT INSERTED.EmployeeID " +
                           "VALUES ('" + fName + "', '" + lName + "', '" + username + "', '" +
                           password + "', " + branchID + ")";

            object result = dbMan.ExecuteScalar(query);
            if (result == null)
                return 0;

            int newEmployeeID = Convert.ToInt32(result);

            // Insert into specific role table based on selection
            string roleQuery = "";
            if (employeeType == "Cashier")
            {
                roleQuery = "INSERT INTO Cashier (EmployeeID) VALUES (" + newEmployeeID + ")";
            }
            else if (employeeType == "Barista")
            {
                roleQuery = "INSERT INTO Barista (EmployeeID) VALUES (" + newEmployeeID + ")";
            }

            if (!string.IsNullOrEmpty(roleQuery))
            {
                int roleResult = dbMan.ExecuteNonQuery(roleQuery);
                if (roleResult <= 0)
                {
                    // Rollback: delete employee if role insertion fails
                    string rollback = "DELETE FROM Employee WHERE EmployeeID = " + newEmployeeID;
                    dbMan.ExecuteNonQuery(rollback);
                    return 0;
                }
            }

            return 1;
        }
        public int UpdateEmployee(int employeeID, string fName, string lName,
                            int branchID, string employeeType)
        {
            //  Update Employee table 
            string query = "UPDATE Employee SET " +
                           "FName = '" + fName + "', " +
                           "LName = '" + lName + "', " +
                           "BranchID = " + branchID + " " +
                           "WHERE EmployeeID = " + employeeID;

            int result = dbMan.ExecuteNonQuery(query);
            if (result <= 0)
                return 0;

            // Check current role
            string checkCashier = "SELECT COUNT(*) FROM Cashier WHERE EmployeeID = " + employeeID;
            string checkBarista = "SELECT COUNT(*) FROM Barista WHERE EmployeeID = " + employeeID;

            bool isCashier = Convert.ToInt32(dbMan.ExecuteScalar(checkCashier)) > 0;
            bool isBarista = Convert.ToInt32(dbMan.ExecuteScalar(checkBarista)) > 0;

            //  Handle role change based on dropdown selection
            if (employeeType == "Cashier" && !isCashier)
            {
                // Delete from Barista, add to Cashier
                if (isBarista)
                    dbMan.ExecuteNonQuery("DELETE FROM Barista WHERE EmployeeID = " + employeeID);
                dbMan.ExecuteNonQuery("INSERT INTO Cashier (EmployeeID) VALUES (" + employeeID + ")");
            }
            else if (employeeType == "Barista" && !isBarista)
            {
                // Delete from Cashier, add to Barista
                if (isCashier)
                    dbMan.ExecuteNonQuery("DELETE FROM Cashier WHERE EmployeeID = " + employeeID);
                dbMan.ExecuteNonQuery("INSERT INTO Barista (EmployeeID) VALUES (" + employeeID + ")");
            }

            return 1;
        }
        public int DeleteEmployee(int employeeID)
        {
            // Check if employee has orders assigned
            string checkOrders = "SELECT COUNT(*) FROM [Order] WHERE CashierID = " + employeeID +
                                " OR BaristaID = " + employeeID;
            object orderCount = dbMan.ExecuteScalar(checkOrders);

            if (orderCount != null && Convert.ToInt32(orderCount) > 0)
                return -1; // Can't delete - has orders

            string query = "DELETE FROM Employee WHERE EmployeeID = " + employeeID;
            return dbMan.ExecuteNonQuery(query);
        }

        public int ResetEmployeePassword(int employeeID, string newPassword)
        {
            string query = "UPDATE Employee SET Password = '" + newPassword + "' " +
                           "WHERE EmployeeID = " + employeeID;
            return dbMan.ExecuteNonQuery(query);
        }

        
    }
}