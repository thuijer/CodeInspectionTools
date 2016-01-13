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
            Console.WriteLine($"Number of classes: {classMemberScores.Count()}");

            Console.WriteLine($"  Highest number of members: {classMemberScores.Max(cms => cms.MemberCount)}");
            Console.WriteLine($"  Average number of members: {classMemberScores.Average(cms => cms.MemberCount):N2}");
            Console.WriteLine($"  Standard deviation of members: {classMemberScores.Select(cms => cms.MemberCount).StdDev():N2}");
        }
    }
}
