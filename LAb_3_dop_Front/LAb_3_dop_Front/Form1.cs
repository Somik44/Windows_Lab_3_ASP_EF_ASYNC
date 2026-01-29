using LAb_3_dop_Front;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAb_3_dop_Front
{
    public partial class Form1 : Form
    {
        private BindingList<Cars> cars = new BindingList<Cars> { };
        private DataGridView DetailTable;

        private ProgressBar progressBar;

        private MenuStrip mainMenu;
        private ApiClient_new _apiClient;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel serverStatusLabel;

        public Form1()
        {
            InitializeComponent();

            Car_table.AllowUserToAddRows = false;

            _apiClient = new ApiClient_new();
            _apiClient.ServerSwitched += OnServerSwitched;

            this.Text = "Cars DB";

            Car_table.AutoGenerateColumns = false;

            Марка.DataPropertyName = "Brand";
            Модель.DataPropertyName = "Name";
            Мощность.DataPropertyName = "Power";
            Скорость.DataPropertyName = "Speed";

            DataGridViewComboBoxColumn typeColumn = new DataGridViewComboBoxColumn();
            typeColumn.Name = "typeColumn";
            typeColumn.HeaderText = "Тип";
            typeColumn.DataPropertyName = "Type";
            typeColumn.DataSource = new List<string> { "Легковой", "Грузовой" };
            typeColumn.Width = 120;
            Car_table.Columns.Add(typeColumn);

            bindingSource1.DataSource = cars;
            Car_table.DataSource = bindingSource1;

            Car_table.CellValueChanged += Car_table_CellValueChanged;
            Car_table.RowPrePaint += Car_table_RowPrePaint;
            Car_table.SelectionChanged += Car_table_SelectionChanged;

            InitializeDetailTable();
            InitializeProgressBar();
            InitializeMenu();
            InitializeStatusStrip();

            _ = LoadCarsFromApiAsync();
            UpdateServerStatus();
        }


        //Инициализации

        private void InitializeStatusStrip()
        {
            statusStrip = new StatusStrip();
            serverStatusLabel = new ToolStripStatusLabel();
            serverStatusLabel.Spring = true;
            serverStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            statusStrip.Items.Add(serverStatusLabel);

            this.Controls.Add(statusStrip);
        }

        private void UpdateServerStatus()
        {
            if (serverStatusLabel != null && _apiClient != null)
            {
                var currentServer = _apiClient.CurrentServerName;
                var serverStatus = _apiClient.GetServerStatus();

                string statusText = $"Текущий сервер: {currentServer} ";

                serverStatusLabel.Text = statusText;
            }
        }

        private void ShowServerStatus()
        {
            var servers = _apiClient.GetServerStatus();
            string statusMessage = "Статус серверов (автоматическое переключение):\n\n";

            foreach (var server in servers)
            {
                string currentIndicator = (server.Name == _apiClient.CurrentServerName) ? " ← ТЕКУЩИЙ" : "";
                statusMessage += $"{server.Name}{currentIndicator}:\n";
                statusMessage += $"  URL: {server.Url}\n";
                statusMessage += $"  Статус: {(server.IsActive ? "АКТИВЕН" : "НЕАКТИВЕН")}\n";
                statusMessage += $"  Приоритет: {server.Priority}\n";
                statusMessage += $"  Последняя проверка: {server.LastCheck:HH:mm:ss}\n\n";
            }


            MessageBox.Show(statusMessage, "Статус серверов",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnServerSwitched(object sender, ServerSwitchedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnServerSwitched(sender, e)));
                return;
            }

            MessageBox.Show(
                $"Произошло автоматическое переключение серверов:\n" +
                $"С {e.FromServer} на {e.ToServer}\n" +
                $"Причина: {e.Reason}",
                "Автоматическое переключение сервера",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            UpdateServerStatus();
        }

        private void InitializeMenu()
        {
            mainMenu = new MenuStrip();

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("Файл");
            ToolStripMenuItem serverMenu = new ToolStripMenuItem("Серверы");

            ToolStripMenuItem loadFromApiItem = new ToolStripMenuItem("Загрузить");
            loadFromApiItem.Click += async (s, e) => await LoadCarsFromApiAsync();

            ToolStripMenuItem saveToApiItem = new ToolStripMenuItem("Сохранить");
            saveToApiItem.Click += async (s, e) => await SaveCarsToApiAsync();

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Выход");
            exitItem.Click += (s, e) => this.Close();

            ToolStripMenuItem serverStatusItem = new ToolStripMenuItem("Статус серверов");
            serverStatusItem.Click += (s, e) => ShowServerStatus();

            fileMenu.DropDownItems.AddRange(new ToolStripItem[] {
                loadFromApiItem, saveToApiItem, new ToolStripSeparator(), exitItem
            });

            serverMenu.DropDownItems.AddRange(new ToolStripItem[] {
                serverStatusItem
            });

            mainMenu.Items.Add(fileMenu);
            mainMenu.Items.Add(serverMenu);

            this.Controls.Add(mainMenu);
            this.MainMenuStrip = mainMenu;
            mainMenu.BringToFront();
        }

        private void InitializeDetailTable()
        {
            DetailTable = new DataGridView();
            DetailTable.Name = "DetailTable";
            DetailTable.Location = new Point(Car_table.Right + 10, Car_table.Top);
            DetailTable.Size = new Size(450, Car_table.Height);
            DetailTable.Visible = false;
            this.Controls.Add(DetailTable);
        }

        private void InitializeProgressBar()
        {
            progressBar = new ProgressBar();
            progressBar.Name = "progressBar";
            progressBar.Parent = groupBox1;
            progressBar.Location = new Point(10, 40);
            progressBar.Size = new Size(Car_table.Width, 20);
            progressBar.Visible = false;
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            groupBox1.Controls.Add(progressBar);
        }


        //API методы
        private async Task LoadCarsFromApiAsync()
        {
            try
            {
                progressBar.Visible = true;
                progressBar.Value = 0;

                var passengersTask = _apiClient.GetPassengersAsync();
                var trucksTask = _apiClient.GetTrucksAsync();

                await Task.WhenAll(passengersTask, trucksTask);

                List<Passenger> passengers = passengersTask.Result;
                List<Truck> trucks = trucksTask.Result;

                cars.Clear();

                progressBar.Value = 50;

                foreach (Passenger passenger in passengers)
                {
                    cars.Add(passenger);
                }

                foreach (Truck truck in trucks)
                {
                    cars.Add(truck);
                }

                progressBar.Value = 100;
                bindingSource1.ResetBindings(false);

                UpdateServerStatus();

                MessageBox.Show($"Загружено {cars.Count} транспортных средств с сервера: {_apiClient.CurrentServerName}", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки из API: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
            }
        }

        private async Task SaveCarsToApiAsync()
        {
            try
            {
                progressBar.Visible = true;
                progressBar.Value = 0;
                int savedCount = 0;
                int totalCount = cars.Count;

                foreach (var car in cars)
                {
                    try
                    {
                        if (car is Passenger passenger)
                        {
                            if (passenger.Id == 0)
                            {
                                await _apiClient.CreatePassengerAsync(passenger);
                            }
                            else
                            {
                                await _apiClient.UpdatePassengerAsync(passenger.Id, passenger);
                            }
                        }
                        else if (car is Truck truck)
                        {
                            if (truck.Id == 0)
                            {
                                await _apiClient.CreateTruckAsync(truck);
                            }
                            else
                            {
                                await _apiClient.UpdateTruckAsync(truck.Id, truck);
                            }
                        }

                        savedCount++;
                        progressBar.Value = (savedCount * 100) / totalCount;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка сохранения транспортного средства {car.Brand} {car.Name}: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Обновляем статус сервера после сохранения
                UpdateServerStatus();

                MessageBox.Show($"Успешно сохранено {savedCount} транспортных средств на сервер: {_apiClient.CurrentServerName}", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Общая ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
            }
        }

        //UI методы
        private void Car_table_SelectionChanged(object sender, EventArgs e)
        {
            if (Car_table.SelectedRows.Count == 0)
            {
                DetailTable.Visible = false;
                return;
            }

            DataGridViewRow selectedRow = Car_table.SelectedRows[0];
            if (selectedRow.IsNewRow)
            {
                DetailTable.Visible = false;
                return;
            }

            Cars selectedCar = selectedRow.DataBoundItem as Cars;
            if (selectedCar == null) return;

            string Key_Brand = selectedCar.Brand;
            string Key_Type = selectedCar.Type;
            bool isPassenger = selectedCar is Passenger;

            _ = UpdateDetailTable(Key_Brand, Key_Type, isPassenger);
        }

        private async Task UpdateDetailTable(string Key_Brand, string Key_Type, bool isPassenger)
        {
            progressBar.Visible = true;
            progressBar.Value = 0;

            Form2 form2 = new Form2();
            form2.Show();

            List<Cars> info = new List<Cars>();

            if (isPassenger)
            {
                List<Passenger> passengers = await _apiClient.GetPassengersByBrandAsync(Key_Brand);
                info.AddRange(passengers);
            }
            else
            {
                List<Truck> trucks = await _apiClient.GetTrucksByBrandAsync(Key_Brand);
                info.AddRange(trucks);
            }

            form2.InitializeColumns(isPassenger);

            foreach (Cars car in info)
            {
                form2.AddSingleCar(car, isPassenger);

                await Task.Delay(300);

                progressBar.Value = (int)((info.IndexOf(car) + 1) * 100.0 / info.Count);
            }

            progressBar.Visible = false;
        }

        private async void Car_table_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = Car_table.Rows[e.RowIndex];

            if (Car_table.Columns[e.ColumnIndex].Name == "typeColumn")
            {
                object selectedValue = row.Cells[e.ColumnIndex].Value;
                Cars selectedCar = Car_table.Rows[e.RowIndex].DataBoundItem as Cars;

                if (selectedValue != null && selectedCar != null)
                {
                    string selectedType = selectedValue.ToString();
                    string currentType = selectedCar is Passenger ? "Пассажирский" : "Грузовой";

                    if (selectedType == currentType)
                        return;

                    try
                    {
                        int oldId = selectedCar.Id;

                        if (selectedType == "Пассажирский")
                        {
                            Passenger newCar = new Passenger(
                                selectedCar.Name,
                                selectedCar.Brand,
                                selectedCar.Power,
                                selectedCar.Speed,
                                selectedCar.RegistrationNumber,
                                "base",
                                0
                            );

                            var createdCar = await _apiClient.CreatePassengerAsync(newCar);

                            if (currentType == "Грузовой")
                            {
                                await _apiClient.DeleteTruckAsync(oldId);
                            }
                            else
                            {
                                await _apiClient.DeletePassengerAsync(oldId);
                            }
                        }
                        else if (selectedType == "Грузовой")
                        {
                            Truck newCar = new Truck(
                                selectedCar.Name,
                                selectedCar.Brand,
                                selectedCar.Power,
                                selectedCar.Speed,
                                selectedCar.RegistrationNumber,
                                0,
                                0
                            );

                            var createdCar = await _apiClient.CreateTruckAsync(newCar);

                            if (currentType == "Пассажирский")
                            {
                                await _apiClient.DeletePassengerAsync(oldId);
                            }
                            else
                            {
                                await _apiClient.DeleteTruckAsync(oldId);
                            }
                        }

                        await LoadCarsFromApiAsync();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при изменении типа: {ex.Message}\n\nДетали: {ex.InnerException?.Message}");
                    }
                }
            }
        }

        private void Car_table_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Car_table.Rows.Count)
                return;

            DataGridViewRow row = Car_table.Rows[e.RowIndex];

            if (row.IsNewRow)
                return;

            Cars car = row.DataBoundItem as Cars;

            if (car == null)
                return;

            if (car is Passenger)
            {
                row.DefaultCellStyle.BackColor = Color.LightGreen;
                row.DefaultCellStyle.ForeColor = Color.Black;
                row.DefaultCellStyle.Font = new Font(Car_table.Font, FontStyle.Bold);
            }
            else if (car is Truck)
            {
                row.DefaultCellStyle.BackColor = Color.LightBlue;
                row.DefaultCellStyle.ForeColor = Color.Black;
                row.DefaultCellStyle.Font = new Font(Car_table.Font, FontStyle.Bold);
            }
        }

        private async void Add_Brand_Click(object sender, EventArgs e)
        {
            using (Form3 form3 = new Form3())
            {
                if (form3.ShowDialog() == DialogResult.OK)
                {
                    if (form3.GetCarType() == "Легковой")
                    {
                        try
                        {
                            var newPassenger = new Passenger(
                                form3.GetBrand(), form3.GetName(), form3.GetPower(), form3.GetSpeed(), form3.GetNumber(), form3.GetValue1(), Convert.ToInt32(form3.GetValue2())
                                );
                            var createdPassenger = await _apiClient.CreatePassengerAsync(newPassenger);
                            cars.Add(createdPassenger);
                            bindingSource1.ResetBindings(false);

                            Car_table.ClearSelection();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка создания пассажирского транспортного средства: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        try
                        {
                            var newTruck = new Truck(
                                form3.GetBrand(), form3.GetName(), form3.GetPower(), form3.GetSpeed(), form3.GetNumber(), Convert.ToInt32(form3.GetValue1()), Convert.ToInt32(form3.GetValue2())
                                );
                            var createdTruck = await _apiClient.CreateTruckAsync(newTruck);
                            cars.Add(createdTruck);
                            bindingSource1.ResetBindings(false);

                            Car_table.ClearSelection();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка создания грузовика: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}