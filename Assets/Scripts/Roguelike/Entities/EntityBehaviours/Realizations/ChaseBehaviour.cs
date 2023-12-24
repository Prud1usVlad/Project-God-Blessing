using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : AbstractChaseBehaviour
{
    private EnemyController _enemyController;
    private Transform _player;
    private NavMeshAgent _agent;

    public override void ChasePlayer()
    {
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

#if UNITY_EDITOR
    public override float GetSightRange()
    {
        return SightRange;
    }
#endif
}