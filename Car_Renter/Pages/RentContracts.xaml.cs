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
    public class VMContracts
    {
        public int Id { get; set; }
        public Guid IDGuid { get; set; }

        public int ClientID { get; set; }
        public string ClientName { get; set; }

        public int SecoundClientID { get; set; }
        public string SecoundClientName { get; set; }


        public int CarID { get; set; }
        public string CarName { get; set; }
        public string CarModel { get; set; }

        public DateTime DateOut { get; set; } = DateTime.Now;
        public DateTime DateIn { get; set; } = DateTime.Now;

        public int DayNumber { get; set; } = 0;
        public double DailyCost { get; set; } = 0;
        public double TotalAmount { get { return DayNumber * DailyCost; } }
        public double TotalCash { get; set; }
        public double NetAmount { get { return TotalAmount - TotalCash; } }


        //يتم حفظ هنا هل تم ارجاع المركبة او لا
        public bool CarReturn { get; set; }
    }


    public class Payments
    {

        public int Id { get; set; } = 0;
        public int ContractId { get; set; } 
        public Guid ContractGuid { get; set; }

        public double Amount { get; set; }
        public DateTime AmountDate { get; set; }
        public string AmountNote { get; set; }

    }

    /// <summary>
    /// Interaction logic for RentContracts.xaml
    /// </summary>
    public partial class RentContracts : UserControl
    {

        Tables.RentContracts RentContractsTable = new Tables.RentContracts();
        public RentContracts()
        {
            InitializeComponent();

            DataContext = RentContractsTable;

         

            Loaded += RentContracts_Loaded;
        }

        private void RentContracts_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Data();
        }

        ObservableCollection<VMContracts> vMContracts;
        private async void Load_Data()
        {
            RentContractsTable = new Tables.RentContracts();

            DataContext = RentContractsTable;

            AddNewPop.IsOpen = false;

            var DbContract = await new DataBase().GetRentContract();
            var DbClient = await new DataBase().GetClients();
            var DbCar = await new DataBase().GetCars();


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
                              DateOut = Contracts.DateOut,
                              DateIn = Contracts.DateIn,
                              DayNumber = Contracts.DayNumber,
                              DailyCost = Contracts.DailyCost,
                              TotalCash = Contracts.TotalCash,
                              ClientName = Clients.ClientName,
                              SecoundClientName = ClientsSecound.ClientName,
                              CarName = Cars.CarName,
                              CarModel = Cars.CarModel,
                              CarReturn= Contracts.CarReturn


                          };



            vMContracts =new ObservableCollection<VMContracts>(results.ToList()) ;




            DataGridList.ItemsSource = vMContracts;
            //DataGridList.ItemsSource = await new DataBase().GetRentContract();

            return;

        }

        #region Validation
        enum ControlsName { txtClient, txtSecoundClient, txtCar, txtDateOut, txtTimeOut, txtDayNumber, txtDailyCost, txtDateIn, txtTimeIn, txtCash }


        public string GetErrorMessage(FrameworkElement uIElement)
        {
            TextBox textBox = new TextBox();
            if (uIElement is TextBox == true)
            {
                textBox = uIElement as TextBox;
            }

            string errorMsg = String.Empty;



            switch (uIElement.Name)
            {
                case nameof(ControlsName.txtClient):
                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "اسم المستاجر لا يمكن ان يكون فارغ";
                        uIElement.Focus();
                    }

                    break;
                case nameof(ControlsName.txtSecoundClient):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "اسم السائق البديل لا يمكن ان يكون فارغ";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtCar):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك السيارة فارغ";
                        uIElement.Focus();

                    }

                    break;

                case nameof(ControlsName.txtDateOut):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك تاريخ الخروج فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text))
                    {
                        errorMsg = "ادخل تاريخ الخروج بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtTimeOut):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك وقت الخروج فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text))
                    {
                        errorMsg = "ادخل وقت الخروج بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;

                case nameof(ControlsName.txtDayNumber):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك عدد الايام فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.Isdouble(textBox.Text) == false)
                    {
                        errorMsg = "ادخل عدد الايام بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtDailyCost):


                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك الكلفة اليومية فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.Isdouble(textBox.Text) == false)
                    {
                        errorMsg = "ادخل الكلفة اليومية بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtDateIn):


                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك تاريخ الارجاع فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text) == false)
                    {
                        errorMsg = "ادخل تاريخ الارجاع بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtTimeIn):


                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك وقت الارجاع فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text) == false)
                    {
                        errorMsg = "ادخل وقت الارجاع بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtCash):


                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك المبلغ المدفوع فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.Isdouble(textBox.Text) == false)
                    {
                        errorMsg = "ادخل المبلغ المدفوع بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
            }

            return errorMsg;



        }

        int Counter = 0;
        void ClearData()
        {
            Counter = 0;


            txtClient.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtClient.ToolTip = null;

            txtSecoundClient.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtSecoundClient.ToolTip = null;

            txtCar.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtCar.ToolTip = null;

            txtDayNumber.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtDayNumber.ToolTip = null;

            txtDailyCost.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtDailyCost.ToolTip = null;

            txtCash.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtCash.ToolTip = null;
        }

        #endregion


        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            AddNewPop.IsOpen = true;

            btnCash.Visibility = Visibility.Hidden;

            RentContractsTable = new Tables.RentContracts();

            DataContext = RentContractsTable;


            SelectClient = new Tables.Clients();

            SelectSecoundClient = new Tables.Clients();

            SelectCar = new Tables.Cars();

            txtClient.DataContext = SelectClient;

            txtSecoundClient.DataContext = SelectSecoundClient;

            txtCar.DataContext = SelectCar;

            ClearData();
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

        private void DataGridList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridList.SelectedIndex == -1) return;
            //btnCash.Visibility = Visibility.Visible;

            ClearData();

            SearchActive = false;
            var VMContracts = (VMContracts)DataGridList.SelectedItem;

            RentContractsTable.Id = VMContracts.Id;
            RentContractsTable.IDGuid = VMContracts.IDGuid;
            RentContractsTable.ClientID = VMContracts.ClientID;
            RentContractsTable.SecoundClientID = VMContracts.SecoundClientID;
            RentContractsTable.CarID = VMContracts.CarID;
            RentContractsTable.DateOut = VMContracts.DateOut;
            RentContractsTable.DateIn = VMContracts.DateIn;
            RentContractsTable.DayNumber = VMContracts.DayNumber;
            RentContractsTable.DailyCost = VMContracts.DailyCost;
            RentContractsTable.TotalCash = VMContracts.TotalCash;
            RentContractsTable.CarReturn = VMContracts.CarReturn;


            SelectClient = new Tables.Clients { Id = VMContracts.ClientID, ClientName = VMContracts.ClientName };

            SelectSecoundClient = new Tables.Clients { Id = VMContracts.SecoundClientID, ClientName = VMContracts.SecoundClientName };



            SelectCar = new Tables.Cars { CarName = VMContracts.CarName + " | " + VMContracts.CarModel, Id = 1 };

            txtClient.DataContext = SelectClient;

            txtSecoundClient.DataContext = SelectSecoundClient;

            txtCar.DataContext = SelectCar;


            DataContext = RentContractsTable;




            AddNewPop.IsOpen = true;

            SearchActive = true;

        }

        private void CloseAdd_Click(object sender, RoutedEventArgs e)
        {
            if(popCash.IsOpen) popCash.IsOpen = false;

           else if(AddNewPop.IsOpen)
            AddNewPop.IsOpen = popCash.IsOpen= false;
            
        }

        private void GridForm_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var uie = e.OriginalSource as UIElement;

                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            string MessageUser = "";

            if (txtCash.Text.Length == 0 || StoriedParameter.Isdouble(txtCash.Text) == false)
            {
                txtCash.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtCash.ToolTip = GetErrorMessage(txtCash);
                MessageUser = GetErrorMessage(txtCash);

                Counter += 1;
            }
            if (txtDailyCost.Text.Length == 0 || StoriedParameter.Isdouble(txtDailyCost.Text) == false)
            {
                txtDailyCost.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtDailyCost.ToolTip = GetErrorMessage(txtDailyCost);
                MessageUser = GetErrorMessage(txtDailyCost);

                Counter += 1;
            }
            if (txtTimeIn.Text.Length == 0 || StoriedParameter.IsDate(txtTimeIn.Text) == false)
            {
                txtTimeIn.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtTimeIn.ToolTip = GetErrorMessage(txtTimeIn);
                MessageUser = GetErrorMessage(txtTimeIn);

                Counter += 1;
            }
            if (txtDateIn.Text.Length == 0 || StoriedParameter.IsDate(txtDateIn.Text) == false)
            {
                txtDateIn.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtDateIn.ToolTip = GetErrorMessage(txtDateIn);
                MessageUser = GetErrorMessage(txtDateIn);

                Counter += 1;
            }

            if (txtDayNumber.Text.Length == 0 || StoriedParameter.Isdouble(txtDayNumber.Text) == false)
            {
                txtDayNumber.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtDayNumber.ToolTip = GetErrorMessage(txtDayNumber);
                MessageUser = GetErrorMessage(txtDayNumber);

                Counter += 1;
            }
            if (txtTimeOut.Text.Length == 0 || StoriedParameter.IsDate(txtTimeOut.Text) == false)
            {
                txtTimeOut.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtTimeOut.ToolTip = GetErrorMessage(txtTimeOut);
                MessageUser = GetErrorMessage(txtTimeOut);

                Counter += 1;
            }
            if (txtDateOut.Text.Length == 0 || StoriedParameter.IsDate(txtDateOut.Text) == false)
            {
                txtDateOut.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtDateOut.ToolTip = GetErrorMessage(txtDateOut);
                MessageUser = GetErrorMessage(txtDateOut);

                Counter += 1;
            }
            if (txtCar.Text.Length == 0 || SelectCar.Id == 0)
            {
                txtCar.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtCar.ToolTip = GetErrorMessage(txtCar);
                MessageUser = GetErrorMessage(txtCar);

                Counter += 1;
            }
            if (txtSecoundClient.Text.Length == 0 || SelectSecoundClient.Id == 0)
            {
                txtSecoundClient.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtSecoundClient.ToolTip = GetErrorMessage(txtSecoundClient);
                MessageUser = GetErrorMessage(txtSecoundClient);

                Counter += 1;
            }
            if (txtClient.Text.Length == 0 || SelectClient.Id == 0)
            {
                txtClient.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtClient.ToolTip = GetErrorMessage(txtClient);
                MessageUser = GetErrorMessage(txtClient);

                Counter += 1;


            }




            txtMessage.Text = MessageUser;

            RentContractsTable.ClientID = SelectClient.Id;
            RentContractsTable.SecoundClientID = SelectSecoundClient.Id;
            RentContractsTable.CarID = SelectCar.Id;


            


           

            if (Counter == 0)
            {
                if (RentContractsTable.Id == 0)
                {
                    await new DataBase().SaveContract(RentContractsTable);
                }
                else
                {
                    await new DataBase().UpdateContract(RentContractsTable);

                }

                Load_Data();
            }
        }

        private void btnReturnCar_Click(object sender, RoutedEventArgs e)
        {
            if (RentContractsTable.CarReturn) RentContractsTable.CarReturn = false;
            else RentContractsTable.CarReturn = true;


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


        Tables.Clients SelectSecoundClient = new Tables.Clients();
        private void ListClientSecoundMain_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchActive = false;
            SelectSecoundClient = (Tables.Clients)ListClientSecoundMain.SelectedItem;

            popSelectClientSecound.IsOpen = false;

            txtSecoundClient.DataContext = SelectSecoundClient;

            SearchActive = true;
        }

        private async void txtSecoundClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchActive)
            {
                if (txtSecoundClient.Text.Length > 0)
                {
                    popSelectClientSecound.IsOpen = true;

                    ListClientSecoundMain.ItemsSource = await new DataBase().SearchClients(txtSecoundClient.Text);
                }

                else popSelectClientSecound.IsOpen = false;
            }
        }
        Tables.Cars SelectCar = new Tables.Cars();
        private void ListCar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchActive = false;
            SelectCar = (Tables.Cars)ListCar.SelectedItem;

            popSelectCar.IsOpen = false;

            SelectCar.CarName = ((Tables.Cars)ListCar.SelectedItem).CarName + " | " + ((Tables.Cars)ListCar.SelectedItem).CarModel;

            txtCar.DataContext = SelectCar;

            SearchActive = true;
        }

        private async void txtCar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchActive)
            {
                if (txtCar.Text.Length > 0)
                {
                    popSelectCar.IsOpen = true;

                    ListCar.ItemsSource = await new DataBase().SearchCars(txtCar.Text);
                }

                else popSelectCar.IsOpen = false;
            }
        }



        ObservableCollection<Payments> Payments = new ObservableCollection<Payments>();
        private void AddAMount_Click(object sender, RoutedEventArgs e)
        {
            if (RentContractsTable.Id != 0)
            {
                Payments.Add(new Pages.Payments { Id =0, ContractGuid = RentContractsTable.IDGuid, ContractId = RentContractsTable.Id, Amount = double.Parse(txtAmount.Text), AmountDate = DateTime.Parse(txtAmountDate.Text) });

                RentContractsTable.TotalCash = Payments.Sum(i => i.Amount);

                PaymentList.ItemsSource = Payments;

                txtAmount.Text = txtAmountNote.Text = "";

                txtAmount.Focus();

               
            }

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            var VMContracts = btn.CommandParameter as VMContracts;



            new ReportManage.ReportWindow(VMContracts).ShowDialog();
        }
    }
}
