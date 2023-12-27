using SYAPSU.classes;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace SYAPSU.User_controls
{
    /// <summary>
    /// Логика взаимодействия для ImageOperator.xaml
    /// </summary>
    public partial class ImageOperator : UserControl
    {
        public ImageOperator()
        {
            InitializeComponent();
            Link.imageOperator = this;
        }

        public void SetImage(string name)
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

        public void ClearImage()
        {
            image.Source = new BitmapImage();
        }
    }
}
