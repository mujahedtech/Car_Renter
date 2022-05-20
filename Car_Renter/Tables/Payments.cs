using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Renter.Tables
{
   public class Payments
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 
        public int ContractId { get; set; }
        public int ClientId { get; set; }

        public Guid ContractGuid { get; set; }
        public DateTime DateEntered { get; set; } = DateTime.Now;


        public double? Amount { get; set; } = null;
        public DateTime AmountDate { get; set; } = DateTime.Now;
        public string AmountNote { get; set; }
        public bool PrimaryPayment { get; set; } = false;

    }
}
