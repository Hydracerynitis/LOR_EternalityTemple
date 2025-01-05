using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_redhood3 : EmotionCardAbilityBase
    {
        private int Reduce;
        private int strcount;
        private Battle.CreatureEffect.CreatureEffect aura;
        private string path = "6/RedHood_Emotion_Aura";
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            Reduce = _owner.MaxHp - (int)_owner.hp;
            int reduce = Reduce;
            int Threshold= (int)(_owner.MaxHp * 0.2);
            _owner.bufListDetail.AddBuf(new Scar(Reduce));
            for (strcount=0 ; reduce > Threshold; reduce -= Threshold)
                strcount += 1;
            SoundEffectManager.Instance.PlayClip("Creature/RedHood_Change");
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBuf(new Scar(Reduce));
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyAura();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, strcount, _owner);
            if (aura != null)
                return;
            aura = MakeEffect(path, target: _owner, apply: false);
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public void Destroy()
        {
            DestroyAura();
            BattleUnitBuf buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is Scar);
            if (buff != null)
                buff.Destroy();
        }
        private void DestroyAura()
        {
            if (aura != null && aura.gameObject != null)
                UnityEngine.Object.Destroy(aura.gameObject);
            aura = null;
        }
        public class Scar : BattleUnitBuf
        {
            private int Reduce;
            public Scar(int Reduce)
            {
                this.Reduce = Reduce;
            }
            public override StatBonus GetStatBonus()
            {
                return new StatBonus() 
                { 
                    hpAdder=-Reduce
                };
            }
        }
    }
}
