using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.TooltipSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu.CharacterCreation
{
    public class StoryCardWidget : TooltipDataProvider, IListItem
    {
        private CharacterStoryCard card;

        public Image charAvatar;
        public TextMeshProUGUI charName;
        public TextMeshProUGUI story;

        public Action Selection { get; set; }

        public void FillItem(object data)
        {
            if (data is not null)
            {
                card = (CharacterStoryCard)data;

                charAvatar.sprite = card.avatar;
                charName.SetText(card.characterName);
                story.SetText(card.story);
            }
        }

        public bool HasData(object data)
        {
            return card.Equals(data);
        }

        public void OnSelected()
        {
        }

        public void OnSelecting()
        {
            Selection.Invoke();
        }

        public override string GetHeader(string tag = null)
        {
            return card.characterName;
        }

        public override string GetContent(string tag = null)
        {
            if (card.story.Length > 500)
                return card.story.Substring(0, 500) + "...";
            else return card.story;
        }
    }
}
