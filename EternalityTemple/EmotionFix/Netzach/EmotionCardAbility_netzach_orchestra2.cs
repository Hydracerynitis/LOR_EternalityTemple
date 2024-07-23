using System;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_netzach_orchestra2 : EmotionCardAbilityBase
    {
        public CameraFilterPack_Noise_TV_2 _filter;
        private int turn;
        public void Filter()
        {
            Camera effectCam = SingletonBehavior<BattleCamManager>.Instance.EffectCam;
            if (effectCam == null)
                return;
            _filter = effectCam.gameObject.AddComponent<CameraFilterPack_Noise_TV_2>();
            _filter.Fade = 0.15f;
            _filter.Fade_Additive = 0.0f;
            _filter.Fade_Distortion = 0.2f;
        }
        public void DestroyFilter()
        {
            if (_filter == null)
                return;
            UnityEngine.Object.Destroy(_filter);
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (_owner.faction == Faction.Enemy)
                turn = 3;
            if (!CheckDupFilter())
                Filter();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            ++turn;
            if (turn % 4 != 0)
            {
                if (BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll(x => x.bufListDetail.GetActivatedBufList().Exists(y => y is Enthusiastic)).Count == 0)
                    DestroyFilter();
                return;
            }
            SoundEffectPlayer.PlaySound("Creature/Sym_movment_3_mov3");
            List<BattleUnitModel> Player = BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll(x => !x.bufListDetail.GetActivatedBufList().Exists(y => y is Enthusiastic));
            if (Player.Count == 0)
                return;
            BattleUnitModel unlucky = RandomUtil.SelectOne(Player);
            unlucky.bufListDetail.AddBuf(new Enthusiastic());
            unlucky.breakDetail.RecoverBreakLife(unlucky.MaxBreakLife);
            unlucky.breakDetail.nextTurnBreak = false;
            unlucky.turnState = BattleUnitTurnState.WAIT_CARD;
            unlucky.breakDetail.RecoverBreak(unlucky.breakDetail.GetDefaultBreakGauge());
            if (!CheckDupFilter())
                Filter();
        }
        private bool CheckDupFilter()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                BattleEmotionCardModel adoration = Helper.SearchEmotion(unit, "SilentOrchestra_Affect");
                if (adoration == null)
                    adoration = Helper.SearchEmotion(unit, "SilentOrchestra_Affect_Enemy");
                if (adoration == null)
                    continue;
                else
                {
                    if(adoration.AbilityList.Find(x => x is EmotionCardAbility_netzach_orchestra2) is EmotionCardAbility_netzach_orchestra2 AdorationAbility)
                    {
                        if (AdorationAbility._filter != null)
                            return true;
                    }
                }
            }
            return false;
        }
        public override void OnEndBattlePhase()
        {
            Destroy();
        }

        public override void OnBattleEnd()
        {
            Destroy();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            Destroy();
        }
        public void Destroy()
        {
            DestroyFilter();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                BattleEmotionCardModel adoration = Helper.SearchEmotion(unit, "SilentOrchestra_Affect_Enemy");
                if (adoration != null)
                {
                    if (adoration.AbilityList.Find(x => x is EmotionCardAbility_netzach_orchestra2) is EmotionCardAbility_netzach_orchestra2 AdorationAbility)
                    {
                        if (AdorationAbility._filter == null)
                            AdorationAbility.Filter();
                    }
                }
                return;
            }
            foreach (BattleUnitModel player in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                if (player.bufListDetail.GetActivatedBufList().Find(x => x is Enthusiastic) is Enthusiastic enthusiastic)
                    enthusiastic.Destroy();
            }
            
        }
        public class Enthusiastic: BattleUnitBuf
        {
            private bool _bControlable = false;
            private bool _bRecoverBreak;
            private static int DmgAdd => RandomUtil.Range(2, 4);
            public override string keywordId => "EF_Enthusiastic";
            public override string keywordIconId => "Orchestra_Enthusiastic";
            public override bool IsControllable => _bControlable;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override bool IsInvincibleBp(BattleUnitModel attacker)
            {
                if (attacker != null && attacker != _owner)
                {
                    if (attacker.faction == Faction.Player)
                    {
                        if (attacker.bufListDetail.GetActivatedBufList().Exists(x => x is Enthusiastic))
                            return true;
                    }
                    else
                        return true;
                }
                return base.IsInvincibleBp(attacker);
            }
            public override int GetBreakDamageIncreaseRate() => 100;
            public override bool IsInvincibleHp(BattleUnitModel attacker)
            {
                if (attacker != null)
                {
                    if (attacker.faction == Faction.Player)
                    {
                        if (!attacker.bufListDetail.GetActivatedBufList().Exists(x => x is Enthusiastic))
                            return true;
                    }
                    else
                    {
                        return base.IsInvincibleHp(attacker);
                    }
                }
                return base.IsInvincibleHp(attacker);
            }
            public override bool TeamKill()
            {
                return true;
            }
            public override int GetDamageReduction(BattleDiceBehavior behavior) => -DmgAdd;
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                BattleUnitModel owner = atkDice.owner;
                bool flag = owner.bufListDetail.GetActivatedBufList().Exists(x => x is Enthusiastic);
                if (owner.faction != _owner.faction || flag || (!_owner.IsBreakLifeZero() || _bRecoverBreak))
                    return;
                _bRecoverBreak = true;
            }
            public override void OnRoundEndTheLast()
            {
                if (!_bRecoverBreak)
                    return;
                _owner.breakDetail.RecoverBreakLife(_owner.MaxBreakLife);
                _owner.breakDetail.nextTurnBreak = false;
                _owner.turnState = BattleUnitTurnState.WAIT_CARD;
                _owner.breakDetail.RecoverBreak(_owner.breakDetail.GetDefaultBreakGauge());
                Destroy();
            }
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlotOrder)
            {
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
                aliveList.Remove(_owner);
                if (aliveList.Count == 0)
                    return base.ChangeAttackTarget(card,currentSlotOrder);
                return RandomUtil.SelectOne(aliveList);
            }
        }
    }
}
