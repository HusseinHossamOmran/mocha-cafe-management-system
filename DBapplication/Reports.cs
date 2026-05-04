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
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DBapplication
{
    public partial class Reports : Form
    {
        DBManager db = new DBManager();

        public Reports()
        {
            InitializeComponent();
            LoadFeedbackComments();
            LoadFeedbackStatistics();
            LoadRatingsBreakdown();
            LoadRatingsPieChart(); 
            LoadCustomerLoyaltyTierGrid();
            LoadLoyaltyTierChart();
            LoadLoyaltyTierPieChart();
            LoadTotalCustomers();
            LoadTotals();
            LoadBaristasAndCashiersWithBranch();
            LoadBaristaCashierPieChart();
            LoadTotalBranches();
            LoadBranchStockBarChart();
        }
        private void LoadRatingsBreakdown()
        {
            string query = "SELECT Rating, COUNT(*) AS Count FROM Feedback GROUP BY Rating";
            DataTable dt = db.ExecuteReader(query);

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["Rating"].HeaderText = "Rating";
            dataGridView1.Columns["Count"].HeaderText = "Number of Feedbacks";
        }
        private void LoadFeedbackComments()
        {
            string query = "SELECT c.Username, f.Comment FROM Feedback f JOIN Customer c ON f.CustomerID = c.CustomerID";
            DataTable dt = db.ExecuteReader(query);

            dataGridView2.AutoGenerateColumns = true;
            dataGridView2.DataSource = dt;
            dataGridView2.Columns["Username"].HeaderText = "User";
            dataGridView2.Columns["Comment"].HeaderText = "Feedback Comment";
        }
        private void LoadRatingsPieChart()
        {
            chart1.Series.Clear(); 
            Series series = new Series("Ratings");
            series.ChartType = SeriesChartType.Pie; 

            string query = "SELECT Rating, COUNT(*) AS Count FROM Feedback GROUP BY Rating";
            DataTable dt = db.ExecuteReader(query);

            foreach (DataRow row in dt.Rows)
            {
                series.Points.AddXY(row["Rating"], row["Count"]);
            }

            chart1.Series.Add(series);

            series.IsValueShownAsLabel = true;
            chart1.Legends[0].Enabled = true;
        }


        private void LoadFeedbackStatistics()
        {
            string totalQuery = "SELECT COUNT(*) FROM Feedback";
            object totalObj = db.ExecuteScalar(totalQuery);
            int totalFeedback = totalObj != null ? Convert.ToInt32(totalObj) : 0;
            label10.Text = $"{totalFeedback}";

            string avgQuery = "SELECT AVG(Rating) FROM Feedback";
            object avgObj = db.ExecuteScalar(avgQuery);
            double avgRating = avgObj != null ? Convert.ToDouble(avgObj) : 0.0;
            label15.Text = $"{avgRating:F1}";
        }
        void LoadTotalCustomers()
        {
            string totalQuery = "SELECT COUNT(*) FROM Customer";
            object totalObj = db.ExecuteScalar(totalQuery);
            int totalCustomers = totalObj != null ? Convert.ToInt32(totalObj) : 0;
            label8.Text = $"{totalCustomers}";
        }
        private void LoadCustomerLoyaltyTierGrid()
        {
            string query = @"
        SELECT Username, LoyaltyBalance,
               CASE 
                 WHEN LoyaltyBalance < 100 THEN 'Bronze'
                 WHEN LoyaltyBalance < 500 THEN 'Silver'
                 ELSE 'Gold'
               END AS Tier
        FROM Customer";

            DataTable dt = db.ExecuteReader(query);
            dataGridView3.AutoGenerateColumns = true;
            dataGridView3.DataSource = dt;

        }
        private void LoadLoyaltyTierPieChart()
        {
            chart3.Series.Clear();
            Series series = new Series("Loyalty Tiers");
            series.ChartType = SeriesChartType.Pie;

            string query = @"
        SELECT 
          CASE 
            WHEN LoyaltyBalance < 100 THEN 'Bronze'
            WHEN LoyaltyBalance < 500 THEN 'Silver'
            ELSE 'Gold'
          END AS Tier,
          COUNT(*) AS Count
        FROM Customer
        GROUP BY 
          CASE 
            WHEN LoyaltyBalance < 100 THEN 'Bronze'
            WHEN LoyaltyBalance < 500 THEN 'Silver'
            ELSE 'Gold'
          END";

            DataTable dt = db.ExecuteReader(query);

            foreach (DataRow row in dt.Rows)
            {
                series.Points.AddXY(row["Tier"], row["Count"]);
            }

            series.IsValueShownAsLabel = true;
            chart3.Series.Add(series);
            chart3.Legends[0].Enabled = true;
        }
        void LoadLoyaltyTierChart()
        {
            chart2.Series.Clear();
            Series series = new Series("Tiers");
            series.ChartType = SeriesChartType.Column;

            string query = @"
        SELECT 
          CASE 
            WHEN LoyaltyBalance < 100 THEN 'Bronze'
            WHEN LoyaltyBalance < 500 THEN 'Silver'
            ELSE 'Gold'
          END AS Tier,
          COUNT(*) AS Count
        FROM Customer
        GROUP BY 
          CASE 
            WHEN LoyaltyBalance < 100 THEN 'Bronze'
            WHEN LoyaltyBalance < 500 THEN 'Silver'
            ELSE 'Gold'
          END";

            DataTable dt = db.ExecuteReader(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    series.Points.AddXY(row["Tier"], row["Count"]);
                }
            }
            else
            {
                // Show a placeholder bar if no data
                series.Points.AddXY("No Customers", 0);
            }

            chart2.Series.Add(series);
        }

        void LoadTotals()
        {
            // 1) Total Baristas -> label4
            string baristaQuery = "SELECT COUNT(*) FROM Barista";
            object baristaObj = db.ExecuteScalar(baristaQuery);
            int totalBaristas = (baristaObj != null && baristaObj != DBNull.Value) ? Convert.ToInt32(baristaObj) : 0;
            label4.Text = $"{totalBaristas}";

            // 2) Total Employees -> label5
            string employeeQuery = "SELECT COUNT(*) FROM Employee";
            object employeeObj = db.ExecuteScalar(employeeQuery);
            int totalEmployees = (employeeObj != null && employeeObj != DBNull.Value) ? Convert.ToInt32(employeeObj) : 0;
            label5.Text = $"{totalEmployees}";

            // 3) Total Cashiers -> label11
            string cashierQuery = "SELECT COUNT(*) FROM Cashier";
            object cashierObj = db.ExecuteScalar(cashierQuery);
            int totalCashiers = (cashierObj != null && cashierObj != DBNull.Value) ? Convert.ToInt32(cashierObj) : 0;
            label11.Text = $"{totalCashiers}";
        }
        private void LoadBaristasAndCashiersWithBranch()
        {
            // ===== Baristas -> dataGridView5 =====
            string qBaristas = @"
        SELECT E.Username, BR.Name AS BranchName
        FROM Employee E
        INNER JOIN Barista B ON E.EmployeeID = B.EmployeeID
        INNER JOIN Branch BR ON E.BranchID = BR.BranchID
        ORDER BY BR.Name, E.Username";

            DataTable dtBaristas = db.ExecuteReader(qBaristas);
            dataGridView5.AutoGenerateColumns = true;
            dataGridView5.DataSource = dtBaristas;

            if (dtBaristas != null)
            {
                dataGridView5.Columns["Username"].HeaderText = "Barista Username";
                dataGridView5.Columns["BranchName"].HeaderText = "Branch";
            }

            // ===== Cashiers -> dataGridView4 =====
            string qCashiers = @"
        SELECT E.Username, BR.Name AS BranchName
        FROM Employee E
        INNER JOIN Cashier C ON E.EmployeeID = C.EmployeeID
        INNER JOIN Branch BR ON E.BranchID = BR.BranchID
        ORDER BY BR.Name, E.Username";

            DataTable dtCashiers = db.ExecuteReader(qCashiers);
            dataGridView4.AutoGenerateColumns = true;
            dataGridView4.DataSource = dtCashiers;

            if (dtCashiers != null)
            {
                dataGridView4.Columns["Username"].HeaderText = "Cashier Username";
                dataGridView4.Columns["BranchName"].HeaderText = "Branch";
            }
        }
        private void LoadBaristaCashierPieChart()
        {
            chart4.Series.Clear();
            Series series = new Series("Employees");
            series.ChartType = SeriesChartType.Pie;

            // Baristas count
            string baristaQuery = "SELECT COUNT(*) AS Count FROM Barista";
            DataTable dtBarista = db.ExecuteReader(baristaQuery);
            if (dtBarista != null && dtBarista.Rows.Count > 0)
                series.Points.AddXY("Barista", dtBarista.Rows[0]["Count"]);

            // Cashiers count
            string cashierQuery = "SELECT COUNT(*) AS Count FROM Cashier";
            DataTable dtCashier = db.ExecuteReader(cashierQuery);
            if (dtCashier != null && dtCashier.Rows.Count > 0)
                series.Points.AddXY("Cashier", dtCashier.Rows[0]["Count"]);

            chart4.Series.Add(series);

            series.IsValueShownAsLabel = true;
            chart4.Legends[0].Enabled = true;
        }

        void LoadTotalBranches()
        {
            string query = "SELECT COUNT(*) FROM Branch";
            object obj = db.ExecuteScalar(query);

            int totalBranches = (obj != null && obj != DBNull.Value) ? Convert.ToInt32(obj) : 0;
            label14.Text = $"{totalBranches}";
        }
        private void LoadBranchStockBarChart()
        {
            chart5.Series.Clear();
            Series series = new Series("Branch Stock");
            series.ChartType = SeriesChartType.Bar;

            string query = @"
        SELECT BR.Name AS BranchName, SUM(A.Quantity) AS TotalStock
        FROM Branch BR
        JOIN AvailableAt A ON BR.BranchID = A.BranchID
        GROUP BY BR.Name";

            DataTable dt = db.ExecuteReader(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    series.Points.AddXY(row["BranchName"], row["TotalStock"]);
                }
            }
            else
            {
                // Fallback if query returns null or no rows
                series.Points.AddXY("No Data", 0);
            }

            chart5.Series.Add(series);

            chart5.ChartAreas[0].AxisX.Title = "Branch";
            chart5.ChartAreas[0].AxisY.Title = "Total Stock of Menu Items";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dgvBranches_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chart4_Click(object sender, EventArgs e)
        {

        }
    }
}
