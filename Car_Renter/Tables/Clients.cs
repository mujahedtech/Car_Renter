using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Renter.Tables
{
    public class Clients:Car_Renter.ViewModel.BaseNotifyViewModel
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Guid IDGuid { get; set; } = Guid.NewGuid();
        public string ClientName { get; set; }
        public string NationalID { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public DateTime Birthdate { get; set; } = DateTime.Now;
        public string LicenseNumber { get; set; }
        public DateTime EndDateLicense { get; set; } = DateTime.Now;
    }
}
