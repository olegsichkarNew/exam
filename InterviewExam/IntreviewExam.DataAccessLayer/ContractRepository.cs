using IntreviewExam.DataAccessLayer.Contracts;
using IntreviewExam.DataAccessLayer.DbConstants;
using IntreviewExam.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.DataAccessLayer
{
    public class ContractRepository : BaseRepository, IContractRepository
    {

        public void Insert(Contract contract)
        {
            var builder = new SqlParametersBuilder()
     .WithStringParam(SqlSpParameterNames.ContractCodeParamName, contract.ContractCode)
     .WithDecimalNullableParam(SqlSpParameterNames.CurrentBalanceParamName, contract.CurrentBalance)
     .WithNullableDateTimeParam(SqlSpParameterNames.DateAccountOpenedParamName, contract.DateAccountOpened)
     .WithNullableDateTimeParam(SqlSpParameterNames.DateOfLastPaymentParamName, contract.DateOfLastPayment)
     .WithDecimalNullableParam(SqlSpParameterNames.InstallmentAmountParamName, contract.InstallmentAmount)
     .WithIntParam(SqlSpParameterNames.InstallmentAmountCurrencyParamName, contract.InstallmentAmountCurrency)
     .WithNullableDateTimeParam(SqlSpParameterNames.NextPaymentDateParamName, contract.NextPaymentDate)
     .WithDecimalNullableParam(SqlSpParameterNames.OriginalAmountParamName, contract.OriginalAmount)
     .WithIntParam(SqlSpParameterNames.OriginalAmountCurrencyParamName, contract.OriginalAmountCurrency)
     .WithDecimalNullableParam(SqlSpParameterNames.OverdueBalanceParamName, contract.OverdueBalance)
     .WithIntParam(SqlSpParameterNames.OverdueBalanceCurrencyParamName, contract.OverdueBalanceCurrency)
     .WithIntParam(SqlSpParameterNames.PhaseOfContractParamName, contract.PhaseOfContract)
     .WithNullableDateTimeParam(SqlSpParameterNames.RealEndDateParamName, contract.RealEndDate)
     .WithIndividualTableParam(SqlSpParameterNames.IndividualsParamName, contract.Individuals)
     .WithSubjectRoleTableParam(SqlSpParameterNames.SubjectRolesParamName, contract.SubjectRoles)
     .WithIntParam(SqlSpParameterNames.CurrentBalanceCurrencyParamName, contract.CurrentBalanceCurrency);
         ExecuteProcedure(SqlSpNames.InsertContractSpName, builder.Build());
        }
        public void BulkInsert(IEnumerable<Contract> contracts)
        {
            var builder = new SqlParametersBuilder()
            .WithContractTableParam(SqlSpParameterNames.ContractsParamName, contracts)
            .WithIndividualBulkTableParam(SqlSpParameterNames.IndividualsParamName, contracts)
            .WithSubjectRoleBulkTableParam(SqlSpParameterNames.SubjectRolesParamName, contracts);
            ExecuteProcedure(SqlSpNames.InsertContractBulkSpName, builder.Build());

        }
        public Individual SearchIndividual(string nationalId)
        {
            return new Individual() { NationalID = nationalId, FirstName = "test"};
        }

 
    }
}
