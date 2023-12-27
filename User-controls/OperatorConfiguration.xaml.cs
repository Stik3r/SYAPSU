using SYAPSU.classes;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using Label = System.Windows.Controls.Label;

namespace SYAPSU.User_controls
{
    /// <summary>
    /// Логика взаимодействия для OperatorConfiguration.xaml
    /// </summary>
    public partial class OperatorConfiguration : UserControl
    {
        TextBox count;
        Operator @operator;
        public OperatorConfiguration()
        {
            InitializeComponent();
            Link.operatorConfiguration = this;
        }

        public void CreateComnfiguration(Operator @operator)
        {
            this.@operator = @operator;
            mainPanel.Children.Clear();
            Link.ClearImage();
            Label label = new Label()
            {
                Content = @operator.Name,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };
            DockPanel.SetDock(label, Dock.Top);
            mainPanel.Children.Add(label);
            SetParams(@operator);
        }

        private void SetParams(Operator @operator)
        {
            if (Link.GetTextBoxParam().Contains(@operator.Name))
            {
                for (int i = 0; i < @operator.Param.Length; i++)
                {

                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    Label paramName = new Label()
                    {
                        Content = @operator.Param[i].name,
                    };
                    paramName.Margin = new Thickness(0, 0, 10, 0);
                    panel.Children.Add(paramName);


                    TextBox textBox = new TextBox();
                    textBox.GotFocus += TextBox_GotFocus;
                    textBox.TextChanged += TextBox_TextChanged;
                    textBox.Text = @operator.Param[i].value.ToString();
                    textBox.PreviewTextInput += PreviewTextImput;
                    textBox.Width = 120;
                    textBox.Height = 20;
                    textBox.VerticalAlignment = VerticalAlignment.Top;
                    textBox.HorizontalAlignment = HorizontalAlignment.Right;
                    panel.Children.Add(textBox);

                    DockPanel.SetDock(panel, Dock.Top);
                    mainPanel.Children.Add(panel);
                }
            }
            else if (@operator.Name == "Серия")
            {
                for(int i = 0; i < @operator.Param.Length; i++)
                {
                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    Label paramName = new Label()
                    {
                        Content = @operator.Param[i].name,
                    };
                    paramName.Margin = new Thickness(0, 0, 10, 0);
                    panel.Children.Add(paramName);
                    if(i == 0)
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.Name = "distance";
                        comboBox.Width = 130;
                        comboBox.ItemsSource = new string[] { "Длинная вода", "Короткая вода" };
                        comboBox.VerticalAlignment = VerticalAlignment.Top;
                        comboBox.HorizontalAlignment = HorizontalAlignment.Right;
                        comboBox.SelectedIndex = @operator.Param[i].value;
                        comboBox.SelectionChanged += ComboBox_SelectionChanged;
                        panel.Children.Add(comboBox);
                        DockPanel.SetDock(panel, Dock.Top);
                        mainPanel.Children.Add(panel);
                    }
                    else if (i == 1)
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.Name = "type";
                        comboBox.Width = 130;
                        comboBox.ItemsSource = new string[] {"Вольный стиль", "На спине", "С доской", "Брасс", "Баттерфляй"};
                        comboBox.VerticalAlignment = VerticalAlignment.Top;
                        comboBox.HorizontalAlignment = HorizontalAlignment.Right;
                        comboBox.SelectedIndex = @operator.Param[i].value;
                        comboBox.SelectionChanged += ComboBox_SelectionChanged;
                        panel.Children.Add(comboBox);
                        DockPanel.SetDock(panel, Dock.Top);
                        mainPanel.Children.Add(panel);
                    }
                }
                MakeCheckboxField(@operator);
            }
            MakeButton();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((sender is ComboBox box) && box.Name == "type")
            {
                @operator.Param[1].value = box.SelectedIndex;
                Link.SetImage(box.SelectedItem.ToString());
            }
            else
            {
                @operator.Param[2].value = ((ComboBox)sender).SelectedIndex;
            }
            Link.Logging(" Изменены параметры оператора \"" + @operator.Name + "\"", LogLvl.info);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (@operator.Name == "Серия")
            {
                if (int.TryParse(((TextBox)sender).Text, out int result))
                {
                    result = result > 10000 ? 10000 : result;
                    ((TextBox)sender).Text = result.ToString();
                    @operator.Param[2].value = result;
                }
            }
            else if(@operator.Name == "Метод круговой тренировки")
            {
                if (int.TryParse(((TextBox)sender).Text, out int result))
                {
                    result = result > 10000 ? 10000 : result;
                    ((TextBox)sender).Text = result.ToString();
                    @operator.Param[0].value = result;
                }
            }
            else
            {
                if (int.TryParse(((TextBox)sender).Text, out int result))
                {
                    result = result > 1000000 ? 1000000 : result;
                    ((TextBox)sender).Text = result.ToString();
                    @operator.Param[0].value = result;
                }
            }
        }

        private void MakeCheckboxField(Operator @operator)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            Label paramName = new Label()
            {
                Content = "Кол-во повторений",
            };
            paramName.Margin = new Thickness(0, 0, 10, 0);
            panel.Children.Add(paramName);

            TextBox textBox = new TextBox();
            textBox.IsEnabled = @operator.Param[2].value > 0 ? true : false;
            count = textBox;
            textBox.Text = @operator.Param[2].value.ToString();
            textBox.PreviewTextInput += PreviewTextImput;
            textBox.GotFocus += TextBox_GotFocus;
            textBox.Width = 120;
            textBox.Height = 20;
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.HorizontalAlignment = HorizontalAlignment.Right;
            textBox.TextChanged += TextBox_TextChanged;
            panel.Children.Add(textBox);

            DockPanel.SetDock(panel, Dock.Top);
            mainPanel.Children.Add(panel);
        }

        private void MakeButton()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            Button button = new Button()
            {
                Content = "Удалить",
                Margin = new Thickness(10, 5, 0, 0)
            };
            button.Click += RemoveButtonClick;
            panel.Children.Add(button);
            DockPanel.SetDock(panel, Dock.Top);
            mainPanel.Children.Add(panel);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)e.OriginalSource;
            tb.Dispatcher.BeginInvoke(
                new Action(delegate
                {
                    tb.SelectAll();
                }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private void PreviewTextImput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            Link.Logging(" Изменены параметры оператора \"" + @operator.Name + "\"", LogLvl.info);
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            Link.CallRemoveOperator();
        }

        public void ClearAll()
        {
            mainPanel.Children.Clear();
        }
    }

    
}
