using System;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix.Malkuth
{
    public class EmotionCardAbility_malkuth_queenbee2 : EmotionCardAbilityBase
    {
        private Dictionary<BattleUnitModel, int> dmgData = new Dictionary<BattleUnitModel, int>();
        public override void OnSelectEmotion()
        {
            BattleUnitBuf attacker = _owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_queenbee_attacker);
            if (attacker == null)
                return;
            attacker.Destroy();
        }
        public override void OnStartBattle()
        {
            if (_owner.faction != Faction.Player)
                return;
            foreach (BattlePlayingCardDataInUnitModel card in _owner.cardSlotDetail.cardAry)
            {
                if (card != null && card.target != null)
                {
                    card.target.bufListDetail.AddBuf(new BattleUnitBuf_queenbee_punish());
                    SoundEffectPlayer.PlaySound("Creature/QueenBee_AtkMode");
                    return;
                }
            }      
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            BattleUnitModel owner = atkDice.owner;
            if (owner == null || owner.faction != Faction.Player)
                return;
            if (!dmgData.ContainsKey(owner))
                dmgData.Add(owner, dmg);
            else
                dmgData[owner] += dmg;
        }
        public override void OnRoundStart()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                if (unit.bufListDetail.GetActivatedBufList().Exists(x => x is BattleUnitBuf_queenbee_attacker))
                    continue;
                if (Helper.SearchEmotion(unit, "QueenBee_Bee_Enemy")!=null || Helper.SearchEmotion(unit, "QueenBee_Bee") != null)
                    continue;
                unit.bufListDetail.AddBuf(new BattleUnitBuf_queenbee_attacker());
            }
            if (dmgData.Count > 0)
            {
                int num = 0;
                BattleUnitModel battleUnitModel = null;
                foreach (KeyValuePair<BattleUnitModel, int> keyValuePair in dmgData)
                {
                    if (keyValuePair.Value > num && !keyValuePair.Key.IsDead())
                    {
                        num = keyValuePair.Value;
                        battleUnitModel = keyValuePair.Key;
                    }
                }
                battleUnitModel?.bufListDetail.AddBuf(new BattleUnitBuf_queenbee_punish());
                SoundEffectPlayer.PlaySound("Creature/QueenBee_AtkMode");
            }
            dmgData.Clear();
        }
        public class BattleUnitBuf_queenbee_punish : BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _aura;
            public override string keywordId => "Queenbee_Punish";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                _aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("1_M/FX_IllusionCard_1_M_BeeMark", 1f, owner.view, owner.view);
            }
            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                DestroyAura();
            }
            public void DestroyAura()
            {
                if (_aura == null)
                    return;
                UnityEngine.Object.Destroy(_aura.gameObject);
                _aura = null;
            }
        }
        public class BattleUnitBuf_queenbee_attacker : BattleUnitBuf
        {
            private static int Dmg => RandomUtil.Range(2, 4);
            public override string keywordId => "Queenbee_Punish";
            public override bool Hide => true;
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_queenbee_punish) == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = Dmg
                });
            }
        }
    }
}
