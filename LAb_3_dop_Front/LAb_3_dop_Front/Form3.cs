using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAb_3_dop_Front
{
    public partial class Form3 : Form
    {
        static private char[] letters = { 'A', 'B', 'E', 'K', 'M', 'H', 'O', 'P', 'C', 'T', 'У', 'X' };
        static private int[] WheelCount = { 4, 6, 8 };

        private string defaultBrand = "Марка";
        private string defaultName = "Модель";
        private string defaultPower = "Мощность";
        private string defaultSpeed = "Макс. Скорость";
        private string defaultNumber = "Номер";
        private string defaultValue1 = "Мультимедия";
        private string defaultValue2 = "Колличество подушек";

        public Form3()
        {
            InitializeComponent();
            this.Text = "Loggin";

            comboBox1.SelectedIndex = 0;
            SetDefaultTextAndColor();

            SubscribeToTextEvents();
        }

        private void SubscribeToTextEvents()
        {
            textBrand.Enter += TextBox_Enter;
            textBrand.Leave += TextBox_Leave;

            textName.Enter += TextBox_Enter;
            textName.Leave += TextBox_Leave;

            textPower.Enter += TextBox_Enter;
            textPower.Leave += TextBox_Leave;

            textSpeed.Enter += TextBox_Enter;
            textSpeed.Leave += TextBox_Leave;

            textNumber.Enter += TextBox_Enter;
            textNumber.Leave += TextBox_Leave;

            textValue1.Enter += TextBox_Enter;
            textValue1.Leave += TextBox_Leave;

            textValue2.Enter += TextBox_Enter;
            textValue2.Leave += TextBox_Leave;
        }

        private void SetDefaultTextAndColor()
        {
            // Установка текста по умолчанию и серого цвета
            textBrand.Text = defaultBrand;
            textBrand.ForeColor = SystemColors.ScrollBar;

            textName.Text = defaultName;
            textName.ForeColor = SystemColors.ScrollBar;

            textPower.Text = defaultPower;
            textPower.ForeColor = SystemColors.ScrollBar;

            textSpeed.Text = defaultSpeed;
            textSpeed.ForeColor = SystemColors.ScrollBar;

            textNumber.Text = defaultNumber;
            textNumber.ForeColor = SystemColors.ScrollBar;

            textValue1.Text = defaultValue1;
            textValue1.ForeColor = SystemColors.ScrollBar;

            textValue2.Text = defaultValue2;
            textValue2.ForeColor = SystemColors.ScrollBar;
        }

        private void ClearTextBoxes()
        {
            SetDefaultTextAndColor();
        }


        //get
        public string GetCarType()
        {
            return comboBox1.SelectedItem?.ToString() ?? "";
        }

        public string GetBrand()
        {
            return textBrand.ForeColor == SystemColors.WindowText ? textBrand.Text : "";
        }

        public string GetName()
        {
            return textName.ForeColor == SystemColors.WindowText ? textName.Text : "";
        }

        public int GetPower()
        {
            if (textPower.ForeColor == SystemColors.WindowText && int.TryParse(textPower.Text, out int result))
                return result;
            return 0;
        }

        public int GetSpeed()
        {
            if (textSpeed.ForeColor == SystemColors.WindowText && int.TryParse(textSpeed.Text, out int result))
                return result;
            return 0;
        }

        public string GetNumber()
        {
            return textNumber.ForeColor == SystemColors.WindowText ? textNumber.Text : "";
        }

        public string GetValue1()
        {
            return textValue1.ForeColor == SystemColors.WindowText ? textValue1.Text : "";
        }

        public string GetValue2()
        {
            return textValue2.ForeColor == SystemColors.WindowText ? textValue2.Text : "";
        }

        // Обработчики событий для текстовых полей
        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.ForeColor == SystemColors.ScrollBar)
            {
                textBox.Text = "";
                textBox.ForeColor = SystemColors.WindowText;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                // Восстановление текста по умолчанию в зависимости от поля
                switch (textBox.Name)
                {
                    case "textBrand": textBox.Text = defaultBrand; break;
                    case "textName": textBox.Text = defaultName; break;
                    case "textPower": textBox.Text = defaultPower; break;
                    case "textSpeed": textBox.Text = defaultSpeed; break;
                    case "textNumber": textBox.Text = defaultNumber; break;
                    case "textValue1": textBox.Text = defaultValue1; break;
                    case "textValue2": textBox.Text = defaultValue2; break;
                }
                textBox.ForeColor = SystemColors.ScrollBar;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                defaultValue1 = "Мультимедия";
                defaultValue2 = "Колличество подушек";
            }
            else
            {
                defaultValue1 = "Колличество колес";
                defaultValue2 = "Объем кузова";
            }

            // Обновление текста полей, если они содержат старый текст по умолчанию
            if (textValue1.ForeColor == SystemColors.ScrollBar)
            {
                textValue1.Text = defaultValue1;
            }
            if (textValue2.ForeColor == SystemColors.ScrollBar)
            {
                textValue2.Text = defaultValue2;
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            string num = $"{letters[rnd.Next(0, 12)]}{Convert.ToString(rnd.Next(0, 1000)).PadLeft(3, '0')}{letters[rnd.Next(0, 12)]}{letters[rnd.Next(0, 12)]}";
            textNumber.Text = num;
            textNumber.ForeColor = SystemColors.WindowText;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректными данными.", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Очищаем поля и закрываем форму с результатом Cancel
            ClearTextBoxes();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateData()
        {
            // Проверка, что все поля заполнены и не содержат текст по умолчанию
            if (textBrand.ForeColor == SystemColors.ScrollBar || string.IsNullOrWhiteSpace(textBrand.Text))
                return false;

            if (textName.ForeColor == SystemColors.ScrollBar || string.IsNullOrWhiteSpace(textName.Text))
                return false;

            if (textPower.ForeColor == SystemColors.ScrollBar || string.IsNullOrWhiteSpace(textPower.Text) || !IsNumeric(textPower.Text))
                return false;

            if (textSpeed.ForeColor == SystemColors.ScrollBar || string.IsNullOrWhiteSpace(textSpeed.Text) || !IsNumeric(textSpeed.Text))
                return false;

            if (textNumber.ForeColor == SystemColors.ScrollBar || string.IsNullOrWhiteSpace(textNumber.Text))
                return false;

            if (textValue1.ForeColor == SystemColors.ScrollBar || string.IsNullOrWhiteSpace(textValue1.Text))
                return false;

            if (textValue2.ForeColor == SystemColors.ScrollBar || string.IsNullOrWhiteSpace(textValue2.Text))
                return false;

            return true;
        }

        private bool IsNumeric(string text)
        {
            return double.TryParse(text, out _);
        }

        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void Form3_Load(object sender, EventArgs e) { }
    }
}