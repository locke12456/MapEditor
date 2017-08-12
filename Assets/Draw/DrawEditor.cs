using UnityEngine;

using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using MapEditor.Draw.utils;
using MapEditor.Draw;
#if UNITY_EDITOR
using UnityEditor;
namespace MapEditor.Draw
{
    using mode;
    using HitPosition = System.Collections.Generic.Dictionary<MapEditor.Draw.DrawObject, MapEditor.Draw.utils.Position>;
    [CustomEditor(typeof(Draw))]
    public class DrawEditor : Editor
    {
        static string appTitle = "DrawEditor";
        private Draw _target;
        private GameObject baseObject = null;
        private List<DrawObject> temp_raycasthit;
        private DrawObject target_object;
        private DrawObject selected_object, pre_selected_object;
        private AbstractDrawMode current_mode;
        private bool target_changed;
        private int selected = 0;
        private List<Action<int, Vector3, Quaternion, float>> methods = new List<Action<int, Vector3, Quaternion, float>>();
        public DrawEditor()
        {

            methods.Add(Handles.RectangleCap);
            methods.Add(Handles.CircleCap);
            methods.Add(Handles.CubeCap);
            methods.Add(Handles.CylinderCap);
            methods.Add(Handles.ConeCap);
        }
        void OnEnable() {

            _target = target as Draw;
            _target.update();
        }

        public override void OnInspectorGUI()
        {
            _target = target as Draw;
            base.OnInspectorGUI();
            selected = EditorGUILayout.Popup("Mode", selected, _target.modes.Keys.ToArray());
        }
        // void OnInspectorGUI()
        //{

        //    if (target_changed)
        //    {
        //        EditorGUILayout.ObjectField("BaseObject", selected_object.childObject, typeof(GameObject));
        //        _target.BaseObject = selected_object.childObject;
        //        target_changed = false;
        //        return;
        //    }

        //}
        void mouseEvent(Event e)
        {
            int id = GUIUtility.GetControlID(appTitle.GetHashCode(), FocusType.Passive);

            switch (e.type)
            {
                case EventType.MouseDrag:
                case EventType.MouseDown:
                    if (e.control)
                    {
                        current_mode.Draw();
                        e.Use();
                    }
                    if (e.shift)
                    {
                        //_target.BaseObject = selected_object.childObject;
                        //target_changed = true;
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

        void draw(RaycastHit hit, float size, int id = 0)
        {
            if(hit.normal!= Vector3.zero)
                methods[_target.preview](1, hit.point, Quaternion.LookRotation(hit.normal), size);
        }

        void OnSceneGUI()
        {
            _target = target as Draw;

            if (!_target.draw) return;
            var e = Event.current;
            RaycastHit hit;
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            var paintLayer = 0;
            int layerMask = 1 << paintLayer;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
            {
                Handles.color = Color.green;
                draw(hit, _target.size);
                string key = _target.modes.Keys.ToArray()[selected];
                DrawMode mode;
                if (_target.modes.TryGetValue(key, out mode))
                {
                    current_mode = mode as AbstractDrawMode;
                    current_mode.painter = _target;
                    current_mode.Positon = e.mousePosition;

                    mode.DrawPreview(
                        layerMask,
                        (hit_ray, size, id) => draw(hit_ray, size, id)
                        );
                }
            }
            mouseEvent(e);
        }
        
    }
}
#endif