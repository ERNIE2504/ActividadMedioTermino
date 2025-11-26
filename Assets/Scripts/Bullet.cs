using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float MAX_LIFE_TIME = 3f;
    private float _lifeTime = 0f;

    public Vector2 Velocity;

    private bool _wasActive = false;

    private void OnEnable()
    {
        _wasActive = true;
        _lifeTime = 0f;
    }

    private void Update()
    {
        transform.position += (Vector3)Velocity * Time.deltaTime;
        _lifeTime += Time.deltaTime;

        if (_lifeTime > MAX_LIFE_TIME)
            Disable();
    }

    private void Disable()
    {
        _lifeTime = 0f;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (_wasActive && BulletPool.Instance != null)
            BulletPool.Instance.ReturnBullet(this);

        _wasActive = false;
    }
}
