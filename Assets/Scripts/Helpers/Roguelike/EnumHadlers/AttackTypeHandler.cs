using System;
using Assets.Scripts.Helpers;

public static class AttackTypeHandler
{
    public static string GetAttackTypeAnimationTag(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Bite:
                return AnimatorHelper.EnemyAnimator.Attack.BiteTag;
            case AttackType.Punch:
                return AnimatorHelper.EnemyAnimator.Attack.PunchTag;
            case AttackType.Throw:
                return AnimatorHelper.EnemyAnimator.Attack.ThrowTag;
            case AttackType.Summon:
                return AnimatorHelper.EnemyAnimator.Attack.SummonTag;
            default:
                throw new Exception("Unhandled attack type");
        }
    }
}