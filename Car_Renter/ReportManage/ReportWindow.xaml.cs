using Microsoft.Reporting.WinForms;
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
using System.Windows.Shapes;

namespace Car_Renter.ReportManage
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        
        public ReportWindow()
        {
            InitializeComponent();


            ReportViewr1.SetDisplayMode(DisplayMode.PrintLayout);

            Loaded += ReportWindow_Loaded;
        }

       public enum Parameter { ClientName, Nationality, PaperType, PaperNumber, Birthday, Address, Phone, LicenseNumber, DateIssued, DateExpired , ContractNumber }

        public enum CarData { CarNo, CarModel, CarColor, DateOut, DateIn, DayNo, CounterOnRecive }


        public enum DriverSecound { SecDriver, OtherPaperNumber, OtherLicenseNo, OtherDateIssued, OtherDateExpired }


        public enum Cost { DailyCost, TotalAmount, Payment, Discount, SubTotal }

        private async void ReportWindow_Loaded(object sender, RoutedEventArgs e)
        {
           

        }
    }
}
