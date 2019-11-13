using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.DataAccessLayer.Models
{
    public class SubjectRole
    {
        public string CustomerCode { get; set; }
        public int RoleOfCustomer { get; set; }
        public decimal? GuaranteeAmount { get; set; }
        public int? GuaranteeAmountCurrency { get; set; }
    }
}
