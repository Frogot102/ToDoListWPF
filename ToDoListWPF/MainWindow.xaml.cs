using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ToDoListWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ToDoItem> Tasks { get; set; } = new ObservableCollection<ToDoItem>();
        public MainWindow()
        {
            InitializeComponent();
            TaskListLb.ItemsSource = Tasks;
        }

        private void AddTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskInputTB.Text))
            {
                Tasks.Add(new ToDoItem { title = TaskInputTB.Text, IsDone = false});
                TaskInputTB.Text = string.Empty;
            }
        }

        private void DeleteTaskBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {

        }
        public  class ToDoItem
        {
            public string title { get; set; }
            public bool IsDone { get; set; }
        }
    }
}
