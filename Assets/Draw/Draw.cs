using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapEditor.Draw.mode;

namespace MapEditor.Draw {
    [ExecuteInEditMode]
    public class Draw : MonoBehaviour
    {
        public MapEditor.Draw.DrawObject refer;
        public GameObject BaseObject;
        public GameObject Painter;
       // public List<string> Modes;
        public bool random = false;
        public bool draw;
        public float size = 1.0f;
        public int preview = 0;
        public Dictionary<string, DrawMode> modes;// = new Dictionary<string, DrawMode>();
        public void update() {
            modes = new Dictionary<string, DrawMode> {
                {"Build Mode",new BuildMode() }
            };
        }
    }
}