using System;
using LOR_DiceSystem;
using EI = EternalityTemple.EternalityInitializer;

namespace EmotionalFix
{
    public class EmotionCardAbility_yesod_freischutz2 : EmotionCardAbilityBase
    {
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.isKeepedCard || curCard.card.GetID()== 1101005)
                return;
            if (!(_owner.bufListDetail.GetActivatedBufList().Find(x => x is Bullet) is Bullet bullet))
            {
                Bullet Bullet = new Bullet();
                _owner.bufListDetail.AddBuf(Bullet);
                Bullet.stack+=1;
            }
            else
            {
                bullet.stack+=1;
                if (bullet.stack < 7)
                    return;
                BattleDiceCardModel TheBullet = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(new LorId(EI.packageId, 1101005)));
                DiceBehaviour dice = TheBullet.XmlData.DiceBehaviourList[0];
                dice.Min = 0;
                dice.Dice = 0;
                foreach(DiceBehaviour Dice in curCard.card.XmlData.DiceBehaviourList)
                {
                    dice.Min += Dice.Min;
                    dice.Dice += Dice.Dice;
                }
                TheBullet.XmlData.DiceBehaviourList[0] = dice;
                BattlePlayingCardDataInUnitModel Card = new BattlePlayingCardDataInUnitModel
                {
                    card = TheBullet
                };
                BattleUnitModel Gunner= RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction));
                if (Gunner == null)
                    Card.owner = _owner;
                else
                    Card.owner = Gunner;
                StageController.Instance.AddAllCardListInBattle(Card, _owner);
                curCard.DestroyDice(DiceMatch.AllDice, DiceUITiming.Start);
                bullet.Destroy();
            }
        }
        public void Destroy()
        {
            if (_owner.bufListDetail.GetActivatedBufList().Find((x => x is Bullet)) is Bullet bullet)
            {
                bullet.Destroy();
            }
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            behavior?.card?.target?.battleCardResultLog?.SetNewCreatureAbilityEffect("2_Y/FX_IllusionCard_2_Y_Seven", 3f);
            behavior?.card?.target?.battleCardResultLog?.SetCreatureEffectSound("Creature/Matan_NormalShot");
        }
        public class Bullet : BattleUnitBuf
        {
            public override string keywordId => "EF_Bullet_Eternal";
            public override string keywordIconId => "Freischutz_Bullet";
            public Bullet() => stack = 0;
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = stack,
                    power=stack/2
                });
            }
        }
    }
}
