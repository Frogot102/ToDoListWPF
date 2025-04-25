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
using System.Globalization;

namespace ToDoListWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<TodoItem> Tasks { get; set; } = new ObservableCollection<TodoItem>();
        public ObservableCollection<TodoItem> FilteredTasks { get; set; } = new ObservableCollection<TodoItem>();
        public MainWindow()
        {
            InitializeComponent();
            TaskListLb.ItemsSource = FilteredTasks;
            UpdateCounter();
        }

        private void AddTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskInputTb.Text))
            {
                var selectedCategory = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                Tasks.Add(new TodoItem
                {
                    Title = TaskInputTb.Text,
                    IsDone = false,
                    DueDate = DueDatePicker.SelectedDate,
                    Category = selectedCategory
                });
                TaskInputTb.Text = string.Empty;
                DueDatePicker.SelectedDate = DateTime.Today;
                CategoryComboBox.SelectedIndex = -1;
                ApplyFilter();
                UpdateCounter();
            }
        }

        private void DeleteTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is TodoItem item)
            {
                Tasks.Remove(item);
                ApplyFilter();
                UpdateCounter();
            }
        }
        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            UpdateCounter();
            ApplyFilter();
        }
        public void UpdateCounter()
        {
            int counter = 0;
            foreach (var task in Tasks)
            {
                if (!task.IsDone)
                {
                    counter++;
                }
            }
            CounterTextTbl.Text = $"Осталось дел: {counter}";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }
        private void ApplyFilter()
        {
            var SearchText = SearchTextBox.Text.ToLower();
            var SelectedCategory = (CategoryFilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            
            //Фильтр по заголовку
            var FilteredByTitle = Tasks.Where(Task => Task.Title != null && Task.Title.Contains(SearchText)).ToList();

            //Фильтр по категории
            List<TodoItem> finalFiltered;
            if (string .IsNullOrEmpty(SelectedCategory) || SelectedCategory == "Все")
            {
                finalFiltered = FilteredByTitle;
            }
            else
            {
                finalFiltered = FilteredByTitle.Where(task => task.Category == SelectedCategory).ToList();
            }

            //Сортировка
            var selectedSort = (SortComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (selectedSort)
            {
                case "По дате (Возрастание)":
                    finalFiltered = finalFiltered.OrderBy(task => task.DueDate).ToList();
                    break;
                case "По дате (Убывание)":
                    finalFiltered = finalFiltered.OrderByDescending(task => task.DueDate).ToList();
                    break;
                case "По статусу":
                    finalFiltered = finalFiltered.OrderBy(task => task.IsDone).ToList();
                    break;
                case "По категории":
                    finalFiltered = finalFiltered.OrderBy(task => task.Category).ToList();
                    break;
                case "Без сортировки":
                default:
                    break;

            }
            FilteredTasks.Clear();
            foreach (var task in finalFiltered)
            {
                FilteredTasks.Add(task);
            }
        }

        private void gif_MediaEnded(object sender, RoutedEventArgs e)
        {
            gif.Position = new TimeSpan(0, 0, 1);
            gif.Play();
        }

        private void DeleteCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            var remaining = Tasks.Where(task => !task.IsDone).ToList();
            Tasks.Clear();
            foreach (var task in remaining)
            {
                Tasks.Add(task);
            }
            UpdateCounter();
            ApplyFilter();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void gif2_MediaEnded(object sender, RoutedEventArgs e)
        {
            gif2.Position = new TimeSpan(0, 0, 1);
            gif2.Play();
        }

        private void gif3_MediaEnded(object sender, RoutedEventArgs e)
        {
            gif.Position = new TimeSpan(0, 0, 1);
            gif.Play();
        }
    }
    public class TodoItem
    {
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public DateTime? DueDate { get; set; }
        public string Category { get; set; }
    }
    public class DoneToTextDecorationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDone = (bool)value;
            return isDone ? TextDecorations.Strikethrough : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
