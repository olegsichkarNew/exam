using InterviewExam.Common;
using InterviewExam.Entities;
using IntreviewExam.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace IntreviewExam.ImportData
{
    public class ImportContract : IImportContract
    {
        protected Dictionary<string, Action<string, Contract>> ContractProcessors { get; private set; }
        protected Dictionary<string, Action<string, Individual>> IndividualProcessors { get; private set; }
        protected Dictionary<string, Action<string, SubjectRole>> SubjectRoleProcessors { get; private set; }
        protected IImportValidator ImportValidator { get; set; }
        protected IContractRepository ContractRepository { get; set; }
        protected IErrorRepository ErrorRepository { get; set; }
        protected ValidatorResult CurrentValidatorResult { get; set; }
        public List<ValidatorResult> ValidatorResults { get; private set; }
        public ImportContract(IImportValidator importValidator,
            IContractRepository contractRepository,
            IErrorRepository errorRepository)
        {
            ImportValidator = importValidator;
            ContractRepository = contractRepository;
            ErrorRepository = errorRepository;
            ContractProcessors = new Dictionary<string, Action<string, Contract>>();
            IndividualProcessors = new Dictionary<string, Action<string, Individual>>();
            SubjectRoleProcessors = new Dictionary<string, Action<string, SubjectRole>>();
            ValidatorResults = new List<ValidatorResult>();
        }
        public void Import()
        {

        }
        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Console.WriteLine("\tValidation error: " + args.Message);

        }

        public void ProcessFile(string pathToXml, string pathToXsd)
        {
            SetProcesors();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, pathToXsd);
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // List<Contract> contracts = new List<Contract>();
            using (XmlReader reader = XmlReader.Create(pathToXml))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "Contract")
                                {
                                    Contract contract = new Contract();
                                    CurrentValidatorResult = new ValidatorResult();

                                    XElement el = XElement.ReadFrom(reader) as XElement;
                                    if (el != null)
                                    {
                                        foreach (var item in el.Elements())
                                        {
                                            switch (item.Name.LocalName)
                                            {
                                                case "ContractCode":
                                                    contract.ContractCode = item.FirstNode.ToString();
                                                    break;
                                                case "ContractData":
                                                    FillContractData(contract, item);
                                                    break;
                                                case "Individual":
                                                    contract.Individuals.Add(FillIndividual(item));
                                                    break;
                                                case "SubjectRole":
                                                    contract.SubjectRoles.Add(FillSubjectRole(item));
                                                    break;
                                            }

                                        }
                                    }
                                    ValidateImportedData(contract);
                                    if (ValidatorResults.Count() == 0)
                                        ContractRepository.Insert(MapContract(contract));
                                    else
                                        ErrorRepository.Insert(new DataAccessLayer.Models.Error()
                                                {
                                                    ContractCode = contract.ContractCode,
                                                    Messages = ValidatorResults.Select(x => x.Message).ToList()
                                                }
                                            );
                                    ValidatorResults.Clear();
                                }
                                break;
                        }
                    }
                }
            }


        }

        //TODO Move to BL
        private DataAccessLayer.Models.Contract MapContract(Contract contract)
        {
            var contractData = new DataAccessLayer.Models.Contract()
            {
                ContractCode = contract.ContractCode,
                CurrentBalance = contract.CurrentBalance,
                CurrentBalanceCurrency = (int)contract.CurrentBalanceCurrency,
                DateAccountOpened = contract.DateAccountOpened,
                DateOfLastPayment = contract.DateOfLastPayment,
                InstallmentAmount = contract.InstallmentAmount,
                InstallmentAmountCurrency = (int)contract.InstallmentAmountCurrency,
                NextPaymentDate = contract.NextPaymentDate,
                OriginalAmount = contract.OriginalAmount,
                OriginalAmountCurrency = (int)contract.OriginalAmountCurrency,
                OverdueBalance = contract.OverdueBalance,
                OverdueBalanceCurrency = (int)contract.OverdueBalanceCurrency,
                PhaseOfContract = (int)contract.PhaseOfContract,
                RealEndDate = contract.RealEndDate,
            };
            if (contract.Individuals != null)
            {
                foreach (var item in contract.Individuals)
                {
                    DataAccessLayer.Models.Individual individual = new DataAccessLayer.Models.Individual()
                    {
                        CustomerCode = item.CustomerCode,
                        DateOfBirth = item.DateOfBirth,
                        FirstName = item.FirstName,
                        Gender = (int)item.Gender,
                        LastName = item.LastName,
                        NationalID = item.NationalID
                    };
                    contractData.Individuals.Add(individual);
                }
            }
            if (contract.SubjectRoles != null)
            {
                foreach (var item in contract.SubjectRoles)
                {
                    DataAccessLayer.Models.SubjectRole subjectRole = new DataAccessLayer.Models.SubjectRole()
                    {
                        CustomerCode = item.CustomerCode,
                        GuaranteeAmount = item.GuaranteeAmount,
                        GuaranteeAmountCurrency = (int?)item.GuaranteeAmountCurrency,
                        RoleOfCustomer = (int)item.RoleOfCustomer
                    };
                }
            }

            return contractData;
        }

        public void ValidateImportedData(Contract contract)
        {
            ValidatorResults.AddRange(ImportValidator.ValidateFields(
                        new ValidatedFields[] { ValidatedFields.DateOfLastPayment, ValidatedFields.DateAccountOpened },
                        new string[] { (contract.DateOfLastPayment < contract.NextPaymentDate).ToString(),
                                       (contract.DateAccountOpened < contract.DateOfLastPayment ).ToString()
                        }
                        ));
            foreach (var item in contract.Individuals)
            {
                ValidatorResults.AddRange(ImportValidator.ValidateFields(
                         new ValidatedFields[] { ValidatedFields.DateOfBirth },
                         new string[] { item.DateOfBirth.ToString() }
                         ));
            }
            foreach (var item in contract.SubjectRoles)
            {
                ValidatorResults.AddRange(ImportValidator.ValidateFields(
                         new ValidatedFields[] { ValidatedFields.GuaranteeAmount },
                         new string[] { ((item.GuaranteeAmount ?? 0) < contract.OriginalAmount).ToString() }
                         ));
            }
        }
        private void ValidateCellValue(string fieldName, string cellValue)
        {
            if (ContractProcessors.ContainsKey(fieldName))
            {
                ContractProcessors[fieldName](cellValue, new Contract());
            }
        }
        private void FillContractData(Contract contract, XElement item)
        {
            try
            {
                foreach (var contractData in item.Elements())
                {
                    switch (contractData.Name.LocalName)
                    {
                        case "DateOfLastPayment":
                            contract.DateOfLastPayment = DateTime.Parse(contractData.FirstNode.ToString());
                            break;
                        case "NextPaymentDate":
                            contract.NextPaymentDate = DateTime.Parse(contractData.FirstNode.ToString());
                            break;
                        case "DateAccountOpened":
                            contract.DateAccountOpened = DateTime.Parse(contractData.FirstNode.ToString());
                            break;
                        case "RealEndDate":
                            contract.RealEndDate = DateTime.Parse(contractData.FirstNode.ToString());
                            break;
                        case "PhaseOfContract":
                            contract.PhaseOfContract = contractData.FirstNode.ToString().ToEnum<ContractPhaseOfContract>();
                            break;
                        case "OriginalAmount":
                            contract.OriginalAmount = decimal.Parse(contractData.Elements().ToArray()[0].FirstNode.ToString(), CultureInfo.InvariantCulture);
                            contract.OriginalAmountCurrency = contractData.Elements().ToArray()[1].FirstNode.ToString().ToEnum<CommonCurrency>();
                            break;
                        case "InstallmentAmount":
                            contract.InstallmentAmount = decimal.Parse(contractData.Elements().ToArray()[0].FirstNode.ToString(), CultureInfo.InvariantCulture);
                            contract.InstallmentAmountCurrency = contractData.Elements().ToArray()[1].FirstNode.ToString().ToEnum<CommonCurrency>();
                            break;
                        case "CurrentBalance":
                            contract.CurrentBalance = decimal.Parse(contractData.Elements().ToArray()[0].FirstNode.ToString(), CultureInfo.InvariantCulture);
                            contract.CurrentBalanceCurrency = contractData.Elements().ToArray()[1].FirstNode.ToString().ToEnum<CommonCurrency>();
                            break;
                        case "OverdueBalance":
                            contract.OverdueBalance = decimal.Parse(contractData.Elements().ToArray()[0].FirstNode.ToString(), CultureInfo.InvariantCulture);
                            contract.OverdueBalanceCurrency = contractData.Elements().ToArray()[1].FirstNode.ToString().ToEnum<CommonCurrency>();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }

        }

        private void ProcessException(Exception ex)
        {
            CurrentValidatorResult.AddNewErrorMessage(ex.Message);
            ValidatorResults.Add(CurrentValidatorResult);
        }

        private Individual FillIndividual(XElement item)
        {
            Individual individual = new Individual();

            try
            {
                foreach (var ind in item.Elements())
                {
                    switch (ind.Name.LocalName)
                    {
                        case "CustomerCode":
                            individual.CustomerCode = ind.FirstNode.ToString();
                            break;
                        case "FirstName":
                            individual.FirstName = ind.FirstNode.ToString();
                            break;
                        case "LastName":
                            individual.LastName = ind.FirstNode.ToString();
                            break;
                        case "Gender":
                            individual.Gender = ind.FirstNode.ToString().ToEnum<Gender>();
                            break;
                        case "DateOfBirth":
                            individual.DateOfBirth = DateTime.Parse(ind.FirstNode.ToString());
                            break;
                        case "IdentificationNumbers":
                            individual.NationalID = ind.Elements().ToArray()[0].FirstNode.ToString();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }

            return individual;
        }
        private SubjectRole FillSubjectRole(XElement item)
        {
            SubjectRole subjectRole = new SubjectRole();
            try
            {
                foreach (var ind in item.Elements())
                {
                    switch (ind.Name.LocalName)
                    {
                        case "CustomerCode":
                            subjectRole.CustomerCode = ind.FirstNode.ToString();
                            break;
                        case "Gender":
                            subjectRole.RoleOfCustomer = ind.FirstNode.ToString().ToEnum<CommonRoleOfCustomer>();
                            break;
                        case "GuaranteeAmount":
                            subjectRole.GuaranteeAmount = decimal.Parse(ind.Elements().ToArray()[0].FirstNode.ToString(), CultureInfo.InvariantCulture);
                            subjectRole.GuaranteeAmountCurrency = ind.Elements().ToArray()[1].FirstNode.ToString().ToEnum<CommonCurrency>();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                ProcessException(ex);
            }

            return subjectRole;
        }
        protected void SetProcesors()
        {
            IndividualProcessors.Add(ValidatedFields.DateOfBirth.ToString(), (v, r) =>
            {
                r.DateOfBirth = DateTime.Parse(v);
            });
            ContractProcessors.Add(ValidatedFields.DateOfLastPayment.ToString(), (v, r) =>
            {
                r.DateOfLastPayment = DateTime.Parse(v);
            });
            ContractProcessors.Add(ValidatedFields.DateAccountOpened.ToString(), (v, r) =>
            {
                r.DateAccountOpened = DateTime.Parse(v);
            });
            SubjectRoleProcessors.Add(ValidatedFields.GuaranteeAmount.ToString(), (v, r) =>
            {
                r.GuaranteeAmount = decimal.Parse(v, CultureInfo.InvariantCulture);
            });
        }
    }
}
