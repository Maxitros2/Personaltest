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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        public string selectedclass = "A";
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            
            ((Border)sender).BorderBrush= (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF2D4BD1"));
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((Border)sender).Name == selectedclass)
                return;

            ((Border)sender).BorderBrush = Brushes.Transparent;
        }

        private void A_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            A.BorderBrush = Brushes.Transparent;
            B.BorderBrush = Brushes.Transparent;
            C.BorderBrush = Brushes.Transparent;
            selectedclass = ((Border)sender).Name;
            
        }
    }
}
