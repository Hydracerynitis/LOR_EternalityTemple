using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Prey = EmotionCardAbility_redhood1.BattleUnitBuf_redhood_prey;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_redhood1 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if ((_target != null && !_target.IsDead()) || curCard.target.faction == _owner.faction || curCard.GetDiceBehaviorList().Find(x => x.Type == BehaviourType.Atk) == null)
                return;
            _target = curCard.target;
            _target.bufListDetail.AddBuf(new Prey());
            _target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Hunted", 1.5f);
            _target.battleCardResultLog?.SetCreatureEffectSound("Creature/RedHood_Gun");
        }
        public override void OnWaveStart()
        {
            _target=null;
        }
        public override void OnSelectEmotion()
        {
           _target=null;
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            BattleUnitModel target = behavior.card?.target;
            if (target == null || target != _target)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = RandomUtil.Range(2, 6)
            });
        }
        public void Destroy()
        {
            if (_target.bufListDetail.GetActivatedBufList().Find(x => x is Prey) is Prey prey)
                prey.Destroy();
        }
    }
}
