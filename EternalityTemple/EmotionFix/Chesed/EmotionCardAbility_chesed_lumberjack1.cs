using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_chesed_lumberjack1 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_Lumberjack_emotion());
        }

        public class BattleUnitBuf_Lumberjack_emotion : BattleUnitBuf
        {
            private SoundEffectPlayer sound;
            private GameObject aura;
            private bool trigger;
            public override string keywordId => "EF_Lumberjack";

            public override string keywordIconId => "Lumberjack_Heart";
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                trigger = card.target == null || card.target.cardSlotDetail.PlayPoint - card.target.cardSlotDetail.ReservedPlayPoint>2;
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!IsAttackDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = RandomUtil.Range(1, 2)
                });
            }

            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Mind", 1f, owner.view, owner.view)?.gameObject;
                sound = SoundEffectManager.Instance.PlayClip("Creature/WoodMachine_Default", true, parent: owner.view.characterRotationCenter.transform);
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }

            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }

            public override void Destroy()
            {
                base.Destroy();
                DestroyAura();
            }

            private void DestroyAura()
            {
                if (aura != null)
                {
                    UnityEngine.Object.Destroy(aura);
                    aura = null;
                }
                if (sound == null)
                    return;
                sound.ManualDestroy();
                sound = null;
            }
        }
    }
}
