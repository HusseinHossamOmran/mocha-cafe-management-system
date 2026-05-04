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
    public partial class Feedback : Form
    {
        DBManager db = new DBManager();
        private int customerID;
        public Feedback(int id)
        {
            InitializeComponent();
            customerID = id;
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a rating (1–5).");
                return;
            }
            int rating = Convert.ToInt32(comboBox1.SelectedItem);
            string comment = textBox1.Text.Trim(); 

            if (string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("Please, enter a comment.");
                return;
            }

            string insertQuery = $"INSERT INTO Feedback (Rating, Comment, CustomerID) " +
                     $"VALUES ({rating}, '{comment}', {customerID})";

            int insert = db.ExecuteNonQuery(insertQuery);

            if (insert >0)
            {
                MessageBox.Show("Thank you for your feedback!");
                textBox1.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Failed to submit feedback.");
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
