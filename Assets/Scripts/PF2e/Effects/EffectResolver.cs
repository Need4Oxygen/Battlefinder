// using System.Collections.Generic;
// using UnityEngine;

// public abstract class PF2E_EffectResolver
// {
//     public static List<PF2E_Effect> Resolve(PF2E_PlayerData playerData, string effectName)
//     {
//         List<PF2E_Effect> result = new List<PF2E_Effect>();

//         switch (effectName)
//         {
//             case "Forged Dwarf":
//                 ForgeDwarf(playerData, result);
//                 break;
//             case "Strong-Blooded Dwarf":
//                 StrongBloodedDwarf(playerData, result);
//                 break;
//             case "Artic Elf":
//                 ArticElf(playerData, result);
//                 break;

//             default:
//                 Debug.LogWarning("[EffectResolver] Effect (" + effectName + ") not found!");
//                 Default(playerData, result);
//                 break;
//         }

//         return result;
//     }

//     private static List<PF2E_Effect> Default(PF2E_PlayerData playerData, List<PF2E_Effect> result)
//     {
//         return result;
//     }

//     private static void ForgeDwarf(PF2E_PlayerData playerData, List<PF2E_Effect> result)
//     {
//         int fireResCalc = Mathf.Clamp(Mathf.FloorToInt(playerData.level / 2), 1, 99);
//         PF2E_Effect fireRes = new PF2E_Effect("Forged Dwarf", "", fireResCalc, E_PF2E_EffectTarget.ResistanceFire,
//             E_PF2E_EffectType.Status);
//         result.Add(fireRes);
//     }

//     private static void StrongBloodedDwarf(PF2E_PlayerData playerData, List<PF2E_Effect> result)
//     {
//         int poisonResCalc = Mathf.Clamp(Mathf.FloorToInt(playerData.level / 2), 1, 99);
//         PF2E_Effect poisonRes = new PF2E_Effect("Strong-Blooded Dwarf", "", poisonResCalc, E_PF2E_EffectTarget.ResistanceFire,
//             E_PF2E_EffectType.Status);
//         result.Add(poisonRes);
//     }

//     private static void ArticElf(PF2E_PlayerData playerData, List<PF2E_Effect> result)
//     {
//         int coldResCalc = Mathf.Clamp(Mathf.FloorToInt(playerData.level / 2), 1, 99);
//         PF2E_Effect poisonRes = new PF2E_Effect("Artic Elf", "", coldResCalc, E_PF2E_EffectTarget.ResistanceCold,
//             E_PF2E_EffectType.Status);
//         result.Add(poisonRes);
//     }

// }
