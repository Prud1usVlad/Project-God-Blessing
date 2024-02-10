using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : AbstractChaseBehaviour
{
    private EnemyController _enemyController;
    private Transform _player;
    private NavMeshAgent _agent;
    private bool _isDead = false;

    public override void ChasePlayer()
    {
        if(_isDead)
        {
            return;
        }
        _enemyController.EnemyAnimator.SetBool(AnimatorHelper.EnemyAnimator.OrcAnimator.IsWalkParameter, true);
        _agent.SetDestination(_player.position);
    }

    public override bool CheckPlayerSightable()
    {
        return Physics.CheckSphere(transform.position, SightRange, _enemyController.WhatIsPlayer);
    }

    public override void Initialize(Transform player)
    {
        _enemyController = GetComponent<EnemyController>();
        _player = player;
        _agent = GetComponent<NavMeshAgent>();
    }

    public override void OnDeathEvent()
    {
        _isDead = true;
        _agent.SetDestination(transform.position);
    }

#if UNITY_EDITOR
    public override float GetSightRange()
    {
        return SightRange;
    }
#endif
}