using Sound;
using System;
using UnityEngine;
using SoundBuf = EmotionCardAbility_bluestar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf;

namespace EmotionalFix.Hokma
{
    public class EmotionCardAbility_hokma_bluestar3 : EmotionCardAbilityBase
    {
        private int round;
        private SoundEffectPlayer _loop;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            round = 3;
            _owner.bufListDetail.AddBuf(new SoundBuf());
            _owner.bufListDetail.AddBuf(new Cooldown(round));

        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is SoundBuf) == null)
                return;
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/9_H/FX_IllusionCard_9_H_Voice");
            if (original == null)
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, BattleSceneRoot.Instance.transform);
            if (creatureEffect?.gameObject.GetComponent<AutoDestruct>() != null)
                return;
            AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
            if (autoDestruct == null)
                return;
            autoDestruct.time = 5f;
            autoDestruct.DestroyWhenDisable();
            SoundEffectPlayer.PlaySound("Creature/BlueStar_Atk");
            //No BlueStar Silence Mod Compatibility
            if (Helper.CheckOtherMod("NoBlueStarSilence"))
                return;
            BattleSoundManager.Instance.EndBgm();
            if (_loop == null)
                return;
            _loop = SoundEffectManager.Instance.PlayClip("Creature/BlueStar_Bgm", true, parent: BattleSceneRoot.Instance.currentMapObject.transform);
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if(round==3)
                _owner.bufListDetail.AddBuf(new SoundBuf());
            _owner.bufListDetail.AddBuf(new Cooldown(round));
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            round--;
            if (round <= 0)
                round = 3;
            DestroyLoopSound();
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            Destroy();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            Destroy();
        }
        private void DestroyLoopSound()
        {
            if (_loop == null || Helper.CheckOtherMod("NoBlueStarSilence"))
                return;
            BattleSoundManager.Instance.StartBgm();
            _loop.ManualDestroy();
            _loop = null;
        }
        public void Destroy()
        {
            DestroyLoopSound();
            BattleUnitBuf Buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is SoundBuf);
            if (Buff != null)
                Buff.Destroy();
            Buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is Cooldown);
            if (Buff != null)
                Buff.Destroy();
        }
        public class Cooldown : BattleUnitBuf
        {
            public override string keywordId => "Emotion_BlueStar_SoundBuf_Cool";
            public Cooldown(int cooldown)
            {
                Init(_owner);
                stack = cooldown;
                if (stack >= 3)
                    stack = 3;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                --stack;
                if (stack > 0)
                    return;
                stack = 3;
                _owner.bufListDetail.AddBuf(new SoundBuf());
            }
        }
    }
}
