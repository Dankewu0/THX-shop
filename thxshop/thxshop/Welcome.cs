using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace thxshop
{
    public partial class Welcome : Form
    {
        private string connectionString = "Data Source = GOOSE; Initial Catalog = THXshop; Integrated Security = True; Encrypt = False";

        public Welcome()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            textBox1.KeyDown += new KeyEventHandler(Welcome_KeyDown);
            textBox2.KeyDown += new KeyEventHandler(Welcome_KeyDown);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reg reg = new Reg();
            this.Hide();
            reg.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            var userRole = CheckUserRole(login, password);
            if (userRole != null)
            {
                this.Hide();
                if (userRole == "Admin")
                {
                    Products prdct = new Products(userRole);
                    this.Hide();
                    prdct.Show();
                    MessageBox.Show("Вы зашли от лица Администратора");
                }
                else if (userRole == "Owner")
                {
                    Owner owner = new Owner(userRole);
                    owner.Show();
                    MessageBox.Show("Шалом! ThxSoMch");
                }
                else
                {
                    Products products = new Products(userRole);
                    products.Show();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private string CheckUserRole(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT password, role FROM Auth WHERE login = @login"; // Изменено на извлечение role
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string hashedPassword = reader["password"].ToString();
                        string role = reader["role"].ToString(); // Извлекаем роль из базы данных
                        // Сравниваем хешированный пароль с сохраненным паролем
                        if (hashedPassword == HashingPassword.HashPassword(password))
                        {
                            return role; // Возвращаем роль пользователя
                        }
                    }
                }
            }
            return null; // Возвращаем null, если логин или пароль неверные
        }

        private void Welcome_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void Welcome_KeyDown(object sender, KeyEventArgs e)
        {
            {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }

            else if (e.KeyCode == Keys.Down)
            {
                if (sender == textBox1)
                {
                    textBox2.Focus();
                }
            }

            else if (e.KeyCode == Keys.Up)
            {
                if (sender == textBox2)
                {
                    textBox1.Focus();
                }
            }
            }
        }
    }
}