using System;
using UnityEngine;

[Serializable]
public class FloatSpring : Spring
{
    public float Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    private float _velocity = 0;
    private float _current = 0;

    public float Current
    {
        get => _current;
        set => _current = value;
    }

    public float UpdateSpring(float target)
    {
        _current = SimulateFloatSpring(_current, target, ref _velocity, mass, tension, friction, Time.deltaTime);
        return _current;
    }

    public FloatSpring(float mass, float tension, float friction, float initial = 0) : base(mass, tension, friction)
    {
        _current = initial;
    }
}