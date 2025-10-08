using UnityEngine;

[CreateAssetMenu(fileName = "ChariotConfig", menuName = "ChariotConfig")]
public class ChariotConfig : ScriptableObject
{
    [Header("Chariot")]
    public float ChariotPosF;
    public float ChariotPosZ;
    public float ChariotPosR;
    
    public float ChariotRotF;
    public float ChariotRotZ;
    public float ChariotRotR;

    [Header("Horses")]
    public float HorseBrakeForce;
    public float HorseFriction;
    public float HorseSpeed;
    public float RushForce;
}
