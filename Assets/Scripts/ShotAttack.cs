using System.Collections;
using UnityEngine;

public static class ShotAttack
{
    public static void SimpleShot (Vector2 origin, Vector2 velocity)
    {
        Bullet bullet = BulletPool.Instance.RequestBullet();
        bullet.transform.position = origin;
        bullet.Velocity = velocity;
    }

    public static void RadialShot(Vector2 origin, Vector2 aimDirection, RadialShotSettings settings)
    {

        float angleBetweenBullets = 360f / settings.NumberOfBullets;

        if (settings.AngleOffset != 0f || settings.PhaseOffset != 0f)
            aimDirection = aimDirection.Rotate(settings.AngleOffset + (settings.PhaseOffset * angleBetweenBullets));

        for (int i = 0; i < settings.NumberOfBullets; i++)
        {
            float bulletDirectionAngle = angleBetweenBullets * i;

            if (settings.RadialMask && bulletDirectionAngle > settings.MaskAngle)
                break;

            Vector2 bulletDirection = aimDirection.Rotate(bulletDirectionAngle);
            SimpleShot(origin, bulletDirection * settings.BulletSpeed);
        }
    }

    public static IEnumerator SpiralShot(
        Vector2 origin,
        float bulletSpeed,
        float angleStep,
        float cooldown,
        float duration)
    {
        float timer = 0f;
        float currentAngle = 0f;

        while (timer < duration)
        {
            Vector2 dir = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            );

            SimpleShot(origin, dir * bulletSpeed);

            currentAngle += angleStep;

            yield return new WaitForSeconds(cooldown);
            timer += cooldown;
        }
    }

    public static IEnumerator SweepConeShot(
        Vector2 origin,
        int bulletsPerWave,
        float bulletSpeed,
        float sweepAngle,
        float waveCooldown,
        float duration)
    {
        float timer = 0f;
        float currentOffset = -sweepAngle;
        bool forward = true;

        while (timer < duration)
        {
            float angleStep = (sweepAngle * 2f) / (bulletsPerWave - 1);

            for (int i = 0; i < bulletsPerWave; i++)
            {
                float ang = currentOffset + (angleStep * i);

                Vector2 dir = new Vector2(
                    Mathf.Cos(ang * Mathf.Deg2Rad),
                    Mathf.Sin(ang * Mathf.Deg2Rad)
                );

                SimpleShot(origin, dir * bulletSpeed);
            }

            if (forward)
            {
                currentOffset += sweepAngle;
                if (currentOffset >= sweepAngle) forward = false;
            }
            else
            {
                currentOffset -= sweepAngle;
                if (currentOffset <= -sweepAngle) forward = true;
            }

            yield return new WaitForSeconds(waveCooldown);
            timer += waveCooldown;
        }
    }
    
}
