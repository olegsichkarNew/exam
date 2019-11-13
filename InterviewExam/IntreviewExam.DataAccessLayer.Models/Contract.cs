using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.DataAccessLayer.Models
{
    public class Contract
    {
        public string ContractCode { get; set; }
        public int PhaseOfContract { get; set; }
        public decimal OriginalAmount { get; set; }
        public int OriginalAmountCurrency { get; set; }
        public decimal InstallmentAmount { get; set; }
        public int InstallmentAmountCurrency { get; set; }
        public decimal CurrentBalance { get; set; }
        public int CurrentBalanceCurrency { get; set; }
        public decimal OverdueBalance { get; set; }
        public int OverdueBalanceCurrency { get; set; }
        public DateTime DateOfLastPayment { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public DateTime DateAccountOpened { get; set; }
        public DateTime RealEndDate { get; set; }
        public List<SubjectRole> SubjectRoles { get; set; } = new List<SubjectRole>();
        public List<Individual> Individuals { get; set; } = new List<Individual>();

    }
}
