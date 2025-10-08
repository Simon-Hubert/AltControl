using System;
using UnityEngine;
using static System.MathF;

[Serializable]
public class SecondOrderDynamics3D
{
    private Vector3 xp;
    private Vector3 y, yd;
    private float k1, k2, k3;
    [SerializeField] private float f, z, r;

    public void Init(Vector3 x0){
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f));
        k3 = r * z / (2 * PI * f);

        xp = x0;
        y = x0;
        yd = Vector3.zero;
    }
    
    public void Init(Vector3 x0, float f, float zeta, float r) {
        this.f = f;
        z = zeta;
        this.r = r;
        
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f));
        k3 = r * z / (2 * PI * f);

        xp = x0;
        y = x0;
        yd = Vector3.zero;
    }

    public Vector3 Update(float DeltaTime, Vector3 x){
        Vector3 xd = (x - xp) / DeltaTime;
        xp = x;

        y += DeltaTime * yd;
        yd += DeltaTime * (x + k3 * xd - y - k1 * yd) / k2;

        return y;
    }
}

[Serializable]
public class SecondOrderDynamics2D
{
    private Vector2 xp;
    private Vector2 y, yd;
    private float k1, k2, k3;
    [SerializeField] private float f, z, r;
    
    public void Init(Vector2 x0){
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f));
        k3 = r * z / (2 * PI * f);

        xp = x0;
        y = x0;
        yd = Vector2.zero;
    }
    
    public void Init(Vector2 x0, float f, float zeta, float r) {
        this.f = f;
        z = zeta;
        this.r = r;
        
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f));
        k3 = r * z / (2 * PI * f);

        xp = x0;
        y = x0;
        yd = Vector2.zero;
    }

    public Vector2 Update(float DeltaTime, Vector2 x){
        
        Vector2 xd = (x - xp) / DeltaTime;
        xp = x;

        y += DeltaTime * yd;
        yd += DeltaTime * (x + k3 * xd - y - k1 * yd) / k2;

        return y;
    }
}


[Serializable]
public class SecondOrderDynamics
{
    private float xp;
    private float y, yd;
    private float k1, k2, k3;
    [SerializeField] private float f, z, r;


    public void Init(float x0){
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f));
        k3 = r * z / (2 * PI * f);

        xp = x0;
        y = x0;
        yd = 0.0f;
    }
    
    public void Init(float x0, float f, float zeta, float r) {
        this.f = f;
        z = zeta;
        this.r = r;
        
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f));
        k3 = r * z / (2 * PI * f);

        xp = x0;
        y = x0;
        yd = 0;
    }

    public float Update(float DeltaTime, float x){
        
        float xd = (x - xp) / DeltaTime;
        xp = x;

        y += DeltaTime * yd;
        yd += DeltaTime * (x + k3 * xd - y - k1 * yd) / k2;

        return y;
    }
}

[Serializable]
public class SecondOrderDynamicsAngle //TODO Adapter pour les Angles
{
    private float xp;
    private float y, yd;
    private float k1, k2, k3;
    [SerializeField] private float f, z, r;
    
    public void Init(float x0, float f, float zeta, float r) {
        this.f = f;
        z = zeta;
        this.r = r;
        
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f));
        k3 = r * z / (2 * PI * f);

        xp = x0;
        y = x0;
        yd = 0;
    }

    public void Init(float x0){
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f));
        k3 = r * z / (2 * PI * f);

        xp = x0;
        y = x0;
        yd = 0;
    }

    public float Update(float DeltaTime, float x){
        float xd = (x - xp) / DeltaTime;
        xp = x;

        y += DeltaTime * yd;
        yd += DeltaTime * (x + k3 * xd - y - k1 * yd) / k2;

        return y;
    }
}
