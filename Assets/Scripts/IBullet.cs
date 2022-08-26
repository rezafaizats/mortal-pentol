using UnityEngine;

interface IBullet
{
    float pentolDamage {get; set;}
    float pentolForce {get; set;}
    float pentolLifetime {get; set;}
    Vector2 pentolDirection {get; set;}
}