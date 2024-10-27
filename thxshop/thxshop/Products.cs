using System;
using System.Windows.Forms;

namespace thxshop
{
    public partial class Products : Form
    {
        public object[] prdcts { get; set; }
        public string sp { get; set; }
        private string userRole; // Переменная для хранения роли пользователя

        public Products(string role) // Конструктор принимает роль пользователя
        {
            InitializeComponent();
            userRole = role; // Сохраняем роль
            prdcts = new object[] { "Компьютерные комплектующие", "Смартфоны и фототехника", "Компьютерная периферия", "Наши Магазины" };
            listBox1.Items.Clear();
            listBox1.Items.AddRange(prdcts);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return; // Проверка на случай, если ничего не выбрано

            sp = listBox1.SelectedItem.ToString();
            switch (sp)
            {
                case "Компьютерные комплектующие":
                    PC cpu = new PC(userRole);
                    this.Hide();
                    cpu.Show();
                    break;
                case "Смартфоны и фототехника":
                    PhonenPhoto phonenPhoto = new PhonenPhoto(userRole); // Передаем роль пользователя
                    this.Hide();
                    phonenPhoto.Show();
                    break;
                case "Компьютерная периферия":
                    Periph periph = new Periph(userRole);
                    this.Hide();
                    periph.Show();
                    break;
                case "Наши Магазины":
                    ourshops ourshops = new ourshops(userRole);
                    this.Hide();
                    ourshops.Show();
                    break;
            }
        }

        private void Products_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}