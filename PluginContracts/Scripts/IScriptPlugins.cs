using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ktos.DjToKey.Plugins.Scripts
{
    public interface IScriptPlugins
    {        
        /// <summary>
        /// List of objects from plugins to be included into script engine when loading
        /// </summary>
        IEnumerable<IScriptObject> Objects { get; }
        
        /// <summary>
        /// List of types from plugins to be included into script engine when loading
        /// </summary>
        IEnumerable<IScriptType> Types { get; }        
    }
}
