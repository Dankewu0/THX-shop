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
    public partial class Periph : Form
    {
        private string connectionString = "Data Source=GOOSE; Initial Catalog=THXshop; Integrated Security = True; Pooling = true; Encrypt = false";
        private string userRole; // Переменная для хранения роли пользователя
        public Periph(string role)
        {
            userRole = role;
            InitializeComponent();
            LoadPeriph();
            CheckUserRole();
        }

        private void LoadPeriph()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Periph", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable projdt = new DataTable();
                adapter.Fill(projdt);
                dataGridView1.DataSource = projdt;
            }
        }
        private void CheckUserRole()
        {
            if (userRole == "Admin" || userRole == "Owner")
            {
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                button2.Visible = true;
                button3.Visible = false;
            }
            else if (userRole == "User ")
            {
                textBox2.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Products products = new Products(userRole);
            this.Hide();
            products.Show();
        }
        private void Periph_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string type = textBox6.Text;
            string company = textBox3.Text;
            string name = textBox2.Text;
            string adress = textBox5.Text;
            string availability = textBox4.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Periph (Тип, Компания, Название, АдресМагазина, Наличие) Values (@Тип, @Компания, @Название, @АдресМагазина, @Наличие)", connection);
                cmd.Parameters.AddWithValue("@Тип", type);
                cmd.Parameters.AddWithValue("@Компания", company);
                cmd.Parameters.AddWithValue("@Название", name);
                cmd.Parameters.AddWithValue("@АдресМагазина", adress);
                cmd.Parameters.AddWithValue("@Наличие", availability);
                cmd.ExecuteNonQuery();
                LoadPeriph();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int idToDelete = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM shops WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@ID", idToDelete);
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
                LoadPeriph();
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }
    }
}

//private string connectionString = "Data Source=Cab109,49172; Initial Catalog=THXshop; Integrated Security = True; Pooling = true; Encrypt = false";