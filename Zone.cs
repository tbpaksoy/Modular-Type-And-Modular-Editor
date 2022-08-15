using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tahsin.Zones
{
    [System.Serializable]
    public class Zone : ISerializationCallbackReceiver
    {
        public Sprite icon;
        public string name;
        public GameObject obj;
        [SerializeReference]
        private Sprite _icon;
        [SerializeReference]
        private GameObject _obj;
        [SerializeReference]
        private string _name;
        public virtual void UnlockZone() 
        {
            //TODO
        }
        public virtual void OnGUI() 
        {
            icon = EditorGUILayout.ObjectField("Icon", icon, typeof(Sprite), false) as Sprite;
            name = EditorGUILayout.TextField("Name", name);
            obj = EditorGUILayout.ObjectField("Represnts", obj, typeof(GameObject), true) as GameObject;
        }

        public virtual void OnBeforeSerialize()
        {
            _icon = icon;
            _obj = obj;
            _name = name;
        }

        public virtual void OnAfterDeserialize()
        {
            icon = _icon;
            obj = _obj;
            name = _name;
        }
    }
}
