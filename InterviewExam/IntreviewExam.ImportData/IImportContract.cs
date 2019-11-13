using InterviewExam.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.ImportData
{
    public interface IImportContract
    {
        void Import();
        void ProcessFile(string pathToXml, string pathToXsd);
        void ValidateImportedData(Contract contract);
    }
}
