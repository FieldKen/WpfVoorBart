using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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

namespace WpfVoorBart
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (MyDataContext ctx = new MyDataContext())
            {
                MyCombobox.ItemsSource = ctx.Students.ToList();
                MyCombobox.SelectedValuePath = "Id";
                MyCombobox.DisplayMemberPath = "Name";
            }
        }

        private void MyCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (MyDataContext ctx = new MyDataContext())
            {
                if (MyCombobox.SelectedItem != null)
                {
                    Student s = ctx.Students.FirstOrDefault(s => s.Id == Convert.ToInt32((sender as ComboBox).SelectedValue));
                    MessageBox.Show($"{s.Name} - {s.Age} - {s.Address}");
                }
            }
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            using (MyDataContext ctx = new MyDataContext())
            {
                ctx.Students.Add(new Student(MyTextBox.Text, new Random().Next(18, 90), $"Stationstraat {new Random().Next(1, 100)}"));
                ctx.SaveChanges();
                MyTextBox.Text = string.Empty;

                MyCombobox.ItemsSource = null;
                MyCombobox.ItemsSource = ctx.Students.ToList();
            }
        }
    }

    public class MyDataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=WpfDatabaseExample;Trusted_Connection=True;");
        }

        public DbSet<Student> Students { get; set; }
    }
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }

        public Student(string name, int age, string address)
        {
            Name = name;
            Age = age;
            Address = address;
        }
    }
}
