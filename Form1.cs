using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;    

namespace StudentDB
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=JayPerales;Initial catalog=StudentDB;Integrated Security=True";
        private object usersTableAdapter;
        private object adapter;

        public object StudentDBDataSet { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter= new SqlDataAdapter("SELECT * FROM Students", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void ClearTextBoxes()
        {
            txtStudentID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAge.Clear();
            txtCourse.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Students (FirstName, LastName, Age, Course) VALUES (@FirstName, @LastName, @Age, @Course)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                    cmd.Parameters.AddWithValue("@Course", txtCourse.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record Added Successfully");
                    LoadData(); 
                    ClearTextBoxes();
                }
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Students SET FirstName=@FirstName, LastName=@LastName, Age=@Age WHERE StudentID=@StudentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                    cmd.Parameters.AddWithValue("@Course", txtCourse.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record Updated Successfully");
                    LoadData();
                    ClearTextBoxes();
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Students WHERE StudentID=@StudentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", txtStudentID.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record Deleted Successfully");
                    LoadData();
                    ClearTextBoxes();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Students", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            // A confirmation box is highly recommended before a destructive action like this!
            if (MessageBox.Show("Are you sure you want to delete ALL records from the table?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "TRUNCATE TABLE Students";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("All records have been deleted successfully.");
                            LoadData(); // Refresh the DataGridView to show an empty table
                            ClearTextBoxes(); // Clear the text boxes
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
