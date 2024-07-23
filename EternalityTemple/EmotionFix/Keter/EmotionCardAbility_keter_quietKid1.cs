using BaseMod;
using Battle.CreatureEffect;
using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix.Keter
{
    public class EmotionCardAbility_keter_quietKid1: EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            behavior.card.target?.bufListDetail.AddBufByEtc<Nail>(1);
            behavior.card.target?.battleCardResultLog?.SetCreatureEffectSound("Creature/Slientgirl_Volt");
        }

        public class Nail : BattleUnitBuf
        {
            private int nailCnt;

            public override string keywordId => "EF_Nail";

            public override string keywordIconId => "KeterFinal_SilenceGirl_Nail";

            public override KeywordBuf bufType => KeywordBuf.Nail;

            public override void OnAddBuf(int addedStack)
            {
                base.OnAddBuf(addedStack);
                if (Singleton<StageController>.Instance.IsLogState())
                {
                    for (int index = 0; index < addedStack && nailCnt < 20; ++index)
                    {
                        ++nailCnt;
                        _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_Volt");
                    }
                }
                else
                {
                    for (int index = 0; index < addedStack && nailCnt < 20; ++index)
                    {
                        ++nailCnt;
                        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_Volt", 1f, _owner.view, _owner.view);
                    }
                }
            }

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                if (atkDice.card.owner == null || Helper.SearchEmotion(atkDice.card.owner, "QuietKid_Hammer_Enemy") != null)
                    return;
                if (stack > 0)
                {
                    int num = (stack - 1) / 5 + 1;
                    if (Singleton<StageController>.Instance.IsLogState())
                    {
                        _owner.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(RemoveAllNail));
                        for (int index = 0; index < num; ++index)
                            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_HammerATK");
                        _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Slientgirl_Hammer");
                    }
                    else
                    {
                        RemoveAllNail();
                        for (int index = 0; index < num; ++index)
                            SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_HammerATK", 1f, _owner.view, _owner.view);
                        SoundEffectPlayer.PlaySound("Creature/Slientgirl_Hammer");
                    }
                    _owner.TakeDamage(stack * 5);
                    stack = 0;
                }
                Destroy();
            }

            public void RemoveAllNail()
            {
                CreatureEffect_Emotion_Nail[] componentsInChildren = _owner.view.GetComponentsInChildren<CreatureEffect_Emotion_Nail>();
                for (int index = componentsInChildren.Length - 1; index >= 0; --index)
                    UnityEngine.Object.Destroy(componentsInChildren[index].gameObject);
            }
        }
    }
}
