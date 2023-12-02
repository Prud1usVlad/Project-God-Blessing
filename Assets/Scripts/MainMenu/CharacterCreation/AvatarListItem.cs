using Assets.Scripts.Helpers.ListView;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu.CharacterCreation
{
    public class AvatarListItem : MonoBehaviour, IListItem
    {
        public Sprite sprite;

        public Image image;

        public Action Selection { get; set; }

        public void FillItem(object data)
        {
            sprite = data as Sprite;
            image.sprite = sprite;
        }

        public bool HasData(object data)
        {
            return sprite.Equals(data);
        }

        public void OnSelected()
        {
        }

        public void OnSelecting()
        {
            Selection?.Invoke();
        }
    }
}
