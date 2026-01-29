namespace LAb_3_dop_Front
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            bindingSource1 = new BindingSource(components);
            Car_table = new DataGridView();
            Марка = new DataGridViewTextBoxColumn();
            Модель = new DataGridViewTextBoxColumn();
            Мощность = new DataGridViewTextBoxColumn();
            Скорость = new DataGridViewTextBoxColumn();
            Add_Brand = new Button();
            groupBox1 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Car_table).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // Car_table
            // 
            Car_table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Car_table.Columns.AddRange(new DataGridViewColumn[] { Марка, Модель, Мощность, Скорость });
            Car_table.Location = new Point(1, 73);
            Car_table.Name = "Car_table";
            Car_table.RowHeadersWidth = 51;
            Car_table.Size = new Size(941, 378);
            Car_table.TabIndex = 0;
            // 
            // Марка
            // 
            Марка.HeaderText = "Марка";
            Марка.MinimumWidth = 6;
            Марка.Name = "Марка";
            Марка.Width = 125;
            // 
            // Модель
            // 
            Модель.HeaderText = "Модель";
            Модель.MinimumWidth = 6;
            Модель.Name = "Модель";
            Модель.Width = 125;
            // 
            // Мощность
            // 
            Мощность.HeaderText = "Мощность Л.С.";
            Мощность.MinimumWidth = 6;
            Мощность.Name = "Мощность";
            Мощность.Width = 125;
            // 
            // Скорость
            // 
            Скорость.HeaderText = "Скорость";
            Скорость.MinimumWidth = 6;
            Скорость.Name = "Скорость";
            Скорость.Width = 125;
            // 
            // Add_Brand
            // 
            Add_Brand.Location = new Point(1049, 26);
            Add_Brand.Name = "Add_Brand";
            Add_Brand.Size = new Size(130, 29);
            Add_Brand.TabIndex = 0;
            Add_Brand.Text = "Добавить марку";
            Add_Brand.UseVisualStyleBackColor = true;
            Add_Brand.Click += Add_Brand_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(Add_Brand);
            groupBox1.Location = new Point(1, 1);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1190, 66);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1192, 488);
            Controls.Add(groupBox1);
            Controls.Add(Car_table);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Car_table).EndInit();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private BindingSource bindingSource1;
        private DataGridView Car_table;
        private DataGridViewTextBoxColumn Марка;
        private DataGridViewTextBoxColumn Модель;
        private DataGridViewTextBoxColumn Мощность;
        private DataGridViewTextBoxColumn Скорость;
        private Button Add_Brand;
        private GroupBox groupBox1;
    }
}
