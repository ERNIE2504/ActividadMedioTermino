using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public int ActiveBullets { get; private set; } = 0;
    private static BulletPool _instance;
    public static BulletPool Instance
    {
        get
        {
            if (_instance == null )
            {
                Debug.LogError("BulletPoot Instance miissing");
            }
                
            return _instance;    
        }
    }

    [SerializeField] private Bullet _bulletPrefab ;
    [SerializeField] private int _initialPoolSize = 50;

    private List<Bullet> _bulletPool = new List<Bullet>();

    private void Awake()
    {
        if (_instance !=null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        else
        {
            _instance = this;
        }
        AddBulletsToPool(_initialPoolSize);
    }

    private void AddBulletsToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Bullet bullet = Instantiate(_bulletPrefab);
            bullet.gameObject.SetActive(false);
            _bulletPool.Add(bullet);
            bullet.transform.parent = transform;
        }
    }

    public Bullet RequestBullet()
    {
        for (int i= 0; i < _bulletPool.Count; i++)
        {
            if (!_bulletPool[i].gameObject.activeSelf)
            {
                ActiveBullets++;
                _bulletPool[i].gameObject.SetActive(true);
                return _bulletPool[i];
            }
        }

        AddBulletsToPool(1);

        Bullet newBullet = _bulletPool[_bulletPool.Count - 1];
        newBullet.gameObject.SetActive(true);
        ActiveBullets++;

        return newBullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        if (ActiveBullets > 0)
            ActiveBullets--;
    }

}
