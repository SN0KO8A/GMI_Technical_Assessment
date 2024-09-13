using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    internal class GridAnalyzer
    {
        private MatchRule[] rules;

        public GridAnalyzer(MatchRule[] rules)
        {
            this.rules = rules;
        }

        public void Analyze(Grid grid)
        {
            foreach (MatchRule rule in rules)
            {
                rule.FindMatches(grid);
            }
        }

        public void DisplayResult()
        {
            foreach (MatchRule rule in rules)
            {
                rule.DisplayResult();
            }
        }
    }
}
