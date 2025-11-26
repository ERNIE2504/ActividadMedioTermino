using TMPro;
using UnityEngine;

public class BulletCounter: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;

    private void Update()
    {
        if (BulletPool.Instance != null)
        {
            counterText.text = "Balas: " + BulletPool.Instance.ActiveBullets;
        }
    }
}
