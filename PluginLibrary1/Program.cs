using AssemblyLoadingTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginLibrary1
{
    public class PluginLibrary1Invoker : MarshalByRefObject, IMyInterface
    {
        public string GetSecretString()
        {
            return new ExternalLibrary.ExternalLibrary().GetVersionString();
        }
    }
}
