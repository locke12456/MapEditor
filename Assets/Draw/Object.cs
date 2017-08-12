using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace MapEditor.Draw
{
    
    public class DrawObject
    {
        public RaycastHit hit;
        public Ray ray;
        public Vector3 pos;
        public GameObject reference;
        public GameObject childObject;
        public bool mouse;
        GameObject groupObject;


        public DrawObject()
        {
            //childObject.name = "test_" + i;
        }
#if UNITY_EDITOR
        public Vector2 GUIPostion {
            get { return HandleUtility.WorldToGUIPoint(pos); }
        }
#endif
        public void build(GameObject group)
        {
            groupObject = group;
            //var s = childObject.AddComponent<Assets.Draw.DrawHint>();

            //Handles.color = color;
            //Handles.RectangleCap(0, hit.point, Quaternion.LookRotation(hit.normal), 0.5f);

        }

#if UNITY_EDITOR
        public void addToGroup()
        {
            childObject = UnityEngine.Object.Instantiate(reference) as GameObject;
            childObject.transform.position = hit.point;
            childObject.transform.parent = groupObject.transform;
            childObject.name = GUIPostion.ToString();
        }
#endif

    }
}