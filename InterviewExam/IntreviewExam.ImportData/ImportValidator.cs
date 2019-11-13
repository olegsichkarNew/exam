using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.ImportData
{
    public class ImportValidator : IImportValidator
    {
        protected Dictionary<ValidatedFields, Action<string>> Validators { get; private set; }

        protected ValidatorResult CurrentValidatorResult { get; set; }
        public ImportValidator()
        {
            Validators = new Dictionary<ValidatedFields, Action<string>>();
            SetValidators();
        }

        protected void SetValidators()
        {
            Validators.Add(ValidatedFields.DateOfBirth, (val) =>
            {
                DateTime date;
                DateTime.TryParse(val, out date);
                var years =  DateTime.Today.Year - date.Year;
                if ( years<18 || years>99)
                {
                    CurrentValidatorResult.AddNewErrorMessage("Individual.DateOfBirth attribute value must be between 18 and 99 years", ValidatedFields.DateOfBirth);
                }
            });
            Validators.Add(ValidatedFields.DateOfLastPayment, (val) =>
            {
                bool isCorrect;
                bool.TryParse(val, out isCorrect);
                if (!isCorrect)
                {
                    CurrentValidatorResult.AddNewErrorMessage("Contract.DateOfLastPayment attribute value must be before (in time) Contract.NextPaymentDate attribute value", ValidatedFields.DateOfLastPayment);
                }
            });
            Validators.Add(ValidatedFields.DateAccountOpened, (val) =>
            {
                bool isCorrect;
                bool.TryParse(val, out isCorrect);
                if (!isCorrect)
                {
                    CurrentValidatorResult.AddNewErrorMessage("Contract.DateAccountOpened attribute value must be before (in time) Contract.DateOfLastPayment attribute value", ValidatedFields.DateAccountOpened);
                }
            });
            Validators.Add(ValidatedFields.GuaranteeAmount, (val) =>
            {
                bool isCorrect;
                bool.TryParse(val, out isCorrect);
                if (!isCorrect)
                {
                    CurrentValidatorResult.AddNewErrorMessage("Sum of ( SubjectRole.GuaranteeAmount ) attribute values must be lower than the Contract.OriginalAmount attribute value", ValidatedFields.GuaranteeAmount);
                }
            });
        }

        public List<ValidatorResult> ValidateFields(ValidatedFields[] fields, string[] values)
        {
            var result = new List<ValidatorResult>();
            for (var i = 0; i < fields.Length; i++)
            {
                CurrentValidatorResult = new ValidatorResult() { Valid = true};
                ValidateFieldValue(fields[i], values[i]);
            
                if (!CurrentValidatorResult.Valid)
                {
                    result.Add(CurrentValidatorResult);
                }
            }
            return result;
        }

        private void ValidateFieldValue(ValidatedFields fieldName, string cellValue)
        {
            if (Validators.ContainsKey(fieldName))
            {
                Validators[fieldName](cellValue);
            }
        }
    }
}
