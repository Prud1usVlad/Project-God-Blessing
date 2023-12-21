using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.Roguelike;
using UnityEngine;

namespace Assets.Scripts.Roguelike.Map
{
    public class Map : MonoBehaviour
    {
        public Camera MapCamera;

        public GameObject Minimap;

        public GameObject MinimapUIObject;

        public GameObject MainCharacterIcon;

        public Action OpenMapFunc;

        private bool _isMapOpened = false;

        private Vector3 _positionDifference;
        private Vector3 _basePosition;
        private Vector3 _mousePosition;

        private bool _drag = false;

        private float? _leftBorder;
        private float? _rightBorder;
        private float? _topBorder;
        private float? _bottomBorder;

        private void Start()
        {
            OpenMapFunc = OpenMap;
            _basePosition = MainCharacterIcon.transform.position;
            _basePosition.z = MapCamera.transform.position.z;

            transform.gameObject.SetActive(false);
        }

        private void OpenMap()
        {
            if (_isMapOpened)
            {
                transform.gameObject.SetActive(false);
                MinimapUIObject.SetActive(true);
            }
            else
            {
                if (PlayerStateHelper.Instance.PlayerState.Equals(PlayerState.Pause))
                {
                    return;
                }

                _basePosition = MainCharacterIcon.transform.position;
                _basePosition.z = MapCamera.transform.position.z;
                MapCamera.transform.position = _basePosition;
                MinimapUIObject.SetActive(false);
                transform.gameObject.SetActive(true);

                if (!_leftBorder.HasValue || !_rightBorder.HasValue
                || !_topBorder.HasValue || !_bottomBorder.HasValue)
                {
                    _leftBorder = float.MaxValue;
                    _rightBorder = float.MinValue;
                    _bottomBorder = float.MaxValue;
                    _topBorder = float.MinValue;

                    foreach (Transform child in Minimap.gameObject.transform)
                    {
                        if (child.tag.Equals(TagHelper.MinimapRoomTag))
                        {
                            _leftBorder = child.position.x < _leftBorder ? child.position.x : _leftBorder;
                            _rightBorder = child.position.x > _rightBorder ? child.position.x : _rightBorder;
                            _bottomBorder = child.position.y < _bottomBorder ? child.position.y : _bottomBorder;
                            _topBorder = child.position.y > _topBorder ? child.position.y : _topBorder;
                        }
                    }
                }
            }

            _isMapOpened = !_isMapOpened;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mousePosition = Input.mousePosition;
            }
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                _positionDifference = _mousePosition - Input.mousePosition;
                if (_drag == false)
                {
                    _drag = true;
                }
            }
            else
            {
                _drag = false;
            }

            if (_drag)
            {
                MapCamera.transform.position += _positionDifference / 30;

                float x = MapCamera.transform.position.x < _leftBorder
                    ? _leftBorder.Value
                    : MapCamera.transform.position.x > _rightBorder
                        ? _rightBorder.Value
                        : MapCamera.transform.position.x;

                float y = MapCamera.transform.position.y < _bottomBorder
                    ? _bottomBorder.Value
                    : MapCamera.transform.position.y > _topBorder
                        ? _topBorder.Value
                        : MapCamera.transform.position.y;

                MapCamera.transform.position = new Vector3(x, y, MapCamera.transform.position.z);

                _mousePosition = Input.mousePosition;
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                _basePosition = MainCharacterIcon.transform.position;
                _basePosition.z = MapCamera.transform.position.z;
                MapCamera.transform.position = _basePosition;
            }
        }
    }
}