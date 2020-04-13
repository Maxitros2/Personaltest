using System;
using System.Collections.Generic;
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

namespace PersonaTest
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public bool isSelected = false;
        public UserControl1()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            ((Border)sender).BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9AD1E6"));
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isSelected)
                return;
            ((Border)sender).BorderBrush = Brushes.Transparent;

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            isSelected = !isSelected;
            if(isSelected)
                ((Border)sender).Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9AD1E6"));
            else
                ((Border)sender).Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF4F7F0"));
        }
    }
}
