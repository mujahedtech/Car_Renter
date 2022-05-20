using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Renter
{
   public class DataBase
    {
        public readonly SQLiteAsyncConnection _database;


        public static string BackUp = "MujahedTech_Bakup.db3";

        public static string Path = "MujahedTech.db3";
        public DataBase(string dbpath = "")
        {
            dbpath = Path;
            
            _database = new SQLiteAsyncConnection(dbpath);

            _database.CreateTableAsync<Tables.Clients>();
            _database.CreateTableAsync<Tables.Cars>();
            _database.CreateTableAsync<Tables.RentContracts>();
            _database.CreateTableAsync<Tables.Payments>();


        }

        public Task BackUpDb()
        {
          
            return _database.BackupAsync(BackUp);

        }

        //Here is For DB CLients
        #region Client
        public Task<List<Tables.Clients>> GetClients()
        {

            return _database.Table<Tables.Clients>().OrderByDescending(i => i.Id).ToListAsync();
        }


        public Task<List<Tables.Clients>> SearchClients(string sValue)
        {

            //return _database.Table<Tables.Clients>().Where(p => p.Id.ToString().Contains(sValue)
            //                          || (p.Id.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.ClientName.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.LicenseNumber.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.NationalID.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.MobileNumber.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                             ).OrderByDescending(i => i.Id).ToListAsync();

            sValue = sValue.Trim().ToLower();

            return _database.Table<Tables.Clients>().Where(p => p.ClientName.ToLower().Contains(sValue)||p.NationalID.ToLower().Contains(sValue)||p.MobileNumber.ToLower().Contains(sValue))
                                   .OrderByDescending(i => i.Id).ToListAsync();
        }


        public Task<int> SaveClient(Tables.Clients Clients)
        {
            return _database.InsertAsync(Clients);

        }

        public Task<int> UpdateClient(Tables.Clients Clients)
        {
            return _database.UpdateAsync(Clients);

        }
        #endregion



        //Here is For DB Cars
        #region Cars
        public Task<List<Tables.Cars>> GetCars()
        {

            return _database.Table<Tables.Cars>().OrderByDescending(i => i.Id).ToListAsync();
        }





        public Task<List<Tables.Cars>> SearchCars(string sValue)
        {

            //return _database.Table<Tables.Clients>().Where(p => p.Id.ToString().Contains(sValue)
            //                          || (p.Id.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.ClientName.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.LicenseNumber.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.NationalID.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.MobileNumber.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                             ).OrderByDescending(i => i.Id).ToListAsync();

            sValue = sValue.Trim().ToLower();

            return _database.Table<Tables.Cars>().Where(p => p.CarName.ToLower().Contains(sValue) || p.CarModel.ToLower().Contains(sValue) || p.CarNumber.ToLower().Contains(sValue))
                                   .OrderByDescending(i => i.Id).ToListAsync();
        }


        public Task<int> SaveCar(Tables.Cars Cars)
        {
            return _database.InsertAsync(Cars);

        }

        public Task<int> UpdateCar(Tables.Cars Cars)
        {
            return _database.UpdateAsync(Cars);

        }
        #endregion


        //Here is For DB Rent Contract
        #region Rent Contract
        public Task<List<Tables.RentContracts>> GetRentContract()
        {

            return _database.Table<Tables.RentContracts>().ToListAsync();
        }


        public Task<List<Tables.RentContracts>> SearchContract(string sValue)
        {

            //return _database.Table<Tables.Clients>().Where(p => p.Id.ToString().Contains(sValue)
            //                          || (p.Id.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.ClientName.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.LicenseNumber.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.NationalID.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.MobileNumber.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                             ).OrderByDescending(i => i.Id).ToListAsync();

            sValue = sValue.Trim().ToLower();

            return _database.Table<Tables.RentContracts>().Where(p => p.CarID.ToString().ToLower().Contains(sValue) || p.ClientID.ToString().ToLower().Contains(sValue) || p.SecoundClientID.ToString().ToLower().Contains(sValue) || p.CarID.ToString().ToLower().Contains(sValue))
                                   .OrderByDescending(i => i.Id).ToListAsync();
        }


        public Task<int> SaveContract(Tables.RentContracts RentContracts)
        {
            return _database.InsertAsync(RentContracts);

        }

        public Task<int> UpdateContract(Tables.RentContracts RentContracts)
        {
            return _database.UpdateAsync(RentContracts);

        }
        #endregion


        //Here is For DB Payments
        #region Rent Contract
        public Task<List<Tables.Payments>> GetPayments()
        {

            return _database.Table<Tables.Payments>().ToListAsync();
        }


        public Task<List<Tables.Payments>> SearchPayments(string sValue)
        {

            //return _database.Table<Tables.Clients>().Where(p => p.Id.ToString().Contains(sValue)
            //                          || (p.Id.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.ClientName.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.LicenseNumber.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.NationalID.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                          || (p.MobileNumber.ToString().Trim().ToLower().Contains(sValue.Trim().ToLower()))
            //                             ).OrderByDescending(i => i.Id).ToListAsync();

            sValue = sValue.Trim().ToLower();

            return _database.Table<Tables.Payments>().Where(p => p.ClientId.ToString().ToLower().Contains(sValue) ) .OrderByDescending(i => i.Id).ToListAsync();
        }


        public Task<int> SavePayment(Tables.Payments Payment)
        {
            return _database.InsertAsync(Payment);

        }

        public Task<int> UpdatePayment(Tables.Payments Payment)
        {
            return _database.UpdateAsync(Payment);

        }
        #endregion
    }
}
