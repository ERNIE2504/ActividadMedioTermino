using System.Collections;
using UnityEngine;

public class MultiShotWeapon : MonoBehaviour
{
    [Header("General")]
    public WeaponPatternType patternType;

    [Header("Radial Pattern")]
    public RadialShotPattern radialPattern;

    [Header("Spiral Settings")]
    public float spiralBulletSpeed = 8f;
    public float spiralAngleStep = 10f;
    public float spiralCooldown = 0.05f;
    public int spiralTotalBullets = 40;

    [Header("Sweep Cone Settings")]
    public int sweepBulletsPerWave = 8;
    public float sweepBulletSpeed = 10f;
    public float sweepAngle = 45f;
    public float sweepWaveCooldown = 0.2f;
    public int sweepWaves = 6;

    private bool isFiring = false;

    private void Update()
    {
        if (!isFiring)
            StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        isFiring = true;

        Vector2 center = transform.position;

        switch (patternType)
        {
            case WeaponPatternType.Radial:
                yield return StartCoroutine(ExecuteRadial());
                break;

            case WeaponPatternType.Spiral:
                yield return StartCoroutine(ShotAttack.SpiralShot(
                    center,
                    spiralBulletSpeed,
                    spiralAngleStep,
                    spiralCooldown,
                    spiralTotalBullets
                ));
                break;

            case WeaponPatternType.SweepCone:
                yield return StartCoroutine(ShotAttack.SweepConeShot(
                    center,
                    sweepBulletsPerWave,
                    sweepBulletSpeed,
                    sweepAngle,
                    sweepWaveCooldown,
                    sweepWaves
                ));
                break;
        }

        isFiring = false;
    }

    private IEnumerator ExecuteRadial()
    {
        yield return StartCoroutine(ExecuteRadialShotPattern(radialPattern));
    }

    private IEnumerator ExecuteRadialShotPattern(RadialShotPattern pattern)
    {
        int lap = 0;
        Vector2 aimDirection = transform.up;
        Vector2 center = transform.position;

        yield return new WaitForSeconds(pattern.StartWait);

        while (lap < pattern.Repetitions)
        {
            for (int i = 0; i < pattern.PatternSettings.Length; i++)
            {
                ShotAttack.RadialShot(center, aimDirection, pattern.PatternSettings[i]);
                yield return new WaitForSeconds(pattern.PatternSettings[i].CooldownAfterShot);
            }

            lap++;
        }

        yield return new WaitForSeconds(pattern.EndWait);
    }
}