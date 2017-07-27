using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Draw : MonoBehaviour
{
    public Assets.Draw.DrawObject refer;
    public GameObject BaseObject;
    public GameObject Painter;
    public bool draw;
    public float size = 1.0f;
    public int preview = 0;
}