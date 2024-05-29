using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField]
    private GunType Gun;
    [SerializeField]
    private Transform GunParent;
    [SerializeField]
    private List<GunScriptableObject> Guns;
    [SerializeField]
    private PlayerIK InverseKinematics;
    [SerializeField]
    private Transform AITarget; // Aggiungi questo campo
    [SerializeField]
    private Transform PlayerHead; // Testa del giocatore
    [SerializeField]
    private float maxAimAngle = 30f; // L'angolo massimo di mira assistita

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject ActiveGun;

    private void Start()
    {
        GunScriptableObject gun = Guns.Find(gun => gun.Type == Gun);

        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            return;
        }

        ActiveGun = gun;
        gun.Spawn(GunParent, this);

        // Memorizza il target dell'AI
        ActiveGun.StoredAimTarget = AITarget;
        ActiveGun.PlayerHead = PlayerHead;
        ActiveGun.maxAimAngle = maxAimAngle;

        // some magic for IK
        Transform[] allChildren = GunParent.GetComponentsInChildren<Transform>();
        InverseKinematics.LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftElbow");
        InverseKinematics.RightElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "RightElbow");
        InverseKinematics.LeftHandIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftHand");
        InverseKinematics.RightHandIKTarget = allChildren.FirstOrDefault(child => child.name == "RightHand");
    }

    private void Update()
    {
        if (ActiveGun != null)
        {
            ActiveGun.UpdateAimTarget(); // Aggiorna l'aim target ogni frame
        }
    }
}
