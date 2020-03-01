using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.BackEnd.Data
{
    public class Transaction
    {
        public DateTime TransactionDate { get; set; }
        public DateTime PostDate { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; }
        public string CardNumber { get; set; }
        public string CardholderName { get; set; }
        public int MCC { get; set; }
        public string MCCDescription { get; set; }
        public string MCCGroup { get; set; }
    }
}
