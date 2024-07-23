using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Keter
{
    public class EmotionCardAbility_keter_pinocchio3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SoundEffectManager.Instance.PlayClip("Creature/Pino_Success")?.SetGlobalPosition(_owner.view.WorldPosition);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            MakeEffect("0/Pinocchio_Curiosity", destroyTime: 3f);
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
            {
                unit.allyCardDetail.ReturnCardInHandToDeck(RandomUtil.SelectOne(_owner.allyCardDetail.GetHand()));
                unit.allyCardDetail.DrawCards(4);
            }
        }
    }
}
