using System;
using System.Collections.Generic;
using System.Linq;
using Inspector.CustomAnalyzers;

namespace Inspector.Reports
{
    class ClassMembersReport
    {
        public void PrintMetrics(IEnumerable<ClassScore> classMemberScores)
        {
            Console.WriteLine($"Number of classes: {classMemberScores.Count()}");

            Console.WriteLine($"  Highest number of members: {classMemberScores.Max(cms => cms.MemberCount)}");
            Console.WriteLine($"  Average number of members: {classMemberScores.Average(cms => cms.MemberCount)}");
        }
    }
}
