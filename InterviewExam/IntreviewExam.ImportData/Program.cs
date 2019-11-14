using IntreviewExam.DataAccessLayer;
using IntreviewExam.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;

namespace IntreviewExam.ImportData
{
    class Program
    {
        static readonly string FileName = System.Configuration.ConfigurationManager.AppSettings["FileName"];
        static readonly string FileNameXsd = System.Configuration.ConfigurationManager.AppSettings["FileNameXsd"];
        static void Main(string[] args)
        {
            IUnityContainer unitycontainer = new UnityContainer();
            unitycontainer.RegisterType<IImportContract, ImportContract>();
            unitycontainer.RegisterType<IImportValidator, ImportValidator>();
            unitycontainer.RegisterType<IConnection, BaseRepository>(); 
            unitycontainer.RegisterType<IErrorRepository, ErrorRepository>(); 
            unitycontainer.RegisterType<IContractRepository, ContractRepository>();
            IImportContract importContract = unitycontainer.Resolve<ImportContract>();
            importContract.ProcessFile(FileName, FileNameXsd);
        }
    }
}
