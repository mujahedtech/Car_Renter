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
    /// <summary>
    /// Interaction logic for ClientReports.xaml
    /// </summary>
    public partial class ClientReports : UserControl
    {
       ReportManage.VMTotalReport vMTotalReport;
        public ClientReports()
        {
            InitializeComponent();

            vMTotalReport = new VMTotalReport();

            DataContext = vMTotalReport;
        }

        private void BtnViewReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void txtClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchActive)
            {
                if (txtClient.Text.Length > 0)
                {
                    popSelectClient.IsOpen = true;

                    ListClientMain.ItemsSource = await new DataBase().SearchClients(txtClient.Text);
                }

                else popSelectClient.IsOpen = false;
            }



        }


        Tables.Clients SelectClient = new Tables.Clients();
        bool SearchActive { get; set; } = true;
        private void ListClientMain_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchActive = false;
            SelectClient = (Tables.Clients)ListClientMain.SelectedItem;

            popSelectClient.IsOpen = false;

            txtClient.DataContext = SelectClient;

            SearchActive = true;

        }

        private async void btnTransaction_Click(object sender, RoutedEventArgs e)
        {
            var Report = new ReportManage.ReportWindow();



            var DbContract = await new DataBase().GetRentContract();
            var DbClient = await new DataBase().GetClients();
            var DbCar = await new DataBase().GetCars();


            var DbPayment = await new DataBase().GetPayments();


            var results = from Contracts in DbContract
                          join Clients in DbClient on Contracts.ClientID equals Clients.Id
                        
                          join Cars in DbCar on Contracts.CarID equals Cars.Id

                          select new VMContracts
                          {
                              Id = Contracts.Id,
                              IDGuid = Contracts.IDGuid,
                              ClientID = Contracts.ClientID,
                              SecoundClientID = Contracts.SecoundClientID,
                              CarID = Contracts.CarID,
                              DateOut = Contracts.DateOut.Value,
                              Phone =Clients.MobileNumber,
                              NationalID=Clients.NationalID,

                              DayNumber = Contracts.DayNumber.Value,
                              DailyCost = Contracts.DailyCost.Value,
                              TotalCash = DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault() == null ? 0 : DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).Sum(i => i.Amount.Value),
                              ClientName = Clients.ClientName,
                              SecoundClientName = DbClient.Where(i => i.Id == Contracts.SecoundClientID).FirstOrDefault() != null ? DbClient.Where(i => i.Id == Contracts.SecoundClientID).FirstOrDefault().ClientName : "",
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

            if (SelectClient.ClientName == "") SelectClient.Id = 0;

            if (SelectClient.Id!=0)
            {
                results = results.Where(i => i.ClientID== SelectClient.Id).ToList();
            }

            var dataTable = LinqHelper.ToDataTable<Pages.VMContracts>(results.ToList());


            Report.ReportViewr1.LocalReport.ReportEmbeddedResource = "Car_Renter.ReportManage.TransactionsReport.rdlc";

            ReportDataSource datasource = new ReportDataSource("DataSet1", dataTable);

            Report.ReportViewr1.LocalReport.DataSources.Clear();
            Report.ReportViewr1.LocalReport.DataSources.Add(datasource);




            string LogoPath = "file:///" + AppDomain.CurrentDomain.BaseDirectory + @"Logo.jpg";


            Microsoft.Reporting.WinForms.ReportParameter[] par = new ReportParameter[]
                      {
                              
                                 new Microsoft.Reporting.WinForms.ReportParameter("ImagePath", LogoPath),

                       };





            Report.ReportViewr1.LocalReport.EnableExternalImages = true;

            Report.ReportViewr1.LocalReport.SetParameters(par);

            Report.ReportViewr1.RefreshReport();






            Report.Show();
        }

        private void btnBalanceSheet_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnViewAll_Click(object sender, RoutedEventArgs e)
        {
            popSelectClient.IsOpen = true;

            ListClientMain.ItemsSource = await new DataBase().GetClients();
        }
    }
}
