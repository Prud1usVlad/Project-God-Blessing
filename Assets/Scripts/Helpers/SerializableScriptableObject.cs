using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class SerializableScriptableObject : ScriptableObject
    {
        [SerializeField] string _guid;
        public string Guid => _guid;

#if UNITY_EDITOR
        void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(this);
            _guid = AssetDatabase.AssetPathToGUID(path);
        }
#endif
    }
}


