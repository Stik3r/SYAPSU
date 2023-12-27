using SYAPSU.classes;
using System.Windows;
using System.Windows.Controls;

namespace SYAPSU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (Link.GetBegin())
            {
                TaskWindow workoutVIew = new TaskWindow();
                workoutVIew.Owner = this;
                workoutVIew.Show();
            }
            else
            {
                MessageBox.Show("Не установлено место старта");
                Link.Logging(" Не установлено место старта ", LogLvl.warn);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Link.SaveData();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            Link.ClearAll();
            var data = Link.LoadData();
            if(data != null)
            {
                Link.SetOperators(data);
                Link.CheckOperators();
                foreach (var item in data)
                {
                    Link.AddCommand(item.Name);
                }
                
            }
        }
    }
}
