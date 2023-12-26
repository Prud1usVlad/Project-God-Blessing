using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.AI;

public class KeepingDistanceChaseBehaviour : AbstractChaseBehaviour
{
    public float Distance;
    private EnemyController _enemyController;
    private Transform _player;
    private NavMeshAgent _agent;
    private bool _isDead = false;
    private Vector3 getChasePosition()
    {
        Vector3 vector = transform.position - _player.position;
        Vector3 direction = vector.normalized;

        return _player.position + Distance * direction;
    }

    public override void ChasePlayer()
    {
        if(_isDead)
        {
            return;
        }
        _enemyController.EnemyAnimator.SetBool(AnimatorHelper.EnemyAnimator.OrcAnimator.IsWalkParameter, true);
        _agent.SetDestination(getChasePosition());
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