using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EternalityEmotion
{
    public class EmotionCardAbility_binah_bossbird2 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            _owner.bufListDetail.AddBuf(new Bigbird());
        }
        public void Effect()
        {
            if (Singleton<StageController>.Instance.IsLogState())
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_See", 2f);
            else
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_See", 1f, _owner.view, _owner.view, 2f);
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBuf(new Bigbird());
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            Effect();
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is Bigbird);
            if (Buff != null)
                Buff.Destroy();
            BattleManagerUI.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(_owner, _owner.faction, _owner.hp, _owner.breakDetail.breakGauge);
        }
        public class Bigbird: BattleUnitBuf
        {
            public override string keywordIconId => "ApocalypseBird_BigEye";
            public override string keywordId => "EF_Bigbird";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction)).bufListDetail.AddBuf(new HalfPower());
            }
        }
        public class HalfPower : BattleUnitBuf
        {
            public override KeywordBuf bufType => KeywordBuf.HalfPower;

            public override string keywordId => "EF_HalfPower";

            public override string keywordIconId => "NullifyPower";

            public override void OnRoundEnd() => Destroy();

            public override bool IsHalfPower() => !_owner.IsImmune(bufType) || base.IsHalfPower();
        }
    }
}
