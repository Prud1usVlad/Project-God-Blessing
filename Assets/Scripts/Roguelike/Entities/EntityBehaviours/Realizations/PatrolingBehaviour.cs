using Assets.Scripts.Helpers;
using UnityEngine;

public class PatrolingBehaviour : AbstractPatrolingBehaviour
{
    private Vector3 walkPoint;
    public float WalkPointRange;

    private EnemyController _enemyController;
    private Transform _player;
    private UnityEngine.AI.NavMeshAgent _agent;
    private bool _walkPointSet;
    private bool _isDead = false;

    public override void Patroling()
    {
        if(_isDead)
        {
            return;
        }
        
        _enemyController.EnemyAnimator.SetBool(AnimatorHelper.EnemyAnimator.OrcAnimator.IsWalkParameter, true);

        if (!_walkPointSet)
        {
            SearchWalkPoint();
        }

        if (_walkPointSet)
        {
            _agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            _walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
        float randomX = Random.Range(-WalkPointRange, WalkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, _enemyController.WhatIsGround))
        {
            _walkPointSet = true;
        }
    }

    public override void OnDeathEvent()
    {
        _isDead = true;
        _agent.SetDestination(transform.position);
    }


    public override void Initialize(Transform player)
    {
        _enemyController = GetComponent<EnemyController>();
        _player = player;
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
}