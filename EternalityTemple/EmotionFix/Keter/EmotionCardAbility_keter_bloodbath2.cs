using System;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class EmotionCardAbility_keter_bloodbath2 : EmotionCardAbilityBase
    {
        private static int Reduce => RandomUtil.Range(2, 5);
        private Dictionary<BehaviourDetail, int> dict;
        private BehaviourDetail atk;
        public override void OnSelectEmotion()
        {
            dict = new Dictionary<BehaviourDetail, int>() { { BehaviourDetail.Slash, 0 }, { BehaviourDetail.Hit, 0 },{ BehaviourDetail.Penetrate,0} };
            atk = BehaviourDetail.None;
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            atk = atkDice.Detail;
            dict[atk] += dmg;
            base.OnTakeDamageByAttack(atkDice, dmg);
        }
        public override void OnRoundStart()
        {
            base.OnRoundEnd();
            int num = dict[BehaviourDetail.Slash];
            List<BehaviourDetail> Dmg = new List<BehaviourDetail>() { BehaviourDetail.Slash};
            if (dict[BehaviourDetail.Hit] > num)
            {
                Dmg.Clear();
                Dmg.Add(BehaviourDetail.Hit);
                num = dict[BehaviourDetail.Hit];
            }
            else if (dict[BehaviourDetail.Hit] == num)
                Dmg.Add(BehaviourDetail.Hit);
            if (dict[BehaviourDetail.Penetrate] > num)
            {
                Dmg.Clear();
                Dmg.Add(BehaviourDetail.Penetrate);
                num = dict[BehaviourDetail.Penetrate];
            }
            else if (dict[BehaviourDetail.Penetrate] == num)
                Dmg.Add(BehaviourDetail.Penetrate);
            switch (RandomUtil.SelectOne(Dmg))
            {
                case BehaviourDetail.Slash:
                    _owner.bufListDetail.AddBuf(new SlashProt(_emotionCard));
                    break;
                case BehaviourDetail.Hit:
                    _owner.bufListDetail.AddBuf(new HitProt(_emotionCard));
                    break;
                case BehaviourDetail.Penetrate:
                    _owner.bufListDetail.AddBuf(new PenetrateProt(_emotionCard));
                    break;
            }
            dict = new Dictionary<BehaviourDetail, int>() { { BehaviourDetail.Slash, 0 }, { BehaviourDetail.Hit, 0 }, { BehaviourDetail.Penetrate, 0 } };
            atk = BehaviourDetail.None;
        }
        public class SlashProt: BattleUnitBuf
        {
            private BattleEmotionCardModel emotionCard;
            public override string keywordIconId => "Roland_4th_DmgReduction_Slash";
            public override string keywordId => "EF_SlashProtect";
            public SlashProt(BattleEmotionCardModel card)
            {
                emotionCard = card;
                stack = 0;
            }
            public override int GetDamageReduction(BattleDiceBehavior behavior)
            {
                if (behavior.Detail == BehaviourDetail.Slash)
                {
                    int reduce = Reduce;
                    _owner.battleCardResultLog?.SetEmotionAbility(true, emotionCard, 0, ResultOption.Sign, -reduce);
                    _owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
                    _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
                    return reduce;
                }
                return base.GetDamageReduction(behavior);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
        public class HitProt : BattleUnitBuf
        {
            private BattleEmotionCardModel emotionCard;
            public override string keywordIconId => "Roland_4th_DmgReduction_Hit";
            public override string keywordId => "EF_HitProtect";
            public HitProt(BattleEmotionCardModel card)
            {
                emotionCard = card;
                stack = 0;
            }
            public override int GetDamageReduction(BattleDiceBehavior behavior)
            {
                if (behavior.Detail == BehaviourDetail.Hit)
                {
                    int reduce = Reduce;
                    _owner.battleCardResultLog?.SetEmotionAbility(true, emotionCard, 0, ResultOption.Sign, -reduce);
                    _owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
                    _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
                    return reduce;
                }
                return base.GetDamageReduction(behavior);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
        public class PenetrateProt : BattleUnitBuf
        {
            private BattleEmotionCardModel emotionCard;
            public override string keywordIconId => "Roland_4th_DmgReduction_Penetrate";
            public override string keywordId => "EF_PenetrateProtect";
            public PenetrateProt(BattleEmotionCardModel card)
            {
                emotionCard = card;
                stack = 0;
            }
            public override int GetDamageReduction(BattleDiceBehavior behavior)
            {
                if (behavior.Detail == BehaviourDetail.Penetrate)
                {
                    int reduce = Reduce;
                    _owner.battleCardResultLog?.SetEmotionAbility(true, emotionCard, 0, ResultOption.Sign, -reduce);
                    _owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
                    _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
                    return reduce;
                }
                return base.GetDamageReduction(behavior);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
