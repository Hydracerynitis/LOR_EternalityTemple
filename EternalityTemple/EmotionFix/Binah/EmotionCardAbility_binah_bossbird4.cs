using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Reflection;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using EmotionalFix;

namespace EmotionalFix
{
    /*public class EmotionCardAbility_binah_bossbird4 : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _aura;
        private List<BattleDiceCardModel> egos = new List<BattleDiceCardModel>();
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_binah_bossbird1.Longbird());
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_binah_bossbird2.Bigbird());
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_binah_bossbird3.Smallbird());
            _owner.allyCardDetail.AddCardToDeck(egos);
            _owner.allyCardDetail.Shuffle();
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            BattleEmotionCardModel Long = Helper.SearchEmotion(_owner, "ApocalypseBird_LongArm_Enemy");
            BattleEmotionCardModel Big = Helper.SearchEmotion(_owner, "ApocalypseBird_BigEye_Enemy");
            BattleEmotionCardModel Small= Helper.SearchEmotion(_owner, "ApocalypseBird_SmallPeak_Enemy");
            if (Long != null)
            {
                EmotionCardAbility_binah_bossbird1 longbird = Long.GetAbilityList().Find(x => x is EmotionCardAbility_binah_bossbird1) as EmotionCardAbility_binah_bossbird1;
                if (longbird != null)
                    longbird.Destroy();
                _owner.emotionDetail.PassiveList.Remove(Long);
                string name = RandomUtil.SelectOne(EmotionFixInitializer.emotion1).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enemy.Find(x => x.Name == name);
                _owner.emotionDetail.ApplyEmotionCard(emotion);
            }
            if (Big!= null)
            {
                EmotionCardAbility_binah_bossbird2 bigbird = Big.GetAbilityList().Find(x => x is EmotionCardAbility_binah_bossbird2) as EmotionCardAbility_binah_bossbird2;
                if (bigbird != null)
                    bigbird.Destroy();
                _owner.emotionDetail.PassiveList.Remove(Big);
                string name = RandomUtil.SelectOne(EmotionFixInitializer.emotion2).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enemy.Find(x => x.Name == name);
                _owner.emotionDetail.ApplyEmotionCard(emotion);
            }
            if (Small != null)
            {
                EmotionCardAbility_binah_bossbird3 smallbird = Small.GetAbilityList().Find(x => x is EmotionCardAbility_binah_bossbird3) as EmotionCardAbility_binah_bossbird3;
                if (smallbird != null)
                    smallbird.Destroy();
                _owner.emotionDetail.PassiveList.Remove(Small);
                string name = RandomUtil.SelectOne(EmotionFixInitializer.emotion2).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enemy.Find(x => x.Name == name);
                _owner.emotionDetail.ApplyEmotionCard(emotion);
            }
            SoundEffectManager.Instance.PlayClip("Creature/BossBird_Birth", false, 4f);
            _aura= DiceEffectManager.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_MonsterAura", 1f, _owner.view, _owner.view);
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_binah_bossbird1.Longbird());
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_binah_bossbird2.Bigbird());
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_binah_bossbird3.Smallbird());
            egos.AddRange(new List<BattleDiceCardModel>() { AddEgoCard(910041), AddEgoCard(910042), AddEgoCard(910043) });
            _owner.allyCardDetail.AddCardToDeck(egos);
            _owner.allyCardDetail.Shuffle();
        }
        private BattleDiceCardModel AddEgoCard(int id)
        {
            DiceCardXmlInfo EgoXML = ItemXmlDataList.instance.GetCardItem(id).Copy(true);
            EgoXML.optionList.Clear();
            DiceCardSpec EgoSpec = EgoXML.Spec.Copy();
            EgoSpec.Cost = 0;
            EgoXML.Spec = EgoSpec;
            EgoXML.Priority = 100;
            EgoXML.Keywords.Clear();
            BattleDiceCardModel Ego = BattleDiceCardModel.CreatePlayingCard(EgoXML);
            Ego.owner = _owner;
            return Ego;
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public void DestroyAura()
        {
            if (_aura == null)
                return;
            UnityEngine.Object.Destroy(_aura.gameObject);
            _aura = null;
        }
    }*/
}
