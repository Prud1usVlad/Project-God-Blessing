using System;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Enums;
using Assets.Scripts.Roguelike.LevelGeneration.Domain.Models;
using UnityEngine;

namespace Assets.Scripts.Helpers.Roguelike.Minimap
{
    public class RoomDecorator : MonoBehaviour
    {
        private Room _room;

        private bool _isVisible;

        private bool _isCurrentRoom;

        private GameObject legendIconPoint;
        private GameObject playerIcon;
        private GameObject legendIcon = null;

        public Room Room
        {
            get
            {
                return _room;
            }
            set
            {
                if (value.RoomType.Equals(RoomType.Spawn))
                {
                    IsVisible = true;
                }
                else
                {
                    IsVisible = false;
                }

                _room = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                gameObject.SetActive(value);

                _isVisible = value;
            }
        }

        public bool IsCurrentRoom
        {
            get
            {
                return _isCurrentRoom;
            }
            set
            {
                if (value)
                {
                    if (playerIcon == null)
                    {
                        foreach (Transform children in transform.parent)
                        {
                            if (children.tag.Equals(TagHelper.MinimapPlayerIconTag))
                            {
                                playerIcon = children.gameObject;
                                break;
                            }
                        }

                        if (playerIcon == null)
                        {
                            throw new Exception("None of player icon is found");
                        }
                    }

                    if (legendIconPoint == null)
                    {

                        foreach (Transform children in transform)
                        {
                            if (children.tag.Equals(TagHelper.LegendIconPointTag))
                            {
                                legendIconPoint = children.gameObject;
                                break;
                            }
                        }

                        if (legendIconPoint == null)
                        {
                            throw new Exception("None of legend icon point is found");
                        }
                    }

                    playerIcon.transform.position = legendIconPoint.transform.position;
                }

                if (legendIcon == null
                    && Room.RoomType != RoomType.Spawn 
                    && Room.RoomType != RoomType.Empty)
                {
                    foreach (Transform children in transform)
                    {
                        if (children.tag.Equals(TagHelper.LegendIconTag))
                        {
                            legendIcon = children.gameObject;
                            break;
                        }
                    }

                    if (legendIcon == null)
                    {
                        throw new Exception("None of legend icon is found");
                    }
                }

                legendIcon?.SetActive(!value);

                _isCurrentRoom = value;
            }
        }
    }
}