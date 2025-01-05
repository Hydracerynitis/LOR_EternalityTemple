using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_chesed_waybackhome2 : EmotionCardAbilityBase
    {
        private int stack = 1;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.bufListDetail.AddBuf(new Home());
            
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBuf(new Home());
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            BattleUnitModel target = curCard?.target;
            if (target == null)
                return;
            if (CheckAbility(target))
            {
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                {
                    dmg = 3 * stack
                });
                ++stack;
                _owner.battleCardResultLog?.SetCreatureAbilityEffect("7/WayBeckHome_Emotion_Atk", 1f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_NormalAtk");
            }
            else
                stack = 0;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            stack = 1;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
            double num1 = Math.Ceiling(aliveList.Count * 0.5);
            int num2 = 0;
            while (aliveList.Count > 0 && num1 > 0.0)
            {
                BattleUnitModel battleUnitModel = RandomUtil.SelectOne(aliveList);
                if (battleUnitModel != null)
                {
                    ++num2;
                    WayBackHome wayBackHomeTarget = new WayBackHome(num2);
                    battleUnitModel.bufListDetail.AddBuf(wayBackHomeTarget);
                    --num1;
                }
                aliveList.Remove(battleUnitModel);
            }
        }
        private bool CheckAbility(BattleUnitModel target)
        {
            BattleUnitBuf battleUnitBuf = target.bufListDetail.GetActivatedBufList().Find(x => x is WayBackHome);
            return battleUnitBuf != null && battleUnitBuf.stack == stack;
        }
        public class Home : BattleUnitBuf
        {
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
            {
                if (idx >= 3)
                    return base.ChangeAttackTarget(card, idx);
                foreach (BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
                {
                    if (enemy.bufListDetail.GetActivatedBufList().Find(x => x is WayBackHome) is WayBackHome goldbrick)
                    {
                        if (goldbrick.stack - 1 == idx)
                            return enemy;
                    }
                }
                return base.ChangeAttackTarget(card, idx);
            }
        }
        public class WayBackHome : BattleUnitBuf
        {
            private GameObject aura;
            public override string keywordId => "EF_WayBackHome";
            public override string keywordIconId => "WayBackHome_Target";
            public WayBackHome(int value) => stack = value;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("7/WayBeckHome_Emotion_Way", 1f, owner.view, owner.view)?.gameObject;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
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
