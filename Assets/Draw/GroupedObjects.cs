using MapEditor.Draw.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
namespace MapEditor.Draw
{
    class GroupedObjects : MonoBehaviour
    {
        public List<DrawObject> ChildObjects;
        private Dictionary<float, List<DrawObject>> matrix;
        //public GameObject[] objects;
        public ObjectsManagement manager;
        
        public GroupedObjects()
        {
            //init();
        }
        public void init()
        {
            manager = gameObject.GetComponent<ObjectsManagement>();
            ChildObjects = manager.Objects;
        }
    }
    [CustomEditor(typeof(GroupedObjects))]
    public class GroupedObjectsEditor : UnityEditor.Editor
    {
        private GroupedObjects _target;
        public GroupedObjectsEditor() {
        }
        void OnEnable() {
            _target = target as GroupedObjects;
            _target.init();
            _target.manager.init();
        }
        void OnSceneGUI()
        {
            //init();
        }

        
    }
}
#endif