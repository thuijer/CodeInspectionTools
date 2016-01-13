using System;
using System.Collections.Generic;
using System.Linq;
using Inspector.CustomAnalyzers;
using Inspector.Infrastructure;

namespace Inspector.Reports
{
    class ClassMembersReport
    {
        public void PrintMetrics(IEnumerable<ClassScore> classMemberScores)
        {
            var scores = classMemberScores.ToList();
            Console.WriteLine($"Number of classes: {scores.Count}");

            Console.WriteLine($"  Highest number of members: {scores.Max(cms => cms.MemberCount)}");
            Console.WriteLine($"  Average number of members: {scores.Average(cms => cms.MemberCount):N2}");
            Console.WriteLine($"  Standard deviation of members: {scores.Select(cms => cms.MemberCount).StdDev():N2}");

            var top10LargestClasses = scores.OrderByDescending(cls => cls.TotalLineCount).Take(10);

            Console.WriteLine("Top 10 Largest classes:");
            foreach (var largeClass in top10LargestClasses)
            {
                Console.WriteLine( $"  {largeClass.Classname} - {largeClass.TotalLineCount}");
            }
        }
    }
}
