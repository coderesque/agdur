﻿using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Agdur
{
    /// <summary>
    /// Provides functionality for outputting the results as XML.
    /// </summary>
    public class XmlOutputStrategy : IOutputStrategy
    {
        /// <inheritdoc/>
        public void Execute(TextWriter writer, IList<IMetric> metrics)
        {
            var settings = new XmlWriterSettings { Indent = true };
            using (var xmlWriter = XmlWriter.Create(writer, settings))
            {
                xmlWriter.WriteStartElement("benchmark");

                var visitor = new XmlOutputMetricVisitor(xmlWriter);
                metrics.Accept(visitor);

                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Provides XML output of the results of a metric.
        /// </summary>
        public class XmlOutputMetricVisitor : IMetricVisitor
        {
            private readonly XmlWriter writer;

            /// <summary>
            /// Creates a new instance of the <see cref="XmlOutputMetricVisitor"/> class.
            /// </summary>
            /// <param name="writer"></param>
            public XmlOutputMetricVisitor(XmlWriter writer)
            {
                Ensure.ArgumentNotNull(writer, "writer");
                this.writer = writer;
            }

            /// <inheritdoc/>
            public void Visit(SingleValueMetric metric)
            {
                Ensure.ArgumentNotNull(metric, "metric");

                string output = Format(metric.GetValue(), metric.UnitOfMeasurement);
                writer.WriteElementString(metric.Name, output);
            }

            /// <inheritdoc/>
            public void Visit(MultipleValueMetric metric)
            {
                Ensure.ArgumentNotNull(metric, "metric");

                var values = metric.GetValues();
                for (int index = 0; index < values.Count; index++)
                {
                    writer.WriteStartElement(metric.Name);
                    writer.WriteAttributeString("index", index.ToString());

                    string value = values[index];
                    string output = Format(value, metric.UnitOfMeasurement);

                    writer.WriteString(output);

                    writer.WriteEndElement();
                }
            }

            private static string Format(string value, string unitOfMeasurement)
            {
                return string.Format("{0} {1}", value, unitOfMeasurement);
            }
        }
    }
}