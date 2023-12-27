using SYAPSU.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace SYAPSU
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        bool finish = false;
        public TaskWindow()
        {
            InitializeComponent();
            List<Operator> operators = new List<Operator>();
            foreach (Operator op in Link.GetOperators())
            {
                operators.Add(op);
            }
            Link.Logging(" Старт тренеровки ", LogLvl.info);
            Start(operators);
        }
        private void StartWorkout(List<Operator> operators, int indx, int count = -1)
        {
            if (indx >= operators.Count || count == 0) 
            {
                return;
            }
            Operator @operator = operators[indx];
            if(@operator.Param.Length > 0)
            {
                switch (@operator.Name) 
                {
                    case "Серия":
                        Dispatcher.Invoke(new Action(() =>
                        {
                            labelName.Content = @operator.Param[1].name + ": " + Link.ExerciseById(@operator.Param[1].value);
                            labelParam.Content = @operator.Param[0].name + ": " + Link.DistanceById(@operator.Param[0].value) + " " +
                                @operator.Param[2].name + ": " + @operator.Param[2].value.ToString();
                            SetImage(Link.ExerciseById(@operator.Param[1].value));
                        }));
                        for(int i =  0; i < @operator.Param[2].value; i++ )
                        {
                            Dispatcher.Invoke(new Action(() =>
                            {
                                labelParam.Content = @operator.Param[0].name + ": " + Link.DistanceById(@operator.Param[0].value) + " " +
                                    @operator.Param[2].name + ": " + (@operator.Param[2].value - i).ToString();
                            }));
                            Thread.Sleep(4000);
                        }
                        StartWorkout(operators, indx + 1, count);
                        break;
                    case "Метод круговой тренировки":
                        var end = operators.FirstOrDefault(x => x.Name == "Конец");
                        int indxEnd = operators.IndexOf(end) + 1;
                        var newList = operators.Take(new Range(indx + 1, indxEnd)).ToList();
                        operators.RemoveRange(indx, indxEnd - indx);
                        StartWorkout(newList, 0, @operator.Param[0].value);
                        break;
                    default:
                        Dispatcher.Invoke(new Action(() =>
                        {
                            labelName.Content = @operator.Name;
                            labelParam.Content = @operator.Param[0].name + ": " + @operator.Param[0].value.ToString();
                            SetImage(@operator.Name);
                        }));
                        for(int  i = 0; i != @operator.Param[0].value; i++)
                        {
                            Dispatcher.Invoke(new Action(() =>
                            {
                                labelParam.Content = @operator.Param[0].name + ": " + (@operator.Param[0].value - i).ToString();
                            }));
                            Thread.Sleep(1000);
                        }
                        StartWorkout(operators, indx + 1, count);
                        break;
                }
            }
            else if (@operator.Name != "Конец")
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    labelName.Content = @operator.Name;
                    labelParam.Content = "";
                    SetImage(@operator.Name);
                }));
                Thread.Sleep(4000);
                StartWorkout(operators, indx + 1, count);
            }
            else
            {
                count--;
                StartWorkout(operators, 0, count);
            }
        }

        private void SetImage(string name)
        {
            try
            {
                var image1 = new BitmapImage();
                image1.BeginInit();
                image1.UriSource = new Uri("/images/" + name + ".gif", UriKind.Relative);
                image1.EndInit();
                ImageBehavior.SetAnimatedSource(image, image1);
            }
            catch (Exception ex)
            {
                image.Source = new BitmapImage(new Uri("/images/NoImage.png", UriKind.Relative));
            }
        }

        async private void Start(List<Operator> operators)
        {
            await Task.Run(() => { StartWorkout(operators, 0); });
            Dispatcher.Invoke(new Action(() =>
            {
                Close();
            }));
        }
    }
}
