using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

public class PF2E_ArmorData
{
    public List<PF2E_Armor> armor { get; set; }
    public List<string> armorcategory { get; set; }
    public List<PF2E_ArmorGroup> armorgroup { get; set; }
}

public class PF2E_Armor
{
    public int level { get; set; }
    public string name { get; set; }
    public string descr { get; set; }

    public int ac_bonus { get; set; }
    public string bulk { get; set; }
    public int dex_cap { get; set; }

    public int strength { get; set; }
    public int check_penalty { get; set; }

    public string category { get; set; }
    public string group { get; set; }

    public int speed_penalty { get; set; }
    public float price_gp { get; set; }
    public string price_text { get; set; }

    public List<string> traits { get; set; }

    public List<Source> source { get; set; }
}

public class PF2E_ArmorGroup
{
    public string name { get; set; }
    public string descr { get; set; }
    public List<Source> source { get; set; }
}
