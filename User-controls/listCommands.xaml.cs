using SYAPSU.classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SYAPSU.User_controls
{
    /// <summary>
    /// Логика взаимодействия для listCommands.xaml
    /// </summary>
    public partial class listCommands : UserControl
    {
        bool isRerange = false;
        public listCommands()
        {
            InitializeComponent();
            Link.listCommands = this;
        }
        
        private void ListBoxCommandsDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is ListBoxItem listitem)
            {
                if(listitem.Parent != listBoxCommands)
                {
                    AddElement(listitem.Content.ToString());
                }
            }
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            isRerange = true;
            int selectedIndex = listBoxCommands.SelectedIndex;
            if (selectedIndex != 0)
                if (e.Key == Key.W)
                {
                    if (selectedIndex > 1)
                    {
                        ListBoxItem item = (ListBoxItem)listBoxCommands.SelectedItem;
                        if(item.Content.ToString() != "Конец")
                        {
                            Swap(selectedIndex, selectedIndex - 1, item);
                            Link.Logging(" Оператор \"" + item.Content.ToString() + "\" пермещен", LogLvl.info);
                        }
                        else
                        {
                            ListBoxItem nextItem = (ListBoxItem)listBoxCommands.Items[selectedIndex - 1];
                            if(!Link.CheckLoop(nextItem.Content.ToString()))
                            {
                                Swap(selectedIndex, selectedIndex - 1, item);
                                Link.Logging(" Оператор \"" + item.Content.ToString() + "\" пермещен", LogLvl.info);
                            }
                        }
                    }
                    else
                        Link.Logging(" Оператор \"" + ((ListBoxItem)listBoxCommands.SelectedItem).Content.ToString() + "\" невозможно переместить", LogLvl.warn);
                }
                else if (e.Key == Key.S)
                {
                    if (selectedIndex < listBoxCommands.Items.Count - 1)
                    {
                        ListBoxItem item = (ListBoxItem)listBoxCommands.SelectedItem;
                        if(!Link.CheckLoop(item.Content.ToString()))
                        {
                            Swap(selectedIndex, selectedIndex + 1, item);
                            Link.Logging(" Оператор \"" + item.Content.ToString() + "\" пермещен", LogLvl.info);
                        }
                        else
                        {
                            ListBoxItem nextItem = (ListBoxItem)listBoxCommands.Items[selectedIndex + 1];
                            if(nextItem.Content.ToString() != "Конец")
                            {
                                Swap(selectedIndex, selectedIndex + 1, item);
                                Link.Logging(" Оператор \"" + item.Content.ToString() + "\" пермещен", LogLvl.info);
                            }
                        }
                    }
                    else
                        Link.Logging(" Оператор \"" + ((ListBoxItem)listBoxCommands.SelectedItem).Content.ToString() + "\" невозможно переместить", LogLvl.warn);
                }
            isRerange = false;
        }

        public void AddElement(string element)
        {
            ListBoxItem itemCopy = new ListBoxItem();
            itemCopy.Content = element;
            if (Link.GetBegin())
            {
                if (!Link.CheckLoop(element) && Link.Add(element))
                {
                    listBoxCommands.Items.Add(itemCopy);
                }
                else if (Link.CheckLoop(element) && !Link.ContainsLoop && Link.Add(element))
                {
                    listBoxCommands.Items.Add(itemCopy);
                    listBoxCommands.Items.Add(new ListBoxItem()
                    {
                        Content = "Конец"
                    });
                }
            }
            else
            {
                listBoxCommands.Items.Insert(0, itemCopy);
                Link.Add(element);
            }
            Link.Logging(" Добавлен оператор \"" + itemCopy.Content.ToString() + "\"", LogLvl.info);
        }

        void Swap(int prevIndex, int nextIndex, object item)
        {
            listBoxCommands.Items.Remove(item);
            listBoxCommands.Items.Insert(nextIndex, item);
            listBoxCommands.SelectedIndex = nextIndex;
            Link.RerangeOperator(nextIndex, prevIndex);
        }

        private void listBoxCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isRerange)
            {
                int index = listBoxCommands.SelectedIndex;
                Link.ChangeConfiguration(Link.GetOperatorById(index));
                if(Link.GetOperatorById(index).Name != "Серия")
                {
                    Link.SetImage(Link.GetOperatorById(index).Name);
                }
                else
                {
                    Link.SetImage(Link.ExerciseById(Link.GetOperatorById(index).Param[1].value));
                }
            }
        }

        public void RemoveCommand()
        {
            isRerange = true;
            Operator tmp = Link.GetOperatorById(listBoxCommands.SelectedIndex);
            if (Link.GetStartPos().Contains(tmp.Name))
            {
                Link.RemoveOperator(Link.GetOperatorById(listBoxCommands.SelectedIndex));
                Link.DisableOperators();
            }
            else if(tmp.Name == "Метод круговой тренировки" || tmp.Name == "Конец")
            {
                Link.RemoveOperator(Link.GetOperatorById(listBoxCommands.SelectedIndex));
                listBoxCommands.Items.RemoveAt(listBoxCommands.SelectedIndex);
                string nameSecond = tmp.Name == "Конец" ? "Метод круговой тренировки" : "Конец";
                var item = listBoxCommands.Items.Cast<ListBoxItem>().First(x => x.Content.ToString() == nameSecond);
                listBoxCommands.SelectedItem = item;
                Link.RemoveOperator(Link.GetOperatorById(listBoxCommands.SelectedIndex));
                Link.ContainsLoop = false;
            }
            else
            {
                Link.RemoveOperator(Link.GetOperatorById(listBoxCommands.SelectedIndex));
            }
            Link.Logging(" Удален оператор \"" + ((ListBoxItem)listBoxCommands.SelectedItem).Content.ToString() + "\"", LogLvl.info);
            listBoxCommands.Items.RemoveAt(listBoxCommands.SelectedIndex);
            isRerange = false;
            listBoxCommands.SelectedIndex = 0;
        }

        public void ClearAll()
        {
            listBoxCommands.SelectionChanged -= listBoxCommands_SelectionChanged;
            listBoxCommands.Items.Clear();
            listBoxCommands.SelectionChanged += listBoxCommands_SelectionChanged;
        }

        public void AddCommand(string command)
        {
            ListBoxItem newItem = new ListBoxItem();
            newItem.Content = command;
            listBoxCommands.Items.Add(newItem);
        }
    }
}
