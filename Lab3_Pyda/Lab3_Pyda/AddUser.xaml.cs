using System;
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
using System.Xml.Serialization;
using System.Text.RegularExpressions;


namespace Lab3_Pyda
{
    public partial class AddUser : Window
    {
        private string nameField;
        private string surnameField;
        private string peselField;
        private string adressField;
        private string cityField;
        //private string birthdayField;
        private DateTime birthdayField;
        private string countryField;
        private BitmapImage imgadd;


        
        public AddUser()
        {
            InitializeComponent();
        }



        private void Button_Dodaj(object sender, RoutedEventArgs e)
        {

            nameField = textBoxImie.Text;
            surnameField = textBoxNazwisko.Text;
            peselField = textBoxPesel.Text;
            adressField = textBoxAdress.Text;
            cityField = textBoxCity.Text;
            birthdayField = (DateTime)DatePickerBirthday.SelectedDate.Value;
            countryField = textBoxCountry.Text;




            try
            {
                MainWindow.PersonList.Add(new MainWindow.Person() { Firstname = nameField, Lastname = surnameField, City = cityField, Pesel = peselField, Adress = adressField, Birthday = birthdayField, Country = countryField, Image = imgadd });
            }
            catch (Exception blad)
            {
                MessageBox.Show(blad.Message);
            }
        }




        private void Button_Photo(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            {
                string filePath;
                openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                openFileDialog.Filter = "Photo Images | *.png; *.jpg; *.jpeg";
                openFileDialog.Title = "Wybierz zdjęcie dla użytkownika";


                if (openFileDialog.ShowDialog() == true)
                {

                    filePath = openFileDialog.FileName;
                    Uri uri = new Uri(filePath);
                    imgDynamic.Source = new BitmapImage(uri);
                    imgadd = new BitmapImage(uri);
                }
            }
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidator.HandleOnlyOneWord(e);
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataValidator.HandleFindingWhiteSpace(e);
        }
    }
}
