using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using EI = EternalityTemple.EternalityInitializer;
using UI;

namespace EternalityTemple
{
    [HarmonyPatch]
    static class ExtraArtworkPatch
    {
        [HarmonyPatch(typeof(UISpriteDataManager),nameof(UISpriteDataManager.GetStoryIcon))]
        [HarmonyPostfix]
        public static void StoryIcon(string story, ref UIIconManager.IconSet __result, UISpriteDataManager __instance)
        {
            if (__result.type == "None")
            {
                if(EI.ArtWorks.TryGetValue(story,out Sprite image))
                {
                    __result = new UIIconManager.IconSet()
                    {
                        type = story,
                        icon = image,
                        iconGlow = image
                    };
                }
            }
        }
    }
}
