using Inspector.CodeMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector.Components
{
    public class SourceFileAnalyzer
    {
        private readonly ICollection<ICodeMetricAnalyzer> analyzers = new List<ICodeMetricAnalyzer>();

        public SourceFileAnalyzer()
        {
        }

        public void AddCodeMetric(ICodeMetricAnalyzer analyzer)
        {
            analyzers.Add(analyzer);
        }

        public IEnumerable<SourceFile> Analyze(Solution solution)
        {
            var files = solution.SourceFiles.ToList();
            files.ForEach(sf => sf.CalculateMetricsWith(analyzers));
            return files;
        }
    }
}
