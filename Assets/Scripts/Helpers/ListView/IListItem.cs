using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helpers.ListView
{
    public interface IListItem
    {
        public Action Selection { get; set; }

        public void FillItem(object data);
        public bool HasData(object data);

        public void OnSelecting();
        public void OnSelected();
    }
}
