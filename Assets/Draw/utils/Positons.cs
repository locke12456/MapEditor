using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Draw.utils
{
    public class Position {
        private bool in_range;
        private float value;
        public Position(float curent) { value = curent; }
        public float Value {
            get { return value; }
        }
        public bool InRange {
            get {
                return in_range;
            }
        }
        public void calc(Func<float> target) {
            Func<float, float> _range = (t) => t + t / 2;
            Func<float, float, float, bool> _in_range =
                (current_dist, target_dist, range) =>
                     current_dist > target_dist / 2 && current_dist < range;
            in_range = _in_range(value, target(), _range(value));
        }
    }
    public class Distance {
        public Position left;
        public Position right;
        public Position top;
        public Position bottom;
        public bool in_left;
        public bool in_right;
        public bool in_top;
        public bool in_bottom;
        public Distance(Vector2 pos, Vector2 target) {
            left = new Position((pos.x) - target.x);
            right =new Position((target.x) - pos.x);
            top = new Position((pos.y) - target.y);
            bottom = new Position((target.y) - pos.y);
        }
        
    }
    public class DiscPosition {
        public Distance distance;
        public void disc(Vector2 pos, Vector2 target)
        {
            distance = new Distance(pos, target);
        }
        public List<Position> calc(DiscPosition target) {
            List<Position> _in_range = new List<Position>();
            Func<bool, bool, bool?> equal = (cur, tar) => cur && tar;
            Func<Position, Func<float>, Func<bool>, bool> disc = (cur, tar, in_range) =>
            {
                cur.calc(tar);
                return in_range();
            };
            Func<Func<bool?>, Position ,Func<float>, Func<bool>,bool> is_in_range = (equals,cur_pos,value,range) => equals() ?? disc(cur_pos,value,range) ;
            Dictionary<Position, Func<bool>> pos = new Dictionary<Position, Func<bool>>() {
                {   distance.top,
                    () => is_in_range(() => equal(OnTop, target.OnTop), distance.top, () => target.distance.top.Value, () => distance.top.InRange)
                },
                {
                    distance.bottom,
                    () => is_in_range(() => equal(OnBottom, target.OnBottom), distance.bottom, () => target.distance.bottom.Value, () => distance.bottom.InRange)
                },
                {
                    distance.left,
                    () => is_in_range(() => equal(OnLeft, target.OnLeft), distance.left, () => target.distance.left.Value, () => distance.left.InRange)
                },
                {
                    distance.right,
                    () => is_in_range(() => equal(OnRight, target.OnRight), distance.right, () => target.distance.right.Value, () => distance.right.InRange)
                }
            };
            foreach (var jud in pos) {
                if (jud.Value())
                    _in_range.Add(jud.Key);
            }
            return _in_range;
        }
        public float Left {
            get { return distance.left.Value; }
        }
        public float Right
        {
            get { return distance.right.Value; }
        }
        public float Top
        {
            get { return distance.top.Value; }
        }
        public float Bottom
        {
            get { return distance.bottom.Value; }
        }
        public bool OnLeft
        {
            get
            {
                return Left > 0;
            }
        }
        public bool OnRight
        {
            get
            {
                return Right > 0;
            }
        }
        public bool OnTopRight
        {
            get
            {
                return (OnTop & OnRight);
            }
        }
        public bool OnTopLeft
        {
            get
            {
                return (OnTop & OnRight);
            }
        }
        public bool OnBottomRight
        {
            get
            {
                return (OnBottom & OnRight);
            }
        }
        public bool OnBottomLeft
        {
            get
            {
                return (OnBottom & OnRight);
            }
        }
        public bool OnTop
        {
            get
            {
                return Top > 0;
            }
        }
        public bool OnBottom
        {
            get
            {
                return Bottom > 0;
            }
        }

    }
    public class Adjust
    {
        private List<RaycastHit> _points = new List<RaycastHit>();
        private List<Vector3> _positions = new List<Vector3>();
        public Adjust()
        {
        }
        public List<RaycastHit> RaycastHits
        {
            get { return _points; }
        }
        public List<Vector3> Positions
        {
            get { return _positions; }
        }

        public void build(GameObject target,int layer,float size)
        {
            Vector3 target_pos = target.transform.position;

            _points.Add(_get_raycast(layer, target_pos));
            _points.Add(_get_raycast(layer, _get_top_position(target_pos, size)));
            _points.Add(_get_raycast(layer, _get_bottom_position(target_pos, size)));
            _points.Add(_get_raycast(layer, _get_right_position(target_pos, size)));
            _points.Add(_get_raycast(layer, _get_left_position(target_pos, size)));
            //_points.Add( _get_raycast(layer, target_pos) );

        }
        private Vector3 _get_right_position(Vector3 target_pos,float shift)
        {
            Vector3 result = new Vector3(0,0,shift);
            return target_pos + result;
        }
        private Vector3 _get_left_position(Vector3 target_pos, float shift)
        {
            Vector3 result = new Vector3(0,0,-shift);
            return target_pos + result;
        }
        private Vector3 _get_top_position(Vector3 target_pos, float shift)
        {
            Vector3 result = new Vector3(-shift, 0, 0);
            return target_pos + result;
        }
        private Vector3 _get_bottom_position(Vector3 target_pos, float shift)
        {
            Vector3 result = new Vector3(shift, 0, 0);
            return target_pos + result;
        }
        private RaycastHit _get_raycast(int layer, Vector3 target_pos)
        {
            RaycastHit hit;
            var position = HandleUtility.WorldToGUIPoint(target_pos);
            var ray = HandleUtility.GUIPointToWorldRay(position);
            _positions.Add(target_pos);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layer))
            {

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
