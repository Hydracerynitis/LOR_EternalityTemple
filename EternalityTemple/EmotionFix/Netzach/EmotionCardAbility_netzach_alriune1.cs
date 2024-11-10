using System;
using LOR_DiceSystem;
using Sound;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_netzach_alriune1 : EmotionCardAbilityBase
    {
        private int _dmgStack;

        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (_owner.IsImmuneDmg())
                return base.BeforeTakeDamage(attacker, dmg);
            if (_owner.passiveDetail.IsInvincible())
                return base.BeforeTakeDamage(attacker, dmg);
            _dmgStack += dmg;
            if (_dmgStack >= _owner.MaxHp * 0.1)
                Ability();
            return base.BeforeTakeDamage(attacker, dmg);
        }

        public void Ability()
        {
            _dmgStack = 0;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
                alive.TakeBreakDamage(RandomUtil.Range(4, 8), DamageType.Emotion, _owner);
            _owner.RecoverHP((int)(_owner.MaxHp*0.05));
            if (Singleton<StageController>.Instance.IsLogState())
            {
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_FlowerPiece", 2f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Ali_FarAtk");
            }
            else
            {
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_FlowerPiece", 1f, _owner.view, _owner.view, 2f);
                SoundEffectPlayer.PlaySound("Creature/Ali_FarAtk");
            }
        }
    }
}
