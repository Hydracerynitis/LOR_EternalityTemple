using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using EternalityTemple;
using Sound;
using Debuff = EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil_Debuf;

namespace EternalityEmotion
{
    public class EmotionCardAbility_tiphereth_clownofnihil3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SoundEffectPlayer.PlaySound("Creature/Nihil_StrongAtk");
            GiveBuf();
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            GiveBuf();
        }
        private void GiveBuf()
        {
            if (Helper.SearchEmotion(_owner, "QueenOfHatred_Snake_Enemy") == null || Helper.SearchEmotion(_owner, "KnightOfDespair_Despair_Enemy") == null || Helper.SearchEmotion(_owner, "Greed_Eat_Enemy") == null || Helper.SearchEmotion(_owner, "Angry_Angry_Enemy") == null || _owner.bufListDetail.GetActivatedBufList().Find(x => x is Nihil) != null)
                return;
            _owner.bufListDetail.AddBuf(new Nihil());
        }
        public class Nihil : BattleUnitBuf
        {
            private bool _effect=false;
            public override string keywordId => "EF_Nihil";
            public override string keywordIconId => "Fusion";
            public Nihil() => stack = 0;
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (alive != _owner)
                    {
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 3, _owner);
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Disarm, 3, _owner);
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, 3, _owner);
                        if(alive.bufListDetail.GetActivatedBufList().Find(x => x is Debuff) ==null)
                            alive.bufListDetail.AddBuf(new Debuff());
                    }
                }
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (_effect)
                    return;
                DiceEffectManager.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Joker_Start", 1f, _owner.view, _owner.view);
                _effect = true;
            }
        }
    }
}
