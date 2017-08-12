using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MapEditor.Draw.mode
{

    public interface DrawMode
    {
        void DrawPreview(int layerMask,Action<RaycastHit, float, int> draw = null);
        void Draw();

    }
    public abstract class AbstractDrawMode : DrawMode
    {
        public List<DrawObject> Objects;
        public Draw painter;
        public Vector2 Positon;
        public abstract void DrawPreview(int layerMask, Action<RaycastHit, float, int> draw = null);
        public abstract void Draw();
    }
}
