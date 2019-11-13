using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.ImportData
{
    public interface IImportValidator
    {
        List<ValidatorResult> ValidateFields(ValidatedFields[] fields, string [] values);
    }
}
