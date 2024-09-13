using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    internal class GridAnalyzer
    {
        private MatchFormations[] formations;

        public GridAnalyzer(MatchFormations[] formations)
        {
            this.formations = formations;
        }

        public void Analyze(Grid grid)
        {
            foreach (MatchFormations rule in formations)
            {
                rule.FindMatches(grid);
            }
        }

        public void DisplayResult()
        {
            foreach (MatchFormations rule in formations)
            {
                rule.DisplayResult();
            }
        }
    }
}
