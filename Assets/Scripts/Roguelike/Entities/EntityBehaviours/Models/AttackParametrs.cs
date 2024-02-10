using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RogueLike/AttackParameters", fileName = "AttackParameters")]
public class AttackParameters : ScriptableObject
{
    public float BaseDamege;
    public float BaseCooldown;
    public float BaseAttackDistance;

    public AttackParameters Copy()
    {
        AttackParameters parameters = CreateInstance<AttackParameters>();
        parameters.BaseDamege = BaseDamege;
        parameters.BaseCooldown = BaseCooldown;
        parameters.BaseAttackDistance = BaseAttackDistance;

        return parameters;
    }
}