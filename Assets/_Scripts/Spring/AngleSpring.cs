using System;
using UnityEngine;

[Serializable]
public class AngleSpring : Spring
{
    public float Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }
    
    private float _velocity;

    public float Current { get; set; }

    public float UpdateSpring(float target)
    {
        Current = SimulateAngleSpring(Current, target, ref _velocity, mass, tension, friction, Time.deltaTime);
        return Current;
    }

    public AngleSpring(float mass, float tension, float friction, float initial = 0) : base(mass, tension, friction)
    {
        Current = initial;
    }
}