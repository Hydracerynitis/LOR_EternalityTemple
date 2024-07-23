using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;
using BaseMod;

namespace EmotionalFix.Tiphereth
{
    public class EmotionCardAbility_tiphereth_servantofwrath1 : EmotionCardAbilityBase
    {
        public override void OnWaveStart()
        {
            _owner.bufListDetail.AddBuf(new Berserk());
        }
        public override void OnSelectEmotion()
        {
            _owner.bufListDetail.AddBuf(new Berserk());
        }
        public class Berserk: BattleUnitBuf
        {
            private GameObject aura;
            public override string keywordId => "EF_Berserk";
            public override string keywordIconId => "Wrath_Head";
            public override int SpeedDiceNumAdder() => 1;
            public override void OnRoundEndTheLast()
            {
                int id = 1104401;
                if (Helper.SearchEmotion(_owner, "NihilClown_fusion_Enemy") != null)
                    id = 1104402;
                BattleDiceCardModel card = _owner.allyCardDetail.AddNewCard(Tools.MakeLorId(id));
                card.SetPriorityAdder(1000);
                card.temporary = true;
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Rage", 1f, owner.view, owner.view)?.gameObject;
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
            public void DestroyAura()
            {
                if (aura == null)
                    return;
                UnityEngine.Object.Destroy(aura);
                aura = null;
            }
        }
    }
}
