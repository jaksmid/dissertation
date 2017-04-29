using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Metadata.Attributes;
using Metadata.Global;

namespace Metadata.Import
{
    public class CsvMetadataImporter : IMetadataImporter
    {
        private readonly AttributeAnalyzer _attributeAnalyzer;

        public CsvMetadataImporter(AttributeAnalyzer attributeAnalyzer)
        {
            _attributeAnalyzer = attributeAnalyzer;
        }

        public DatasetMetadata ImportMetadata(string fileName)
        {
            Trace.WriteLine("Importing Metadata: "+fileName);
            int rows = 0;
            bool isArff = fileName.EndsWith(".arff",StringComparison.CurrentCultureIgnoreCase);
            using (var reader = new StreamReader(fileName))
            {
                string line;
                bool first = true;
                var attributes = new List<List<string>>();
                while ((line = reader.ReadLine()) != null)
                {
                    rows++;
                    line = line.TrimStart();
                    if (isArff && (line.StartsWith("%") || line.StartsWith("@")))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    string[] linesplit = line.Split(',');

                    if (first)
                    {
                        attributes.AddRange(linesplit.Select(v => new List<string>()));
                        if (linesplit[0].IndexOf("doctype", StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            throw new Exception("403 or 404");
                        }
                    }
                    for (int i = 0; i < linesplit.Length; i++)
                    {
                        attributes[i].Add(linesplit[i]);
                    }
                    first = false;
                }
                var targetStringAttributes = attributes[attributes.Count - 1];
                var target = _attributeAnalyzer.CreateCorrespondingAttribute(targetStringAttributes);
                target.IsTarget = true;
                List<double> targetValues;
                if (target is NumericalAttribute)
                {
                    targetValues =
                        NumericalAttribute.RecastToReals(targetStringAttributes)
                            .Select(d => d.HasValue ? d.Value : 0)
                            .ToList();
                }
                else
                {
                    targetValues =
                        CategoricalMetadata.RecastCategoricalsToReals(targetStringAttributes)
                            .Select(d => d.HasValue ? d.Value : 0)
                            .ToList();
                }
                var nonTargetAttributes = attributes.GetRange(0, attributes.Count - 1);
                var atlist = new List<AttributeMetadata>();
                for (int attNr = 0; attNr < nonTargetAttributes.Count; attNr++)
                {
                    Trace.WriteLine("Processing attribute " + (attNr + 1) + "/" + nonTargetAttributes.Count);
                    var currentAttribute = nonTargetAttributes[attNr];
                    //skip attributes with only single value
                    if (currentAttribute.GroupBy(x => x).Count() == 1) continue;
                    atlist.Add(_attributeAnalyzer.CreateCorrespondingAttribute(currentAttribute, targetValues,
                        target is NumericalAttribute));
                }
                atlist.Add(target);
                return new DatasetMetadata(atlist, Path.GetFileNameWithoutExtension(fileName), rows);
            }
        }
    }
}