using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewExam.Entities
{
    public class Contract
    {
        public string ContractCode { get; set; }
        public ContractPhaseOfContract PhaseOfContract { get; set; }
        public decimal OriginalAmount { get; set; }
        public CommonCurrency  OriginalAmountCurrency { get; set; }
        public decimal InstallmentAmount { get; set; }
        public CommonCurrency InstallmentAmountCurrency { get; set; }
        public decimal CurrentBalance { get; set; }
        public CommonCurrency CurrentBalanceCurrency { get; set; }
        public decimal OverdueBalance { get; set; }
        public CommonCurrency OverdueBalanceCurrency { get; set; }
        public DateTime DateOfLastPayment { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public DateTime DateAccountOpened { get; set; }
        public DateTime RealEndDate { get; set; }
        public bool IsValid { get; set; }

        public List<SubjectRole> SubjectRoles { get; set; } = new List<SubjectRole>();
        public List<Individual> Individuals { get; set; } = new List<Individual>();

    }
}
