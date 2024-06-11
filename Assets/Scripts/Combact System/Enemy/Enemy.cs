using UnityEngine;

[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    public EnemyHealth Health;
    public EnemyPainResponse PainResponse;

    private void Start()
    {
        Health.OnTakeDamage += PainResponse.HandlePain;
        Health.OnDeath += Die;
    }

    private void Die(Vector3 Position)
    {
        PainResponse.HandleDeath();
    }
}
