using System;
using System.Collections.Generic;

namespace DemoDurableTaskAnalyzer.Models
{
    public class OrchestratorResult
    {
        public OrchestratorResult(string instanceId)
        {
            Id = instanceId;
            Items = new List<string>();
        }

        public string Id { get; }

        public List<string> Items { get; set; }
    }
}
