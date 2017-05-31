using Sefin.Importer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Jobs.FaiCose
{
    public abstract class BaseCompanyJob : BaseJob
    {

        public BaseCompanyJob(string abi)
        {
            Abi = abi;
        }

        protected string Abi { get; set; }



        protected void CompanyLog(string message)
        {
            // log specifico per l'abi
            Log(Abi + "] " + message);
        }

    }
}
