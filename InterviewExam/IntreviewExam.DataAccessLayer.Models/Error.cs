using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.DataAccessLayer.Models
{
    public class Error
    {
        public string ContractCode { get; set; }
        public List<string> Messages { get; set; } = new List<string>();

    }
}
