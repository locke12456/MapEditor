using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
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
        RaycastHit hit;
        var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        var paintLayer = 0;
        int layerMask = 1 << paintLayer;
        _adjust_range(layerMask);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            Handles.color = Color.green;
            draw(hit, _target.size);
            Dictionary<RaycastHit, Position> draw_plases = new Dictionary<RaycastHit, Position>();
            _find_hit_places(e.mousePosition,draw_plases);
            _draw_hit_places(draw_plases);
        }
        mouseEvent(e);
        //Debug.Log(0);

    }

    private void _find_hit_places(Vector2 mouse, Dictionary<RaycastHit, Position> draw_plases)
    {
        DiscPosition pos = new DiscPosition();
        pos.disc(target_object.GUIPostion, mouse);
        foreach (DrawObject current in temp_raycasthit)
        {
            Handles.color = Color.green;
            DiscPosition hit_pos = new DiscPosition();
            hit_pos.disc(target_object.GUIPostion, current.GUIPostion);
            List<Position> hit_places = pos.calc(hit_pos);
            if (hit_places.Count > 0)
            {
                hit_places.Sort((x, y) => (x.Value < y.Value ? 1 : 0));
                draw_plases.Add(current.hit, hit_places[0]);
            }
            else
                draw(current.hit, _target.size, temp_raycasthit.IndexOf(current) + 1);
        }
    }

    private void _draw_hit_places(Dictionary<RaycastHit, Position> draw_plases)
    {
        var sortedDict = from entry in draw_plases orderby entry.Value.Value descending select entry;
        foreach (var item in sortedDict)
        {
            Handles.color = Color.red;

            if (sortedDict.First().Value != item.Value)
                Handles.color = Color.green;

            draw(item.Key, _target.size);
        }
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
