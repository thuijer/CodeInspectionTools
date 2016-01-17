using System;
using System.Collections.Generic;
using System.Linq;
using Inspector.CustomAnalyzers;
using Inspector.Infrastructure;
using System.IO;

namespace Inspector.Reports
{
    class ClassMembersReport
    {
        public void PrintMetrics(IEnumerable<ClassScore> classMemberScores, StreamWriter writer)
        {
            var scores = classMemberScores.ToList();
            writer.WriteLine($"Number of classes: {scores.Count}");

            writer.WriteLine($"  Highest number of members: {scores.Max(cms => cms.MemberCount)}");
            writer.WriteLine($"  Average number of members: {scores.Average(cms => cms.MemberCount):N2}");
            writer.WriteLine($"  Standard deviation of members: {scores.Select(cms => cms.MemberCount).StdDev():N2}");

            var top10LargestClasses = scores.OrderByDescending(cls => cls.TotalLineCount).Take(10);

            writer.WriteLine("Top 10 Largest classes:");
            foreach (var largeClass in top10LargestClasses)
            {
                writer.WriteLine( $"  {largeClass.Classname} - {largeClass.TotalLineCount}");
            }
        }
    }
}
