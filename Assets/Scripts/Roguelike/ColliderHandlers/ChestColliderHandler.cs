using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Roguelike.Entities.Player;
using UnityEngine;

public class ChestColliderHandler : MonoBehaviour, IColliderHandler
{
    public Transform _InteractTransform;

    public Transform InteractTransform
    {
        get
        {
            return _InteractTransform;
        }
    }
    public Animator Animator;
    public TreasureLootController TreasureLootController;

    private GameObject Player;
    private GameObject InteractionText;

    private PlayerInputController _playerInputController;
    private KeyValuePair<PlayerInteractDestination, IColliderHandler> _interactDestination;

    private bool _isOpened = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag(TagHelper.PlayerTag);
        Transform uiText = GameObject.FindGameObjectWithTag(TagHelper.UITextTag).transform;
        _playerInputController = Player.GetComponent<PlayerInputController>();
        _interactDestination = new KeyValuePair<PlayerInteractDestination, IColliderHandler>(
                    PlayerInteractDestination.OpenChest, this);

        foreach (Transform child in uiText)
        {
            if (child.tag.Equals(TagHelper.PlayerInteractUITextTag))
            {
                InteractionText = child.gameObject;
                InteractionText.SetActive(false);

                break;
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(_isOpened)
        {
            return;
        }
        if (other.transform.tag.Equals(TagHelper.ColliderTags.PlayerInteractColliderTag))
        {
            InteractionText.SetActive(true);

            _playerInputController.InteractDestination.Insert(0, _interactDestination);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag.Equals(TagHelper.ColliderTags.PlayerInteractColliderTag))
        {
            int index = _playerInputController.InteractDestination
                    .IndexOf(_interactDestination);
            if (index != -1)
            {
                _playerInputController.InteractDestination
                                .RemoveAt(index);
            }

        }
    }

    public void Interact()
    {
        _isOpened = true;
        Animator.SetTrigger(AnimatorHelper.TreasureAnimator.OpenChestTrigger);
        TreasureLootController.Loot();
    }
}
