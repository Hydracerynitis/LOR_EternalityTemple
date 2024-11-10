using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_geburah_nothing1 : EmotionCardAbilityBase
    {
        private bool _triggered;

        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (!CheckTrigger(behavior))
                return;
            _triggered = true;
            int num = behavior.DiceVanillaValue + behavior.PowerAdder;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = num
            });
            if (Singleton<StageController>.Instance.IsLogState())
                _owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(Filter));
            else
                Filter();
        }

        public void Filter()
        {
            CameraFilterUtil.RedColorFilter();
            SoundEffectPlayer.PlaySound("Creature/NothingThere_Goodbye");
        }

        private bool CheckTrigger(BattleDiceBehavior behavior)
        {
            if (_triggered)
                return false;
            BattlePlayingCardDataInUnitModel[] array = Singleton<StageController>.Instance.GetAllCards().ToArray();
            BattlePlayingCardDataInUnitModel card = behavior.card;
            foreach (BattlePlayingCardDataInUnitModel cardDataInUnitModel in array)
            {
                if (cardDataInUnitModel.owner == _owner && cardDataInUnitModel != card)
                    return false;
            }
            if (card != null)
            {
                Queue<BattleDiceBehavior> cardBehaviorQueue = card.cardBehaviorQueue;
                if(cardBehaviorQueue != null && cardBehaviorQueue.Count<=0)
                    return true;
            }
            return false;
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            _triggered = false;
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _triggered = false;
        }
    }
}
