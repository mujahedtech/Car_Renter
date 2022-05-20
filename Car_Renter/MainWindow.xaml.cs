using Car_Renter.ReportManage;
using Firebase.Storage;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
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
    class AccountUsers
    {
        public Guid UserID { get; set; }

        public string UserName { get; set; }

        public string UserPWD { get; set; }

        public string UserMSG { get; set; }

        public string UserType { get; set; }

        public DateTime DateCreate { get; set; }

    }
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

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CheckApp("Saleh_hardan", "Mujahed1200");
        }

        private async void btnMainButtons(object sender, RoutedEventArgs e)
        {
            double Width = GridNameButtons.ColumnDefinitions[1].ActualWidth;
            GridCursor.Width = Width;
            int index = int.Parse(((Button)e.Source).Uid);


            GridCursor.Margin = new Thickness(0 + (Width * index), 0, 0, 0);


          

            switch (index)
            {
                case 0:
                    GridMain.Children.Clear();
                    GridMain.Children.Add(new Pages.Home());
                   
                    break;
                case 1:
                    PopRentTrans.IsOpen = true;
                    break;
                case 2:
                    //GridMain.Children.Add(new Pages.FarmCycle());
                    break;
                case 3:

                    PopReports.IsOpen = true;



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


                    //Here Upload to Google

                    MessageBox.Show("يتم الان رفع النسخة الاحتياطية");

                    btnUpload_Clicked();
                    break;

                case 6:

                   

                    break;

                case 5:



                    break;
            }
        }

        private async void btnUpload_Clicked()
        {
            try
            {

                await new DataBase().BackUpDb();



                ProgressBar progressBar = new ProgressBar();
                progressBar.Foreground = Brushes.LightGreen;
                progressBar.Height = 20;

                progressBar.BorderBrush = Brushes.White;
                progressBar.BorderThickness = new Thickness(1);
                TextBlock percentStr = new TextBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 15, Foreground = Brushes.Black };
                progressBar.Maximum = 100;
                progressBar.Minimum = 0;

                GridProgress.Children.Clear();
                GridProgress.Children.Add(progressBar);
                GridProgress.Children.Add(percentStr);



                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                var path = DataBase.Path;

                var stream = File.Open(DataBase.BackUp, FileMode.Open);
                var deviceName = System.Environment.MachineName;

                var task = new FirebaseStorage("khiratserv.appspot.com",
                    new FirebaseStorageOptions
                    {
                        ThrowOnCancel = true
                    })
                      .Child(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
                    .Child(deviceName)
                    .Child(DataBase.BackUp)
                    .PutAsync(stream);

                task.Progress.ProgressChanged += (s, args) =>
                {
                    progressBar.Value = args.Percentage;
                    percentStr.Text = progressBar.Value.ToString() + " %";
                };

                var downloadlink = await task;


                var progressHide = new Progress<int>(
                ValueProgress =>
                {
                    GridProgress.Children.Clear();
                });


                await Task.Run(() => { HideProgress(10, progressHide); });
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message, "مشكلة في رفع قاعدة البيانات");
               
            }



        }

        void HideProgress(int Time, IProgress<int> progress)
        {

            System.Threading.Thread.Sleep(Time * 100);
            progress.Report(0);

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

        private void ReportsClick(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            PopReports.IsOpen = false;

            GridMain.Children.Clear();

            switch (index)
            {
                case 0:
                   
                    GridMain.Children.Add(new ReportManage.TotalReportView());
                    break;
                case 1:
                    GridMain.Children.Add(new ReportManage.ClientReports());
                    break;
               
            }
        }





        Firebase.Database.FirebaseClient firebaseClient;
        //دالة تقوم بالتواصل مع فاير بيك من اجل عمل البرنامج
        async void CheckApp(string User, string PWD)
        {
            try
            {
                firebaseClient = new Firebase.Database.FirebaseClient("https://khiratserv-default-rtdb.asia-southeast1.firebasedatabase.app/");

                var account = (await firebaseClient.Child("AccountUsersKhirat").OnceAsync<AccountUsers>()).Select(item => new AccountUsers
                {
                    UserID = item.Object.UserID,
                    UserMSG = item.Object.UserMSG,
                    UserName = item.Object.UserName,
                    UserPWD = item.Object.UserPWD,
                    UserType = item.Object.UserType
                }).ToList();
                var UsersAccounts = new System.Collections.ObjectModel.ObservableCollection<AccountUsers>(account).Where(i => i.UserName == User && i.UserPWD == PWD).ToList();


                //في حال ان المستخدم غير الموجود
                //if (UsersAccounts.Count() == 0)
                //{
                //    foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
                //    {
                //        if (window.GetType() == typeof(MainWindow))
                //        {


                //        }
                //    }
                //    return;
                //}


                //في حال ان التطبيق يوجد به امر توقف
                if (UsersAccounts.Count() > 0)
                {
                    if (UsersAccounts[0].UserType == "Stop")
                    {
                        foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(MainWindow))
                            {
                                (window as MainWindow).GridPage.Children.Clear();

                                string Message = "عزيزي المستخدم" + Environment.NewLine;
                                Message += "يوجد مشكلة بالبرنامج" + Environment.NewLine;
                                Message += "انتهت النسخة التجريبية" + Environment.NewLine;
                                Message += "يرجى التواصل مع المبرمج" + Environment.NewLine;

                                (window as MainWindow).GridPage.Children.Add(new Pages.ErrorPage(Message));
                            }
                        }
                    }
                }
            }
            catch (Exception m)
            {

                MessageBox.Show(m.Message);
            }





        }
    }
}
