using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using BaseMod;

namespace EmotionalFix.Tiphereth
{
    public class EmotionCardAbility_tiphereth_kingofgreed2 : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect aura;
        private int count;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false, 2f);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (count > 0)
            {
                aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Happiness", 1f, _owner.view, _owner.view);
                SoundEffectPlayer.PlaySound("Creature/Greed_MakeDiamond");
            }       
            count = 0;
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyAura();
            for(int i=count;i>0 ;i--)
            {
                List<KeywordBuf> keywords = GetAvailableBuf();
                if (keywords.Count <= 0)
                    return;
                 _owner.bufListDetail.AddKeywordBufByEtc(RandomUtil.SelectOne(keywords), 1);
            }
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            ++count;
        }
        public void DestroyAura()
        {
            if (aura == null)
                return;
            UnityEngine.Object.Destroy(aura.gameObject);
            aura = null;
        }

        public List<KeywordBuf> GetAvailableBuf()
        {
            List<KeywordBuf> output=new List<KeywordBuf>();
            BattleUnitBuf_strength strength = _owner.bufListDetail.FindBuf<BattleUnitBuf_strength>(BufReadyType.NextRound);
            if (strength==null || strength.stack<3)
                output.Add(KeywordBuf.Strength);
            BattleUnitBuf_endurance endurance = _owner.bufListDetail.FindBuf<BattleUnitBuf_endurance>(BufReadyType.NextRound);
            if (endurance==null || endurance.stack < 3)
                output.Add(KeywordBuf.Endurance);
            BattleUnitBuf_quickness quickness = _owner.bufListDetail.FindBuf<BattleUnitBuf_quickness>(BufReadyType.NextRound);
            if (quickness==null || quickness.stack < 3)
                output.Add(KeywordBuf.Quickness);
            return output;
        }
    }
}
