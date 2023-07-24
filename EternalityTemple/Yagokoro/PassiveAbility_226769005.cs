using EternalityTemple.Kaguya;
using HyperCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Yagokoro
{
    public class PassiveAbility_226769005: PassiveAbilityBase
    {
        private bool IsActivate=false;
        private int moonStack;
        public override void OnRoundEndTheLast()
        {
            if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_KaguyaBuf7) != null)
            { 
            IsActivate = true;
            }
        }
        public override void OnStartBattle()
        {
            if (!IsActivate)
                return;
            moonStack = 0;
            while (moonStack < owner.cardSlotDetail.cardAry.Count)
            {
                BattlePlayingCardDataInUnitModel card = owner.cardSlotDetail.cardAry[moonStack];
                if (card == null || card.cardAbility == null || !(card.cardAbility is MoonCardAbility))
                    return;
                MoonCardAbility moonAbility = card.cardAbility as MoonCardAbility;
                if(moonAbility.CanActivateMoon(moonStack+1))
                    moonAbility.ActivateMoonAbility=moonStack+1;
                moonStack++;
            }
        }
        public override void OnRoundEnd()
        {
            if (!IsActivate || moonStack < 4)
                return;
            MoonBuf moonBuf = owner.bufListDetail.GetActivatedBufList().Find(x => x is MoonBuf) as MoonBuf;
            if (moonBuf != null)
                moonBuf.Update();
            else
                owner.bufListDetail.AddBuf(new BattleUnitBuf_Moon1());
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (!IsActivate || curCard.cardAbility == null || !(curCard.cardAbility is MoonCardAbility))
                return;
            MoonCardAbility moonAbility = curCard.cardAbility as MoonCardAbility;
            switch (moonAbility.ActivateMoonAbility)
            {
                case 1:
                    moonAbility.OnFirstMoon();
                    break;
                case 2:
                    moonAbility.OnSecondMoon();
                    break;
                case 3:
                    moonAbility.OnThirdMoon();
                    break;
                case 4:
                    moonAbility.OnForthMoon();
                    break;
                case 5:
                    moonAbility.OnFifthMoon();
                    break;
            }
        }
        public override void OnRoundEndTheLast_ignoreDead()
        {
            if(isActiavted)
                EternalityInitializer.ResetSpeedDiceColor();
        }
    }
}
