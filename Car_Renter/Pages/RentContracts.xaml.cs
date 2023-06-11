using Car_Renter.ReportManage;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Car_Renter.Pages
{
    public class VMContracts : ViewModel.BaseNotifyViewModel
    {
        public int Id { get; set; }
        public int PaymentID { get; set; }
        public Guid IDGuid { get; set; }

        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string Phone { get; set; }
        public string NationalID { get; set; }

        public int SecoundClientID { get; set; }
        public string SecoundClientName { get; set; }


        public int CarID { get; set; }
        public string CarName { get; set; }
        public string CarModel { get; set; }
        public string CarFullName { get { return CarName + " | " + CarModel; } }


        public int DayNumber { get; set; } = 0;

        public DateTime DateOut { get; set; } = DateTime.Now;
        public DateTime DateIn { get { return DateOut.AddDays(DayNumber); } }


        public double DailyCost { get; set; } = 0;
        public double TotalAmount { get { return DayNumber * DailyCost; } }
        public double TotalCash { get; set; }
        public double NetAmount { get { return TotalAmount - TotalCash; } }


        //يتم حفظ هنا هل تم ارجاع المركبة او لا
        public bool CarReturn { get; set; }



        //ForReportView

        public string CarNumber { get; set; }
        public string CarColor { get; set; }

        public string Details { get { return "عقد ايجار " + ClientName + " " + "للسيارة" + " " + CarName + " " + CarModel + " " + CarColor + " " + CarNumber; } }

        public string Date { get { return DateOut.ToShortDateString(); } }


        //متغير من اجل عرض كم تبقى دفعه على الحساب
        public double NetAmountUpdated { get; set; } 


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

        ObservableCollection<VMContracts> vMContracts = new ObservableCollection<VMContracts>();

       
        private async void Load_Data()
        {
            RentContractsTable = new Tables.RentContracts();

            DataContext = RentContractsTable;

            AddNewPop.IsOpen = false;

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

                              DayNumber = Contracts.DayNumber.Value,
                              DailyCost = Contracts.DailyCost.Value,
                              TotalCash = DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault() == null ? 0 : DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).Sum(i => i.Amount.Value),
                              ClientName = Clients.ClientName,
                              SecoundClientName = DbClient.Where(i => i.Id == Contracts.SecoundClientID).FirstOrDefault()!=null? DbClient.Where(i=>i.Id==Contracts.SecoundClientID).FirstOrDefault().ClientName:"",
                              CarName = Cars.CarName,
                              CarModel = Cars.CarModel,
                              CarReturn = Contracts.CarReturn,
                              CarNumber = Cars.CarNumber,
                              CarColor = Cars.CarColor,
                              PaymentID = DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault() == null ? 0 : DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault().Id


                          };





         


            DataGridList.ItemsSource = vMContracts = new ObservableCollection<VMContracts>(results.ToList());



            //DataGridList.ItemsSource = await new DataBase().GetRentContract();

            return;

        }

        #region Validation
        enum ControlsName { txtClient, txtSecoundClient, txtCar, txtDateOut, txtTimeOut, txtDayNumber, txtDailyCost, txtDateIn, txtTimeIn, txtCash }

        enum ControlsNameMiniClient { txtClientName, txtNationalID, txtAddress, txtMobile, txtBirthday, txtLicenseNumber, txtEndLicense }
        enum ControlsNamePayment { txtAmount, txtAmountDate, txtAmountNote }


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
                #region Contracts



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

                #endregion
                #region MiniClient

                case nameof(ControlsNameMiniClient.txtClientName):
                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "اسم المستاجر لا يمكن ان يكون فارغ";
                        uIElement.Focus();
                    }

                    break;
                case nameof(ControlsNameMiniClient.txtNationalID):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك رقم الهوية فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.Isdouble(textBox.Text) == false)
                    {
                        errorMsg = "ادخل رقم الهوية بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsNameMiniClient.txtAddress):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك العنوان فارغ";
                        uIElement.Focus();

                    }

                    break;

                case nameof(ControlsNameMiniClient.txtMobile):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك رقم الهاتف فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.Isdouble(textBox.Text) == false)
                    {
                        errorMsg = "ادخل رقم الهاتف بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsNameMiniClient.txtBirthday):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك تاريخ الميلاد فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text))
                    {
                        errorMsg = "ادخل تاريخ الميلاد بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;

                case nameof(ControlsNameMiniClient.txtLicenseNumber):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك رقم الرخصة فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.Isdouble(textBox.Text) == false)
                    {
                        errorMsg = "ادخل رقم الرخصة بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsNameMiniClient.txtEndLicense):


                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك تاريخ انتهاء الرخصة فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text))
                    {
                        errorMsg = "ادخل تاريخ انتهاء الرخصة بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                #endregion


                #region Payments

                case nameof(ControlsNamePayment.txtAmountDate):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك تاريخ الدفعه فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text))
                    {
                        errorMsg = "ادخل تاريخ الدفعه بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;

                case nameof(ControlsNamePayment.txtAmount):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك قيمة الدفعه فارغ";
                        uIElement.Focus();
                    }
                    if (textBox.Text=="0")
                    {
                        errorMsg = "لا يمكن ترك قيمة الدفعه فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.Isdouble(textBox.Text) == false)
                    {
                        errorMsg = "ادخل قيمة الدفعه بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;


                    #endregion

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



            #region MiniClient

            txtClientName.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtClientName.ToolTip = null;

            txtNationalID.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtNationalID.ToolTip = null;

            txtAddress.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtAddress.ToolTip = null;

            txtMobile.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtMobile.ToolTip = null;

            txtBirthday.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtBirthday.ToolTip = null;

            txtLicenseNumber.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtLicenseNumber.ToolTip = null;

            txtEndLicense.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtEndLicense.ToolTip = null;

            #endregion


            #region Payments

            txtAmount.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtAmount.ToolTip = null;

            txtAmountDate.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtAmountDate.ToolTip = null;

            txtAmountNote.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtAmountNote.ToolTip = null;
            #endregion
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

            SelectCar = new VMCars();

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


        int PaymentID = 0;
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

            RentContractsTable.DayNumber = VMContracts.DayNumber;
            RentContractsTable.DailyCost = VMContracts.DailyCost;
            RentContractsTable.TotalCash = VMContracts.TotalCash;
            RentContractsTable.CarReturn = VMContracts.CarReturn;


            PaymentID = VMContracts.PaymentID;


            SelectClient = new Tables.Clients { Id = VMContracts.ClientID, ClientName = VMContracts.ClientName };

            SelectSecoundClient = new Tables.Clients { Id = VMContracts.SecoundClientID, ClientName = VMContracts.SecoundClientName };



            SelectCar = new VMCars { CarFullName = VMContracts.CarName + " | " + VMContracts.CarModel, Id = VMContracts.CarID };

            txtClient.DataContext = SelectClient;

            txtSecoundClient.DataContext = SelectSecoundClient;

            txtCar.DataContext = SelectCar;


            DataContext = RentContractsTable;




            AddNewPop.IsOpen = true;

            SearchActive = true;

        }

        private void CloseAdd_Click(object sender, RoutedEventArgs e)
        {
            if (popCash.IsOpen) popCash.IsOpen = false;

            else if (AddNewPop.IsOpen)
                AddNewPop.IsOpen = popCash.IsOpen = false;

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
            //if (txtDateIn.Text.Length == 0 || StoriedParameter.IsDate(txtDateIn.Text) == false)
            //{
            //    txtDateIn.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
            //    txtDateIn.ToolTip = GetErrorMessage(txtDateIn);
            //    MessageUser = GetErrorMessage(txtDateIn);

            //    Counter += 1;
            //}

            if (txtDayNumber.Text.Length == 0 || StoriedParameter.Isdouble(txtDayNumber.Text) == false)
            {
                txtDayNumber.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtDayNumber.ToolTip = GetErrorMessage(txtDayNumber);
                MessageUser = GetErrorMessage(txtDayNumber);

                Counter += 1;
            }
            if ( StoriedParameter.IsDate(txtTimeOut.Text) == false)
            {
                txtTimeOut.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtTimeOut.ToolTip = GetErrorMessage(txtTimeOut);
                MessageUser = GetErrorMessage(txtTimeOut);

                Counter += 1;
            }
            if ( StoriedParameter.IsDate(txtDateOut.Text) == false)
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
            //if (txtSecoundClient.Text.Length == 0 || SelectSecoundClient.Id == 0)
            //{
            //    txtSecoundClient.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
            //    txtSecoundClient.ToolTip = GetErrorMessage(txtSecoundClient);
            //    MessageUser = GetErrorMessage(txtSecoundClient);

            //    Counter += 1;
            //}
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
                    await new DataBase().SavePayment(new Tables.Payments
                    {
                        ClientId = RentContractsTable.ClientID,
                        Amount = RentContractsTable.TotalCash,
                        ContractGuid = RentContractsTable.IDGuid,
                        AmountDate = DateTime.Now,
                        AmountNote = "دفعه مع العقد",
                        ContractId = RentContractsTable.Id,
                        PrimaryPayment = true
                    });

                    Load_Data();
                }
                else
                {
                    await new DataBase().UpdateContract(RentContractsTable);
                    await new DataBase().UpdatePayment(new Tables.Payments
                    {
                        ClientId = RentContractsTable.ClientID,
                        Amount = RentContractsTable.TotalCash,
                        ContractGuid = RentContractsTable.IDGuid,
                        AmountDate = DateTime.Now,
                        AmountNote = "دفعه مع العقد",
                        ContractId = RentContractsTable.Id,
                        PrimaryPayment = true,
                        Id = PaymentID
                    }); ;

                    Load_Data();
                }

               
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

        VMCars SelectCar = new VMCars();
        private void ListCar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchActive = false;




            SelectCar = (VMCars)ListCar.SelectedItem;

            SelectCar.CarFullName = SelectCar.CarName + " | " + SelectCar.CarModel;

            popSelectCar.IsOpen = false;

            //SelectCar.CarName = car.CarName + " | " + car.CarModel;

            txtCar.DataContext = SelectCar;

            txtMessage.Text = SelectCar.CarFullName;

            SearchActive = true;
        }

        private async void txtCar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchActive)
            {
                if (txtCar.Text.Length > 0)
                {
                    popSelectCar.IsOpen = true;


                    var DbContract = await new DataBase().GetRentContract();

                    var DbCar = await new DataBase().GetCars();


                    var results = from Cars in DbCar
                                  select new VMCars
                                  {
                                      Id = Cars.Id,
                                      CarColor = Cars.CarColor,
                                      CarModel = Cars.CarModel,
                                      CarName = Cars.CarName,
                                      CarNumber = Cars.CarNumber,
                                      CarYear = Cars.CarYear,
                                      EndInsuranceDate = Cars.EndInsuranceDate,
                                      EndLicenseDate = Cars.EndLicenseDate,
                                      IDGuid = Cars.IDGuid,
                                      StartInsuranceDate = Cars.StartInsuranceDate,
                                      StartLicenseDate = Cars.StartLicenseDate,
                                      CarReturn = DbContract.Where(i => i.CarID == Cars.Id).LastOrDefault() == null ? true : DbContract.Where(i => i.CarID == Cars.Id).LastOrDefault().CarReturn



                                  };


                    var sValue = txtCar.Text;



                    ListCar.ItemsSource = results.Where(p => p.CarName.ToLower().Contains(sValue) || p.CarModel.ToLower().Contains(sValue) || p.CarNumber.ToLower().Contains(sValue))
                                   .OrderByDescending(i => i.Id).ToList().Where(i => i.CarReturn).ToList();
                }

                else popSelectCar.IsOpen = false;
            }
        }





        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }



       async void PrintReport(VMContracts VMContracts)
        {
            var Report = new ReportManage.ReportWindow();

            Report.ReportViewr1.LocalReport.ReportEmbeddedResource = "Car_Renter.ReportManage.ContractRent.rdlc";


            var DbClient = await new DataBase().GetClients();
            var DbCar = await new DataBase().GetCars();

            var Car = DbCar.Where(i => i.Id == VMContracts.CarID).FirstOrDefault();

            string LogoPath = "file:///" + AppDomain.CurrentDomain.BaseDirectory + @"Logo.jpg";

          

            Microsoft.Reporting.WinForms.ReportParameter[] par = new ReportParameter[]
                           {

                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.ClientName),VMContracts.ClientName),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.Nationality),"فلسطينية"),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.PaperType),"هوية مدنية"),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.PaperNumber),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().NationalID),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.Birthday),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().Birthdate.ToShortDateString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.Address),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().Address),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.Phone),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().MobileNumber),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.LicenseNumber),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().LicenseNumber),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.DateIssued)," "),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.DateExpired),DbClient.Where(i=>i.Id==VMContracts.ClientID).FirstOrDefault().EndDateLicense.ToShortDateString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Parameter.ContractNumber),VMContracts.Id.ToString()),






                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.CarData.CarNo),Car.CarNumber),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.CarData.CarModel),Car.CarName+" "+Car.CarModel),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.CarData.CarColor),Car.CarColor),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.CarData.DateOut),VMContracts.DateOut.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.CarData.DateIn),VMContracts.DateIn.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.CarData.DayNo),VMContracts.DayNumber.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.CarData.CounterOnRecive),"0"),



                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.DriverSecound.SecDriver),DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault()!=null? DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault().ClientName:" "),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.DriverSecound.OtherPaperNumber),DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault()!=null?DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault().NationalID:" "),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.DriverSecound.OtherLicenseNo),DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault()!=null?DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault().LicenseNumber:" "),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.DriverSecound.OtherDateIssued)," "),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.DriverSecound.OtherDateExpired),DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault()!=null?DbClient.Where(i=>i.Id==VMContracts.SecoundClientID).FirstOrDefault().EndDateLicense.ToShortDateString():" "),


                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Cost.DailyCost),VMContracts.DailyCost.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Cost.TotalAmount),VMContracts.TotalAmount.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Cost.Payment),VMContracts.TotalCash.ToString()),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Cost.Discount),"0"),
                                new Microsoft.Reporting.WinForms.ReportParameter(nameof(ReportWindow.Cost.SubTotal),VMContracts.NetAmount.ToString()),


                                new Microsoft.Reporting.WinForms.ReportParameter("ImageHeader",LogoPath),





                            };

            Report.ReportViewr1.LocalReport.EnableExternalImages = true;
            Report.ReportViewr1.LocalReport.SetParameters(par);

            Report.ReportViewr1.RefreshReport();


            Report.Show();
        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            var VMContracts = btn.CommandParameter as VMContracts;


            PrintReport(VMContracts);




        }

        private void txtCar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }



        private async void btnSearchCarSearch_Click(object sender, RoutedEventArgs e)
        {
            popSelectCar.IsOpen = true;
            var DbContract = await new DataBase().GetRentContract();

            var DbCar = await new DataBase().GetCars();


            var results = from Cars in DbCar
                          select new VMCars
                          {
                              Id = Cars.Id,
                              CarColor = Cars.CarColor,
                              CarModel = Cars.CarModel,
                              CarName = Cars.CarName,
                              CarNumber = Cars.CarNumber,
                              CarYear = Cars.CarYear,
                              EndInsuranceDate = Cars.EndInsuranceDate,
                              EndLicenseDate = Cars.EndLicenseDate,
                              IDGuid = Cars.IDGuid,
                              StartInsuranceDate = Cars.StartInsuranceDate,
                              StartLicenseDate = Cars.StartLicenseDate,
                              CarReturn = DbContract.Where(i => i.CarID == Cars.Id).LastOrDefault() == null ? true : DbContract.Where(i => i.CarID == Cars.Id).LastOrDefault().CarReturn



                          };

            var sValue = txtCar.Text;



            ListCar.ItemsSource = results.Where(i => i.CarReturn).ToList();
        }

        private void txtSecoundClient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("");
        }

        private async void btnSearchSecoundClient_Click(object sender, RoutedEventArgs e)
        {

            int index = int.Parse(((Button)e.Source).Uid);

            switch (index)
            {
                case 0:
                    popSelectClient.IsOpen = true;

                    ListClientMain.ItemsSource = await new DataBase().GetClients();
                    break;

                case 1:
                    popSelectClientSecound.IsOpen = true;

                    ListClientSecoundMain.ItemsSource = await new DataBase().GetClients();
                    break;

            }

        }

        private async void btnReturnCar_Click_1(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            var VMContracts = btn.CommandParameter as VMContracts;


            if (MessageBox.Show("هل انت متاكد من ارجاع السيارة" + VMContracts.CarNumber + " | " + VMContracts.CarFullName,
"Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                RentContractsTable.Id = VMContracts.Id;
                RentContractsTable.IDGuid = VMContracts.IDGuid;
                RentContractsTable.ClientID = VMContracts.ClientID;
                RentContractsTable.SecoundClientID = VMContracts.SecoundClientID;
                RentContractsTable.CarID = VMContracts.CarID;
                RentContractsTable.DateOut = VMContracts.DateOut;

                RentContractsTable.DayNumber = VMContracts.DayNumber;
                RentContractsTable.DailyCost = VMContracts.DailyCost;
                RentContractsTable.TotalCash = VMContracts.TotalCash;
                RentContractsTable.CarReturn = VMContracts.CarReturn;

                RentContractsTable.CarReturn = true;

                await new DataBase().UpdateContract(RentContractsTable);

                Load_Data();
            }
            else
            {
                // Do not close the window  
            }



        }


        Tables.Clients Clients;
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddNewClientPop.IsOpen = true;

            Clients = new Tables.Clients();

            txtClientName.Focus();

            AddNewClientPop.DataContext = Clients;
        }

        private void CloseMiniClientAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewClientPop.IsOpen = false;
        }



        private async void btnAddMiniClient_Click(object sender, RoutedEventArgs e)
        {

            ClearData();
            string MessageUser = "";
            if (txtEndLicense.Text.Length == 0 || StoriedParameter.IsDate(txtEndLicense.Text) == false)
            {
                txtEndLicense.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtEndLicense.ToolTip = GetErrorMessage(txtEndLicense);
                Counter += 1;
                MessageUser = GetErrorMessage(txtEndLicense);
            }
            if (txtLicenseNumber.Text.Length == 0 || StoriedParameter.Isdouble(txtLicenseNumber.Text) == false)
            {
                txtLicenseNumber.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtLicenseNumber.ToolTip = GetErrorMessage(txtLicenseNumber);
                MessageUser = GetErrorMessage(txtLicenseNumber);

                Counter += 1;
            }
            if (txtBirthday.Text.Length == 0 || StoriedParameter.IsDate(txtBirthday.Text) == false)
            {
                txtBirthday.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtBirthday.ToolTip = GetErrorMessage(txtBirthday);
                MessageUser = GetErrorMessage(txtBirthday);

                Counter += 1;
            }
            if (txtMobile.Text.Length == 0)
            {
                txtMobile.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtMobile.ToolTip = GetErrorMessage(txtMobile);
                MessageUser = GetErrorMessage(txtMobile);

                Counter += 1;
            }
            if (txtAddress.Text.Length == 0)
            {
                txtAddress.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtAddress.ToolTip = GetErrorMessage(txtAddress);
                MessageUser = GetErrorMessage(txtAddress);

                Counter += 1;
            }
            if (txtNationalID.Text.Length == 0 || StoriedParameter.Isdouble(txtNationalID.Text) == false)
            {
                txtNationalID.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtNationalID.ToolTip = GetErrorMessage(txtNationalID);
                MessageUser = GetErrorMessage(txtNationalID);

                Counter += 1;
            }
            if (txtClientName.Text.Length == 0)
            {
                txtClientName.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtClientName.ToolTip = GetErrorMessage(txtClientName);
                MessageUser = GetErrorMessage(txtClientName);

                Counter += 1;


            }

            txtMessageMiniClient.Text = MessageUser;


            if (Counter == 0)
            {
                if (Clients.Id == 0)
                {
                    await new DataBase().SaveClient(Clients);
                    AddNewClientPop.IsOpen = false;

                    SearchActive = false;
                    SelectClient = Clients;

                    SelectSecoundClient = Clients;
                    txtSecoundClient.DataContext = SelectSecoundClient;

                    txtClient.DataContext = SelectClient;

                    SearchActive = true;





                }
            }
        }



       




        Tables.Payments paymentsClass = new Tables.Payments();
        VMContracts VMContracts;
        private async void btnAddPayment_Click(object sender, RoutedEventArgs e)
        {
            popCash.IsOpen = true;

            var btn = sender as System.Windows.Controls.Button;
            VMContracts = btn.CommandParameter as VMContracts;


            VMContracts.NetAmountUpdated = VMContracts.TotalAmount - VMContracts.TotalCash;

            popCash.DataContext = VMContracts;

            paymentsClass.ClientId = VMContracts.ClientID;
            paymentsClass.ContractId = VMContracts.Id;
            paymentsClass.ContractGuid = VMContracts.IDGuid;


            var Data = await new DataBase().GetPayments();

            PaymentList.ItemsSource = Data.Where(i => i.ContractGuid == VMContracts.IDGuid).ToList();


            GridAdd.DataContext = paymentsClass;

        }

        private async void AddAMount_Click(object sender, RoutedEventArgs e)
        {

            ClearData();
            string MessageUser = "";

            Counter = 0;

            if (txtAmountDate.Text.Length == 0 || StoriedParameter.IsDate(txtAmountDate.Text) == false)
            {
                txtAmountDate.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtAmountDate.ToolTip = GetErrorMessage(txtAmountDate);
                MessageUser = GetErrorMessage(txtAmountDate);

                Counter += 1;
            }
            if (txtAmount.Text.Length == 0 || txtAmount.Text =="0"|| StoriedParameter.Isdouble(txtAmount.Text) == false)
            {
                txtAmount.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtAmount.ToolTip = GetErrorMessage(txtAmount);
                MessageUser = GetErrorMessage(txtAmount);

                Counter += 1;
            }


            if (Counter > 0) return;



            if (paymentsClass.Id == 0)
            {
                await new DataBase().SavePayment(paymentsClass);

              


                var Data = await new DataBase().GetPayments();

                PaymentList.ItemsSource = Data.Where(i => i.ContractGuid == VMContracts.IDGuid).ToList();

               



                paymentsClass = new Tables.Payments();
                paymentsClass.ClientId = VMContracts.ClientID;
                paymentsClass.ContractId = VMContracts.Id;
                paymentsClass.ContractGuid = VMContracts.IDGuid;


                VMContracts.TotalCash = Data.Where(i => i.ContractGuid == VMContracts.IDGuid).Sum(I=>I.Amount).Value;
                VMContracts.NetAmountUpdated = VMContracts.TotalAmount - VMContracts.TotalCash;


                GridAdd.DataContext = paymentsClass;
            }
            else
            {
                await new DataBase().UpdatePayment(paymentsClass);

                var Data = await new DataBase().GetPayments();

                PaymentList.ItemsSource = Data.Where(i => i.ContractGuid == VMContracts.IDGuid).ToList();

                paymentsClass = new Tables.Payments();

                paymentsClass.ClientId = VMContracts.ClientID;
                paymentsClass.ContractId = VMContracts.Id;
                paymentsClass.ContractGuid = VMContracts.IDGuid;

                GridAdd.DataContext = paymentsClass;
            }


        }

        private void PaymentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PaymentList.SelectedItem != null)
            {

                paymentsClass = (Tables.Payments)PaymentList.SelectedItem;

                if (paymentsClass.PrimaryPayment == false)
                {
                    GridAdd.DataContext = paymentsClass;
                }

            }

        }

        private async void btnSaveAndPrint_Click(object sender, RoutedEventArgs e)
        {
            btnAdd.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));


            if (Counter > 0)
                return;

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

                              DayNumber = Contracts.DayNumber.Value,
                              DailyCost = Contracts.DailyCost.Value,
                              TotalCash = DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault() == null ? 0 : DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).Sum(i => i.Amount.Value),
                              ClientName = Clients.ClientName,
                              SecoundClientName = DbClient.Where(i => i.Id == Contracts.ClientID).FirstOrDefault().ClientName,
                              CarName = Cars.CarName,
                              CarModel = Cars.CarModel,
                              CarReturn = Contracts.CarReturn,
                              CarNumber = Cars.CarNumber,
                              CarColor = Cars.CarColor,
                              PaymentID = DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault() == null ? 0 : DbPayment.Where(i => i.ContractGuid == Contracts.IDGuid).FirstOrDefault().Id


                          };


            PrintReport(results.LastOrDefault());
        }
    }
}
