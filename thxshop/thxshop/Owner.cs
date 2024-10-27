using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace thxshop
{
    public partial class Owner : Form
    {
        private string connectionString = "Data Source=GOOSE; Initial Catalog=THXshop; Integrated Security = True; Pooling = true; Encrypt = false";
        private string userRole; // Переменная для хранения роли пользователя

        public Owner(string role)
        {
            userRole = role;
            InitializeComponent();
            LoadAccounts();
        }
        private void LoadAccounts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Auth", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable projdt = new DataTable();
                adapter.Fill(projdt);
                dataGridView1.DataSource = projdt;
            }
        }
        private void Owner_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string role = textBox1.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Auth (Role) Values (@Role)", connection);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.ExecuteNonQuery();
                LoadAccounts();
            }
        }
        private void DeleteRecord()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int idToDelete = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value); 
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Auth WHERE ID = @ID"; 
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@ID", idToDelete);
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
                LoadAccounts(); // Обновляем данные после удаления
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }

        }
    
    private void button2_Click(object sender, EventArgs e)
        {
         DeleteRecord();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Products pr = new Products(userRole);
            this.Hide();
            pr.Show();
        }
    }
}
