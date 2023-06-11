using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Car_Renter.Pages
{
    /// <summary>
    /// Interaction logic for CarIsOut.xaml
    /// </summary>
    public partial class CarIsOut : UserControl
    {
        public CarIsOut()
        {
            InitializeComponent();

            Load_Data();
        }


        ObservableCollection<VMContracts> vMContracts;
        private async void Load_Data()
        {
           
            var DbContract = await new DataBase().GetRentContract();
            var DbClient = await new DataBase().GetClients();
            var DbCar = await new DataBase().GetCars();


            var results = from Contracts in DbContract
                          join Clients in DbClient on Contracts.ClientID equals Clients.Id
                          //join ClientsSecound in DbClient on Contracts.SecoundClientID equals ClientsSecound.Id
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
                              TotalCash = Contracts.TotalCash.Value,
                              ClientName = Clients.ClientName,
                              //SecoundClientName = ClientsSecound.ClientName,
                              CarName = Cars.CarName,
                              CarModel = Cars.CarModel,
                              CarReturn = Contracts.CarReturn
                          };

            vMContracts = new ObservableCollection<VMContracts>(results.Where(i => i.CarReturn == false).ToList());


            DataGridList.ItemsSource = results.Where(i=>i.CarReturn==false).ToList();
          

            return;

        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Search = txtSearch.Text;

            if (Search.Length > 0)
            {
                DataGridList.ItemsSource = vMContracts.Where(i => (i.CarName + i.CarModel).ToString().ToLower().Contains(Search) || i.ClientName.ToString().ToLower().Contains(Search) || i.SecoundClientName.ToString().ToLower().Contains(Search)).ToList();
                return;
            }

            else
            {
                DataGridList.ItemsSource = vMContracts;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
