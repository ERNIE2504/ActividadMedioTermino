using System.Collections;
using UnityEngine;

public class PatternSequenceController : MonoBehaviour
{
    [Header("Radial Shot")]
    public RadialShotPattern radialPattern;

    [Header("Spiral Shot Settings")]
    public float spiralBulletSpeed = 5f;
    public float spiralAngleStep = 10f;
    public float spiralCooldown = 0.05f;
    public int spiralTotalBullets = 100;

    [Header("Sweep Cone Settings")]
    public int sweepBulletsPerWave = 10;
    public float sweepBulletSpeed = 6f;
    public float sweepAngle = 60f;
    public float sweepWaveCooldown = 0.2f;
    public int sweepWaves = 12;

    private void Start()
    {
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        Vector2 center = transform.position;
        Vector2 aimDir = transform.up;


        yield return StartCoroutine(ExecuteRadial(radialPattern, 10f));

        yield return StartCoroutine(
            ShotAttack.SpiralShot(
                center,
                spiralBulletSpeed,
                spiralAngleStep,
                spiralCooldown,
                10f
            )
        );


        
        yield return StartCoroutine(
            ShotAttack.SweepConeShot(
                center,
                sweepBulletsPerWave,
                sweepBulletSpeed,
                sweepAngle,
                sweepWaveCooldown,
                10f
            )
        );


        
    }

    private IEnumerator ExecuteRadial(RadialShotPattern pattern, float duration)
{
    float timer = 0f;
    Vector2 aimDirection = transform.up;
    Vector2 center = transform.position;

    yield return new WaitForSeconds(pattern.StartWait);

    while (timer < duration)
    {
        for (int i = 0; i < pattern.PatternSettings.Length; i++)
        {
            ShotAttack.RadialShot(center, aimDirection, pattern.PatternSettings[i]);

            float wait = pattern.PatternSettings[i].CooldownAfterShot;
            timer += wait;

            yield return new WaitForSeconds(wait);

            if (timer >= duration)
                break;
        }
    }
    yield return new WaitForSeconds(pattern.EndWait);
}
}
