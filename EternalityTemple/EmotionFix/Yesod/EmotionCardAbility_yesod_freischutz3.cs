using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Yesod
{
    public class EmotionCardAbility_yesod_freischutz3 : EmotionCardAbilityBase
    {
        private string _PREFAB_PATH = "Battle/DiceAttackEffects/CreatureBattle/EGO_Freischutz_6thBullet";
        private bool _effect;
        private bool trigger;

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            trigger = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                if(alive.bufListDetail.GetActivatedBufList().Find(x => x is Flame)==null)
                    alive.bufListDetail.AddBuf(new Flame());
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (trigger && _owner.faction == Faction.Enemy)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                if (alive.bufListDetail.GetActivatedBufList().Find(x => x is Flame) == null)
                    alive.bufListDetail.AddBuf(new Flame());
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            trigger = true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (trigger && _owner.faction == Faction.Enemy)
                return;
            if (!_effect)
            {
                _effect = true;
                Util.LoadPrefab(_PREFAB_PATH).GetComponent<FarAreaEffect_EGO_Freischutz_6thBullet>().Init(_owner);
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
                if (aliveList.Count > 0)
                    Util.LoadPrefab(_PREFAB_PATH).GetComponent<FarAreaEffect_EGO_Freischutz_6thBullet>().Init(aliveList[0]);
            }
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 3, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2, _owner);
        }
        public class Flame : BattleUnitBuf
        {
            public override string keywordIconId => "Matan_Flame";
            public override string keywordId => "EF_Flame_Eternal";
            public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
            {
                if (base.GetResistHP(origin, detail) == AtkResist.Endure)
                    return AtkResist.Vulnerable;
                return base.GetResistHP(origin, detail) == AtkResist.Resist ? AtkResist.Weak : base.GetResistHP(origin, detail);
            }

            public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
            {
                if (base.GetResistBP(origin, detail) == AtkResist.Endure)
                    return AtkResist.Vulnerable;
                return base.GetResistBP(origin, detail) == AtkResist.Resist ? AtkResist.Weak : base.GetResistBP(origin, detail);
            }
        }
    }
}
