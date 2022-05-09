using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Renter.Tables
{
   public class Cars
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Guid IDGuid { get; set; } = Guid.NewGuid();
        public string CarName { get; set; }
        public string CarModel { get; set; }
        public string CarYear { get; set; }
        public string CarNumber { get; set; }
        public string CarColor { get; set; }

        public DateTime StartLicenseDate { get; set; } = DateTime.Now;

        //تاريخ انتهاء الترخيص
        public DateTime EndLicenseDate { get; set; } = DateTime.Now;



        public DateTime StartInsuranceDate { get; set; } = DateTime.Now;

        //تاريخ انتهاء التامين
        public DateTime EndInsuranceDate { get; set; } = DateTime.Now;
    }
}
