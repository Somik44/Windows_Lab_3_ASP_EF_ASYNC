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
    public partial class Form2 : Form
    {
        private DataGridView DetailTable;
        private List<Cars> cars;
        private bool columnsInitialized = false;

        public Form2()
        {
            InitializeComponent();
            InitializeDetailTable();
            cars = new List<Cars>();
            this.Name = "Detailed information";
        }

        private void InitializeDetailTable()
        {
            DetailTable = new DataGridView();
            DetailTable.Name = "DetailTable";
            DetailTable.Location = new Point(0, 0);
            DetailTable.Size = new Size(this.Width, this.Height);
            DetailTable.Visible = true;
            DetailTable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(DetailTable);
        }

        public void InitializeColumns(bool isPassenger)
        {
            if (columnsInitialized) return;

            DetailTable.Columns.Clear();
            DetailTable.Rows.Clear();

            if (isPassenger)
            {
                DetailTable.Columns.Add("RegistrationNumber", "Рег. номер");
                DetailTable.Columns.Add("MultimediaSystem", "Мультимедиа");
                DetailTable.Columns.Add("AirbagCount", "Подушки безопасности");
            }
            else
            {
                DetailTable.Columns.Add("RegistrationNumber", "Рег. номер");
                DetailTable.Columns.Add("WheelCount", "Количество колес");
                DetailTable.Columns.Add("BodyVolume", "Объем кузова");
            }

            columnsInitialized = true;
        }

        public void AddSingleCar(Cars car, bool isPassenger)
        {
            if (!columnsInitialized)
            {
                InitializeColumns(isPassenger);
            }

            cars.Add(car);

            if (isPassenger && car is Passenger passenger)
            {
                DetailTable.Rows.Add(
                    passenger.RegistrationNumber,
                    passenger.MultimediaSystem,
                    passenger.AirbagCount
                );
            }
            else if (!isPassenger && car is Truck truck)
            {
                DetailTable.Rows.Add(
                    truck.RegistrationNumber,
                    truck.WheelCount,
                    truck.BodyVolume
                );
            }

            if (DetailTable.Rows.Count > 0)
            {
                DetailTable.FirstDisplayedScrollingRowIndex = DetailTable.Rows.Count - 1;
            }
            this.Refresh();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DetailTable.Width = this.ClientSize.Width;
            DetailTable.Height = this.ClientSize.Height;
        }
    }
}