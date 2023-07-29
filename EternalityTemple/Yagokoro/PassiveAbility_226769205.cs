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
            if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_KaguyaBuf7) == null || owner.speedDiceResult.Count < 5)
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
    }
}
