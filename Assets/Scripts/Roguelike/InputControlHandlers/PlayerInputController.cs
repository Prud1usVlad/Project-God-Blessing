using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Roguelike.Entities.Player;
using Assets.Scripts.Roguelike.LevelGeneration;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerInputController : MonoBehaviour
{
    [Header("Player movement parameters")]
    public float MovementSpeed = 10f;
    public float DashSpeedBoost = 2f;
    public int DodgeFrameAmount = 15;
    public float DodgeSpeed = .01f;
    [Header("Game objects")]
    public GameObject Player;
    public Camera PlayerCamera;
    public Animator Animator;
    public GameObject Weapon;
    public GameObject Gadget;
    public GameObject Level;

    [Header("UI Elements")]
    public GameObject InteractionText;
    public GameObject GadgetWheel;
    public GameObject LootTab;

    [NonSerialized]
    public Action Interact;
    [NonSerialized]
    public List<KeyValuePair<PlayerInteractDestination, IInteractColliderHandler>> InteractDestination;

    private Action _leftMovement;
    private Action _bottomMovement;
    private Action _rightMovement;
    private Action _topMovement;
    private Action _dodge;
    private Action _meleeAttack;
    private Action _gadgetThrow;
    private Action _collect;
    private Action _openChest;
    private Action _openDoor;
    private Action _openGadgetWheel;
    private Action _openLootTab;
    private Action _death;

    private Vector3 _movementLeftVector = new Vector3(-1, 0, 0);
    private Vector3 _movementBottomVector = new Vector3(0, 0, -1);
    private Vector3 _movementRightVector = new Vector3(1, 0, 0);
    private Vector3 _movementTopVector = new Vector3(0, 0, 1);
    private Vector3 _lastMousePosition;

    private bool _isDash = false;
    private float _speed = 0f;
    private bool _isDodge = false;
    private bool _isStaticAnimation = false;
    private bool _isDead = false;
    private int _currentFixedDodgeFrame;
    private Vector3 _dodgeDirection;
    private Vector3 _lastDodgeDirection;

    private Rigidbody _rigidBody;
    private Weapon _weapon;
    private Player _player;
    private ObjectGadgetHandler _objectGadgetHandler;

    private GameObject _gadgetObject = null;
    private RaycastHit _hit = default;

    public void OnDodgeEnd()
    {
        _isDodge = false;
        _isStaticAnimation = false;

        _currentFixedDodgeFrame = 0;
        _rigidBody.velocity = Vector3.zero;
    }

    public void OnMeleeAttackEnd()
    {
        _weapon.EndAttack();
        _isStaticAnimation = false;
        _rigidBody.velocity = Vector3.zero;
    }

    public void OnGadgetThrowingEnd()
    {
        _isStaticAnimation = false;
        _rigidBody.velocity = Vector3.zero;
        Weapon.SetActive(true);
    }

    public void OnCollectEnd()
    {
        _isStaticAnimation = false;
        _rigidBody.velocity = Vector3.zero;
        Weapon.SetActive(true);
        InteractDestination[0].Value.Interact();
        InteractDestination.RemoveAt(0);
    }

    public void OnOpenChestEnd()
    {
        _isStaticAnimation = false;
        _rigidBody.velocity = Vector3.zero;
        Weapon.SetActive(true);
        InteractDestination[0].Value.Interact();
        InteractDestination.RemoveAt(0);
    }

    public void OnOpenDoorEnd()
    {
        _isStaticAnimation = false;
        _rigidBody.velocity = Vector3.zero;
        Weapon.SetActive(true);
        InteractDestination[0].Value.Interact();
        InteractDestination.RemoveAt(0);
    }

    public void OnRestart()
    {
        _isDead = false;
        _isStaticAnimation = false;
        PlayerStateHelper.Instance.PlayerState = PlayerState.InGame;
        Animator.SetTrigger(AnimatorHelper.PlayerAnimator.ReviveTrigger);
        _rigidBody.velocity = Vector3.zero;
    }

    public void OnThrow()
    {
        Transform currentRoom = Level.GetComponent<LevelRoomSpawner>().GetCurrentRoom().transform;
        _gadgetObject.transform.SetParent(currentRoom);

        Vector3 hitPoint = _hit.point;
        hitPoint.y = transform.position.y;

        _gadgetObject.GetComponent<GadgetController>().Throw(hitPoint);
        _gadgetObject = null;
        _hit = default;
    }


    private void Start()
    {
        GadgetWheel.SetActive(false);
        InteractDestination = new List<KeyValuePair<PlayerInteractDestination, IInteractColliderHandler>>();
        _rigidBody = GetComponent<Rigidbody>();
        _weapon = Weapon.GetComponent<Weapon>();
        _player = GetComponent<Player>();
        _objectGadgetHandler = Gadget.GetComponent<ObjectGadgetHandler>();
        LootTab.SetActive(false);

        _leftMovement += delegate ()
        {
            if (Input.GetKey(KeyCode.A))
            {
                moveLeft();
            }

            if (Input.GetKeyUp(KeyCode.A) && _rigidBody.velocity.x < 0)
            {
                _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, _rigidBody.velocity.z);
            }
        };

        _bottomMovement += delegate ()
        {
            if (Input.GetKey(KeyCode.S))
            {
                moveBottom();
            }

            if (Input.GetKeyUp(KeyCode.S) && _rigidBody.velocity.z < 0)
            {
                _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _rigidBody.velocity.y, 0);
            }
        };

        _rightMovement += delegate ()
        {
            if (Input.GetKey(KeyCode.D))
            {
                moveRight();
            }

            if (Input.GetKeyUp(KeyCode.D) && _rigidBody.velocity.x > 0)
            {
                _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, _rigidBody.velocity.z);
            }
        };

        _topMovement += delegate ()
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveTop();
            }

            if (Input.GetKeyUp(KeyCode.W) && _rigidBody.velocity.z > 0)
            {
                _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _rigidBody.velocity.y, 0);
            }
        };

        _dodge += delegate ()
        {
            if (_isStaticAnimation)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidBody.velocity = Vector3.zero;
                _isDodge = true;
                _isStaticAnimation = true;
                Animator.SetTrigger(AnimatorHelper.PlayerAnimator.DodgeTrigger);
                float horisontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                if (horisontal != 0 || vertical != 0)
                {
                    _dodgeDirection = new Vector3(horisontal, 0, vertical);
                    _lastMousePosition = Input.mousePosition;
                }
                else
                {
                    if (_lastMousePosition != Input.mousePosition)
                    {
                        _dodgeDirection = Input.mousePosition;
                        _dodgeDirection.x = (_dodgeDirection.x - Screen.width / 2);
                        _dodgeDirection.z = (_dodgeDirection.y - Screen.height / 2);

                        if (Math.Abs(_dodgeDirection.x) > Math.Abs(_dodgeDirection.z))
                        {
                            _dodgeDirection.z = (_dodgeDirection.z < 0 && _dodgeDirection.z < 0 ? -1 : 1)
                                * _dodgeDirection.z / _dodgeDirection.x;
                            _dodgeDirection.x = _dodgeDirection.x > 0 ? 1 : -1;
                        }
                        else
                        {
                            _dodgeDirection.x = (_dodgeDirection.z < 0 && _dodgeDirection.z < 0 ? -1 : 1)
                                * _dodgeDirection.x / _dodgeDirection.z;
                            _dodgeDirection.z = _dodgeDirection.z > 0 ? 1 : -1;
                        }

                        _dodgeDirection.y = 0;
                    }
                    else
                    {
                        _dodgeDirection = _lastDodgeDirection;
                    }
                }
                _lastDodgeDirection = _dodgeDirection;

                _rigidBody.velocity = _dodgeDirection * DodgeSpeed;
            };
        };

        _meleeAttack += delegate ()
        {
            if (_isStaticAnimation)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _weapon.StartAttack();
                _rigidBody.velocity = Vector3.zero;
                _isStaticAnimation = true;
                setPlayerDirectionByMouse();
                Animator.SetTrigger(AnimatorHelper.PlayerAnimator.MeleeAttackTrigger);
            }
        };

        _death += delegate ()
        {
            if (_isDead)
            {
                return;
            }

            if (_player.Health <= 0)
            {
                _isDead = true;
                PlayerStateHelper.Instance.PlayerState = PlayerState.Dead;
                Animator.SetTrigger(AnimatorHelper.PlayerAnimator.DeathTrigger);
                _rigidBody.velocity = Vector3.zero;
            }
        };

        _gadgetThrow += delegate ()
        {
            if (_isStaticAnimation)
            {
                return;
            }

            if (Input.GetMouseButtonUp(1))
            {
                Weapon.SetActive(false);
                _isStaticAnimation = true;
                setPlayerDirectionByMouse();

                GameObject instance = Instantiate(GadgetHandler.Instance.Gadgets[GadgetHandler.Instance.CurrentGudgetNumber],
                    Gadget.transform.position, Gadget.transform.rotation);
                Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out _hit);

                _gadgetObject = instance;
                _gadgetObject.transform.SetParent(Gadget.transform);

                Animator.SetTrigger(AnimatorHelper.PlayerAnimator.GadgetTrigger);
                _rigidBody.velocity = Vector3.zero;
            }
        };

        _collect += delegate ()
        {
            if (_isStaticAnimation)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Weapon.SetActive(false);

                Transform interactTransform = InteractDestination[0].Value.InteractTransform;
                transform.position = new Vector3(
                    interactTransform.position.x,
                    Player.transform.position.y,
                    interactTransform.position.z);
                Player.transform.rotation = interactTransform.rotation;

                _isStaticAnimation = true;
                Animator.SetTrigger(AnimatorHelper.PlayerAnimator.CollectTrigger);
                _rigidBody.velocity = Vector3.zero;
            }
        };

        _openChest += delegate ()
        {
            if (_isDead)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Weapon.SetActive(false);

                Transform interactTransform = InteractDestination[0].Value.InteractTransform;
                transform.position = new Vector3(
                    interactTransform.position.x,
                    Player.transform.position.y,
                    interactTransform.position.z);
                Player.transform.rotation = interactTransform.rotation;
                _isStaticAnimation = true;
                Animator.SetTrigger(AnimatorHelper.PlayerAnimator.OpenChestTrigger);
                _rigidBody.velocity = Vector3.zero;
            }
        };

        _openDoor += delegate ()
        {
            if (_isStaticAnimation)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Weapon.SetActive(false);

                Transform interactTransform = InteractDestination[0].Value.InteractTransform;
                transform.position = new Vector3(
                    interactTransform.position.x,
                    Player.transform.position.y,
                    interactTransform.position.z);
                Player.transform.rotation = interactTransform.rotation;

                _isStaticAnimation = true;
                Animator.SetTrigger(AnimatorHelper.PlayerAnimator.OpenDoorTrigger);
                _rigidBody.velocity = Vector3.zero;
            }
        };

        Interact += delegate ()
        {
            if (_isStaticAnimation || !InteractDestination.Any())
            {
                return;
            }

            switch (InteractDestination[0].Key)
            {
                case PlayerInteractDestination.None:
                    return;
                case PlayerInteractDestination.OpenChest:
                    _openChest.Invoke();
                    break;
                case PlayerInteractDestination.OpenDoor:
                    _openDoor.Invoke();
                    return;
                case PlayerInteractDestination.Collect:
                    _collect.Invoke();
                    return;
                default:
                    throw new Exception("Unhandled interact destination");
            }
        };

        _openGadgetWheel += delegate ()
        {
            if (Input.GetKeyDown(KeyCode.Q)
                && !PlayerStateHelper.Instance.PlayerState.Equals(PlayerState.Pause))
            {
                PlayerStateHelper.Instance.PlayerState = PlayerState.Pause;
                GadgetWheel.SetActive(true);
                //Time.timeScale = 0.1f;
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                PlayerStateHelper.Instance.PlayerState = PlayerState.InGame;
                GadgetWheel.SetActive(false);
                //Time.timeScale = 1f;
            }
        };

        _openLootTab += delegate ()
        {
            if (Input.GetKeyDown(KeyCode.Tab)
                && !PlayerStateHelper.Instance.PlayerState.Equals(PlayerState.Pause))
            {
                LootTab.SetActive(true);
                PlayerStateHelper.Instance.PlayerState = PlayerState.Pause;
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                LootTab.SetActive(false);
                PlayerStateHelper.Instance.PlayerState = PlayerState.InGame;
            }
        };
    }

    private void Update()
    {
        _death.Invoke();
        if (_isDead)
        {
            return;
        }

        InteractionText.SetActive(InteractDestination.Any());

        _openGadgetWheel.Invoke();
        _openLootTab.Invoke();

        if (_isStaticAnimation || PlayerStateHelper.Instance.PlayerState.Equals(PlayerState.Pause))
        {
            return;
        }

        _dodge.Invoke();
        _meleeAttack.Invoke();
        _gadgetThrow.Invoke();

        Interact.Invoke();

        if (_isStaticAnimation)
        {
            return;
        }

        float horisontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _rigidBody.velocity = new Vector3(horisontal != 0 ? _rigidBody.velocity.x : 0,
            0,
            vertical != 0 ? _rigidBody.velocity.z : 0);

        _isDash = Input.GetKey(KeyCode.LeftShift);
        _speed = MovementSpeed * (_isDash ? DashSpeedBoost : 1);

        _leftMovement.Invoke();
        _bottomMovement.Invoke();
        _rightMovement.Invoke();
        _topMovement.Invoke();

        float maxSpeed = Math.Max(
            Math.Abs(_rigidBody.velocity.x),
            Math.Abs(_rigidBody.velocity.z));

        Animator.SetFloat(AnimatorHelper.PlayerAnimator.MovementSpeedParameter, maxSpeed);
        Animator.SetBool(AnimatorHelper.PlayerAnimator.IsDashParameter, _isDash);

        if (horisontal != 0 || vertical != 0)
        {
            Player.transform.LookAt(Player.transform.position + new Vector3(horisontal, 0, vertical));
            _lastMousePosition = Input.mousePosition;
        }
        else
        {
            if (_lastMousePosition != Input.mousePosition)
            {
                setPlayerDirectionByMouse();
            }
        }

    }

    private void FixedUpdate()
    {
        if (_isDodge)
        {
            _currentFixedDodgeFrame++;
        }
    }

    private void setPlayerDirectionByMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.x = mousePosition.x - Screen.width / 2;
        mousePosition.z = mousePosition.y - Screen.height / 2;
        mousePosition.y = 0;

        Player.transform.LookAt(Player.transform.position + mousePosition);
    }

    private void moveRight()
    {
        if (_rigidBody.velocity.x < _speed)
        {
            _rigidBody.velocity += _movementRightVector * _speed;
        }
    }

    private void moveBottom()
    {
        if (_rigidBody.velocity.z > -_speed)
        {
            _rigidBody.velocity += _movementBottomVector * _speed;
        }
    }

    private void moveLeft()
    {
        if (_rigidBody.velocity.x > -_speed)
        {
            _rigidBody.velocity += _movementLeftVector * _speed;
        }
    }

    private void moveTop()
    {
        if (_rigidBody.velocity.z < _speed)
        {
            _rigidBody.velocity += _movementTopVector * _speed;
        }
    }
}
