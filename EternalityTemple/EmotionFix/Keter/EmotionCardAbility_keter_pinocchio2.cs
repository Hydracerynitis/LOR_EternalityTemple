using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_keter_pinocchio2 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SoundEffectPlayer.PlaySound("Creature/Pino_Lie");
            SetFilter("0/Pinocchio_Emotion_Select");
        }

        public override void OnRoundStart()
        {
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction)).bufListDetail.AddBuf(new LieIndicator());
        }
        public class LieIndicator: BattleUnitBuf
        {
            public override string keywordId => "EF_Lie";
            public override string keywordIconId => "KeterFinal_ChangeCostAll";

            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                foreach (BattleDiceCardModel battleDiceCardModel in _owner.allyCardDetail.GetAllDeck())
                {
                    if (battleDiceCardModel.GetOriginCost() <= 3)
                        battleDiceCardModel.AddBufWithoutDuplication(new RandomCostBuf());
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
        public class RandomCostBuf : BattleDiceCardBuf
        {
            private int _cost;

            public RandomCostBuf() => _cost = RandomUtil.Range(0, 3);

            public override int GetCost(int oldCost) => _cost;
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
