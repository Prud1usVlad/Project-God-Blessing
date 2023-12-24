using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Roguelike.Entities.Enemy;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyDecorator))]
public class EnemyController : MonoBehaviour
{
    public Transform HitMarkerContainer;
    public GameObject HitMarkerPrefab;
    public Animator EnemyAnimator;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlayer;

    public bool _isPlayerInSightRange;
    public bool _isPlayerInAttackRange;

    public Action OnDeathEvent;
    public Action OnAttackEnd;
    private List<AbstractAttackBehaviour> _attackBehaviours = new List<AbstractAttackBehaviour>();
    private AbstractChaseBehaviour _chaseBehaviour;
    private AbstractPatrolingBehaviour _patrolingBehaviour;
    private bool _isDead = false;
    public bool IsInAnimation;
    private Transform _player;
    private EnemyParameters _parameters;

    private void Start()
    {
        _parameters = GetComponent<EnemyDecorator>().EnemyParameters;
        _player = GameObject.FindWithTag(TagHelper.PlayerTag).transform;

        _attackBehaviours = GetComponents<AbstractAttackBehaviour>().OrderBy(x => x.Priority).ToList();
        _chaseBehaviour = GetComponent<AbstractChaseBehaviour>();
        _patrolingBehaviour = GetComponent<AbstractPatrolingBehaviour>();

        if (!_attackBehaviours.Any())
        {
            throw new Exception("Empty attack list");
        }

        _chaseBehaviour.Initialize(_player);
        _patrolingBehaviour.Initialize(_player);

        foreach (IAttackBehaviour attackBehaviour in _attackBehaviours)
        {
            attackBehaviour.Initialize(_player);
        }
    }

    private void Update()
    {
        if (_isDead || IsInAnimation)
        {
            return;
        }

        _isPlayerInSightRange = _chaseBehaviour.CheckPlayerSightable();

        foreach (IAttackBehaviour attackBehaviour in _attackBehaviours)
        {
            _isPlayerInAttackRange = attackBehaviour.CheckAttackRange();

            if (!_isPlayerInSightRange && !_isPlayerInAttackRange)
            {
                _patrolingBehaviour.Patroling();
            }
            if (_isPlayerInSightRange && !_isPlayerInAttackRange)
            {
                _chaseBehaviour.ChasePlayer();
            }
            if (_isPlayerInAttackRange && _isPlayerInSightRange
                && attackBehaviour.CheckAttackPossibility()
                && PlayerStateHelper.Instance.PlayerState.Equals(PlayerState.InGame))
            {
                IsInAnimation = true;
                attackBehaviour.Attack();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (_isDead)
        {
            return;
        }
        _parameters.EnemyBaseHealth -= damage;

        GameObject hitMarker = Instantiate(HitMarkerPrefab, HitMarkerContainer.position, HitMarkerPrefab.transform.rotation);
        hitMarker.GetComponent<TextMesh>().text = damage.ToString();

        if (_parameters.EnemyBaseHealth <= 0 && !_isDead)
        {
            EnemyAnimator.SetBool(AnimatorHelper.EnemyAnimator.OrcAnimator.IsWalkParameter, false);
            EnemyAnimator.SetTrigger(AnimatorHelper.EnemyAnimator.OrcAnimator.DieTrigger);
            _isDead = true;
            OnDeathEvent.Invoke();
            Destroy(gameObject, _parameters.EnemyBaseDisapearingTime);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (IAttackBehaviour attackBehaviour in _attackBehaviours)
        {
            Gizmos.DrawWireSphere(transform.position, attackBehaviour.GetAttackRange());
        }
        if (_chaseBehaviour != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _chaseBehaviour.GetSightRange());
        }
    }
#endif
}
