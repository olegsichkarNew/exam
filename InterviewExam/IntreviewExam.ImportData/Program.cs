using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace IntreviewExam.ImportData
{
    class Program
    {
        const string FileName = @"d:\Sample.xml";
        const string FileNameXsd = @"d:\Data.xsd";
        static void Main(string[] args)
        {
            IUnityContainer unitycontainer = new UnityContainer();
            unitycontainer.RegisterType<IImportContract, ImportContract>();
            unitycontainer.RegisterType<IImportValidator, ImportValidator>();
            ImportContract importContract = unitycontainer.Resolve<ImportContract>();
            importContract.ProcessFile(FileName, FileNameXsd);
        }
    }
}
