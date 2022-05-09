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
        Pages.VMContracts VMContracts;
        public ReportWindow(Pages.VMContracts vMContracts)
        {
            InitializeComponent();

            VMContracts = vMContracts;

            Loaded += ReportWindow_Loaded;
        }

        enum Parameter { ClientName, Nationality, PaperType, PaperNumber, Birthday, Address, Phone, LicenseNumber, DateIssued, DateExpired }

        enum CarData { CarNo, CarModel, CarColor, DateOut, DateIn, DayNo, CounterOnRecive }


        enum DriverSecound { SecDriver, OtherPaperNumber, OtherLicenseNo, OtherDateIssued, OtherDateExpired }


        enum Cost { DailyCost, TotalAmount, Payment, Discount, SubTotal }

        private async void ReportWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ReportViewr1.SetDisplayMode(DisplayMode.PrintLayout);

            ReportViewr1.LocalReport.ReportEmbeddedResource = "Car_Renter.ReportManage.ContractRent.rdlc";


            var DbClient = await new DataBase().GetClients();
            var DbCar = await new DataBase().GetCars();

            var Car = DbCar.Where(i => i.Id == VMContracts.CarID).FirstOrDefault();

            Microsoft.Reporting.WinForms.ReportParameter[] par = new ReportParameter[]
                           {

                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.ClientName),VMContracts.ClientName),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.Nationality),"فلسطينية"),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.PaperType),"هوية مدنية"),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.PaperNumber),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().NationalID),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.Birthday),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().Birthdate.ToShortDateString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.Address),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().Address),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.Phone),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().MobileNumber),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.LicenseNumber),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().LicenseNumber),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.DateIssued)," "),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Parameter.DateExpired),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().EndDateLicense.ToShortDateString()),






                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(CarData.CarNo),Car.CarNumber),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(CarData.CarModel),Car.CarName+" "+Car.CarModel),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(CarData.CarColor),Car.CarColor),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(CarData.DateOut),VMContracts.DateOut.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(CarData.DateIn),VMContracts.DateIn.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(CarData.DayNo),VMContracts.DayNumber.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(CarData.CounterOnRecive),"0"),



                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(DriverSecound.SecDriver),DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault().ClientName),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(DriverSecound.OtherPaperNumber),DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault().NationalID),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(DriverSecound.OtherLicenseNo),DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault().LicenseNumber),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(DriverSecound.OtherDateIssued)," "),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(DriverSecound.OtherDateExpired),DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault().EndDateLicense.ToShortDateString()),


                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Cost.DailyCost),VMContracts.DailyCost.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Cost.TotalAmount),VMContracts.TotalAmount.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Cost.Payment),VMContracts.TotalCash.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Cost.Discount),"0"),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(Cost.SubTotal),VMContracts.NetAmount.ToString()),




                            };
            ReportViewr1.LocalReport.SetParameters(par);

            this.ReportViewr1.RefreshReport();
        }
    }
}
