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
    /// <summary>
    /// Logika interakcji dla klasy EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        public int klucz;
        public EditUser(int item)
        {
            klucz = item;
            InitializeComponent();

            textBoxPesel.Text = MainWindow.PersonList[klucz].Pesel;
            textBoxNazwisko.Text = MainWindow.PersonList[klucz].Lastname;
            textBoxImie.Text = MainWindow.PersonList[klucz].Firstname;
            textBoxAdress.Text = MainWindow.PersonList[klucz].Adress;
            textBoxCity.Text = MainWindow.PersonList[klucz].City;
            textBoxCountry.Text = MainWindow.PersonList[klucz].Country;
            DatePickerBirthday.SelectedDate = MainWindow.PersonList[klucz].Birthday;
            //x.Content = klucz;
            imgDynamic.Source = MainWindow.PersonList[klucz].Image;
        }

        private void Modify(object sender, RoutedEventArgs e)
        {
            MainWindow.PersonList[Convert.ToInt32(klucz)].Pesel = textBoxPesel.Text;
            MainWindow.PersonList[Convert.ToInt32(klucz)].Lastname = textBoxNazwisko.Text;
            MainWindow.PersonList[Convert.ToInt32(klucz)].Firstname = textBoxImie.Text;
            MainWindow.PersonList[Convert.ToInt32(klucz)].Adress = textBoxAdress.Text;
            MainWindow.PersonList[Convert.ToInt32(klucz)].City = textBoxCity.Text;
            MainWindow.PersonList[(klucz)].Birthday = (DateTime)DatePickerBirthday.SelectedDate;
            MainWindow.PersonList[Convert.ToInt32(klucz)].Image = (BitmapImage)imgDynamic.Source;
            this.Close();
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

        private void textBox_Preview_DIGITS(object sender, TextCompositionEventArgs e)
        {
            DataValidator.HandleOnlyDigits(e);
        }

        private void textBox_PreviewTextInput_WORDS(object sender, TextCompositionEventArgs e)
        {
            DataValidator.HandleOnlyWords(e);
        }


    }
}
