using SYAPSU.classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SYAPSU.User_controls
{
    /// <summary>
    /// Логика взаимодействия для Operators.xaml
    /// </summary>
    public partial class Operators : UserControl
    {
        public Operators()
        {
            InitializeComponent();
            Link.operators = this;
            listBoxOperators = listBoxOperators;
            Link.funcOff = DisablePosOperators;
            foreach (var op in listBoxOperators.Items)
            {
                if(op is ListBoxItem)
                {
                    (op as ListBoxItem).MouseMove += MouseMoveItems;
                }
                
            }
        }

        public void EnableOperators()
        {
            foreach (var op in listBoxOperators.Items)
            {
                if (op is ListBoxItem && !(op as ListBoxItem).IsEnabled)
                {
                    (op as ListBoxItem).IsEnabled = true;
                }

            }
        }

        private void MouseMoveItems(object sender, MouseEventArgs e)
        {
            Point mPos = e.GetPosition(null);

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(mPos.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(mPos.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                try
                {
                    ListBoxItem listBoxItem = (ListBoxItem)listBoxOperators.SelectedItem;
                    DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, listBoxItem), DragDropEffects.Copy);
                    if (listBoxItem.Parent == null)
                    {
                        listBoxOperators.Items.Add(listBoxItem);
                    }
                }
                catch { }
            }
        }

        private void DisablePosOperators()
        {
            for(int i = 0; i < 2; i++)
            {
                ((ListBoxItem)listBoxOperators.Items[i]).IsEnabled = false;
            }
        }

        public void DisableOperators()
        {
            for(int i = 0; i < listBoxOperators.Items.Count; i++)
            {
                if(i < 2)
                {
                    ((ListBoxItem)listBoxOperators.Items[i]).IsEnabled = true;
                }
                else
                {
                    ((ListBoxItem)listBoxOperators.Items[i]).IsEnabled = false;
                }
            }
        }
    }
}
