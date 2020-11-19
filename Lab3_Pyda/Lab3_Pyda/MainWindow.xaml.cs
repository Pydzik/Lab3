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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;

namespace Lab3_Pyda
{

    public partial class MainWindow : Window
    {
        [XmlArray("DataGridXAML"), XmlArrayItem(typeof(List<Person>), ElementName = "Person")]
        public static List<Person> PersonList = new List<Person>();
        AddUser formularz = new AddUser();
        public MainWindow()
        {
            InitializeComponent();
            formularz.Show();
        }


        private void SaveXML(object oSender, RoutedEventArgs eRoutedEventArgs)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Person>));

            using (FileStream fs = new FileStream("lab3.xml", FileMode.Create))
            {
                ser.Serialize(fs, PersonList);
            }
        }

        private void LoadXML(object osender, RoutedEventArgs eRoutedEventArgs)
        {

            try
            {
                var mySerializer = new XmlSerializer(typeof(List<Person>));
                var myFileStream = new FileStream("lab3.xml", FileMode.Open);

                PersonList = (List<Person>)mySerializer.Deserialize(myFileStream);
                ListViewXAML.ItemsSource = PersonList;
            }
            catch
            {
                MessageBox.Show("Nie odnaleziono pliku");
            }


        }
        private void RefreshWindow(object sender, RoutedEventArgs e)
        {
            ListViewXAML.ItemsSource = null;
            ListViewXAML.ItemsSource = PersonList;
        }

        public class Person
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Pesel { get; set; }
            public string Phone { get; set; }
            public string City { get; set; }
            public string Adress { get; set; }
        }
    }
}
