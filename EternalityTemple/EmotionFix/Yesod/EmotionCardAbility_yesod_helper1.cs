using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using Sound;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_yesod_helper1: EmotionCardAbilityBase
    {
        private GameObject _aura;
        public override void OnRoundStart_after()
        {
            base.OnRoundStart_after();
            if (_owner.cardSlotDetail.PlayPoint < _owner.cardSlotDetail.GetMaxPlayPoint())
                return;
            if (_aura != null)
                DestroyAura();
            _aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Charge", 1f, _owner.view, _owner.view)?.gameObject;
            SoundEffectPlayer.PlaySound("Creature/Helper_FullCharge");
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_helper.BattleUnitBuf_Emotion_Helper_Charge());
        }

        public override void OnRoundEnd()
        {
            DestroyAura();
        }

        public override void OnDie(BattleUnitModel killer)
        {
            DestroyAura();
        }

        public void DestroyAura()
        {
            if (_aura == null)
                return;
            UnityEngine.Object.Destroy(_aura);
            _aura = null;
        }

        public class BattleUnitBuf_Emotion_Helper_Charge : BattleUnitBuf
        {
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, RandomUtil.Range(1, 2), owner);
            }
            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                if (behavior == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = RandomUtil.Range(2, 7)
                });
            }
            public override void OnRoundEnd()
            {
                Destroy();
            }
        }
    }
}
