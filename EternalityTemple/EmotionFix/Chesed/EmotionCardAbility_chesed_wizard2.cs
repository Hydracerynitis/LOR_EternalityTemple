using LOR_DiceSystem;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Potion = EmotionCardAbility_wizard2.BattleUnitBuf_potion;
using Pocket= EmotionCardAbility_wizard2.BattleUnitBuf_pocket;
using Heart = EmotionCardAbility_wizard2.BattleUnitBuf_heart;
using Home = EmotionCardAbility_wizard2.BattleUnitBuf_home;
using Change = EmotionCardAbility_wizard2.BattleUnitBuf_change;

namespace EternalityEmotion
{
    public class EmotionCardAbility_chesed_wizard2: EmotionCardAbilityBase
    {
        private static List<WizardBufType> _remainedList = new List<WizardBufType>();
        private WizardBufType _selectedBuf;

        public override void OnSelectEmotion()
        {
            _remainedList.Clear();
            if(!CheckBuff(WizardBufType.Potion))
                _remainedList.Add(WizardBufType.Potion);
            if (!CheckBuff(WizardBufType.Pocket))
                _remainedList.Add(WizardBufType.Pocket);
            if (!CheckBuff(WizardBufType.Heart))
                _remainedList.Add(WizardBufType.Heart);
            if (!CheckBuff(WizardBufType.Home))
                _remainedList.Add(WizardBufType.Home);
            if (!CheckBuff(WizardBufType.Change))
                _remainedList.Add(WizardBufType.Change);
            WizardBufType bufType = RandomUtil.SelectOne(_remainedList);
            switch (bufType)
            {
                case WizardBufType.Potion:
                    _owner.bufListDetail.AddBuf(new Potion());
                    break;
                case WizardBufType.Pocket:
                    _owner.bufListDetail.AddBuf(new Pocket());
                    break;
                case WizardBufType.Heart:
                    _owner.bufListDetail.AddBuf(new Heart());
                    break;
                case WizardBufType.Home:
                    _owner.bufListDetail.AddBuf(new Home());
                    break;
                case WizardBufType.Change:
                    _owner.bufListDetail.AddBuf(new Change());
                    break;
            }
            _selectedBuf = bufType;
        }
        public override void OnWaveStart()
        {
            switch (_selectedBuf)
            {
                case WizardBufType.Potion:
                    _owner.bufListDetail.AddBuf(new Potion());
                    break;
                case WizardBufType.Pocket:
                    _owner.bufListDetail.AddBuf(new Pocket());
                    break;
                case WizardBufType.Heart:
                    _owner.bufListDetail.AddBuf(new Heart());
                    break;
                case WizardBufType.Home:
                    _owner.bufListDetail.AddBuf(new Home());
                    break;
                case WizardBufType.Change:
                    _owner.bufListDetail.AddBuf(new Change());
                    break;
            }
        }
        public bool CheckBuff(WizardBufType bufType)
        {
            foreach(BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                switch (bufType)
                {
                    case WizardBufType.Potion:
                        if (ally.bufListDetail.GetActivatedBufList().Find(x => x is Potion) != null)
                            return true;
                        break;
                    case WizardBufType.Pocket:
                        if (ally.bufListDetail.GetActivatedBufList().Find(x => x is Pocket) != null)
                            return true;
                        break;
                    case WizardBufType.Heart:
                        if (ally.bufListDetail.GetActivatedBufList().Find(x => x is Heart) != null)
                            return true;
                        break;
                    case WizardBufType.Home:
                        if (ally.bufListDetail.GetActivatedBufList().Find(x => x is Home) != null)
                            return true;
                        break;
                    case WizardBufType.Change:
                        if (ally.bufListDetail.GetActivatedBufList().Find(x => x is Change) != null)
                            return true;
                        break;
                }
            }
            return false;
        }
        public enum WizardBufType
        {
            Potion,
            Pocket,
            Heart,
            Home,
            Change,
        }
    }
}
