﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Lab3_Pyda
{
    public partial class AddUser : Window
    {
        private string nameField;
        private string surnameField;
        private string peselField;
        private string phoneField;
        private string adressField;
        private string cityField;

        public AddUser()
        {
            InitializeComponent();
        }

        private void Button_Dodaj(object sender, RoutedEventArgs e)
        {
            nameField = textBoxImie.Text;
            surnameField = textBoxNazwisko.Text;
            peselField = textBoxPesel.Text;
            phoneField = textBoxNumer.Text;
            adressField = textBoxAdress.Text;
            cityField = textBoxMiasto.Text;

            try
            {
                MainWindow.PersonList.Add(new MainWindow.Person() { Firstname = nameField, Lastname = surnameField, City = cityField, Pesel = peselField, Phone = phoneField, Adress = adressField });
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
        }

        /*
        private void ImageLoad(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                imgDynamic.Source = new BitmapImage(fileUri);
            }

        }
        */
    }
}
