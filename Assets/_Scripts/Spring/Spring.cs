using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;using Vector3 = UnityEngine.Vector3;

// public enum SpringPreset
// {
//     Default,
//     Gentle,
//     Wobbly,
//     Stiff,
//     Slow,
//     Molasses,
// }
//     public static readonly Dictionary<SpringPreset, Tuple<float, float, float>> SpringPresets = new()
//     {
//         { SpringPreset.Default , new Tuple<float, float, float>(1, 0, 0) },
//         { SpringPreset.Default , new Tuple<float, float, float>(1, 0, 0) },
//         { SpringPreset.Default , new Tuple<float, float, float>(1, 0, 0) },
//         { SpringPreset.Default , new Tuple<float, float, float>(1, 0, 0) },
//         { SpringPreset.Default , new Tuple<float, float, float>(1, 0, 0) },
//         { SpringPreset.Default , new Tuple<float, float, float>(1, 0, 0) },
//     };


[Serializable]
public abstract class Spring
{
    public float mass = 1;
    public float tension = 120;
    public float friction = 26;

    public Spring(float mass, float tension, float friction)
    {
        this.mass = mass;
        this.tension = tension;
        this.friction = friction;
    }
    
    public static Vector3 SimulateVector3Spring(Vector3 current, Vector3 target, ref Vector3 velocity, float mass, float tension, float friction, float deltaTime)
    {
        var mainForce = tension * (target - current);
        var frictionForce = velocity * (friction * -1);
        var accel = (mainForce + frictionForce) / mass;
        
        velocity += accel * Time.deltaTime;
        current += velocity * Time.deltaTime;
        return current;
    }
    
    /// <summary>
    /// The same as SimulateFloatSpring but instead of doing <code>target - current</code>
    /// it does <code>Mathf.DeltaAngle(current, target)</code>
    /// </summary>
    public static float SimulateAngleSpring(float current, float target, ref float velocity, float mass, float tension, float friction, float deltaTime)
    {
        var mainForce = tension * Mathf.DeltaAngle(current, target);
        var frictionForce = friction * velocity * -1;
        var accel = (mainForce + frictionForce) / mass;
        
        velocity += accel * Time.deltaTime;
        current += velocity * Time.deltaTime;
        return current;
    }
    
    public static float SimulateFloatSpring(float current, float target, ref float velocity, float mass, float tension, float friction, float deltaTime)
    {
        var mainForce = tension * (target - current);
        var frictionForce = friction * velocity * -1;
        var accel = (mainForce + frictionForce) / mass;
        
        velocity += accel * Time.deltaTime;
        current += velocity * Time.deltaTime;
        return current;
    }
}