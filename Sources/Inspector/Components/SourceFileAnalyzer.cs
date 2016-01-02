using Inspector.CodeMetrics;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Components
{
    public class SourceFileAnalyzer
    {
        private readonly ICollection<ICodeMetricAnalyzer> analyzers = new List<ICodeMetricAnalyzer>();

        public SourceFileAnalyzer()
        {
        }

        public void AddAnalyzer(ICodeMetricAnalyzer analyzer)
        {
            analyzers.Add(analyzer);
        }

        public IEnumerable<SourceFile> AppendCodeMetrics(IEnumerable<SourceFile> sourceFiles)
        {
            var analyzedFiles = sourceFiles.ToList();
            analyzedFiles.ForEach(sf => sf.CalculateMetricsWith(analyzers));
            return analyzedFiles;
        }
    }
}
