using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Yesod
{
    public class EmotionCardAbility_yesod_murderer1 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            BattleUnitModel target = behavior.card?.target;
            if (target == null)
                return;
            BattleUnitBuf Debuff = target.bufListDetail.GetActivatedBufList().Find((x => x is HitDebuff));
            if (Debuff == null)
                target.bufListDetail.AddBuf(new HitDebuff());
            else
                Debuff.stack += 1;
            target.battleCardResultLog?.SetCreatureAbilityEffect("2/AbandonedMurder_Hit", 1f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Abandoned_Atk");
        }
        public class HitDebuff : BattleUnitBuf
        {
            public override string keywordIconId => "Weak";
            public override string keywordId => "EF_HitDebuff_Eternal";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 1;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (!IsAttackDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmgRate = -50,
                    breakRate = -50
                });
                stack -= 1;
                if (stack <= 0)
                    Destroy();
            }
        }
    }
}
