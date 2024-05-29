using UnityEngine;
using UnityEngine.UI;

public class ShootingBarController : MonoBehaviour
{
    [SerializeField]
    private GunScriptableObject gunScriptableObject;
    public Slider shootingBar;
    public int maxShots = 100;
    private int currentShots;

    void Start()
    {
        shootingBar.maxValue = maxShots;
        gunScriptableObject.i = 0;
    }
    private void Update()
    {
        UpdateShootingBar();
    }
    private void UpdateShootingBar()
    {

        shootingBar.value = gunScriptableObject.i;
    }
}
