using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.SaveSystem;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    internal class SaveListItem : MonoBehaviour, IListItem
    {
        [SerializeField]
        private FameTranslation fameTranslation;
        [SerializeField]
        private SaveSystem.SaveSystem saveSystem;
        private SaveFile saveFile;

        public TextMeshProUGUI charName;
        public TextMeshProUGUI saveType;
        public TextMeshProUGUI fame;
        public TextMeshProUGUI ingameDay;
        public TextMeshProUGUI date;

        public Action Selection { get; set; }

        public GameEvent deleteEvent;
        public GameEvent loadEvent;

        public void FillItem(object data)
        {
            saveFile = (SaveFile)data;

            charName.SetText(saveFile.characterName);
            saveType.SetText(saveFile.type);
            fame.SetText(fameTranslation.GetProgressPercentage(saveFile.fame) + "%");
            ingameDay.SetText(saveFile.day.ToString());
            date.SetText(saveFile.date);
        }

        public bool HasData(object data)
        {
            return saveFile.Equals(data);
        }

        public void OnSelected()
        {
        }

        public void OnSelecting()
        {
        }

        public void OnDelete()
        {
            deleteEvent.Raise(saveFile.fileName);
        }

        public void OnLoad()
        {
            loadEvent.Raise(saveFile.fileName);
        }
    }
}
