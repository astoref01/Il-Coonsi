using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    public ImpactType ImpactType;
    public DamageConfigScriptableObject DamageConfig;
    public GunType Type;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    public int i = 0;


    public ShootConfigScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;
    public AudioConfigScriptableObject audioConfig;
    private AudioSource ShootingAudioSource;

    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;

    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    public Transform AimTarget { get; private set; } // Mantieni privato, con solo il getter pubblico
    public Transform StoredAimTarget; // Memorizza l'AimTarget precedente
    public Transform PlayerHead; // Testa del giocatore
    public float maxAimAngle = 30f; // L'angolo massimo di mira assistita

    public void Start()
    {
        i = 0;
    }

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0; // in editor this will not be properly reset, in build it's fine
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
        ShootingAudioSource = Model.GetComponent<AudioSource>();

        // Inizializza AimTarget come null
        AimTarget = null;
    }

    public void UpdateAimTarget()
    {
        if (StoredAimTarget != null)
        {
            Vector3 directionToTarget = (StoredAimTarget.position - PlayerHead.position).normalized;
            float angle = Vector3.Angle(PlayerHead.forward, directionToTarget);

            if (angle > maxAimAngle)
            {
                if (AimTarget != null)
                {
                }
                AimTarget = null; // Disattiva l'aim assist se l'angolo è maggiore del massimo consentito
            }
            else
            {
                if (AimTarget == null)
                {
                }
                AimTarget = StoredAimTarget; // Riattiva l'aim assist se l'angolo è minore o uguale al massimo consentito
            }
        }
    }

    public void Shoot()
    {

        if (Time.time > ShootConfig.FireRate + LastShootTime)
        {
            LastShootTime = Time.time;
            ShootSystem.Play();
            audioConfig.PlayShootingClip(ShootingAudioSource);

            Vector3 shootDirection;

            if (AimTarget != null)
            {
                shootDirection = (AimTarget.position - ShootSystem.transform.position).normalized;
            }
            else
            {
                shootDirection = ShootSystem.transform.forward
                    + new Vector3(
                        Random.Range(
                            -ShootConfig.Spread.x,
                            ShootConfig.Spread.x
                        ),
                        Random.Range(
                            -ShootConfig.Spread.y,
                            ShootConfig.Spread.y
                        ),
                        Random.Range(
                            -ShootConfig.Spread.z,
                            ShootConfig.Spread.z
                        )
                    );
                shootDirection.Normalize();
            }

            if (Physics.Raycast(
                    ShootSystem.transform.position,
                    shootDirection,
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask
                ))
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        ShootSystem.transform.position,
                        hit.point,
                        hit
                    )
                );
            }
            else
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        ShootSystem.transform.position,
                        ShootSystem.transform.position + (shootDirection * TrailConfig.MissDistance),
                        new RaycastHit()
                    )
                );
            }
            

        }

    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    {

        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null; // avoid position carry-over from last frame if reused

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        if (Hit.collider != null)
        {
            SurfaceManager.Instance.HandleImpact(
                Hit.transform.gameObject,
                EndPoint,
                Hit.normal,
                ImpactType,
                0
            );

            if (Hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(DamageConfig.GetDamage(distance));
                i = i + 1;
            }

        }

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    public void ApplyDamage(Vector3 startPosition, Vector3 endPoint, int damage)
    {
        // Logica per applicare il danno direttamente
        if (Physics.Raycast(startPosition, (endPoint - startPosition).normalized, out RaycastHit hit, Vector3.Distance(startPosition, endPoint)))
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}