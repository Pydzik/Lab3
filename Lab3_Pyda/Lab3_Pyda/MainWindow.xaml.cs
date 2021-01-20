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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using System.Data.SqlClient;

namespace Lab3_Pyda
{

    public partial class MainWindow : Window
    {

        string connectionString = "MultipleActiveResultSets=True";
        SqlConnection cnn;
        SqlCommand command, insert_command;
        string sql, sql_insert, sql_insert2, Output = "";

        [XmlArray("DataGridXAML"), XmlArrayItem(typeof(List<Person>), ElementName = "Person")]
        public static List<Person> PersonList = new List<Person>();
        AddUser formularz = new AddUser();
        public MainWindow()
        {
            InitializeComponent();
            

            

            connectionString = @"Data Source=LENOVO-Y720;Initial Catalog=Projekt;User ID=Lenovo-Y720;Trusted_Connection=True;";
            cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
            }
            catch
            {
                MessageBox.Show("Nastąpił błąd podczas łączenia z bazą danych!");
            }
            

            formularz.Show();
        }


        private void SaveXML(object oSender, RoutedEventArgs eRoutedEventArgs)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Person>));

            using (FileStream fs = new FileStream("Projekt.xml", FileMode.Create))
            {
                ser.Serialize(fs, PersonList);
            }
        }

        private void LoadXML(object osender, RoutedEventArgs eRoutedEventArgs)
        {

            try
            {
                var mySerializer = new XmlSerializer(typeof(List<Person>));
                var myFileStream = new FileStream("Projekt.xml", FileMode.Open);

                PersonList = (List<Person>)mySerializer.Deserialize(myFileStream);
                ListViewXAML.ItemsSource = PersonList;
            }
            catch
            {
                MessageBox.Show("Plik nie istnieje");
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
            public string Country { get; set; }
            public DateTime Birthday { get; set; }
            [XmlIgnore()]
            public BitmapImage Image { get; set; }

            [XmlElement("Imgxml")]
            public string imgxml { get { return Image.UriSource.ToString(); } set { Image = new BitmapImage(new Uri(value)); } }
        }

        private void SaveDB(object sender, RoutedEventArgs e)
        {
            SqlDataReader dataReader;


            foreach (Person person in PersonList)
            {
                

                sql = "SELECT Pesel FROM dbo.Ludzie WHERE Pesel LIKE " + person.Pesel + ";";
                sql_insert = "INSERT INTO dbo.Ludzie VALUES (\'" + person.Pesel + "\', \'" + person.Firstname + "\', \'" + person.Lastname + "\', \'" + person.Birthday.ToString("yyyy-MM-dd") + "\');";
                sql_insert2 = "INSERT INTO dbo.Zamieszkanie VALUES (\'" + person.Pesel + "\', \'" + person.City + "\', \'" + person.Adress + "\', \'" + person.Country + "\');";
                //MessageBox.Show(sql_insert);
                command = new SqlCommand(sql, cnn);
                dataReader = command.ExecuteReader();
                //MessageBox.Show("Próba dodania... " + person.Pesel);

                if (!dataReader.Read())
                {
                    dataReader.Close();
                    command.Dispose();

                    insert_command = new SqlCommand(sql_insert, cnn);
                    int dataInsert = insert_command.ExecuteNonQuery();
                    insert_command.Dispose();
                    insert_command = new SqlCommand(sql_insert2, cnn);
                    int dataInsert2 = insert_command.ExecuteNonQuery();


                    insert_command.Dispose();
                    MessageBox.Show("Dodano nowy rekord!");
                    continue;
                }

                dataReader.Close();
                command.Dispose();
                
                


            }            
            command.Dispose();
            insert_command.Dispose();

        }

        private void LoadDB(object sender, RoutedEventArgs e)
        {
            MainWindow.PersonList.Clear();
            SqlDataReader dataReader;
            sql = "SELECT FirstName, LastName, dbo.Ludzie.PESEL, Miasto, DataUrodzenia, Kraj, Ulica FROM dbo.Ludzie INNER JOIN dbo.Zamieszkanie ON dbo.Ludzie.PESEL = dbo.Zamieszkanie.PESEL";

            command = new SqlCommand(sql, cnn);
            dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {

                try
                {
                    MainWindow.PersonList.Add(new MainWindow.Person() { Firstname = (string)dataReader.GetValue(0), Lastname = (string)dataReader.GetValue(1), Pesel = (string)dataReader.GetValue(2), City = (string)dataReader.GetValue(3), Birthday = (DateTime)dataReader.GetValue(4), Country = (string)dataReader.GetValue(5), Adress = (string)dataReader.GetValue(6) });
                    RefreshWindow(sender, e);
                }
                catch (Exception blad)
                {
                    MessageBox.Show(blad.Message);
                }
            }
            dataReader.Close();
            command.Dispose();
        }

        public void ListViewXAML_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedIndex;
            if (item >= 0)
            {
                EditUser mod = new EditUser(item);
                mod.Show();
            }
        }
    }
}
