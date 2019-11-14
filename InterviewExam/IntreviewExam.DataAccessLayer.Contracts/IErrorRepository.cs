using IntreviewExam.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.DataAccessLayer.Contracts
{
    public interface IErrorRepository
    {
        void Insert(Error error);
        void BulkInsert(IEnumerable<Error> errors);
    }
}
