using EternalityTemple.Kaguya;
using HyperCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Yagokoro
{
    public class PassiveAbility_226769205 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            PassiveAbility_226769005 passive = owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_226769005) as PassiveAbility_226769005;
            if (!passive.IsActivate || passive.TempActivate)
                return;
            MoonBuf moonBuf = owner.bufListDetail.GetActivatedBufList().Find(x => x is MoonBuf) as MoonBuf;
            if (moonBuf != null)
                moonBuf.Update();
            else
                owner.bufListDetail.AddBuf(new BattleUnitBuf_Moon1());
        }
        //队友达到情感四后若永林仍未情感四则立升一级情感
        public override void OnRoundStart()
        {
            if(BattleObjectManager.instance.GetAliveList(owner.faction).Find((BattleUnitModel x)=>x.emotionDetail.EmotionLevel >= 4) != null && owner.emotionDetail.EmotionLevel < 4)
            {
                owner.emotionDetail.SetEmotionLevel(owner.emotionDetail.EmotionLevel + 1);
            }
        }
        public override void OnRollSpeedDice()
        {
            base.OnRollSpeedDice();
            if(Singleton<StageController>.Instance.RoundTurn % 2 == 1 && Singleton<StageController>.Instance.RoundTurn < 7)
            {
                owner.allyCardDetail.AddNewCard(new LorId(EternalityInitializer.packageId, 226769124)).SetPriorityAdder(9999);
            }
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            BattleUnitModel result = null;
            if (card.GetID().packageId != EternalityInitializer.packageId)
            {
                return result;
            }
            if (card.GetID().id == 226769016 && idx == 0)
            {
                List<BattleUnitModel> aliveList2 = BattleObjectManager.instance.GetAliveList(owner.faction);
                aliveList2.Remove(owner);
                if (aliveList2.Count > 0)
                {
                    result = RandomUtil.SelectOne<BattleUnitModel>(aliveList2);
                }
            }
            return result;
        }
    }
}
