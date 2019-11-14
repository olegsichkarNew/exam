using IntreviewExam.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.DataAccessLayer.Contracts
{
    public interface IContractRepository
    {
         void Insert(Contract contract);
         void BulkInsert(IEnumerable<Contract> contracts);
        Individual SearchIndividual(string nationalId);
    }
}
