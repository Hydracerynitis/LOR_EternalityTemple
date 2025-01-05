using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace EternalityEmotion
{
    public class EmotionCardAbility_hod_blackswan1 : EmotionCardAbilityBase
    {
        private GameObject aura;
        private List<KeywordBuf> ActivatedBuf=new List<KeywordBuf>();
        private KeywordBuf[] debuff => new KeywordBuf[]
        {
            KeywordBuf.Paralysis,
            KeywordBuf.Vulnerable,
            KeywordBuf.Weak,
            KeywordBuf.Disarm,
            KeywordBuf.Binding
        };
        public override void OnSelectEmotion()
        {
            SoundEffectPlayer.PlaySound("Creature/Shark_Ocean");
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (ActivatedBuf.Count <= 0)
                return;
            KeywordBuf buff = RandomUtil.SelectOne(ActivatedBuf);
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
                unit.bufListDetail.AddKeywordBufByEtc(buff, 1);
            behavior?.card?.target?.battleCardResultLog?.SetNewCreatureAbilityEffect("3_H/FX_IllusionCard_3_H_Dertyfeather", 2f);
        }
        public override void OnRoundStart()
        {
            for (int i=0; i<3;i++)
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(RandomUtil.SelectOne(debuff),1);
            aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("3_H/FX_IllusionCard_3_H_Dertyfeather_Loop", 1f, _owner.view, _owner.view)?.gameObject;
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            ActivatedBuf.Clear();
            foreach(KeywordBuf buftype in debuff)
            {
                if (_owner.bufListDetail.GetActivatedBuf(buftype) != null)
                    ActivatedBuf.Add(buftype);
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyAura();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public void DestroyAura()
        {
            if (aura == null)
                return;
            UnityEngine.Object.Destroy(aura.gameObject);
            aura = null;
        }
    }
}
