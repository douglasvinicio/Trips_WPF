using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Trips
{
    public partial class MainWindow : Window
    {
        const string DataFileName = @"..\..\trips.txt";
        static List<Trip> TripList = new List<Trip>();

        public MainWindow()
        {
            InitializeComponent();
            LoadDataFromFile();
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(DataFileName))
            {
                try
                {
                    string[] linesArray = File.ReadAllLines(DataFileName);
                    foreach (string line in linesArray)
                    {
                        try
                        {
                            Trip trip = new Trip(line);
                            TripList.Add(trip);
                        }
                        catch (InvalidDataException ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error reading from file: " + ex.Message);
                }

            }
            //Populating list view with file data
            lvTripList.ItemsSource = TripList;
        }

        private void btnAddTrip_Click(object sender, RoutedEventArgs e)
        {
            // validating fields before adding to the list
            if (txtDestination.Text == "" || txtName.Text == "" || txtPassport.Text == "" || dpDepartureDate.Text == "")
            {
                MessageBox.Show("Please review your trip information. It cannot have blank fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string destination = txtDestination.Text;
            string name = txtName.Text;
            string passport = txtPassport.Text;
            string departureDate = dpDepartureDate.Text;
            string returnDate = dpReturnDate.Text;

            Trip trip = new Trip(destination,name,passport,departureDate,returnDate);
            TripList.Add(trip);

            ResetValues();
        }

        private void lvTripList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Activating buttons when Item in the list is selected
            btnUpdateTrip.IsEnabled = true;
            btnDeleteTrip.IsEnabled = true;


            var selectedTrip = lvTripList.SelectedItem;

            if (selectedTrip is Trip)
            {
                Trip trip = (Trip)lvTripList.SelectedItem;


                // Assigning values as an object inside the application
                txtDestination.Text = trip.Destination;
                txtName.Text = trip.Name;
                txtPassport.Text = trip.Passport;
                dpDepartureDate.Text = trip.DepartureDate;
                dpReturnDate.Text = trip.ReturnDate;
            }
        }

        public void SaveFile()
        {
            using (StreamWriter writer = new StreamWriter(DataFileName))
            {
                foreach (Trip trip in TripList)
                {
                    writer.WriteLine(trip.ToDataString());
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFile();
        }

        private void btnUpdateTrip_Click(object sender, RoutedEventArgs e)
        {
            // Checking if at least one item is selected before updating ( Update button isSelected attribute is False ) 
            if (lvTripList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select at least one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtDestination.Text == "" || txtName.Text == "" || txtPassport.Text == "")
            {
                MessageBox.Show("Please review your trip information. It cannot have blank fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string newDestination = txtDestination.Text;
            string newName = txtName.Text;
            string newPassport = txtPassport.Text;
            string newDepartureDate = dpDepartureDate.Text;
            string newReturnDate = dpReturnDate.Text;

            Trip tripUpdate = (Trip)lvTripList.SelectedItem;

            tripUpdate.Destination = newDestination;
            tripUpdate.Name = newName;
            tripUpdate.Passport = newPassport;
            tripUpdate.DepartureDate = newDepartureDate;
            tripUpdate.ReturnDate = newReturnDate;

            ResetValues();
        }

        private void ResetValues()
        {
            lvTripList.Items.Refresh();
            txtDestination.Clear();
            txtName.Clear();
            txtPassport.Clear();
            dpDepartureDate.Text = "";
            dpReturnDate.Text = "";
            lvTripList.SelectedIndex = -1;
            btnUpdateTrip.IsEnabled = false;
            btnDeleteTrip.IsEnabled = false;
        }

        private void btnDeleteTrip_Click(object sender, RoutedEventArgs e)
        {
            Trip tripDelete = (Trip)lvTripList.SelectedItem;
            TripList.Remove(tripDelete);

            ResetValues();
        }

        private void btnSaveSelected_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Trip file (*.trip)|*.trip";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    foreach (Trip trip in TripList)
                    {
                        writer.WriteLine(trip.ToDataString());
                    }
                }
            }
        }
    }
}
