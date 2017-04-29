using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Metadata.Attributes
{
    public class BooleanMetadata : CategoricalMetadata
    {
        public BooleanMetadata()
        {
            
        }

        [JsonIgnore]
        public override string Type
        {
            get { return "Boolean"; }
        }

        public BooleanMetadata(List<double?> values, List<double> targetValues, bool forRegression):base(values, targetValues, forRegression)
        {
            
        }
    }
}