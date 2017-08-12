using MapEditor.Draw.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MapEditor.Draw.mode
{
    using UnityEditor;
    using HitPosition = System.Collections.Generic.Dictionary<MapEditor.Draw.DrawObject, MapEditor.Draw.utils.Position>;
    public class BuildMode : AbstractDrawMode
    {
        private GameObject baseObject;
        private DrawObject target_object;
        private DrawObject selected_object, pre_selected_object;
        private Action<RaycastHit, float, int> _draw;

        public override void Draw()
        {
            if (pre_selected_object == null || selected_object.GUIPostion != pre_selected_object.GUIPostion)
            {
                selected_object.addToGroup();
                painter.BaseObject = selected_object.childObject;
                pre_selected_object = selected_object;
            }
        }
        public override void DrawPreview(int layerMask, Action<RaycastHit, float, int> draw)
        {
            HitPosition draw_plases = new HitPosition();
            _draw = draw;
            _adjust_range(layerMask);
            _find_hit_places(Positon, draw_plases);
            _draw_hit_places(draw_plases);
        }
        private void _find_hit_places(Vector2 mouse, HitPosition draw_plases)
        {
            DiscPosition pos = new DiscPosition();
            pos.disc(target_object.GUIPostion, mouse);
            foreach (DrawObject current in Objects)
            {
                Handles.color = Color.green;
                DiscPosition hit_pos = new DiscPosition();
                hit_pos.disc(target_object.GUIPostion, current.GUIPostion);
                List<Position> hit_places = pos.calc(hit_pos);
                if (hit_pos <= pos && hit_places.Count > 0)
                {
                    hit_places.Sort((x, y) => (x.Value < y.Value ? 1 : 0));
                    draw_plases.Add(current, hit_places[0]);
                }
                else
                    //urrent.mouse = false;
                    _draw(current.hit, painter.size, Objects.IndexOf(current) + 1);
            }
        }

        private void _draw_hit_places(HitPosition draw_plases)
        {
            var sortedDict = from entry in draw_plases orderby entry.Value.Value descending select entry;

            foreach (var item in sortedDict)
            {
                Handles.color = Color.red;
                item.Key.mouse = false;
                if (sortedDict.First().Value != item.Value)
                    Handles.color = Color.green;
                else selected_object = item.Key;
                _draw(item.Key.hit, painter.size,0);
            }
        }

        private void _adjust_range(int layerMask)
        {
            if (painter.BaseObject != baseObject)
            {
                Objects = new List<DrawObject>();
                Adjust adj = new Adjust();
                adj.build(painter.BaseObject, layerMask, painter.size * 2);
                baseObject = painter.BaseObject;
                foreach (RaycastHit hit in adj.RaycastHits)
                {
                    DrawObject obj = new DrawObject();
                    int index = adj.RaycastHits.IndexOf(hit);
                    obj.reference = baseObject;
                    obj.hit = hit;
                    obj.pos = adj.Positions[index];
                    obj.build(painter.Painter);
                    if (0 == index) target_object = obj;
                    else Objects.Add(obj);
                }
            }
        }

    }
}
