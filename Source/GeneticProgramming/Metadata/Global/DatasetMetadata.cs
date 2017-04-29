using System;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Data.Models;
using Metadata.Attributes;
using Newtonsoft.Json;

namespace Metadata.Global
{
    public class DatasetMetadata
    {
        private IEnumerable<AttributeMetadata> _attributes;
        private String _name;

        public int RowsCount { get; set; }

        public int AttributesCount { get; set; }

        [JsonIgnore]
        public IEnumerable<NumericalAttribute> NumericalAttributes
        {
            get { return NonTargetAttributes.OfType<NumericalAttribute>(); }
        }

        [JsonIgnore]
        public IEnumerable<NumericalAttribute> RealAttributes
        {
            get { return NonTargetAttributes.OfType<NumericalAttribute>().Where(a => !a.IntegersOnly); }
        }

        [JsonIgnore]
        public IEnumerable<NumericalAttribute> IntegerAttributes
        {
            get { return NonTargetAttributes.OfType<NumericalAttribute>().Where(a=>a.IntegersOnly); }
        }

        [JsonIgnore]
        public IEnumerable<CategoricalMetadata> CategoricalAttributes
        {
            get { return NonTargetAttributes.OfType<CategoricalMetadata>(); }
        }

        public DatasetMetadata()
        {
            
        }

        public DatasetMetadata(IEnumerable<AttributeMetadata> atlist, string name, int rows)
        {
            Name = name;
            var attributeMetadatas = atlist as IList<AttributeMetadata> ?? atlist.ToList();
            Attributes = attributeMetadatas;
            AttributesCount = attributeMetadatas.Count;
            RowsCount = rows;
        }

        [JsonIgnore]
        public IEnumerable<AttributeMetadata> NonTargetAttributes
        {
            get { return Attributes.Where(a => !a.IsTarget); }
        }

        [JsonIgnore]
        public AttributeMetadata TargetAttribute
        {
            get { return Attributes.First(a => a.IsTarget); }
        }

        [JsonIgnore]
        public bool HasAttributeWithMissingValues
        {
            get { return Attributes.Any(a => a.MissingValues); }
        }

        [JsonIgnore]
        public int NumberOfBooleanAttributes
        {
            get { return Attributes.Count(a => a is BooleanMetadata && !a.IsTarget); }
        }

        [JsonIgnore]
        public int NumberOfCategoricalAttributes
        {
            get { return Attributes.Count(a => a is CategoricalMetadata && !a.IsTarget); }
        }

        [JsonIgnore]
        public int NumberOfRealAttributes
        {
            get { return Attributes.Count(a => a is NumericalAttribute && !((NumericalAttribute)a).IntegersOnly && !a.IsTarget); }
        }

        [JsonIgnore]
        public int NumberOfIntegerAttributes
        {
            get { return Attributes.Count(a => a is NumericalAttribute && ((NumericalAttribute)a).IntegersOnly && !a.IsTarget); }
        }

        [JsonIgnore]
        public int NumberOfNumericalAttributes
        {
            get { return Attributes.Count(a => a is NumericalAttribute && !a.IsTarget); }
        }

        [JsonIgnore]
        public string Hash { get; set; }

        [JsonIgnore]
        public GlobalDatasetMetadata GlobalMetadata { get; set; }

        public IEnumerable<AttributeMetadata> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
