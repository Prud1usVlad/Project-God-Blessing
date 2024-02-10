using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.Roguelike.Minimap;
using Assets.Scripts.Roguelike;
using Assets.Scripts.Roguelike.Entities.Player;
using UnityEngine;

public class DoorColliderHandler : MonoBehaviour, IInteractColliderHandler
{

    public Transform _InteractTransform;

    public Transform InteractTransform
    {
        get
        {
            return _InteractTransform;
        }
    }

    private GameObject _player;
    private GameObject _interactionText;
    private WallDecorator _wallDecorator;

    private PlayerInputController _playerInputController;
    private KeyValuePair<PlayerInteractDestination, IInteractColliderHandler> _interactDestination;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(TagHelper.PlayerTag);
        Transform uiText = GameObject.FindGameObjectWithTag(TagHelper.UITextTag).transform;
        _playerInputController = _player.GetComponent<PlayerInputController>();
        _interactDestination = new KeyValuePair<PlayerInteractDestination, IInteractColliderHandler>(
                    PlayerInteractDestination.OpenDoor, this);

        foreach (Transform child in uiText)
        {
            if (child.tag.Equals(TagHelper.PlayerInteractUITextTag))
            {
                _interactionText = child.gameObject;
                _interactionText.SetActive(false);

                break;
            }
        }

        _wallDecorator = transform.parent.parent.parent.GetComponent<WallDecorator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(TagHelper.ColliderTags.PlayerInteractColliderTag))
        {
            _interactionText.SetActive(true);

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
        if (_wallDecorator.BottomDoor == transform.parent.gameObject)
        {
            RoomMovementController.Instance.MoveDownDoorFunc.Invoke();
        }

        if (_wallDecorator.RightDoor == transform.parent.gameObject)
        {
            RoomMovementController.Instance.MoveRightDoorFunc.Invoke();
        }

        if (_wallDecorator.TopDoor == transform.parent.gameObject)
        {
            RoomMovementController.Instance.MoveTopDoorFunc.Invoke();
        }

        if (_wallDecorator.LeftDoor == transform.parent.gameObject)
        {
            RoomMovementController.Instance.MoveLeftDoorFunc.Invoke();
        }
    }
}

