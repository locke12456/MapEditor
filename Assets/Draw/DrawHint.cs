using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Draw
{
    public class DrawHint : MonoBehaviour
    {
        public GameObject obj;
        public DrawObject draw;

        public void BuildObject()
        {
            Instantiate(obj, draw.hit.point , Quaternion.identity);
        }

    }
}
