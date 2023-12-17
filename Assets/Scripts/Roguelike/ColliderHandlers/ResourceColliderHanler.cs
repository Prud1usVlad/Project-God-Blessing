using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Roguelike.Entities.Player;
using UnityEngine;

public class ResourceColliderHanler : MonoBehaviour, IColliderHandler
{
    public Transform _InteractTransform;

    public Transform InteractTransform
    {
        get
        {
            return _InteractTransform;
        }
    }

    public float InteractDistance = 1f;

    private GameObject Player;
    private GameObject InteractionText;

    private PlayerInputController _playerInputController;
    private KeyValuePair<PlayerInteractDestination, IColliderHandler> _interactDestination;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag(TagHelper.PlayerTag);
        Transform uiText = GameObject.FindGameObjectWithTag(TagHelper.UITextTag).transform;
        _playerInputController = Player.GetComponent<PlayerInputController>();
        _interactDestination = new KeyValuePair<PlayerInteractDestination, IColliderHandler>(
                    PlayerInteractDestination.Collect, this);

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
        if (other.transform.tag.Equals(TagHelper.ColliderTags.PlayerInteractColliderTag))
        {
            InteractionText.SetActive(true);
            float segmentLength = Vector3.Distance(transform.parent.position, Player.transform.position);
            float t = InteractDistance / segmentLength;

            _InteractTransform.position = Vector3.Lerp(transform.parent.position, Player.transform.position, t);
            Vector3 directionToTarget = transform.position - Player.transform.position;

            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

            _InteractTransform.rotation = rotationToTarget;

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

    }
}
