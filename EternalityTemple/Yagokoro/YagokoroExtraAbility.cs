using System.Collections.Generic;
using UnityEngine;

namespace EternalityTemple.Yagokoro
{
    public class MoonCardAbility: DiceCardSelfAbilityBase
    {
        public int ActivateMoonAbility = 0;
        //用来判断这个书页可以用在哪几个月相骰上 e.g. 月相1月相2月相3的情况下 slot==1 slot==2 slot==3的时候 return true
        public virtual bool CanActivateMoon(int slot)
        {
            return false;
        }
        //触发月相1的效果
        public virtual void OnFirstMoon()
        {

        }
        //触发月相2的效果
        public virtual void OnSecondMoon()
        {

        }
        //触发月相3的效果
        public virtual void OnThirdMoon()
        {

        }
        //触发月相4的效果
        public virtual void OnForthMoon()
        {

        }
        //触发月相5的效果
        public virtual void OnFifthMoon()
        {

        }
        //文本UI相关
        public override string[] Keywords
        {
            get
            {
                List<string> output = GetMoonKeywords();
                if (output.Count > 4)
                    output = output.GetRange(0, 4);
                return output.ToArray();
            }
        }

        public virtual List<string> GetMoonKeywords()
        {
            return new List<string>() { "EternityCard3_Keyword" };
        }
        public int moonPreview = 0;
        public override void OnApplyCard()
        {
            MoonCardAbility moonAbility = card.card._script as MoonCardAbility;
            if (moonAbility == null)
                return;
            PassiveAbility_226769005 moonPassive = owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_226769005) as PassiveAbility_226769005;
            if (moonPassive!=null && (moonPassive.TempActivate || moonPassive.IsActivate) && CanActivateMoon(owner.cardOrder+1))
                moonAbility.moonPreview = owner.cardOrder + 1;
            else
                moonAbility.moonPreview = -1;
        }
        public override void OnReleaseCard()
        {
            MoonCardAbility moonAbility = card.card._script as MoonCardAbility;
            if (moonAbility == null)
                return;
            moonAbility.moonPreview = 0;
        }
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            MoonCardAbility moonAbility = card.card._script as MoonCardAbility;
            if (moonAbility == null)
                return;
            moonAbility.moonPreview = 0;
        }
        public virtual string GetMoonAbilityText()
        {
            return "";
        }
        public virtual string GetFullMoonAbilityText()
        {
            return "";
        }
    }
}
