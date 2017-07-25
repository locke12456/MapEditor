using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Draw.utils
{
    public class Adjust
    {
        private List<RaycastHit> _points;
        public Adjust()
        {
        }
        public RaycastHit build(GameObject target,int layer,float depth)
        {
            RaycastHit hit;
            var position = HandleUtility.WorldToGUIPoint(target.transform.position);
            var ray = HandleUtility.GUIPointToWorldRay(position);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layer)) {
                
            }
            return hit;
        }
        private RaycastHit _adjust_postion(Vector3 origin, Vector3 direction, int mask)
        {
            RaycastHit hit;

            if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, mask))
            {

            }
            return hit;
        }
    }
}
