using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix.Geburah
{
    public class EmotionCardAbility_geburah_nothing2 : EmotionCardAbilityBase
    {
        private bool _triggered;

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _triggered = false;
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            _triggered = false;
        }

        public override void CheckDmg(int dmg, BattleUnitModel target)
        {
            base.CheckDmg(dmg, target);
            if (_triggered)
                return;
            _triggered = true;
            target?.battleCardResultLog?.SetCreatureAbilityEffect("6/Nothing_Gun_Effect", 3f);
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_HelloHeal", 2f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/NothingThere_Hello");
            _owner.RecoverHP(dmg);
        }

        public override void AfterDiceAction(BattleDiceBehavior behavior)
        {
            base.AfterDiceAction(behavior);
            _triggered = true;
        }
    }
}
