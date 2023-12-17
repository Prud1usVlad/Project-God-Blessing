using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Action _dodgeFunc;
    private Action _meleeAttackFunc;
    private Action _throwFunc;
    private Action _collectFunc;
    private Action _openChestFunc;
    private Action _openDoorFunc;


    public GameObject PlayerObject;

    private void Start()
    {
        PlayerInputController playerInputController = PlayerObject.GetComponent<PlayerInputController>();
        _dodgeFunc += playerInputController.OnDodgeEnd;
        _meleeAttackFunc += playerInputController.OnMeleeAttackEnd;
        _throwFunc += playerInputController.OnGadgetThrowingEnd;
        _collectFunc += playerInputController.OnCollectEnd;
        _openChestFunc += playerInputController.OnOpenChestEnd;
        _openDoorFunc += playerInputController.OnOpenDoorEnd;
    }

    public void OnDodgeAnimationEnd()
    {
        _dodgeFunc.Invoke();
    }

    public void OnMeleeAttackAnimationEnd()
    {
        _meleeAttackFunc.Invoke();
    }

    public void OnThrowAnimationEnd()
    {
        _throwFunc.Invoke();
    }

    public void OnCollectAnimationEnd()
    {
        _collectFunc.Invoke();
    }

    public void OnOpenDoorAnimationEnd()
    {
        _openDoorFunc.Invoke();
    }

    public void OnOpenChestAnimationEnd()
    {
        _openChestFunc.Invoke();
    }

    public void OnDeathAnimationEnd()
    {

    }
}
