using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Metadata.Attributes;

namespace Metadata.Import
{
    public class AttributeAnalyzer
    {
        public AttributeMetadata CreateCorrespondingAttribute(List<string> values,List<double> targetValues=null,bool forRegression=false)
        {
            if (AllBool(values))
            {
                return new BooleanMetadata(CategoricalMetadata.RecastCategoricalsToReals(values), targetValues, forRegression);
            }
            if (AllInt(values))
            {
                return new NumericalAttribute(NumericalAttribute.RecastToReals(values), targetValues, forRegression, true);
            }
            if (AllReal(values))
            {
                return new NumericalAttribute(NumericalAttribute.RecastToReals(values), targetValues, forRegression, false);
            }
            return new CategoricalMetadata(CategoricalMetadata.RecastCategoricalsToReals(values), targetValues, forRegression);
        }

        public bool AllBool(IEnumerable<string> values)
        {
            foreach (var value in values.Where(value => !AttributeMetadata.IsMissingValue(value)))
            {
                bool b;
                if (!bool.TryParse(value, out b)) return false;
            }
            return true;
        }

        private bool AllReal(IEnumerable<string> values)
        {
            foreach (var value in values.Where(value => !AttributeMetadata.IsMissingValue(value)))
            {
                double b;
                if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out b)) return false;
            }
            return true;
        }

        private bool AllInt(IEnumerable<string> values)
        {
            foreach (var value in values.Where(value => !AttributeMetadata.IsMissingValue(value)))
            {
                int b;
                if (!int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out b))
                {
                    return false;
                    
                }
            }
            return true;
        }
    }
}
