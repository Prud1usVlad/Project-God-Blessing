using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Roguelike.Entities.Enemy;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyDecorator))]
public class EnemyController : MonoBehaviour
{
    public GameObject AttackCollider;
    public Transform HitMarkerContainer;
    public GameObject HitMarkerPrefab;
    public Animator EnemyAnimator;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlayer;

    public Vector3 walkPoint;

    public float WalkPointRange;

    public float SightRange = 100f;
    public float AttackRange = 5f;
    public float AttackCooldown = 0.5f;
    public bool _isPlayerInSightRange;
    public bool _isPlayerInAttackRange;

    public Action OnDeathEvent;

    private bool _isAttack;
    private bool _walkPointSet;
    private bool _isDead = false;
    private bool _isInAnimation = false;
    private bool _isInPunchCooldown = false;
    private NavMeshAgent _agent;
    private Transform _player;
    private EnemyParameters _parameters;

    private void Start()
    {
        //Physics.IgnoreCollision(transform.GetComponent<Collider>(), _player.GetComponent<Collider>(), true);
        _agent = GetComponent<NavMeshAgent>();
        _parameters = GetComponent<EnemyDecorator>().EnemyParameters;
        _player = GameObject.FindWithTag(TagHelper.PlayerTag).transform;
    }

    private void Update()
    {
        if (_isDead || _isInAnimation || _isInPunchCooldown)
        {
            return;
        }

        _isPlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        _isPlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if (!_isPlayerInSightRange && !_isPlayerInAttackRange)
        {
            EnemyAnimator.SetBool(AnimatorHelper.EnemyAnimators.OrcAnimator.IsWalkParameter, true);
            Patroling();
        }
        if (_isPlayerInSightRange && !_isPlayerInAttackRange)
        {
            EnemyAnimator.SetBool(AnimatorHelper.EnemyAnimators.OrcAnimator.IsWalkParameter, true);
            ChasePlayer();
        }
        if (_isPlayerInAttackRange && _isPlayerInSightRange && PlayerStateHelper.Instance.PlayerState.Equals(PlayerState.InGame))
        {
            EnemyAnimator.SetBool(AnimatorHelper.EnemyAnimators.OrcAnimator.IsWalkParameter, false);
            EnemyAnimator.SetTrigger(AnimatorHelper.EnemyAnimators.OrcAnimator.PunchTrigger);
            AttackPlayer();
        }
    }

    private void Patroling()
    {
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

        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
            _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        _agent.SetDestination(transform.position);

        transform.LookAt(_player);

        if (!_isAttack)
        {
            AttackCollider.SetActive(true);
            _isAttack = true;
            _isInAnimation = true;
        }
    }

    public void OnResetAttack()
    {
        AttackCollider.SetActive(false);
        _isInAnimation = false;
        _isAttack = false;
        _isInPunchCooldown = true;

        Invoke(nameof(ResetAttackCooldown), AttackCooldown / _parameters.EnemyBaseAttackSpeed);
    }

    private void ResetAttackCooldown()
    {
        _isInPunchCooldown = false;
    }

    public void TakeDamage(float damage)
    {
        if(_isDead)
        {
            return;
        }
        _parameters.EnemyBaseHealth -= damage;

        GameObject hitMarker = Instantiate(HitMarkerPrefab, HitMarkerContainer.position, HitMarkerPrefab.transform.rotation);
        hitMarker.GetComponent<TextMesh>().text = damage.ToString();

        if (_parameters.EnemyBaseHealth <= 0 && !_isDead)
        {
            EnemyAnimator.SetBool(AnimatorHelper.EnemyAnimators.OrcAnimator.IsWalkParameter, false);
            EnemyAnimator.SetTrigger(AnimatorHelper.EnemyAnimators.OrcAnimator.DieTrigger);
            _isDead = true;
            OnDeathEvent.Invoke();
            Destroy(gameObject, _parameters.EnemyBaseDisapearingTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }
}
