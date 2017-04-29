using System;
using System.Collections.Generic;

namespace Metadata.Import
{
    public class GlobalMetadataSettings
    {
        public GlobalMetadataSettings(GlobalMetadataInclusion globalMetadataInclusion, HashSet<string> filter)
        {
            GlobalMetadataInclusion = globalMetadataInclusion;
            Filter = filter;
        }

        public GlobalMetadataSettings(GlobalMetadataInclusion globalMetadataInclusion)
        {
            GlobalMetadataInclusion = globalMetadataInclusion;
            Filter = new HashSet<string>();
        }

        public GlobalMetadataInclusion GlobalMetadataInclusion { get; set; }
        public HashSet<string> Filter { get; set; }

        public override string ToString()
        {
            if (GlobalMetadataInclusion == GlobalMetadataInclusion.DontInclude) return GlobalMetadataInclusion.ToString();
            return GlobalMetadataInclusion + "[" + String.Join(";",Filter) + "]";
        }
    }
}
