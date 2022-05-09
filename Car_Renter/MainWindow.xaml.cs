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

namespace Car_Renter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            GridMain.Children.Add(new Pages.Home());
        }

        private void btnMainButtons(object sender, RoutedEventArgs e)
        {
            double Width = GridNameButtons.ColumnDefinitions[1].ActualWidth;
            GridCursor.Width = Width;
            int index = int.Parse(((Button)e.Source).Uid);


            GridCursor.Margin = new Thickness(0 + (Width * index), 0, 0, 0);


          

            switch (index)
            {
                case 0:
                    GridMain.Children.Add(new Pages.Home());
                    GridMain.Children.Clear();
                    break;
                case 1:
                    PopRentTrans.IsOpen = true;
                    break;
                case 2:
                    //GridMain.Children.Add(new Pages.FarmCycle());
                    break;
                case 3:
                   
                    break;
                case 4:


                    break;
                case 5:


                    break;
                case 6:


                    break;
                case 7:

                    break;

            }
        }



        private void btnTopButtons(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            switch (index)
            {
                case 0:
                    if (WindowState == WindowState.Maximized)
                    {
                        WindowState = WindowState.Normal;
                        PackIconWindowsState.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                    }
                    else if (WindowState != WindowState.Maximized)
                    {
                        this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                        WindowState = WindowState.Maximized;
                        PackIconWindowsState.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                    }

                    break;
                case 1:
                    Application.Current.Shutdown();
                    break;
                case 2:
                    WindowState = WindowState.Minimized;
                    break;
                case 3:
                   
                    break;

                case 4:
                   
                    break;

                case 6:

                   

                    break;

                case 5:



                    break;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            PopRentTrans.IsOpen = false;

            GridMain.Children.Clear();

            switch (index)
            {
                case 0:
                    GridMain.Children.Add(new Pages.ClientsManage());
                    break;
                case 1:
                    GridMain.Children.Add(new Pages.CarsManage());
                    break;
                case 2:
                    GridMain.Children.Add(new Pages.RentContracts());
                    break;
                case 3:
                    GridMain.Children.Add(new Pages.CarIsOut());
                    break;
            }
        }
    }
}
