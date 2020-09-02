using System;
using System.Collections.Generic;

namespace DemoDurableTaskAnalyzer.Models
{
    public class OrchestratorResult
    {
        public OrchestratorResult()
        {
            Id = Guid.NewGuid();
            Items = new List<string>();
        }

        public Guid Id { get; }

        public List<string> Items { get; set; }
    }
}
