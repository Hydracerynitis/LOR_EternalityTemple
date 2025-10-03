using EternalityTemple.Kaguya;
using System;
using System.Linq;

namespace EternalityTemple.Yagokoro
{
    public class PassiveAbility_226769005: PassiveAbilityBase
    {
        public bool IsActivate=false;
        public bool TempActivate = false;
        private YagokoroBuf13 moonChecker =null;

        public override void OnRoundEndTheLast()
        {
            if (TempActivate)
            {
                IsActivate = false;
                TempActivate = false;
            }
            
        }
        public override void OnRoundStartAfter()
        {
            if (BattleUnitBuf_KaguyaBuf.GetStack(owner) >= 7 && owner.Book.GetSpeedDiceRule(owner).speedDiceList
                .FindAll(x => !x.breaked).Count() >= 5)
            {
                IsActivate = true;
                TempActivate=false;
                if (moonChecker == null || moonChecker.IsDestroyed())
                {
                    moonChecker = new YagokoroBuf13() { stack=0};
                    owner.bufListDetail.AddBuf(moonChecker);
                }
            }
            else if (owner.bufListDetail.GetActivatedBufList().Find(x => x is YagokoroBuf12) != null && !IsActivate)
            {
                IsActivate = false;
                TempActivate = true;
            }
            if (IsActivate)
                desc = Singleton<PassiveXmlList>.Instance.GetData(EternalityInitializer.GetLorId(226769105)).desc;
            else
                desc = Singleton<PassiveXmlList>.Instance.GetData(EternalityInitializer.GetLorId(226769005)).desc;
        }
        public override void OnStartBattle()
        {
            if (IsActivate || TempActivate)
            {
                int unavailable=owner.speedDiceResult.FindAll(x => x.breaked).Count;
                for (int i = 0; i < 5; i++)
                {
                    if (i >= owner.speedDiceCount)
                        break;
                    BattlePlayingCardDataInUnitModel card = owner.cardSlotDetail.cardAry[i];
                    if (card == null || card.cardAbility == null || !(card.cardAbility is MoonCardAbility))
                        continue;
                    MoonCardAbility moonAbility = card.cardAbility as MoonCardAbility;
                    if (moonAbility.CanActivateMoon(i + 1))
                    {
                        moonAbility.ActivateMoonAbility = i + 1;
                    }
                }
            }
        }
        public override void OnRoundEnd()
        {
            if (!IsActivate || moonChecker==null || moonChecker.moonCompletion.Count < 5)
                return;
            MoonBuf moonBuf = owner.bufListDetail.GetActivatedBufList().Find(x => x is MoonBuf) as MoonBuf;
            if (moonBuf != null)
                moonBuf.Update();
            else
                owner.bufListDetail.AddBuf(new BattleUnitBuf_Moon1());
            moonChecker.moonCompletion.Clear();
            moonChecker.stack = 0;
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if(IsActivate || TempActivate)
            {
                if (curCard.cardAbility == null || !(curCard.cardAbility is MoonCardAbility))
                    return;
                MoonCardAbility moonAbility = curCard.cardAbility as MoonCardAbility;
                if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Moon3) != null && curCard.card.XmlData.Script != "YagokoroCard1")
                {
                    owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Heal");
                    moonAbility.OnFirstMoon();
                    moonAbility.OnSecondMoon();
                    moonAbility.OnThirdMoon();
                    moonAbility.OnForthMoon();
                    moonAbility.OnFifthMoon();
                    return;
                }
                else
                {
                    if (moonAbility.ActivateMoonAbility == 0)
                        return;
                    owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Heal");
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
                    if (IsActivate && moonChecker!=null)
                    {
                        moonChecker.moonCompletion.Add(moonAbility.ActivateMoonAbility);
                        moonChecker.stack=moonChecker.moonCompletion.Count;
                    }
                        
                }
            }
        }
        public override void OnRoundEndTheLast_ignoreDead()
        {
            if(isActiavted)
                EternalityInitializer.ResetSpeedDiceColor();
        }
        public override void OnWaveStart()
        {
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769024));
            if (EternalityParam.GetFaction(owner.faction).currentMoon != null)
            {
                BattleUnitBuf moonBuf = Activator.CreateInstance(EternalityParam.GetFaction(owner.faction).currentMoon) as BattleUnitBuf;
                owner.bufListDetail.AddBuf(moonBuf);
            }
        }
    }
}
