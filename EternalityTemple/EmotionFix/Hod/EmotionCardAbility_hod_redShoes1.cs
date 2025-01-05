using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_hod_redShoes1 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            if (!(_owner.bufListDetail.GetActivatedBufList().Exists(x => x is ShinyShoes)))
            {
                _owner.bufListDetail.AddBuf(new ShinyShoes());
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_On")?.SetGlobalPosition(_owner.view.WorldPosition);
            }
        }
        public override void OnSelectEmotion()
        {
            _owner.bufListDetail.AddBuf(new ShinyShoes());
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_On")?.SetGlobalPosition(_owner.view.WorldPosition);
        }
        public void Destroy()
        {
            if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is ShinyShoes) is ShinyShoes shoe)
                shoe.Destroy();
        }
        public class ShinyShoes: BattleUnitBuf
        {
            public override string keywordId => "EF_ShinyShoes_Eternal";
            public override string keywordIconId => "CopiousBleeding";
            public override int GetDamageIncreaseRate() => 50;
            public override int GetBreakDamageIncreaseRate() => 50;
            public override int GetSpeedDiceAdder(int speedDiceResult)
            {
                return RandomUtil.Range(1,2);
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (!IsAttackDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = RandomUtil.Range(1, 3)
                });
            }
            public override void OnDie()
            {
                BattleUnitModel killer= _owner.lastAttacker;
                if (killer == null)
                    killer = RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction));
                killer.bufListDetail.AddBuf(new ShinyShoes());
            }
        }
    }
}
