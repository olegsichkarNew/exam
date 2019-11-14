using IntreviewExam.DataAccessLayer.Contracts;
using IntreviewExam.DataAccessLayer.DbConstants;
using IntreviewExam.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.DataAccessLayer
{
    public class ErrorRepository : BaseRepository, IErrorRepository
    {
        public void BulkInsert(IEnumerable<Error> errors)
        {
            var builder = new SqlParametersBuilder()
              .WithErrorTableParam(SqlSpParameterNames.ErrorsParamName,  errors);
            ExecuteProcedure(SqlSpNames.InsertErrorBulkLogSpName, builder.Build());

        }

        public void Insert(Error error)
        {
            var builder = new SqlParametersBuilder()
             .WithStringParam(SqlSpParameterNames.ContractCodeParamName, error.ContractCode)
             .WithStringTableParam(SqlSpParameterNames.MessagesParamName, UserDefinedTableTypes.StringValuesTableTypeName, error.Messages);
            ExecuteProcedure(SqlSpNames.InsertErrorLogSpName, builder.Build());
        }
    }
}
