namespace LAb_3_dop_Front
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comboBox1 = new ComboBox();
            textBrand = new TextBox();
            textName = new TextBox();
            textPower = new TextBox();
            textSpeed = new TextBox();
            textValue1 = new TextBox();
            textNumber = new TextBox();
            button = new Button();
            textValue2 = new TextBox();
            buttonOK = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Легковой", "Грузовой" });
            comboBox1.Location = new Point(0, 0);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(171, 28);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // textBrand
            // 
            textBrand.ForeColor = SystemColors.ScrollBar;
            textBrand.Location = new Point(0, 34);
            textBrand.Name = "textBrand";
            textBrand.Size = new Size(171, 27);
            textBrand.TabIndex = 1;
            textBrand.Text = "Марка";
            // 
            // textName
            // 
            textName.ForeColor = SystemColors.ScrollBar;
            textName.Location = new Point(0, 67);
            textName.Name = "textName";
            textName.Size = new Size(171, 27);
            textName.TabIndex = 2;
            textName.Text = "Модель";
            // 
            // textPower
            // 
            textPower.ForeColor = SystemColors.ScrollBar;
            textPower.Location = new Point(0, 100);
            textPower.Name = "textPower";
            textPower.Size = new Size(171, 27);
            textPower.TabIndex = 3;
            textPower.Text = "Мощность";
            // 
            // textSpeed
            // 
            textSpeed.ForeColor = SystemColors.ScrollBar;
            textSpeed.Location = new Point(0, 133);
            textSpeed.Name = "textSpeed";
            textSpeed.Size = new Size(171, 27);
            textSpeed.TabIndex = 4;
            textSpeed.Text = "Макс. Скорость";
            // 
            // textValue1
            // 
            textValue1.ForeColor = SystemColors.ScrollBar;
            textValue1.Location = new Point(0, 199);
            textValue1.Name = "textValue1";
            textValue1.Size = new Size(171, 27);
            textValue1.TabIndex = 5;
            // 
            // textNumber
            // 
            textNumber.ForeColor = SystemColors.ScrollBar;
            textNumber.Location = new Point(0, 166);
            textNumber.Name = "textNumber";
            textNumber.Size = new Size(171, 27);
            textNumber.TabIndex = 6;
            textNumber.Text = "Номер";
            textNumber.TextChanged += textBox3_TextChanged;
            // 
            // button
            // 
            button.Location = new Point(177, 166);
            button.Name = "button";
            button.Size = new Size(122, 29);
            button.TabIndex = 7;
            button.Text = "Сгенерировать";
            button.UseVisualStyleBackColor = true;
            button.Click += button_Click;
            // 
            // textValue2
            // 
            textValue2.ForeColor = SystemColors.ScrollBar;
            textValue2.Location = new Point(0, 232);
            textValue2.Name = "textValue2";
            textValue2.Size = new Size(171, 27);
            textValue2.TabIndex = 8;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(0, 275);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(94, 29);
            buttonOK.TabIndex = 9;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(177, 275);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(94, 29);
            buttonCancel.TabIndex = 10;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(314, 321);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            Controls.Add(textValue2);
            Controls.Add(button);
            Controls.Add(textNumber);
            Controls.Add(textValue1);
            Controls.Add(textSpeed);
            Controls.Add(textPower);
            Controls.Add(textName);
            Controls.Add(textBrand);
            Controls.Add(comboBox1);
            Name = "Form3";
            Text = "Form3";
            Load += Form3_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox1;
        private TextBox textBrand;
        private TextBox textName;
        private TextBox textPower;
        private TextBox textSpeed;
        private TextBox textValue1;
        private TextBox textNumber;
        private Button button;
        private TextBox textValue2;
        private Button buttonOK;
        private Button buttonCancel;
    }
}