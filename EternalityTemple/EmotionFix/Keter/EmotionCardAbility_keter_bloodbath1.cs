using System;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using EI = EternalityTemple.EternalityInitializer;

namespace EternalityEmotion
{
    public class EmotionCardAbility_keter_bloodbath1 : EmotionCardAbilityBase
    {
        private List<BattleDiceBehavior> DefenseDice = new List<BattleDiceBehavior>();
        private Battle.CreatureEffect.CreatureEffect effect;
        public override void OnSelectEmotion()
        {
            effect = MakeEffect("0/BloodyBath_Blood");
            if (effect != null)
                effect.transform.SetParent(_owner.view.characterRotationCenter.transform.parent);
            SoundEffectManager.Instance.PlayClip("Creature/BloodBath_Water")?.SetGlobalPosition(_owner.view.WorldPosition);
        }
        public override int GetBreakDamageReduction(BattleDiceBehavior behavior)
        {
            _owner.battleCardResultLog?.SetEmotionAbility(true, _emotionCard, 0, ResultOption.Sign, RandomUtil.Range(2, 5));
            return -RandomUtil.Range(2, 5);
        }
        public override void OnRoundEnd()
        {
            DefenseDice.Clear();
            for (; _owner.cardSlotDetail.keepCard.cardBehaviorQueue.Count > 0;)
            {
                BattleDiceBehavior dice = _owner.cardSlotDetail.keepCard.cardBehaviorQueue.Dequeue();
                if (IsDefenseDice(dice.Detail))
                    DefenseDice.Add(dice);
            }
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (DefenseDice.Count > 0)
                _owner.cardSlotDetail.keepCard.AddBehaviours(BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(new LorId(EI.packageId,1110001))), DefenseDice);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (!IsDefenseDice(behavior.Detail))
                return;
            _owner.battleCardResultLog?.SetEmotionAbility(false, _emotionCard, 1, ResultOption.Default);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = RandomUtil.Range(1, 2)
            });
        }
        public override void OnLayerChanged(string layerName)
        {
            if (layerName == "Character")
                layerName = "CharacterUI";
            if (effect == null)
                return;
            effect.SetLayer(layerName);
        }
    }
}
