using UnityEngine;
using System.Collections;
using MapEditor.Draw;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
namespace Draw {

[CustomEditor(typeof(Path))]
    public class PathEditor : Editor
    {
        static string appTitle = "PathEditor";
        private Path _target;
        private Vector3 last;
        void mouseEvent(Event e)
        {
            int id = GUIUtility.GetControlID(appTitle.GetHashCode(), FocusType.Passive);
            switch (e.type)
            {
                //case EventType.MouseDrag:
                case EventType.MouseDown:
                    if (e.control)
                    {
                        _target.WalkingPathes.Add(last);
                        //last = null;
                        e.Use();
                    }
                    if (e.shift)
                    {
                        e.Use();
                    }
                    break;
                case EventType.layout:
                    HandleUtility.AddDefaultControl(id);
                    break;
                case EventType.MouseMove:
                    HandleUtility.Repaint();
                    break;
            }
        }
        void OnSceneGUI()
        {
            _target = (Path)target;

            if (!_target.preview) return;
            var e = Event.current;
            RaycastHit hit;
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            var paintLayer = 0;
            int layerMask = 1 << paintLayer;
           
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
            {
                last = hit.transform.position;
            }
            var selected = from entry in _target.WalkingPathes where entry != last select entry;
            Vector3 pos1 = Vector3.zero, pos2 = Vector3.zero;

            Handles.color = Color.green;
            foreach (var trans in selected) {
                pos1 = pos1 == Vector3.zero ? trans : pos2 == Vector3.zero ? pos1 : pos2;
                pos2 = trans;
                if (pos1 != pos2)
                {
                    Handles.DrawLine(pos1,pos2);
                }
            }


            if (!_target.draw) return;
            mouseEvent(e);
            if (selected.Count() < 1) return;

            Handles.DrawLine(pos2,last);
        }
    }
}
#endif