using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Draw.utils;

[CustomEditor(typeof(Draw))]
public class DrawEditor : UnityEditor.Editor
{
    private Draw _target;
    private List<Action<int, Vector3, Quaternion, float>> methods = new List<Action<int, Vector3, Quaternion, float>> ();
    public DrawEditor() {
        
        methods.Add( Handles.RectangleCap );
        methods.Add( Handles.CircleCap );
        methods.Add( Handles.CubeCap );
        methods.Add( Handles.CylinderCap );
        methods.Add( Handles.ConeCap );
    }
    void mouseEvent(Event e) {
        switch (e.type) {
            case EventType.MouseMove:
                HandleUtility.Repaint();
                break;
        }
    }

    void draw(RaycastHit hit,float size) {
        Handles.color = Color.green;
        //Handles.CircleCap(1, hit.point, Quaternion.LookRotation(hit.normal), 1);
        methods[_target.preview](1, hit.point, Quaternion.LookRotation(hit.normal), size);
        Handles.ArrowCap(0, hit.point, Quaternion.LookRotation(hit.normal), size);
    }
   void OnSceneGUI()
    {
        _target = (Draw)target;
        if (!_target.draw) return;
        var e = Event.current;
        RaycastHit hit;
        var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        var size = _target.BaseObject.GetComponent<Renderer>().bounds.size;
        var paintLayer = 0;
        int layerMask = 1 << paintLayer;
        if (Physics.Raycast(ray.origin, ray.direction,out hit, Mathf.Infinity, layerMask))
        {
            draw(hit, _target.size);
            Adjust adj = new Adjust();
            draw(adj.build(_target.BaseObject, layerMask,0), _target.size);
            //Debug.Log(e.mousePosition);
        }
        mouseEvent(e);
        //Debug.Log(0);

    }
}
