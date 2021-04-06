using System;
using System.Linq;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using Tools;
using Pathfinder2e.Containers;

namespace Pathfinder2e.Character
{

    public class APIC
    {
        [YamlIgnore] public Character charData { get { return RawData.character; } set { RawData.character = value; } }
        [YamlIgnore] public string selector { get { return RawData.selector; } set { RawData.selector = value; } }
        [YamlIgnore] public string ability { get { return RawData.ability; } set { RawData.ability = value; } }
        [YamlIgnore] public int initialScore { get { return RawData.initialScore; } set { RawData.initialScore = value; } }

        [YamlIgnore] public string prof { get { return RawData.prof; } }
        [YamlIgnore] public string profLvl20 { get { return RawData.profLvl20; } }
        [YamlIgnore] public string profColored { get { return RawData.profColored; } }

        [YamlIgnore] public int ablScore { get { return RawData.ablScore; } }
        [YamlIgnore] public int profScore { get { return RawData.profScore; } }
        [YamlIgnore] public int itemScore { get { return RawData.itemScore; } }
        [YamlIgnore] public int tempScore { get { return RawData.tempScore; } }

        [YamlIgnore] public int score { get { return initialScore + ablScore + profScore + itemScore + tempScore; } }
        [YamlIgnore] public int dcScore { get { return 10 + profScore; } }

        public virtual IEnumerable<RuleElement> GetElements()
        {
            return charData.RE_GetBy_Sel(selector);
        }

        public void Refresh()
        {
            IEnumerable<RuleElement> elements = GetElements();

            (string, string) proficiencies = DB.Prof_FindMax(elements, charData.level);
            RawData.prof = proficiencies.Item1;
            RawData.profLvl20 = proficiencies.Item2;
            RawData.profColored = DB.Prof_Abbr2AbbrColored(prof);

            RawData.ablScore = charData.Abl_GetMod(ability);
            RawData.profScore = charData.level + DB.Prof_Abbr2Score(prof);

            int item = 0; elements.Where(a => a.type == "item").ForEach(a => { int value = a.value.ToInt(); item = value > item ? value : item; });
            int circ = 0; elements.Where(a => a.type == "circumstance").ForEach(a => { int value = a.value.ToInt(); circ = value > circ ? value : circ; });
            int stat = 0; elements.Where(a => a.type == "status").ForEach(a => { int value = a.value.ToInt(); stat = value > stat ? value : stat; });
            int untyped = elements.Where(a => a.type == "untyped").Sum(a => a.value.ToInt());

            RawData.itemScore = item;
            RawData.tempScore = circ + stat + untyped;
        }

        public APICData RawData = new APICData();

        public APIC() { }

        public APIC(string selector, Character charData, string abl, int initialScore)
        {
            this.selector = selector;
            this.charData = charData;
            this.ability = abl;
            this.initialScore = initialScore;

            Refresh();
        }
    }


    public class APIC_Lore : APIC
    {
        public APIC_Lore() { }

        public APIC_Lore(string selector, string loreName, Character charData, string abl, int initialScore) : base(selector, charData, abl, initialScore)
        {
            this.selector = selector;
            this.RawData.loreName = loreName;
            this.charData = charData;
            this.ability = abl;
            this.initialScore = initialScore;
        }

        public override IEnumerable<RuleElement> GetElements()
        {
            return charData.elements.Where(x => x.selector == selector && x.value == RawData.loreName);
        }
    }


    public class APIC_Skill : APIC
    {
        public APIC_Skill() { }

        public APIC_Skill(string selector, Character charData, string abl, int initialScore) : base(selector, charData, abl, initialScore)
        {
            this.selector = selector;
            this.charData = charData;
            this.ability = abl;
            this.initialScore = initialScore;
        }

        public override IEnumerable<RuleElement> GetElements()
        {
            return charData.elements_skills.Where(x => x.selector == selector);
        }
    }

}
