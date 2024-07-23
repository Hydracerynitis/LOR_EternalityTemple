using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix.Binah
{
    public class EmotionCardAbility_binah_bossbird1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.bufListDetail.AddBuf(new Longbird());
        }
        public override void OnWaveStart()
        {
            _owner.bufListDetail.AddBuf(new Longbird());
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is Longbird);
            if (Buff != null)
                Buff.Destroy();
        }
        public class Longbird: BattleUnitBuf
        {
            public override string keywordIconId => "ApocalypseBird_LongArm";
            public override string keywordId => "EF_Longbird";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
                _owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
            }
            public override bool IsImmune(BattleUnitBuf buf)
            {
                return buf.positiveType == BufPositiveType.Negative || base.IsImmune(buf);
            }
        }
    }
}
