using AssemblyLoadingTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginLibrary2
{
    public class PluginLibrary2Invoker : IMyInterface
    {
        public string GetSecretString()
        {
            return new ExternalLibrary.ExternalLibrary().GetVersionString();
        }
    }
}
