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

namespace Car_Renter.Pages
{
    public class VMCars:ViewModel.BaseNotifyViewModel
    {

      
        public int Id { get; set; }
        public Guid IDGuid { get; set; } 
        public string CarName { get; set; }
        public string CarModel { get; set; }
        public string CarYear { get; set; }
        public string CarNumber { get; set; }
        public string CarColor { get; set; }

        public DateTime StartLicenseDate { get; set; } = DateTime.Now;

       
        public DateTime EndLicenseDate { get; set; } = DateTime.Now;



        public DateTime StartInsuranceDate { get; set; } = DateTime.Now;

        //تاريخ انتهاء التامين
        public DateTime EndInsuranceDate { get; set; } = DateTime.Now;

        public bool CarReturn { get; set; } = true;

        public string CarFullName { get; set; }


    }



    /// <summary>
    /// Interaction logic for CarsManage.xaml
    /// </summary>
    public partial class CarsManage : UserControl
    {

        VMCars Cars = new VMCars();
        public CarsManage()
        {
            InitializeComponent();

            DataContext = Cars;

            Farms_LoadedAsync();
        }

        private async void Farms_LoadedAsync()
        {
            AddNewPop.IsOpen = false;


            var DbContract = await new DataBase().GetRentContract();
          
            var DbCar = await new DataBase().GetCars();


           

            var results = from Cars in DbCar select new VMCars
                          {
                             Id=Cars.Id,
                              CarColor= Cars.CarColor,
                              CarModel= Cars.CarModel,
                              CarName= Cars.CarName,
                              CarNumber= Cars.CarNumber,
                              CarYear= Cars.CarYear,
                              EndInsuranceDate= Cars.EndInsuranceDate,
                              EndLicenseDate= Cars.EndLicenseDate,
                              IDGuid= Cars.IDGuid,
                              StartInsuranceDate= Cars.StartInsuranceDate,
                              StartLicenseDate= Cars.StartLicenseDate,
                              CarReturn= DbContract.Where(i => i.CarID == Cars.Id).LastOrDefault() == null ? true : DbContract.Where(i => i.CarID == Cars.Id).LastOrDefault().CarReturn



        };

            DataGridList.ItemsSource = results.ToList();

            return;

        }


        #region Validation
        enum ControlsName
        {
            txtCarName,
            txtCarModel,
            txtYear,
            txtCarNumber,
            txtCarColor,
            txtStartLicenseDate,
            txtEndLicenseDate,
            txtStartInsuranceDate,
            txtEndInsuranceDate
        }


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
                case nameof(ControlsName.txtCarName):
                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "نوع السيارة لا يمكن ان يكون فارغ";
                        uIElement.Focus();
                    }

                    break;
                case nameof(ControlsName.txtCarModel):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "موديل السيارة لا يمكن ان يكون فارغ";
                        uIElement.Focus();
                    }
                    
                    break;
                case nameof(ControlsName.txtYear):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك سنة الصنع فارغ";
                        uIElement.Focus();

                    }
                    else if (StoriedParameter.Isdouble(textBox.Text) == false)
                    {
                        errorMsg = "ادخل سنة الصنع بشكل صحيح";
                        uIElement.Focus();
                    }

                    break;

                case nameof(ControlsName.txtCarNumber):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك رقم السيارة فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.Isdouble(textBox.Text) == false)
                    {
                        errorMsg = "ادخل رقم السيارة بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtCarColor):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك لون السيارة فارغ";
                        uIElement.Focus();
                    }
                   
                    break;

                case nameof(ControlsName.txtStartLicenseDate):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك تاريخ بداية الترخيص فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text) == false)
                    {
                        errorMsg = "ادخل تاريخ بداية الترخيص بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtEndLicenseDate):


                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك تاريخ نهاية الترخيص فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text))
                    {
                        errorMsg = "ادخل تاريخ نهاية الترخيص بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtStartInsuranceDate):


                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك تاريخ بداية التامين فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text))
                    {
                        errorMsg = "ادخل تاريخ بداية التامين بشكل صحيح";
                        uIElement.Focus();
                    }
                    break;
                case nameof(ControlsName.txtEndInsuranceDate):


                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك تاريخ انتهاء التامين فارغ";
                        uIElement.Focus();
                    }
                    else if (StoriedParameter.IsDate(textBox.Text))
                    {
                        errorMsg = "ادخل تاريخ انتهاء التامين بشكل صحيح";
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


            txtCarName.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtCarName.ToolTip = null;

            txtCarModel.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtCarModel.ToolTip = null;

            txtYear.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtYear.ToolTip = null;

            txtCarNumber.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtCarNumber.ToolTip = null;

            txtCarColor.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Ok));
            txtCarColor.ToolTip = null;
           
        }

        #endregion


        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            AddNewPop.IsOpen = true;

            Cars = new VMCars();

            DataContext = Cars;
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridList.ItemsSource = await new DataBase().SearchCars(txtSearch.Text); ;
        }

        private void DataGridList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddNewPop.StaysOpen = true;

            Cars = (VMCars)DataGridList.SelectedItem;

            DataContext = Cars;


            Car.Id = Cars.Id;
            Car.IDGuid = Cars.IDGuid;

            AddNewPop.IsOpen = true;
        }



        Tables.Cars Car = new Tables.Cars();

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            string MessageUser = "";

            if (txtStartLicenseDate.Text.Length == 0 || StoriedParameter.IsDate(txtStartLicenseDate.Text) == false)
            {
                txtStartLicenseDate.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtStartLicenseDate.ToolTip = GetErrorMessage(txtStartLicenseDate);
                MessageUser = GetErrorMessage(txtStartLicenseDate);

                Counter += 1;
            }
            if (txtEndLicenseDate.Text.Length == 0 || StoriedParameter.IsDate(txtEndLicenseDate.Text) == false)
            {
                txtEndLicenseDate.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtEndLicenseDate.ToolTip = GetErrorMessage(txtEndLicenseDate);
                MessageUser = GetErrorMessage(txtEndLicenseDate);

                Counter += 1;
            }
            if (txtStartInsuranceDate.Text.Length == 0 || StoriedParameter.IsDate(txtStartInsuranceDate.Text) == false)
            {
                txtStartInsuranceDate.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtStartInsuranceDate.ToolTip = GetErrorMessage(txtStartInsuranceDate);
                MessageUser = GetErrorMessage(txtStartInsuranceDate);

                Counter += 1;
            }
            if (txtEndInsuranceDate.Text.Length == 0 || StoriedParameter.IsDate(txtEndInsuranceDate.Text) == false)
            {
                txtEndInsuranceDate.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtEndInsuranceDate.ToolTip = GetErrorMessage(txtEndInsuranceDate);
                MessageUser = GetErrorMessage(txtEndInsuranceDate);

                Counter += 1;
            }
           
            if (txtCarColor.Text.Length == 0)
            {
                txtCarColor.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtCarColor.ToolTip = GetErrorMessage(txtCarColor);
                MessageUser = GetErrorMessage(txtCarColor);

                Counter += 1;
            }
            if (txtCarNumber.Text.Length == 0 || StoriedParameter.Isdouble(txtCarNumber.Text) == false)
            {
                txtCarNumber.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtCarNumber.ToolTip = GetErrorMessage(txtCarNumber);
                MessageUser = GetErrorMessage(txtCarNumber);

                Counter += 1;
            }
            if (txtYear.Text.Length == 0 || StoriedParameter.Isdouble(txtYear.Text) == false)
            {
                txtYear.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtYear.ToolTip = GetErrorMessage(txtYear);
                MessageUser = GetErrorMessage(txtYear);

                Counter += 1;
            }
            if (txtCarModel.Text.Length == 0)
            {
                txtCarModel.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtCarModel.ToolTip = GetErrorMessage(txtCarModel);
                MessageUser = GetErrorMessage(txtCarModel);

                Counter += 1;


            }
            if (txtCarName.Text.Length == 0)
            {
                txtCarName.FontFamily = new FontFamily(nameof(StoriedParameter.Validtion.Error));
                txtCarName.ToolTip = GetErrorMessage(txtCarName);
                MessageUser = GetErrorMessage(txtCarName);

                Counter += 1;

            }



            Car = new Tables.Cars();

            Car.Id = Cars.Id;
            Car.CarName = Cars.CarName;
            Car.CarModel = Cars.CarModel;
            Car.CarColor = Cars.CarColor;
            Car.CarYear = Cars.CarYear;
            Car.CarNumber = Cars.CarNumber;
            Car.StartLicenseDate = Cars.StartLicenseDate;
            Car.EndLicenseDate = Cars.EndLicenseDate;
            Car.StartInsuranceDate = Cars.StartInsuranceDate;
            Car.EndInsuranceDate = Cars.EndInsuranceDate;

            txtMessage.Text = MessageUser;

            if (Counter == 0)
            {
                if (Car.Id == 0)
                {
                    await new DataBase().SaveCar(Car);
                }
                else
                {
                    await new DataBase().UpdateCar(Car);
                }

                Farms_LoadedAsync();
            }
        }

        private void CloseAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewPop.IsOpen = false;
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
    }
}
