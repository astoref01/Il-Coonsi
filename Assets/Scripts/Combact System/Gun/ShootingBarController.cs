using UnityEngine;
using UnityEngine.UI;

public class ShootingBarController : MonoBehaviour
{
    [SerializeField]
    private GunScriptableObject gunScriptableObject;
    public Slider shootingBar;
    public int maxShots = 100;
    private int resetThreshold = 50;

    void Start()
    {
        shootingBar.maxValue = maxShots;
        gunScriptableObject.i = 0;
    }

    void Update()
    {
        UpdateShootingBar();
    }

    private void UpdateShootingBar()
    {
        shootingBar.value = gunScriptableObject.i;
        if (shootingBar.value >= resetThreshold && Input.GetMouseButtonDown(1))
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
        }
    }
}
