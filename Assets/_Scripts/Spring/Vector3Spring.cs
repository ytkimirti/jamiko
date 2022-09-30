using System;
using UnityEngine;

[Serializable]
public class Vector3Spring : Spring
{
    public Vector3 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    private Vector3 _velocity;

    public Vector3 Current { get; set; }

    public Vector3 UpdateSpring(Vector3 target)
    {
        Current = SimulateVector3Spring(Current, target, ref _velocity, mass, tension, friction, Time.deltaTime);
        return Current;
    }

    public Vector3Spring(float mass, float tension, float friction) : base(mass, tension, friction) {}
    
    public Vector3Spring(float mass, float tension, float friction, Vector3 initial) : base(mass, tension, friction)
    {
        Current = initial;
    }
}