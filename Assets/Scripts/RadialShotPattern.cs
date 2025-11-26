using UnityEngine;

[CreateAssetMenu(menuName="BulletHell System/Radial Shot pattern")]

public class RadialShotPattern : ScriptableObject
{
    public int Repetitions;
    public float StartWait = 0f;
    public float EndWait = 0f;
    public RadialShotSettings[] PatternSettings;
    

}
