using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helpers.ListView
{
    public interface IListItem
    {
        public void FillItem(object data);
        public bool HasData(object data);

        public void OnSelected();
        public void OnUnselected();
    }
}
