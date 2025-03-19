using CodeDevelop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace CodeDevelop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {

        private ObservableCollection<Training> _trainings;
        private Training _selectedTraining;

        public ObservableCollection<Training> Trainings
        {
            get { return _trainings; }
            set { _trainings = value; }
        }

        public Training SelectedTraining
        {
            get { return _selectedTraining; }
            set
            {
                _selectedTraining = value;
                TrainingDescriptionTb.Text = _selectedTraining?.description;
            }
        }

        public HomeWindow()
        {
            InitializeComponent();

            // Заполняем Listview данными из таблицы Training
            TrainingList.ItemsSource = App.context.Training.ToList();

            // Заполняем TextBlock данными из таблицы Training
            //TrainingDescriptionTb.DataContext = App.context.Training.ToList();
            //TrainingDescriptionTb.DataContext = (TrainingList.SelectedItem as Training).description;

            TrainingList.SelectedIndex = 0;

        }

       
        private void StartLearningBt_Click(object sender, RoutedEventArgs e)
        {
            StartLearningBt.Visibility = Visibility.Collapsed;
            StopLearningBt.Visibility = Visibility.Visible;
            MessageBox.Show($"Начало изучения: {SelectedTraining.name}");
        }

        private void StopLearningBt_Click(object sender, RoutedEventArgs e)
        {
            StartLearningBt.Visibility = Visibility.Visible;
            StopLearningBt.Visibility = Visibility.Collapsed;
            MessageBox.Show($"Остановка изучения: {SelectedTraining.name}");
        }

        private void ProfileBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TrainingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = TrainingList.SelectedItem as Training;
            if (selectedItem != null)
            {
                TrainingDescriptionTb.Text = selectedItem.description;
            }
            else
            {
                TrainingDescriptionTb.Text = string.Empty;
            }
        }
    }
}
