using System;
using System.Collections.Generic;
using System.Linq;
using Inspector.IfSQ;

namespace Inspector.Reports
{
    class IfSQLevel2Report
    {
        public void PrintMetrics(IEnumerable<Level2Score> ifsqScores)
        {
            PrintDetails(ifsqScores);
            PrintTotal(ifsqScores);
        }

        private static void PrintDetails(IEnumerable<Level2Score> ifsqScores)
        {
            Console.WriteLine(Level2Score.HeaderString());
            ifsqScores.OrderByDescending(s => s.Rating).ToList().ForEach(s => Console.WriteLine(s));
        }

        private static void PrintTotal(IEnumerable<Level2Score> ifsqScores)
        {
            int totalLines = ifsqScores.Sum(sf => sf.Loc);
            var total = new Level2Score(totalLines, ifsqScores);
            Console.WriteLine(total);
        }

    }
}
