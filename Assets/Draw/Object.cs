using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Draw
{
    
    public class DrawObject
    {
        public RaycastHit hit;
        public Ray ray;
        public Vector3 pos;
        public GameObject reference;
        GameObject childObject;

        public DrawObject()
        {
            //childObject.name = "test_" + i;
        }
        public Vector2 GUIPostion {
            get { return HandleUtility.WorldToGUIPoint(pos); }
        }
        public void build(GameObject group)
        {
            //childObject = UnityEngine.Object.Instantiate(reference) as GameObject;
            //childObject.transform.position = hit.point;
            
            //childObject.transform.parent = group.transform;
            //var s = childObject.AddComponent<Assets.Draw.DrawHint>();
            
            //Handles.color = color;
            //Handles.RectangleCap(0, hit.point, Quaternion.LookRotation(hit.normal), 0.5f);
            
        }

        
    }
}
