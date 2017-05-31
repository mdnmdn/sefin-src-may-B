using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Jobs.FaiCose
{
    public class ProcessaAnagrificheJob : BaseCompanyJob
    {
        public ProcessaAnagrificheJob(string abi) : base(abi)
        {
        }

        public override void RunJob()
        {
            CompanyLog("Processing...");
        }
    }
}
