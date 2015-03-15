using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace MipSim.IDE
{
    public class MipsimHighlightingDefinition : IHighlightingDefinition
    {
        private HighlightingRuleSet ruleSet;

        public MipsimHighlightingDefinition()
        {
            ruleSet = new HighlightingRuleSet();

            //rule 1
            var rule1 = new HighlightingRule();
            rule1.Regex = new Regex(@"\b(DADDU|daddu|DMULT|dmult|OR|or|DSLLV|dsllv|SLT|slt|BNE|bne|LW|lw|LWU|lwu|SW|sw|DADDIU|daddiu|ANDI|andi|J|j)\b");
            rule1.Color = new HighlightingColor();
            rule1.Color.FontWeight = System.Windows.FontWeights.Bold;
            rule1.Color.Foreground = new MipsimInstructionBrush();
            ruleSet.Rules.Add(rule1);

            //rule 2
            var rule2 = new HighlightingRule();
            rule2.Regex = new Regex(@"\b(R0|r0|R1|r1|R2|r2|R3|r3|R4|r4|R5|r5|R6|r6|R7|r7|R8|r8|R9|r9)\b");
            rule2.Color = new HighlightingColor();
            rule2.Color.Foreground = new MipsimRegisterBrush();
            ruleSet.Rules.Add(rule2);

            //rule 3
            var rule3 = new HighlightingRule();
            rule3.Regex = new Regex(@"\b(R10|r10|R11|r11|R12|r12|R13|r13|R14|r14|R15|r15|R16|r16|R17|r17|R18|r18|R19|r19)\b");
            rule3.Color = new HighlightingColor();
            rule3.Color.Foreground = new MipsimRegisterBrush();
            ruleSet.Rules.Add(rule3);

            //rule 4
            var rule4 = new HighlightingRule();
            rule4.Regex = new Regex(@"\b(R20|r20|R21|r21|R22|r22|R23|r23|R24|r24|R25|r25|R26|r26|R27|r27|R28|r28|R29|r29)\b");
            rule4.Color = new HighlightingColor();
            rule4.Color.Foreground = new MipsimRegisterBrush();
            ruleSet.Rules.Add(rule4);

            //rule 5
            var rule5 = new HighlightingRule();
            rule5.Regex = new Regex(@"\b(R30|r30|R31|r31)\b");
            rule5.Color = new HighlightingColor();
            rule5.Color.Foreground = new MipsimRegisterBrush();
            ruleSet.Rules.Add(rule5);

        }
        public HighlightingColor GetNamedColor(string name)
        {
            return new HighlightingColor();
        }

        public HighlightingRuleSet GetNamedRuleSet(string name)
        { 
            return ruleSet;
        }

        public HighlightingRuleSet MainRuleSet
        {
            get { return ruleSet; }
        }

        public string Name
        {
            get { return "Mipsim"; }
        }

        public IEnumerable<HighlightingColor> NamedHighlightingColors
        {
            get { return new List<HighlightingColor>().AsEnumerable<HighlightingColor>(); }
        }
    }

    public class MipsimInstructionBrush : HighlightingBrush
    {
        public override Brush GetBrush(ICSharpCode.AvalonEdit.Rendering.ITextRunConstructionContext context)
        {
            var converter = new System.Windows.Media.BrushConverter();
            return (Brush)converter.ConvertFromString("#0000FF");
        }
    }

    public class MipsimRegisterBrush : HighlightingBrush
    {
        public override Brush GetBrush(ICSharpCode.AvalonEdit.Rendering.ITextRunConstructionContext context)
        {
            var converter = new System.Windows.Media.BrushConverter();
            return (Brush)converter.ConvertFromString("#7F0000");
        }
    }
}
