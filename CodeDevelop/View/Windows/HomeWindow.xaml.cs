using CodeDevelop.Model;

using System;
//using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
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
using System.Configuration;

namespace CodeDevelop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {

        private ObservableCollection<Training> _trainings;
        private Training _selectedTraining;
        private List<TrainingTest> _tests;
        private int _currentTestIndex = 0;
        private int _currentTabIndex = 0;

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

            LoadData();

            // Заполняем Listview данными из таблицы Training
            //TrainingList.ItemsSource = App.context.Training.ToList();
            TrainingList.ItemsSource = Trainings;

            // Заполняем TextBlock данными из таблицы Training
            //TrainingDescriptionTb.DataContext = App.context.Training.ToList();
            //TrainingDescriptionTb.DataContext = (TrainingList.SelectedItem as Training).description;

            TrainingList.SelectedIndex = 0;

        }

        private void LoadData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CodeDevelopEntities2"].ConnectionString;
            string query = "SELECT id, name, description, difficultyID, trainNum, nameLanguage FROM Training t where exists(select null from UserTraining u where u.userid=" + App.currentUser.Id + " and t.id=u.trainingid)";

            Trainings = new ObservableCollection<Training>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        Trainings.Add(new Training
                        {
                            Id = Convert.ToInt32(row["id"]),
                            name = row["name"] as string,
                            description = row["description"] as string,
                            difficultyID = Convert.ToInt32(row["difficultyID"]),
                            trainNum = Convert.ToInt32(row["trainNum"]),
                            nameLanguage = row["nameLanguage"] as string
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                }
            }
        }

        private void LoadTests(int trainingId)
        {
            // Получение строки подключения из файла конфигурации
            string connectionString = ConfigurationManager.ConnectionStrings["CodeDevelopEntities2"].ConnectionString;

            // SQL-запрос для выбора данных из таблицы TrainingTest
            string query = "SELECT Id, text, answer1, answer2, answer3, answer4, rightAnswer, trainingId FROM TrainingTest WHERE trainingId = @trainingId";

            _tests = new List<TrainingTest>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@trainingId", trainingId);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        _tests.Add(new TrainingTest
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            text = row["text"] as string,
                            answer1 = row["answer1"] as string,
                            answer2 = row["answer2"] as string,
                            answer3 = row["answer3"] as string,
                            answer4 = row["answer4"] as string,
                            rightAnswer = row["rightAnswer"] as string,
                            trainingId = Convert.ToInt32(row["trainingId"])
                        });
                    }

                    if (_tests.Count > 0)
                    {
                        ShowTest(_currentTestIndex);
                    }
                    else
                    {
                        MessageBox.Show("Тесты для данного тренировочного занятия не найдены.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке тестов: " + ex.Message);
                }
            }
        }

        private void ShowTest(int index)
        {
            if (index >= 0 && index < _tests.Count)
            {
                var test = _tests[index];
                TestQuestionTb.Text = test.text;
                RadioButton1.Content = test.answer1;
                RadioButton2.Content = test.answer2;
                RadioButton3.Content = test.answer3;
                RadioButton4.Content = test.answer4;

                // Сбросить выбор радиокнопок
                RadioButton1.IsChecked = false;
                RadioButton2.IsChecked = false;
                RadioButton3.IsChecked = false;
                RadioButton4.IsChecked = false;

                // Показать вкладку с тестом
                TrainingStep.SelectedIndex = 1;
            }
            else
            {
                MessageBox.Show("Тесты завершены.");
                // Перейти к следующей вкладке или завершить тестирование
                TrainingStep.SelectedIndex = 2; // Предполагается, что у вас есть вкладка для завершения теста
            }
        }

        private void CheckAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTestIndex >= 0 && _currentTestIndex < _tests.Count)
            {
                var test = _tests[_currentTestIndex];
                string selectedAnswer = "";

                if (RadioButton1.IsChecked == true)
                {
                    selectedAnswer = test.answer1;
                }
                else if (RadioButton2.IsChecked == true)
                {
                    selectedAnswer = test.answer2;
                }
                else if (RadioButton3.IsChecked == true)
                {
                    selectedAnswer = test.answer3;
                }
                else if (RadioButton4.IsChecked == true)
                {
                    selectedAnswer = test.answer4;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите один из вариантов ответа.");
                    return;
                }

                if (selectedAnswer == test.rightAnswer)
                {
                    MessageBox.Show("Правильный ответ!");
                }
                else
                {
                    MessageBox.Show("Неправильный ответ. Правильный ответ: " + test.rightAnswer);
                }

                selectedAnswer = string.Empty;
                // Перейти к следующему тесту
                _currentTestIndex++;
                ShowTest(_currentTestIndex);
            }
        }


        private void ProfileBt_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ExitBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow();
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

            LoadTests(selectedItem.Id);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            /*switch (_currentTabIndex)
            {
                case 0:
                    // Перейти к вкладке с тестом
                    if (_tests != null && _tests.Count > 0)
                    {
                        TrainingStep.SelectedIndex = 1;
                        _currentTabIndex = 1;
                        _currentTestIndex = 0;
                        ShowTest(_currentTestIndex);
                    }
                    else
                    {
                        MessageBox.Show("Тесты для данного тренировочного занятия не найдены.");
                    }
                    break;
                case 1:
                    // Проверить ответы на тестах
                    if (_currentTestIndex >= 0 && _currentTestIndex < _tests.Count)
                    {
                        MessageBox.Show("Вы еще не завершили все тесты.");
                        ShowTest(_currentTestIndex);
                    }
                    else
                    {
                        // Все тесты завершены, перейти к вкладке завершения
                        TrainingStep.SelectedIndex = 2;
                        _currentTabIndex = 2;
                        NextButton.Content = "Завершить";
                    }
                    break;
                case 2:
                    // Завершение тестирования
                    MessageBox.Show("Тестирование завершено.");
                    // Здесь можно добавить дополнительную логику после завершения тестирования
                    break;
                default:
                    break;
            }*/
        }
    }
}
