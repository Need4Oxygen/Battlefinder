using System.Collections.Generic;
using Pathfinder2e;
using Pathfinder2e.Containers;
using UnityEngine;
using YamlDotNet;

namespace Pathfinder2e.Character
{

    public class APIC
    {
        public APIC() { }

        public APIC(string selector, CharacterData charData, string abl, int initialScore)
        {
            this.selector = selector;
            this.charData = charData;
            this.abl = abl;
            this.initialScore = initialScore;

            Refresh();
        }

        public CharacterData charData = null;
        public string selector = "";
        public string abl = "";
        public int initialScore = 0;

        public string prof = "U";
        public string profColored = "";

        public int ablScore = 0;
        public int profScore = 0;
        public int itemScore = 0;
        public int tempScore = 0;

        public int score { get { return initialScore + ablScore + profScore; } }

        public int dcScore { get { return 10 + profScore; } }

        public virtual IEnumerable<RuleElement> GetElements()
        {
            return charData.RE_Get(selector);
        }

        public void Refresh()
        {
            IEnumerable<RuleElement> elements = GetElements();

            prof = DB.Prof_FindMax(elements);
            profColored = DB.Prof_Abbr2AbbrColored(prof);

            ablScore = charData.Abl_GetMod(abl);
            profScore = charData.level + DB.Prof_Abbr2Score(DB.Prof_FindMax(elements));
        }
    }

    public class APIC_Lore : APIC
    {
        public APIC_Lore() { }

        public APIC_Lore(string selector, string loreName, CharacterData charData, string abl, int initialScore) : base(selector, charData, abl, initialScore)
        {
            this.selector = selector;
            this.loreName = loreName;
            this.charData = charData;
            this.abl = abl;
            this.initialScore = initialScore;
        }

        public string loreName = "";

        public override IEnumerable<RuleElement> GetElements()
        {
            return charData.RE_Get(selector, loreName);
        }
    }

    public class APIC_Skill : APIC
    {
        public APIC_Skill() { }

        public APIC_Skill(string selector, CharacterData charData, string abl, int initialScore) : base(selector, charData, abl, initialScore)
        {
            this.selector = selector;
            this.charData = charData;
            this.abl = abl;
            this.initialScore = initialScore;
        }

        public override IEnumerable<RuleElement> GetElements()
        {
            return charData.RE_GetFromSkill(selector);
        }
    }

}
