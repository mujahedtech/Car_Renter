using Car_Renter.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Car_Renter.ReportManage
{

    public class VMTotalReport : Car_Renter.ViewModel.BaseNotifyViewModel
    {

        public DateTime DateFrom { get; set; } = DateTime.Now;
        public DateTime DateTo { get; set; } = DateTime.Now;

        public bool DateSearch { get; set; }


    }


    /// <summary>
    /// Interaction logic for TotalReportView.xaml
    /// </summary>
    public partial class TotalReportView : UserControl
    {
        VMTotalReport vMTotalReport;
        public TotalReportView()
        {
            InitializeComponent();

            vMTotalReport = new VMTotalReport();

            DataContext = vMTotalReport;
        }

        private async void BtnViewReport_Click(object sender, RoutedEventArgs e)
        {



            var Report = new ReportManage.ReportWindow();



            var DbContract = await new DataBase().GetRentContract();
            var DbClient = await new DataBase().GetClients();
            var DbCar = await new DataBase().GetCars();
            var DbPayment = await new DataBase().GetPayments();


            var results = from Contracts in DbContract
                          join Clients in DbClient on Contracts.ClientID equals Clients.Id
                          join ClientsSecound in DbClient on Contracts.SecoundClientID equals ClientsSecound.Id
                          join Cars in DbCar on Contracts.CarID equals Cars.Id

                          select new VMContracts
                          {
                              Id = Contracts.Id,
                              IDGuid = Contracts.IDGuid,
                              ClientID = Contracts.ClientID,
                              SecoundClientID = Contracts.SecoundClientID,
                              CarID = Contracts.CarID,
                              DateOut = Contracts.DateOut.Value,

                              DayNumber = Contracts.DayNumber.Value,
                              DailyCost = Contracts.DailyCost.Value,
                              TotalCash = DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault() == null ? 0 : DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).Sum(i => i.Amount.Value),
                              ClientName = Clients.ClientName,
                              SecoundClientName = ClientsSecound.ClientName,
                              CarName = Cars.CarName,
                              CarModel = Cars.CarModel,
                              CarReturn = Contracts.CarReturn,
                              CarNumber = Cars.CarNumber,
                              CarColor = Cars.CarColor,
                              PaymentID = DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault() == null ? 0 : DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault().Id


                          };


            if (vMTotalReport.DateSearch)
            {
                results = results.Where(i => i.DateOut >= vMTotalReport.DateFrom && i.DateOut <= vMTotalReport.DateTo).ToList();
            }

            var dataTable = LinqHelper.ToDataTable<Pages.VMContracts>(results.ToList());


            Report.ReportViewr1.LocalReport.ReportEmbeddedResource = "Car_Renter.ReportManage.TotalBalanceReport.rdlc";

            ReportDataSource datasource = new ReportDataSource("DataSet1", dataTable);

            Report.ReportViewr1.LocalReport.DataSources.Clear();
            Report.ReportViewr1.LocalReport.DataSources.Add(datasource);

            string ReportHeader = "كشف حساب كلي من اول المدة الى نهاية المدة";

            if (vMTotalReport.DateSearch) ReportHeader = "كشف حساب كلي من تاريخ " + vMTotalReport.DateFrom.ToShortDateString() + " الى تاريخ " + vMTotalReport.DateTo.ToShortDateString();


            string LogoPath = "file:///" + AppDomain.CurrentDomain.BaseDirectory + @"Logo.jpg";


            Microsoft.Reporting.WinForms.ReportParameter[] par = new ReportParameter[]
                      {

                                new Microsoft.Reporting.WinForms.ReportParameter("ReportHeader",ReportHeader),
                                 new Microsoft.Reporting.WinForms.ReportParameter("ImagePath", LogoPath),

                       };





            Report.ReportViewr1.LocalReport.EnableExternalImages = true;

            Report.ReportViewr1.LocalReport.SetParameters(par);


            Report.ReportViewr1.RefreshReport();






            Report.Show();
        }
    }
}
