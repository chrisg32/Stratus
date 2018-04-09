using System.Collections.Generic;
using CG.Commons.Collections;

namespace Stratus.Models
{
    public class Settings
    {
        public string HomePage { get; set; }
        public bool CrashReporting { get; set; } = false;
        public IDictionary<string, bool> ExtensionsEnabled { get; set; } = new SafeDictionary<string, bool>(true);
    }
}
