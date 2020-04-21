using System.Collections.Generic;
using UnityEngine;

public abstract class PF2E_EffectResolver
{
    public static List<PF2E_Fact> Resolve(ref PF2E_PlayerData playerData, string effectName)
    {
        List<PF2E_Fact> result = new List<PF2E_Fact>();

        switch (effectName)
        {
            case "Forged Dwarf":
                ForgeDwarf(ref playerData, ref result);
                break;
            case "Strong-Blooded Dwarf":
                StrongBloodedDwarf(ref playerData, ref result);
                break;
            case "Artic Elf":
                ArticElf(ref playerData, ref result);
                break;

            default:
                Debug.LogWarning("[EffectResolver] Effect (" + effectName + ") not found!");
                Default(ref playerData, ref result);
                break;
        }

        return result;
    }

    private static List<PF2E_Fact> Default(ref PF2E_PlayerData playerData, ref List<PF2E_Fact> result)
    {
        return result;
    }

    private static void ForgeDwarf(ref PF2E_PlayerData playerData, ref List<PF2E_Fact> result)
    {
        int fireResCalc = Mathf.Clamp(Mathf.FloorToInt(playerData.level / 2), 1, 99);
        PF2E_Fact fireRes = new PF2E_Fact("Forged Dwarf", "", fireResCalc, E_PF2E_EffectTarget.ResistanceFire,
            E_PF2E_EffectType.Status);
        result.Add(fireRes);
    }

    private static void StrongBloodedDwarf(ref PF2E_PlayerData playerData, ref List<PF2E_Fact> result)
    {
        int poisonResCalc = Mathf.Clamp(Mathf.FloorToInt(playerData.level / 2), 1, 99);
        PF2E_Fact poisonRes = new PF2E_Fact("Strong-Blooded Dwarf", "", poisonResCalc, E_PF2E_EffectTarget.ResistanceFire,
            E_PF2E_EffectType.Status);
        result.Add(poisonRes);
    }

    private static void ArticElf(ref PF2E_PlayerData playerData, ref List<PF2E_Fact> result)
    {
        int coldResCalc = Mathf.Clamp(Mathf.FloorToInt(playerData.level / 2), 1, 99);
        PF2E_Fact poisonRes = new PF2E_Fact("Artic Elf", "", coldResCalc, E_PF2E_EffectTarget.ResistanceCold,
            E_PF2E_EffectType.Status);
        result.Add(poisonRes);
    }

}
