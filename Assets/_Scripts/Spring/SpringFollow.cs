using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFollow : MonoBehaviour
{
    public float mass;
    public float strength;
    public float dampening;
    
    public Vector3 Velocity { get; private set; }
    public Vector3 Value { get; private set; }
}
