using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewExam.Entities
{
    public class SubjectRole
    {
        public string CustomerCode { get; set; }
        public CommonRoleOfCustomer RoleOfCustomer { get; set; }
        public decimal? GuaranteeAmount { get; set; }
        public CommonCurrency? GuaranteeAmountCurrency { get; set; }
    }
}
