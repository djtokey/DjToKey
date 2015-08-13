using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ktos.DjToKey.Plugins.Contracts
{
    public interface IScriptEngine
    {
        IEnumerable<Metadata> Plugins { get; }
        void Configure();
        void Execute(Script s, object value, object ctrl);
    }    
}
