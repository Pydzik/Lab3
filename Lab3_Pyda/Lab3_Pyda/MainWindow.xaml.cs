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
        SqlCommand command;
        string sql;

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
                
                
                command = new SqlCommand(sql, cnn);
                dataReader = command.ExecuteReader();

                if (!dataReader.Read())
                {
                    SqlCommand insert_stored;

                    insert_stored = new SqlCommand("_InsertDB @pesel = @pesel, @FirstName = @FirstName, @LastName = @LastName, @DataUrodzenia = @DataUrodzenia, @Miasto = @Miasto, @Ulica = @Ulica, @Kraj = @Kraj", cnn);
                    insert_stored.Parameters.Add("pesel", System.Data.SqlDbType.Char).Value = person.Pesel;
                    insert_stored.Parameters.Add("FirstName", System.Data.SqlDbType.VarChar).Value = person.Firstname;
                    insert_stored.Parameters.Add("LastName", System.Data.SqlDbType.VarChar).Value = person.Lastname;
                    insert_stored.Parameters.Add("DataUrodzenia", System.Data.SqlDbType.DateTime).Value = person.Birthday.ToString("yyyy-MM-dd");
                    insert_stored.Parameters.Add("Miasto", System.Data.SqlDbType.VarChar).Value = person.City;
                    insert_stored.Parameters.Add("Ulica", System.Data.SqlDbType.VarChar).Value = person.Adress;
                    insert_stored.Parameters.Add("Kraj", System.Data.SqlDbType.VarChar).Value = person.Country;
                    dataReader.Close();
                    command.Dispose();

                    int insertStored = insert_stored.ExecuteNonQuery();

                    insert_stored.Dispose();
                    MessageBox.Show("Dodano nowy rekord!");
                    continue;
                }
                else
                {
                    SqlCommand update_stored;

                    update_stored = new SqlCommand("_UpdateDB @pesel = @pesel, @FirstName = @FirstName, @LastName = @LastName, @DataUrodzenia = @DataUrodzenia, @Miasto = @Miasto, @Ulica = @Ulica, @Kraj = @Kraj", cnn);
                    update_stored.Parameters.Add("pesel", System.Data.SqlDbType.Char).Value = person.Pesel;
                    update_stored.Parameters.Add("FirstName", System.Data.SqlDbType.VarChar).Value = person.Firstname;
                    update_stored.Parameters.Add("LastName", System.Data.SqlDbType.VarChar).Value = person.Lastname;
                    update_stored.Parameters.Add("DataUrodzenia", System.Data.SqlDbType.DateTime).Value = person.Birthday.ToString("yyyy-MM-dd");
                    update_stored.Parameters.Add("Miasto", System.Data.SqlDbType.VarChar).Value = person.City;
                    update_stored.Parameters.Add("Ulica", System.Data.SqlDbType.VarChar).Value = person.Adress;
                    update_stored.Parameters.Add("Kraj", System.Data.SqlDbType.VarChar).Value = person.Country;


                    dataReader.Close();
                    command.Dispose();

                    int updateStored = update_stored.ExecuteNonQuery();

                    update_stored.Dispose();
                    continue;

                }

                
                


            }            
            command.Dispose();


        }

        private void IleWpisow(object sender, RoutedEventArgs e)
        {
            SqlCommand skalarna = new SqlCommand("_Show_IleWpisow", cnn);
            MessageBox.Show("W bazie danych znajduje się " + skalarna.ExecuteScalar().ToString()+" wpisów.");
            skalarna.Dispose();
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
