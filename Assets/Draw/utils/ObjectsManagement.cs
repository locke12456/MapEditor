using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MapEditor.Draw.utils
{
    class ObjectsManagement : MonoBehaviour
    {
        private List<DrawObject> _childObjects;
        private Dictionary<float, List<DrawObject>> matrix;
        //public GameObject[] objects;

        public ObjectsManagement()
        {
            _childObjects = new List<DrawObject>();
            //init();
        }
        void Start()
        {
        }
        public List<DrawObject> Objects
        {
            get { return _childObjects; }
        }
        public void init()
        {
            List<DrawObject> childObjects = new List<DrawObject>();
            int count = gameObject.transform.childCount;
            if (count != _childObjects.Count)
            {
                for (int i = 0; i < count; i++)
                {
                    GameObject obj = gameObject.transform.GetChild(i).gameObject;
                    if (childObjects.Find((tar) => { return tar.childObject == obj; }) == null)
                    {
                        DrawObject new_object = new DrawObject();
                        var p = obj.transform.position;
                        obj.transform.position = new Vector3((float)Math.Round(p.x, 1), (float)Math.Round(p.y, 1), (float)Math.Round(p.z, 1));
                        new_object.childObject = obj;
                        childObjects.Add(new_object);
                    }
                }
            }
            //Func<Vector3, Vector3, bool> less_x = (cur, tar) => cur.x <= tar.x;
            //Func<Vector3, Vector3, bool> less_z = (cur, tar) => cur.z <= tar.z;
            var sortedDict = from entry in childObjects orderby entry.childObject.transform.position.z, entry.childObject.transform.position.x ascending select entry;
            //_childObjects.Sort((cur, tar) => { return less_z(tar.childObject.transform.position, cur.childObject.transform.position) ? 1 : 0; });
            foreach (var obj in sortedDict)
            {
                Debug.Log(obj.childObject.transform.position.ToString());
            }
            _childObjects = sortedDict.ToList();
        }
    }
}
