using UnityEngine;

[CreateAssetMenu(fileName = "Audio Config", menuName = "Guns/Audio Config", order = 5)]
public class AudioConfigScriptableObject : ScriptableObject
{
    [Range(0, 1f)]
    public float Volume = 1f;
    public AudioClip[] FireClips;

    public void PlayShootingClip(AudioSource AudioSource, bool IsLastBullet = false)
    {
        {
            AudioSource.PlayOneShot(FireClips[Random.Range(0, FireClips.Length)], Volume);
        }
    }

}
