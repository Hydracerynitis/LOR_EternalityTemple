using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Kaguya
{
    public class PassiveAbility_226769002: PassiveAbilityBase, IsBufImmune
    {
        private bool ImmuneActive =false;

        public bool IsImmune(BattleUnitBuf buf)
        {
            return buf.positiveType==BufPositiveType.Negative && ImmuneActive;
        }

        public override void OnRollSpeedDice()
        {
            foreach (Dice dice in this.owner.speedDiceResult.FindAll(x => x.value >=4))
                dice.value = 99;
            foreach (Dice dice in owner.speedDiceResult.FindAll(x => x.value <= 3 && !x.breaked))
                dice.value = 1;
            this.owner.speedDiceResult.Sort((d1, d2) =>
            {
                if (d1.breaked && d2.breaked)
                {
                    if (d1.value > d2.value)
                        return -1;
                    return d1.value < d2.value ? 1 : 0;
                }
                if (d1.breaked && !d2.breaked)
                    return -1;
                if (!d1.breaked && d2.breaked)
                    return 1;
                if (d1.value > d2.value)
                    return -1;
                return d1.value < d2.value ? 1 : 0;
            });
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            ImmuneActive = false;
        }
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            ImmuneActive = checkIsImmune(attackerCard.targetSlotOrder);
        }
        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            ImmuneActive = checkIsImmune(owner.currentDiceAction.slotOrder);
        }
        public override void OnStartTargetedByAreaAtk(BattlePlayingCardDataInUnitModel attackerCard)
        {
            if (attackerCard.target == owner)
                ImmuneActive = checkIsImmune(attackerCard.targetSlotOrder);
        }
        private bool checkIsImmune(int slotOrder)
        {
            if (slotOrder >= 0 && slotOrder < owner.speedDiceCount && owner.speedDiceResult[slotOrder].value == 1)
                return true;
            else
                return false;
        }
        public override void OnEndParrying()
        {
            base.OnEndParrying();
            ImmuneActive = false;
        }
        public override void OnEndOneSideVictim(BattlePlayingCardDataInUnitModel attackerCard)
        {
            base.OnEndOneSideVictim(attackerCard);
            ImmuneActive = false;
        }
    }
}
