using System;
using Battle.DiceAttackEffect;
using UnityEngine;
using System.Collections.Generic;
using LOR_DiceSystem;

namespace EternalityTemple
{
	//铃仙平a
	public class DiceAttackEffect_Udong_Fire : DiceAttackEffect
	{
		public override void Initialize(BattleUnitView self, BattleUnitView target, float destroyTime)
		{
			this._bHasDamagedEffect = true;
			this._self = self.model;
			this._target = target.model;
			this._selfTransform = self.atkEffectRoot;
			this._targetTransform = target.atkEffectRoot;
			this.atkdir = (((double)(target.WorldPosition - self.WorldPosition).x > 0.0) ? Direction.RIGHT : Direction.LEFT);
		}
		public override void Start()
		{
			AssetBundle assetBundle = EternalityInitializer.assetBundle["Eternality"];
			this.gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("P1-3"));
			this.gameObject.transform.parent = this._selfTransform;
			this.gameObject.transform.localPosition = new Vector3(1.75f, 1.5f, 0f);
			this.gameObject.transform.localScale = Vector3.one;
			this.gameObject.transform.localRotation = Quaternion.identity;
			this.gameObject.layer = 8;
			Transform[] componentsInChildren = this.gameObject.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.GetComponent<Renderer>() != null)
				{
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder < 0)
					{
						componentsInChildren[i].gameObject.layer = 20;
					}
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder >= 0)
					{
						componentsInChildren[i].gameObject.layer = 8;
					}
				}
			}
			if (this.atkdir.Equals(Direction.LEFT))
			{
				this.gameObject.transform.localPosition = new Vector3(-1.75f, 1.625f, 0f);
				this.gameObject.transform.Rotate(Vector3.right, 11.4f);
				this.gameObject.transform.Rotate(Vector3.up, 180f);
			}
			this.gameObject.SetActive(true);
		}
		public Direction atkdir;
		public new GameObject gameObject;
		public BattleUnitModel _target;
	}
	public class DiceAttackEffect_Udong_Eye : DiceAttackEffect
	{
		public override void Initialize(BattleUnitView self, BattleUnitView target, float destroyTime)
		{
			this._bHasDamagedEffect = true;
			this._self = self.model;
			this._target = target.model;
			this._selfTransform = self.atkEffectRoot;
			this._targetTransform = target.atkEffectRoot;
			this.atkdir = (((double)(target.WorldPosition - self.WorldPosition).x > 0.0) ? Direction.RIGHT : Direction.LEFT);
		}
		public override void Start()
		{
			AssetBundle assetBundle = EternalityInitializer.assetBundle["Eternality"];
			this.gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("P2"));
			this.gameObject.transform.parent = this._selfTransform;
			this.gameObject.transform.localPosition = new Vector3(1.75f, 1.5f, 0f);
			this.gameObject.transform.localScale = Vector3.one;
			this.gameObject.transform.localRotation = Quaternion.identity;
			this.gameObject.layer = 8;
			Transform[] componentsInChildren = this.gameObject.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.GetComponent<Renderer>() != null)
				{
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder < 0)
					{
						componentsInChildren[i].gameObject.layer = 20;
					}
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder >= 0)
					{
						componentsInChildren[i].gameObject.layer = 8;
					}
				}
			}
			if (this.atkdir.Equals(Direction.LEFT))
			{
				this.gameObject.transform.localPosition = new Vector3(-1.75f, 1.625f, 0f);
				this.gameObject.transform.Rotate(Vector3.right, 11.4f);
				this.gameObject.transform.Rotate(Vector3.up, 180f);
			}
			this.gameObject.SetActive(true);
		}
		public Direction atkdir;
		public new GameObject gameObject;
		public BattleUnitModel _target;
	}
	public class BehaviourAction_Udong_Eye : BehaviourActionBase
	{
		public override List<RencounterManager.MovingAction> GetMovingAction(ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
		{
			List<RencounterManager.MovingAction> result;
			if (self.result == Result.Win && opponent.behaviourResultData != null)
			{
				opponent.infoList.Clear();
				this._self = self.view.model;
				List<RencounterManager.MovingAction> list = new List<RencounterManager.MovingAction>();
				RencounterManager.MovingAction movingAction = new RencounterManager.MovingAction(ActionDetail.Default, CharMoveState.Stop, 0f, false, 0.2f, 1f);
				movingAction.customEffectRes = "Udong_Eye";
				movingAction.SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE);
				list.Add(movingAction);
				RencounterManager.MovingAction movingAction2 = new RencounterManager.MovingAction(ActionDetail.Hit, CharMoveState.Stop, 0f, false, 0.8f, 1f);
				movingAction2.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
				list.Add(movingAction2);
				RencounterManager.MovingAction movingAction3 = new RencounterManager.MovingAction(ActionDetail.Slash, CharMoveState.Stop, 0f, false, 3f, 1f);
				movingAction3.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
				list.Add(movingAction3);
				RencounterManager.MovingAction movingAction4 = new RencounterManager.MovingAction(ActionDetail.S4, CharMoveState.Stop, 0f, false, 1f, 1f);
				movingAction4.SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE);
				list.Add(movingAction4);

				RencounterManager.MovingAction item = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 0f, false, 0.2f, 1f);
				item.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
				opponent.infoList.Add(item);
				RencounterManager.MovingAction item2 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 0f, false, 0.8f, 1f);
				item.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
				opponent.infoList.Add(item);
				RencounterManager.MovingAction item3 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 0f, false, 3f, 1f);
				item.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
				opponent.infoList.Add(item);
				RencounterManager.MovingAction item4 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 0f, false, 1f, 1f);
				item.SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE);
				opponent.infoList.Add(item);
				result = list;
			}
			else
			{
				result = base.GetMovingAction(ref self, ref opponent);
			}
			return result;
		}
	}
	public class DiceAttackEffect_Neet_RS : DiceAttackEffect
	{
		public override void Initialize(BattleUnitView self, BattleUnitView target, float destroyTime)
		{
			this._bHasDamagedEffect = true;
			this._self = self.model;
			this._target = target.model;
			this._selfTransform = self.atkEffectRoot;
			this._targetTransform = target.atkEffectRoot;
			this.atkdir = (((double)(target.WorldPosition - self.WorldPosition).x > 0.0) ? Direction.RIGHT : Direction.LEFT);
		}
		public override void Start()
		{
			AssetBundle assetBundle = EternalityInitializer.assetBundle["Eternality"];
			this.gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("P3"));
			this.gameObject.transform.parent = this._selfTransform;
			this.gameObject.transform.localPosition = new Vector3(1.75f, 1.5f, 0f);
			this.gameObject.transform.localScale = Vector3.one;
			this.gameObject.transform.localRotation = Quaternion.identity;
			this.gameObject.layer = 8;
			Transform[] componentsInChildren = this.gameObject.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.GetComponent<Renderer>() != null)
				{
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder < 0)
					{
						componentsInChildren[i].gameObject.layer = 20;
					}
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder >= 0)
					{
						componentsInChildren[i].gameObject.layer = 8;
					}
				}
			}
			if (this.atkdir.Equals(Direction.LEFT))
			{
				this.gameObject.transform.localPosition = new Vector3(-1.75f, 1.625f, 0f);
				this.gameObject.transform.Rotate(Vector3.right, 11.4f);
				this.gameObject.transform.Rotate(Vector3.up, 180f);
			}
			this.gameObject.SetActive(true);
		}
		public Direction atkdir;
		public new GameObject gameObject;
		public BattleUnitModel _target;
	}
	public class DiceAttackEffect_Doc_AOE : DiceAttackEffect
	{
		public override void Initialize(BattleUnitView self, BattleUnitView target, float destroyTime)
		{
			this._bHasDamagedEffect = true;
			this._self = self.model;
			this._target = target.model;
			this._selfTransform = self.atkEffectRoot;
			this._targetTransform = target.atkEffectRoot;
			this.atkdir = (((double)(target.WorldPosition - self.WorldPosition).x > 0.0) ? Direction.RIGHT : Direction.LEFT);
		}
		public override void Start()
		{
			AssetBundle assetBundle = EternalityInitializer.assetBundle["Eternality"];
			this.gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("P4"));
			this.gameObject.transform.parent = this._selfTransform;
			this.gameObject.transform.localPosition = new Vector3(0f, 0.5f, 0f);
			this.gameObject.transform.localScale = Vector3.one;
			this.gameObject.transform.localRotation = Quaternion.identity;
			this.gameObject.layer = 8;
			Transform[] componentsInChildren = this.gameObject.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.GetComponent<Renderer>() != null)
				{
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder < 0)
					{
						componentsInChildren[i].gameObject.layer = 20;
					}
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder >= 0)
					{
						componentsInChildren[i].gameObject.layer = 8;
					}
				}
			}
			this.gameObject.SetActive(true);
		}
		public Direction atkdir;
		public new GameObject gameObject;
		public BattleUnitModel _target;
	}
	public class BehaviourAction_Doc_AOE : BehaviourActionBase
	{
		public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
		{
			this._self = self;
			FarAreaEffect_Doc_AOE farAreaEffect = new GameObject().AddComponent<FarAreaEffect_Doc_AOE>();
			farAreaEffect.Init(self, Array.Empty<object>());
			return farAreaEffect;
		}
	}
	public class FarAreaEffect_Doc_AOE : FarAreaEffect
	{
		public override void Init(BattleUnitModel self, params object[] args)
		{
			base.Init(self, args);
			this.OnEffectStart();
			this._elapsed = 0f;
			Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay(0f, 0.5f);
		}
		public override void Update()
		{
			if (this.state == FarAreaEffect.EffectState.Start)
			{
				this.state = FarAreaEffect.EffectState.GiveDamage;
				this._self.view.charAppearance.ChangeMotion(ActionDetail.S1);
				SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("Doc_AOE", 1f, _self.view, _self.view, 1f);
				return;
			}
			else if (this.state == FarAreaEffect.EffectState.GiveDamage)
			{
				this._elapsed += Time.deltaTime;
				if (this._elapsed >= 3f)
				{
					this._elapsed = 0f;
					this.isRunning = false;
					this.state = FarAreaEffect.EffectState.End;
					return;
				}
			}
			else if (this.state == FarAreaEffect.EffectState.End)
			{
				this._elapsed += Time.deltaTime;
				if (this._elapsed > 2f)
				{
					this._self.view.charAppearance.ChangeMotion(ActionDetail.Default);
					this.state = FarAreaEffect.EffectState.None;
					this._elapsed = 0f;
					return;
				}
			}
			else if (this.state == FarAreaEffect.EffectState.None)
			{
				if (this._self.view.FormationReturned)
				{
					Destroy(base.gameObject);
				}
			}
		}
		private float _elapsed;
	}
	public class BehaviourAction_Neet_AOE : BehaviourActionBase
	{
		public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
		{
			this._self = self;
			FarAreaEffect_Neet_AOE farAreaEffect = new GameObject().AddComponent<FarAreaEffect_Neet_AOE>();
			farAreaEffect.Init(self, Array.Empty<object>());
			return farAreaEffect;
		}
	}
	public class DiceAttackEffect_Neet_AOE : DiceAttackEffect
	{
		public override void Initialize(BattleUnitView self, BattleUnitView target, float destroyTime)
		{
			this._bHasDamagedEffect = true;
			this._self = self.model;
			this._target = target.model;
			this._selfTransform = self.atkEffectRoot;
			this._targetTransform = target.atkEffectRoot;
			this.atkdir = (((double)(target.WorldPosition - self.WorldPosition).x > 0.0) ? Direction.RIGHT : Direction.LEFT);
		}
		public override void Start()
		{
			AssetBundle assetBundle = EternalityInitializer.assetBundle["Eternality"];
			this.gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("P5"));
			this.gameObject.transform.parent = this._selfTransform;
			this.gameObject.transform.localPosition = new Vector3(1.75f, 1.5f, 0f);
			this.gameObject.transform.localScale = Vector3.one;
			this.gameObject.transform.localRotation = Quaternion.identity;
			this.gameObject.layer = 8;
			Transform[] componentsInChildren = this.gameObject.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.GetComponent<Renderer>() != null)
				{
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder < 0)
					{
						componentsInChildren[i].gameObject.layer = 20;
					}
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder >= 0)
					{
						componentsInChildren[i].gameObject.layer = 8;
					}
				}
			}
			if (this.atkdir.Equals(Direction.LEFT))
			{
				this.gameObject.transform.localPosition = new Vector3(-1.75f, 1.625f, 0f);
				this.gameObject.transform.Rotate(Vector3.right, 11.4f);
				this.gameObject.transform.Rotate(Vector3.up, 180f);
			}
			this.gameObject.SetActive(true);
		}
		public Direction atkdir;
		public new GameObject gameObject;
		public BattleUnitModel _target;
	}
	public class FarAreaEffect_Neet_AOE : FarAreaEffect
	{
		public override void Init(BattleUnitModel self, params object[] args)
		{
			base.Init(self, args);
			this.OnEffectStart();
			this._elapsed = 0f;
			Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay(0f, 0.5f);
		}
		public override void Update()
		{
			if (this.state == FarAreaEffect.EffectState.Start)
			{
				this.state = FarAreaEffect.EffectState.GiveDamage;
				SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("Neet_AOE", 1f, _self.view, _self.view, 1f);
				this._self.view.charAppearance.ChangeMotion(ActionDetail.S1);
				return;
			}
			else if (this.state == FarAreaEffect.EffectState.GiveDamage)
			{
				this._elapsed += Time.deltaTime;
				if (this._elapsed >= 2f)
				{
					this._elapsed = 0f;
					this.isRunning = false;
					this.state = FarAreaEffect.EffectState.End;
					return;
				}
			}
			else if (this.state == FarAreaEffect.EffectState.End)
			{
				this._elapsed += Time.deltaTime;
				if (this._elapsed > 4f)
				{
					this._self.view.charAppearance.ChangeMotion(ActionDetail.Default);
					this.state = FarAreaEffect.EffectState.None;
					this._elapsed = 0f;
					return;
				}
			}
			else if (this.state == FarAreaEffect.EffectState.None)
			{
				if (this._self.view.FormationReturned)
				{
					Destroy(base.gameObject);
				}
			}
		}
		private float _elapsed;
	}
	public class DiceAttackEffect_Udong_AOE : DiceAttackEffect
	{
		public override void Initialize(BattleUnitView self, BattleUnitView target, float destroyTime)
		{
			this._bHasDamagedEffect = true;
			this._self = self.model;
			this._target = target.model;
			this._selfTransform = self.atkEffectRoot;
			this._targetTransform = target.atkEffectRoot;
			this.atkdir = (((double)(target.WorldPosition - self.WorldPosition).x > 0.0) ? Direction.RIGHT : Direction.LEFT);
		}
		public override void Start()
		{
			AssetBundle assetBundle = EternalityInitializer.assetBundle["Eternality"];
			this.gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("P6"));
			this.gameObject.transform.parent = this._selfTransform;
			this.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			this.gameObject.transform.localScale = Vector3.one;
			this.gameObject.transform.localRotation = Quaternion.identity;
			this.gameObject.layer = 8;
			Transform[] componentsInChildren = this.gameObject.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.GetComponent<Renderer>() != null)
				{
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder < 0)
					{
						componentsInChildren[i].gameObject.layer = 20;
					}
					if (componentsInChildren[i].gameObject.GetComponent<Renderer>().sortingOrder >= 0)
					{
						componentsInChildren[i].gameObject.layer = 8;
					}
				}
			}
			this.gameObject.SetActive(true);
		}
		public Direction atkdir;
		public new GameObject gameObject;
		public BattleUnitModel _target;
	}
	public class BehaviourAction_Udong_AOE : BehaviourActionBase
	{
		public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
		{
			this._self = self;
			FarAreaEffect_Udong_AOE farAreaEffect = new GameObject().AddComponent<FarAreaEffect_Udong_AOE>();
			farAreaEffect.Init(self, Array.Empty<object>());
			return farAreaEffect;
		}
	}
	public class FarAreaEffect_Udong_AOE : FarAreaEffect
	{
		public override void Init(BattleUnitModel self, params object[] args)
		{
			base.Init(self, args);
			this.OnEffectStart();
			this._elapsed = 0f;
			Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay(0f, 0.5f);
		}
		public override void Update()
		{
			if (this.state == FarAreaEffect.EffectState.Start)
			{
				this._self.view.charAppearance.ChangeMotion(ActionDetail.S7);
				SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("Udong_AOE", 1f, _self.view, _self.view, 1f);
				this.state = FarAreaEffect.EffectState.GiveDamage;
				return;
			}
			else if (this.state == FarAreaEffect.EffectState.GiveDamage)
			{
				this._elapsed += Time.deltaTime;
				if (this._elapsed >= 1.6f)
				{
					this._elapsed = 0f;
					this.isRunning = false;
					this.state = FarAreaEffect.EffectState.End;
					
					return;
				}
			}
			else if (this.state == FarAreaEffect.EffectState.End)
			{
				this._elapsed += Time.deltaTime;
				if (this._elapsed > 1f)
				{
					this._self.view.charAppearance.ChangeMotion(ActionDetail.Default);
					this.state = FarAreaEffect.EffectState.None;
					this._elapsed = 0f;
					return;
				}
			}
			else if (this.state == FarAreaEffect.EffectState.None)
			{
				if (this._self.view.FormationReturned)
				{
					Destroy(base.gameObject);
				}
			}
		}
		private float _elapsed;
	}
}