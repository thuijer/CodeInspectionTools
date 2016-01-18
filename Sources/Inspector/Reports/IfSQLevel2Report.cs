using System;
using System.Collections.Generic;
using System.Linq;
using Inspector.IfSQ;
using System.IO;

namespace Inspector.Reports
{
    class IfSQLevel2Report
    {
        public void PrintMetrics(IEnumerable<Level2Score> ifsqScores, StreamWriter writer)
        {
            PrintDetails(ifsqScores, writer);
            PrintTotal(ifsqScores, writer);
        }

        private static void PrintDetails(IEnumerable<Level2Score> ifsqScores, StreamWriter writer)
        {
            writer.WriteLine(Level2Score.HeaderString());
            ifsqScores.OrderByDescending(s => s.Rating).ToList().ForEach(s => writer.WriteLine(s));
        }

        private static void PrintTotal(IEnumerable<Level2Score> ifsqScores, StreamWriter writer)
        {
            int totalLines = ifsqScores.Sum(sf => sf.Loc);
            var total = new Level2Score(totalLines, ifsqScores);
            writer.WriteLine(total);
        }

    }
}
