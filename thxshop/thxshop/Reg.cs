using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace thxshop
{
    public partial class Reg : Form
    {
        private string connectionString = "Data Source = GOOSE; Initial Catalog = THXshop; Integrated Security = True; Encrypt = False";

        public Reg()
        {
            InitializeComponent();
            LoadAuth();
            textBox2.PasswordChar = '*';
            textBox1.KeyDown += new KeyEventHandler(Reg_KeyDown);
            textBox2.KeyDown += new KeyEventHandler(Reg_KeyDown);
        }

        private void LoadAuth()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Auth", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable authTable = new DataTable();
                adapter.Fill(authTable);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;

            if (IsLoginExists(login))
            {
                MessageBox.Show("This login already exists. Please choose another.");
                return;
            }

            string hashedPassword = HashingPassword.HashPassword(password);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Auth (login, password, role) VALUES (@login, @password, 'User ')", connection);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.ExecuteNonQuery();
                LoadAuth();
            }
            button1.Text = "Thanks";
            Welcome form = new Welcome();
            this.Hide();
            form.Show();
        }

        private bool IsLoginExists(string login)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(1) FROM Auth WHERE login = @login", connection);
                command.Parameters.AddWithValue("@login", login);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Welcome form = new Welcome();
            this.Hide();
            form.Show();
        }

        private void Reg_FormClosed(object sender, FormClosedEventArgs e)
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

        private void Reg_KeyDown(object sender, KeyEventArgs e)
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
