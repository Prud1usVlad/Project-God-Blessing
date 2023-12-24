using System;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Roguelike.Entities.Enemy;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class SpawnAttackBehaviour : AbstractAttackBehaviour
{
    public GameObject SpawnObject;
    public List<Transform> SpawnPositions;

    [SerializeField]
    private AttackParameters _attackParameters;
    private EnemyController _enemyController;
    private Transform _player;
    private Transform _playerHitboxCollider;
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
        AttackType = AttackType.Summon;
    }

    private void OnResetAttack()
    {
        if (IsAttackCooldown)
        {
            return;
        }

        _isAttack = false;
        IsAttackCooldown = true;
        _enemyController.IsInAnimation = false;

        Invoke(nameof(ResetAttackCooldown), AttackParameters.BaseCooldown / _parameters.EnemyBaseAttackSpeed);
    }

    private void ResetAttackCooldown()
    {
        _enemyController.OnSpawnEnd -= OnResetAttack;
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
            _enemyController.OnSpawnEnd += OnResetAttack;
            _enemyController.OnSpawnEvent += Spawn;
            _isAttack = true;
        }
    }

    private void Spawn()
    {
        foreach (Transform position in SpawnPositions)
        {
            GameObject spawnedObject = Instantiate(SpawnObject, position.position, Quaternion.identity, transform);
            spawnedObject.transform.LookAt(_playerHitboxCollider);
        }

        _enemyController.OnSpawnEvent -= Spawn;
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
        _playerHitboxCollider = GameObject.FindWithTag(TagHelper.ColliderTags.PlayerHitboxColliderTag).transform;
        _attackTypeAnimationTag = AttackTypeHandler.GetAttackTypeAnimationTag(AttackType);
    }

#if UNITY_EDITOR
    public override float GetAttackRange()
    {
        return AttackParameters.BaseAttackDistance;
    }
#endif
}