using System;
using LOR_DiceSystem;
using System.IO;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Yesod
{
    public class EmotionCardAbility_yesod_butterfly3 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null)
                return;
            double Ratio = (target.MaxHp - target.hp) / target.MaxHp-0.25;
            int stack = (int)(Ratio * 100);
            if (stack <= 0)
                return;
            if (stack >= 50)
                stack = 50;
            if (!behavior.card.card.XmlData.IsFloorEgo())
            {
                if (behavior.card.card.XmlData.Spec.Ranged == CardRange.Near)
                    _owner.battleCardResultLog?.SetCreatureAbilityEffect("2/Butterfly_Emotion_Effect_Spread_Near", 1f);
                else
                    _owner.battleCardResultLog?.SetCreatureAbilityEffect("2/Butterfly_Emotion_Effect_Spread", 1f);
            }
            if (!(target.bufListDetail.GetActivatedBufList().Find(x => x is SealGauge) is SealGauge sealGauge))
            {
                SealGauge SealGauge = new SealGauge();
                target.bufListDetail.AddBuf(SealGauge);
                SealGauge.stack += stack;
            }
            else
            {
                sealGauge.stack += stack;
                if (sealGauge.stack >= 100)
                {
                    if (!(target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Butterfly_Emotion_Seal) is BattleUnitBuf_Butterfly_Emotion_Seal butterflyEmotionSeal))
                    {
                        BattleUnitBuf_Butterfly_Emotion_Seal ButterflyEmotionSeal = new BattleUnitBuf_Butterfly_Emotion_Seal();
                        target.bufListDetail.AddBuf(ButterflyEmotionSeal);
                        ButterflyEmotionSeal.Add();
                    }
                    else
                        butterflyEmotionSeal.Add();
                    sealGauge.Destroy();
                }
            }
        }
        public class SealGauge : BattleUnitBuf
        {
            public override string keywordIconId => "Butterfly_Seal";
            public override string keywordId => "EF_SealGauge_Eternal";
            public override void Init(BattleUnitModel owner)
            {
                stack = 0;
            }
        }
        public class BattleUnitBuf_Butterfly_Emotion_Seal : BattleUnitBuf
        {
            private int addedThisTurn;
            private int deleteThisTurn;
            public override string keywordIconId => "Butterfly_Seal";
            public override string keywordId => "EF_ButterflySeal_Eternal";
            public BattleUnitBuf_Butterfly_Emotion_Seal() => stack = 0;
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                stack -= deleteThisTurn;
                if (stack > 0)
                    return;
                Destroy();
            }
            public override int SpeedDiceBreakedAdder() => stack;
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                deleteThisTurn = addedThisTurn;
                addedThisTurn = 0;
            }
            public void Add()
            {
                stack += 1;
                addedThisTurn += 1;
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Lock");
            }
        }
    }
}
