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
    /// <summary>
    /// Interaction logic for ClientsManage.xaml
    /// </summary>
    public partial class ClientsManage : UserControl
    {

        Tables.Clients Clients;
        public ClientsManage()
        {
            InitializeComponent();
            Clients = new Tables.Clients();

            DataContext = Clients;

            Load_Data();
        }


      
        private async void Load_Data()
        {
            AddNewPop.IsOpen = false; 

            DataGridList.ItemsSource = await new DataBase().GetClients();

            return;

        }



        #region Validation
        enum ControlsName { txtClientName, txtNationalID, txtAddress, txtMobile, txtBirthday, txtLicenseNumber, txtEndLicense }


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
                case nameof(ControlsName.txtClientName):
                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "اسم المستاجر لا يمكن ان يكون فارغ";
                        uIElement.Focus();
                    }
                  
                    break;
                case nameof(ControlsName.txtNationalID):

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
                case nameof(ControlsName.txtAddress):

                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        errorMsg = "لا يمكن ترك العنوان فارغ";
                        uIElement.Focus();

                    }
                   
                    break;

                case nameof(ControlsName.txtMobile):

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
                case nameof(ControlsName.txtBirthday):

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
                    
                case nameof(ControlsName.txtLicenseNumber):

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
                case nameof(ControlsName.txtEndLicense):

                   
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
            }

            return errorMsg;



        }


        void ClearData()
        {
            Counter = 0;
          

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
        }

        #endregion


        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            AddNewPop.IsOpen = true;

            Clients = new Tables.Clients();

            DataContext = Clients;

            txtClientName.Focus();
        }

        private void CloseAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewPop.IsOpen = false;

            AddNewPop.StaysOpen = false;
        }

        int Counter = 0;

       
        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            string MessageUser = "";
            if (txtEndLicense.Text.Length == 0||StoriedParameter.IsDate(txtEndLicense.Text) ==false)
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




            txtMessage.Text = MessageUser;


            if (Counter==0)
            {
                if (Clients.Id==0)
                {
                    await new DataBase().SaveClient(Clients);
                }
                else
                {
                    await new DataBase().UpdateClient(Clients);
                }

                Load_Data();
            }





          

        }

       

        private void GridForm_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                var uie = e.OriginalSource as UIElement;

                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));
            }
        }

        private void DataGridList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddNewPop.StaysOpen = true;

            Clients = (Tables.Clients)DataGridList.SelectedItem;

            DataContext = Clients;

            AddNewPop.IsOpen = true;

            


        }



        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
           
            DataGridList.ItemsSource = await new DataBase().SearchClients(txtSearch.Text); ;
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridList.ItemsSource = await new DataBase().SearchClients(txtSearch.Text); ;
        }
    }
}
