using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_tiphereth_clownofnihil1 : EmotionCardAbilityBase
    {
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            base.OnLoseParrying(behavior);
            if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is Void) != null)
                return;
            if (!(_owner.bufListDetail.GetActivatedBufList().Find(x => x is VoidReady) is VoidReady emotionVoidReady))
            {
                emotionVoidReady = new VoidReady();
                _owner.bufListDetail.AddBuf(emotionVoidReady);
            }
            emotionVoidReady.Add();
        }
        public class VoidReady : BattleUnitBuf
        {
            public int StackMax = 25;

            public override string keywordIconId => "CardBuf_NihilClown_Card";

            public override string keywordId => "Buf_NihilClown_Card";

            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                if (stack < StackMax)
                    return;
                _owner.bufListDetail.AddBuf(new Void());
                Destroy();
            }
            public void Add()
            {
                ++stack;
                if (stack <= StackMax)
                    return;
                stack = StackMax;
            }
        }
        public class Void : BattleUnitBuf
        {
            private GameObject aura;
            private int cnt;

            public override string keywordIconId => "CardBuf_NihilClown_Card";

            public override string keywordId => "NihilClown_Card";

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                cnt = 0;
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 5, _owner);
            }
            public override bool IsImmune(BattleUnitBuf buf) => buf.positiveType == BufPositiveType.Negative || base.IsImmune(buf);
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
                owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_MagicGirl", 1f, owner.view, owner.view)?.gameObject;
                SoundEffectPlayer.PlaySound("Creature/Nihil_Filter");
            }
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel owner = atkDice?.owner;
                if (owner == null || cnt >= 2)
                    return;
                ++cnt;
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 1, _owner);
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, _owner);
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
                if (aura == null)
                    return;
                UnityEngine.Object.Destroy(aura);
                aura = null;
            }
        }
    }
}
