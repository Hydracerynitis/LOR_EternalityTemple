using System;
using UnityEngine;
public class PassiveAbility_226769020 : PassiveAbility_226769019
{
    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
        base.OnSucceedAttack(behavior);
        owner.RecoverHP(2);
        owner.breakDetail.RecoverBreak(2);
    }
}