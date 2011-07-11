﻿using System.IO;
using Agdur.Abstractions;

namespace Agdur
{
    public class TextGenerator
    {
        private readonly IBenchmarkBuilderContinutation builder;

        public TextGenerator(IBenchmarkBuilderContinutation builder)
        {
            this.builder = builder;
        }

        public string Generate(TextWriter writer, OutputStrategyBase outputStrategy)
        {
            builder.ToCustom(writer).AsCustom(outputStrategy);
            return writer.ToString();
        }
    }
}