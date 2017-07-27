using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Draw.utils;
using Assets.Draw;

[CustomEditor(typeof(Draw))]
public class DrawEditor : UnityEditor.Editor
{
    private Draw _target;
    private GameObject baseObject = null;
    private List<DrawObject> temp_raycasthit;
    private DrawObject target_object;
    private List<Action<int, Vector3, Quaternion, float>> methods = new List<Action<int, Vector3, Quaternion, float>> ();
    public DrawEditor() {
        
        methods.Add( Handles.RectangleCap );
        methods.Add( Handles.CircleCap );
        methods.Add( Handles.CubeCap );
        methods.Add( Handles.CylinderCap );
        methods.Add( Handles.ConeCap );
    }
    void mouseEvent(Event e) {
        int id = GUIUtility.GetControlID(0, FocusType.Passive);
        switch (e.type) {
            case EventType.MouseDown:
                Debug.Log(id);
                Debug.Log(HandleUtility.nearestControl);
                break;
            case EventType.MouseMove:
                HandleUtility.Repaint();
                break;
        }
    }

    void draw(RaycastHit hit,float size,int id = 0) {
        
        //Handles.CircleCap(1, hit.point, Quaternion.LookRotation(hit.normal), 1);
        methods[_target.preview](1, hit.point, Quaternion.LookRotation(hit.normal), size);
        //Handles.ArrowCap(id, hit.point, Quaternion.LookRotation(hit.normal), size);
    }

    void OnSceneGUI()
    {
        _target = (Draw)target;
        if (!_target.draw) return;
        var e = Event.current;
        RaycastHit hit,hit2;
        var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        var paintLayer = 0;
        int layerMask = 1 << paintLayer;
        _adjust_range(layerMask);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            Handles.color = Color.green;
            draw(hit, _target.size);
            DiscPosition pos = new DiscPosition();
            pos.disc(target_object.GUIPostion, e.mousePosition);
            foreach (DrawObject current in temp_raycasthit)
            {
                Handles.color = Color.green;
                DiscPosition hit_pos = new DiscPosition();
                hit_pos.disc(target_object.GUIPostion, current.GUIPostion);
                var po = hit_pos.calc(pos);
                if(po.Count>0)
                { 
                        Handles.color = Color.red;
                }
                draw(current.hit, _target.size, temp_raycasthit.IndexOf(current) + 1);
            }
            //Debug.Log(e.mousePosition);
        }
        mouseEvent(e);
        //Debug.Log(0);

    }

    private void _adjust_range(int layerMask)
    {
        if (_target.BaseObject != baseObject)
        {
            temp_raycasthit = new List<DrawObject>();
            Adjust adj = new Adjust();
            adj.build(_target.BaseObject, layerMask, 1);
            baseObject = _target.BaseObject;

            foreach(RaycastHit hit in adj.RaycastHits)
            {
                DrawObject obj = new DrawObject();
                int index = adj.RaycastHits.IndexOf(hit);
                obj.reference = baseObject;
                obj.hit = hit;
                obj.pos = adj.Positions[index];
                Debug.Log(obj.pos);
                obj.build(_target.Painter);
                if (0 == index) target_object = obj;
                else temp_raycasthit.Add(obj);
            }
        }
    }
}
