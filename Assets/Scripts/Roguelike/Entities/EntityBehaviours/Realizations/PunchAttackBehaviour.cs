using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Roguelike.Entities.Enemy;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class PunchAttackBehaviour : AbstractAttackBehaviour
{
    public GameObject AttackCollider;
    [SerializeField]
    private AttackParameters _attackParameters;
    private EnemyController _enemyController;
    private Transform _player;
    private NavMeshAgent _agent;
    private EnemyParameters _parameters;
    private string _attackTypeAnimationTag;
    private bool _isAttack = false;
    public override AttackParameters AttackParameters
    {
        get
        {
            return _attackParameters;
        }
        set
        {
            _attackParameters = _attackParameters.Copy();
        }
    }

    private void Start()
    {
        AttackParameters = _attackParameters;
        AttackType = AttackType.Punch;
    }
    private void OnResetAttack()
    {
        if (IsAttackCooldown)
        {
            return;
        }

        AttackCollider.SetActive(false);
        _isAttack = false;
        IsAttackCooldown = true;
        _enemyController.IsInAnimation = false;

        Invoke(nameof(ResetAttackCooldown), AttackParameters.BaseCooldown / _parameters.EnemyBaseAttackSpeed);
    }

    private void ResetAttackCooldown()
    {
        _enemyController.OnPunchEnd -= OnResetAttack;
        IsAttackCooldown = false;
    }

    public override void Attack()
    {
        if (IsAttackCooldown || _isAttack)
        {
            return;
        }

        _enemyController.EnemyAnimator.SetBool(AnimatorHelper.EnemyAnimator.OrcAnimator.IsWalkParameter, false);
        _enemyController.EnemyAnimator.SetTrigger(_attackTypeAnimationTag);

        _agent.SetDestination(transform.position);

        transform.LookAt(_player);

        if (!_isAttack)
        {
            _enemyController.OnPunchEnd += OnResetAttack;
            AttackCollider.SetActive(true);
            _isAttack = true;
        }
    }

    public override bool CheckAttackRange()
    {
        return Physics.CheckSphere(transform.position, AttackParameters.BaseAttackDistance, _enemyController.WhatIsPlayer);
    }

    public override bool CheckAttackPossibility()
    {
        return !_isAttack && !IsAttackCooldown;
    }

    public override void Initialize(Transform player)
    {
        _enemyController = GetComponent<EnemyController>();
        _player = player;
        _agent = GetComponent<NavMeshAgent>();
        _parameters = GetComponent<EnemyDecorator>().EnemyParameters;
        _attackTypeAnimationTag = AttackTypeHandler.GetAttackTypeAnimationTag(AttackType);
    }

#if UNITY_EDITOR
    public override float GetAttackRange()
    {
        return AttackParameters.BaseAttackDistance;
    }
#endif
}