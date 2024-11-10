using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EI = EternalityTemple.EternalityInitializer;
using UnityEngine;
using LOR_XML;
using HarmonyLib;
using Mod;
using LOR_DiceSystem;
using System.Reflection.Emit;
using System.Reflection;

namespace EternalityTemple
{
    [HarmonyPatch]
    static class AbnormalityLoader
    {
        [HarmonyPatch(typeof(BattleEmotionCardModel), MethodType.Constructor, new System.Type[] { typeof(EmotionCardXmlInfo), typeof(BattleUnitModel) })]
        [HarmonyPriority(200)]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> BattleEmotionCardModel_ctor_In(
      IEnumerable<CodeInstruction> instructions,
      ILGenerator ilgen)
        {
            List<CodeInstruction> codes = instructions.ToList<CodeInstruction>();
            Label trueJumpLabel = ilgen.DefineLabel();
            Label falseJumpLabel = ilgen.DefineLabel();
            MethodInfo moveNextMethod = AccessTools.Method(typeof(List<string>.Enumerator), "MoveNext");
            MethodInfo createInstanceMethod = AccessTools.Method(typeof(Activator), "CreateInstance", new System.Type[1]
            {
        typeof (System.Type)
            });
            MethodInfo getTypeMethod = AccessTools.Method(typeof(System.Type), "GetType", new System.Type[1]
            {
        typeof (string)
            });
            for (int i = 0; i < codes.Count; ++i)
            {
                int num;
                if ((codes[i].opcode == OpCodes.Ldloca || codes[i].opcode == OpCodes.Ldloca_S) && TryGetIntValue(codes[i].operand, out num) && num == 0)
                {
                    if (i < codes.Count - 1 && CodeInstructionExtensions.Is(codes[i + 1], OpCodes.Call, moveNextMethod))
                        yield return new CodeInstruction(OpCodes.Nop).WithLabels(falseJumpLabel);
                }
                else if (CodeInstructionExtensions.Is(codes[i], OpCodes.Call, createInstanceMethod))
                {
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Brtrue, (object)trueJumpLabel);
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Br, (object)falseJumpLabel);
                    yield return new CodeInstruction(OpCodes.Nop).WithLabels(trueJumpLabel);
                }
                yield return codes[i];
                if (CodeInstructionExtensions.Is(codes[i], OpCodes.Call, getTypeMethod))
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Call, (object)AccessTools.Method(typeof(AbnormalityLoader), "BattleEmotionCardModel_ctor_CheckCustomAbility"));
                }
            }
        }
        internal static bool TryGetIntValue(object operand, out int value)
        {
            switch (operand)
            {
                case IConvertible convertible:
                    try
                    {
                        value = convertible.ToInt32((IFormatProvider)null);
                        return true;
                    }
                    catch
                    {
                        break;
                    }
                case LocalBuilder localBuilder:
                    value = localBuilder.LocalIndex;
                    return true;
            }
            value = 0;
            return false;
        }
        private static System.Type BattleEmotionCardModel_ctor_CheckCustomAbility(
      System.Type oldType,
      string name)
        {
            System.Type emotionCardAbilityType = FindEmotionCardAbilityType(name.Trim());
            return (object)emotionCardAbilityType != null ? emotionCardAbilityType : oldType;
        }

        public static System.Type FindEmotionCardAbilityType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = (string)null;
            }
            System.Type ability = System.Type.GetType("EmotionalFix.EmotionCardAbility_" + name.Trim());
            if (ability != null)
            {
                Debug.Log("Eternality Abnormality: " + "EmotionalFix.EmotionCardAbility_" + name.Trim() + " Found");
                return ability;
            }
                
            else
            {
                Debug.Log("Eternality Abnormality: " + "EmotionalFix.EmotionCardAbility_" + name.Trim() + " not Found");
                List<System.Type> types = Assembly.Load("Assembly-CSharp").GetTypes().ToList();
                ability = types.Find(t => t.Name == "EmotionCardAbility_" + name.Trim());
                Debug.Log("Replace Abnormality: " + (ability != null? ability.Name : "null"));
                return ability;
            }
        }
        public static void LoadAbnormality(string language)
        {
            if (EI.BMexist)
                return;
            Dictionary<string, AbnormalityCard> dict = Singleton<AbnormalityCardDescXmlList>.Instance._dictionary;
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/AbnormalityCards/");
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath + "/Localization/" + language + "/AbnormalityCards/").GetFiles())
            {
                try
                {
                    using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                    {
                        foreach (Sephirah sephirah in ((AbnormalityCardsRoot)new XmlSerializer(typeof(AbnormalityCardsRoot)).Deserialize(stringReader)).sephirahList)
                        {
                            foreach (AbnormalityCard abnormalityCard in sephirah.list)
                                dict[abnormalityCard.id] = abnormalityCard;
                        }
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/AbnormalityLTLError_" + file.Name + ".txt", ex.ToString());
                }
            }
        }
        public static void LoadEmotion()
        {
            List<EmotionCardXmlInfo> list = EmotionCardXmlList.Instance._list;
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath + "/EmotionCard/").GetFiles())
            {
                Debug.Log("Eternality: Emotion Card File " + file.FullName);
                try
                {
                    using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                    {
                        EmotionCardXmlRoot root= (EmotionCardXmlRoot)new XmlSerializer(typeof(EmotionCardXmlRoot)).Deserialize(stringReader);
                        list.AddRange(root.emotionCardXmlList);
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/EmotionCardError_" + file.Name + ".txt", ex.ToString());
                }
            }
        }

    }
}

