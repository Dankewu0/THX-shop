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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace thxshop
{
    public partial class ourshops : Form
    {
        private string connectionString = "Data Source=GOOSE; Initial Catalog=THXshop; Integrated Security = True; Pooling = true; Encrypt = false";
        private string userRole; // Переменная для хранения роли пользователя
        public ourshops(string role)
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM shops", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable projdt = new DataTable();
                adapter.Fill(projdt);
                dataGridView1.DataSource = projdt;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Products products = new Products(userRole);
            this.Hide();
            products.Show();
        }
        private void ourshops_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string coun = textBox6.Text;
            string adress = textBox3.Text;
            string status = textBox2.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO shops (Страна, АдресМагазина, Статус) Values (@Страна, @АдресМагазина, @Статус)", connection);
                cmd.Parameters.AddWithValue("@Страна", coun);
                cmd.Parameters.AddWithValue("@АдресМагазина", adress);
                cmd.Parameters.AddWithValue("@Статус", status);
                cmd.ExecuteNonQuery();
                LoadPeriph();
            }
        }
            private void CheckUserRole()
            {
                if (userRole == "Admin" || userRole == "Owner")
                {
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox6.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    label6.Visible = true;
                    button2.Visible = true;
                textBox1.Visible = true;
                button4.Visible = true;
                label4.Visible = true;
                button3.Visible = true;
            }
                else if (userRole == "User ")
                {
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    textBox3.Visible = false;
                    textBox6.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label6.Visible = false;
                    button2.Visible = false;
                button4.Visible = false;
                button3.Visible = false;
                label4.Visible = false;
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

        private void button4_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox1.Text); // Идентификатор записи
            string newValue = null;
            string columnToUpdate = null;

            if (!string.IsNullOrWhiteSpace(textBox6.Text))
            {
                newValue = textBox6.Text;
                columnToUpdate = "Страна"; // Название колонки в таблице
            }
            else if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                newValue = textBox3.Text;
                columnToUpdate = "Адрес"; // Название колонки в таблице
            }
            else if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                newValue = textBox2.Text;
                columnToUpdate = "Статус"; // Название колонки в таблице
            }

            // Если ни одно текстовое поле не заполнено, сообщаем об этом
            if (newValue == null || columnToUpdate == null)
            {
                MessageBox.Show("Пожалуйста, заполните одно из текстовых полей для обновления.");
                return;
            }

            string query = $"UPDATE PP SET {columnToUpdate} = @NewValue WHERE ID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NewValue", newValue);
                command.Parameters.AddWithValue("@ID", id);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("Перезайдите, чтоб изменения применились");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }
