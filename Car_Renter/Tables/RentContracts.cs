using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Renter.Tables
{
   public class RentContracts : Car_Renter.ViewModel.BaseNotifyViewModel
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Guid IDGuid { get; set; } = Guid.NewGuid();

        public int ClientID { get; set; }

        public int SecoundClientID { get; set; }


        public int CarID { get; set; }

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
}
