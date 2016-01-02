using Inspector.CodeMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector.IfSQ
{
    public class Level2Score
    {
        private const int Sp1ScoreTreshold = 200;
        private const int Sp2ScoreTreshold = 4;
        private const int Sp3ScoreTreshold = 10;

        public Level2Score(SourceFile sf)
        {
            Project = sf.Project.Name;
            File = sf.FileName;
            Loc = sf.LinesOfCode;

            Sp1 = sf.MethodScores.OfType<MethodLengthScore>().Where(ms => ms.Score > Sp1ScoreTreshold).Sum(ms => ms.Score - Sp1ScoreTreshold);
            Sp2 = sf.MethodScores.OfType<NestingLevelScore>().Where(ms => ms.Score > Sp2ScoreTreshold).Sum(ms=>ms.LineCountPerLevel[Sp2ScoreTreshold+1]);
            Sp3 = sf.MethodScores.OfType<ControlFlowComplexityScore>().Where(ms => ms.Score > Sp3ScoreTreshold).Sum(ms => ms.Score- Sp3ScoreTreshold);
        }
        public string Solution { get; private set; }
        public string Project { get; private set; }
        public string File { get; private set; }
        public string Language { get; private set; }
        public int Wip1 { get; private set; }
        public int Wip2 { get; private set; }
        public int Wip3 { get; private set; }
        public int Sp1 { get; private set; }
        public int Sp2 { get; private set; }
        public int Sp3 { get; private set; }
        public int Spm1 { get; private set; }
        public int Spm2 { get; private set; }
        public int Spm3 { get; private set; }
        public int Total { get { return Wip1 + Wip2 + Wip3 + Sp1 + Sp2 + Sp3 + Spm1 + Spm2 + Spm3; } }
        public int Loc { get; private set; }

        public int DefectsPerKloc
        {
            get
            {
                var kloc = Loc / 1000.0;
                return (int)(Total / kloc);
            }
        }

        public string Rating
        {
            get
            {
                if (DefectsPerKloc <= 1)
                    return "AAA";
                if (DefectsPerKloc <= 2)
                    return "AA";
                if (DefectsPerKloc <= 5)
                    return "A";
                if (DefectsPerKloc <= Sp3ScoreTreshold)
                    return "B";
                if (DefectsPerKloc <= 20)
                    return "C";
                if (DefectsPerKloc <= 50)
                    return "D";
                if (DefectsPerKloc <= 100)
                    return "E";
                if (DefectsPerKloc <= 200)
                    return "F";
                if (DefectsPerKloc <= 500)
                    return "FF";
                return "FFF";
            }
        }

        public override string ToString()
        {
            return $"\"{Project}\",\"{File}\",\"{Rating}\",{DefectsPerKloc},{Sp1},{Sp2},{Sp3}";
        }
    }
}
