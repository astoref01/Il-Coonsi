using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SurfaceManager : MonoBehaviour
{
    private static SurfaceManager _instance;
    public static SurfaceManager Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one SurfaceManager active in the scene! Destroying latest one: " + name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [SerializeField]
    private List<SurfaceType> Surfaces = new List<SurfaceType>();
    [SerializeField]
    private Surface DefaultSurface;
    private Dictionary<GameObject, ObjectPool<GameObject>> ObjectPools = new();

    public void HandleImpact(GameObject HitObject, Vector3 HitPoint, Vector3 HitNormal, ImpactType Impact, int TriangleIndex)
    {
       
        // Controlla se l'oggetto colpito è un Terrain
        if (HitObject.TryGetComponent<Terrain>(out Terrain terrain))
        {
            // Ottiene le texture attive dal Terrain al punto di impatto
            List<TextureAlpha> activeTextures = GetActiveTexturesFromTerrain(terrain, HitPoint);

            // Itera attraverso le texture attive
            foreach (TextureAlpha activeTexture in activeTextures)
            {
                // Trova il tipo di superficie corrispondente alla texture attiva
                SurfaceType surfaceType = Surfaces.Find(surface => surface.Albedo == activeTexture.Texture);

                if (surfaceType != null)
                {
                    // Se esiste un tipo di superficie corrispondente, applica gli effetti di impatto
                    foreach (Surface.SurfaceImpactTypeEffect typeEffect in surfaceType.Surface.ImpactTypeEffects)
                    {
                        if (typeEffect.ImpactType == Impact)
                        {
                            // Esegue gli effetti di superficie con l'alpha della texture
                            PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, activeTexture.Alpha);
                        }
                    }
                }
                else
                {
                    // Se non esiste un tipo di superficie corrispondente, applica gli effetti di impatto della superficie di default
                    foreach (Surface.SurfaceImpactTypeEffect typeEffect in DefaultSurface.ImpactTypeEffects)
                    {
                        if (typeEffect.ImpactType == Impact)
                        {
                            // Esegue gli effetti di superficie con un'alpha di 1 (default)
                            PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, 1);
                        }
                    }
                }
            }
        }
        // Se l'oggetto colpito non è un Terrain, controlla se è un Renderer
        else if (HitObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            // Ottiene la texture attiva dal Renderer al punto di impatto
            Texture activeTexture = GetActiveTextureFromRenderer(renderer, TriangleIndex);

        
            // Trova il tipo di superficie corrispondente alla texture attiva
            SurfaceType surfaceType = Surfaces.Find(surface => surface.Albedo == activeTexture);

            if (surfaceType != null)
            {
                // Se esiste un tipo di superficie corrispondente, applica gli effetti di impatto
                foreach (Surface.SurfaceImpactTypeEffect typeEffect in surfaceType.Surface.ImpactTypeEffects)
                {
                    if (typeEffect.ImpactType == Impact)
                    {
                        // Esegue gli effetti di superficie con un'alpha di 1 (default)
                        PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, 1);
                    }
                }
            }
            else
            {
                // Se non esiste un tipo di superficie corrispondente, applica gli effetti di impatto della superficie di default
                foreach (Surface.SurfaceImpactTypeEffect typeEffect in DefaultSurface.ImpactTypeEffects)
                {
                    if (typeEffect.ImpactType == Impact)
                    {
                        // Esegue gli effetti di superficie con un'alpha di 1 (default)
                        PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, 1);
                    }
                }
            }
        }
    }


    private List<TextureAlpha> GetActiveTexturesFromTerrain(Terrain Terrain, Vector3 HitPoint)
    {
        Vector3 terrainPosition = HitPoint - Terrain.transform.position;
        Vector3 splatMapPosition = new Vector3(
            terrainPosition.x / Terrain.terrainData.size.x,
            0,
            terrainPosition.z / Terrain.terrainData.size.z
        );

        int x = Mathf.FloorToInt(splatMapPosition.x * Terrain.terrainData.alphamapWidth);
        int z = Mathf.FloorToInt(splatMapPosition.z * Terrain.terrainData.alphamapHeight);

        float[,,] alphaMap = Terrain.terrainData.GetAlphamaps(x, z, 1, 1);

        List<TextureAlpha> activeTextures = new List<TextureAlpha>();
        for (int i = 0; i < alphaMap.Length; i++)
        {
            if (alphaMap[0, 0, i] > 0)
            {
                activeTextures.Add(new TextureAlpha()
                {
                    Texture = Terrain.terrainData.terrainLayers[i].diffuseTexture,
                    Alpha = alphaMap[0, 0, i]
                });
            }
        }

        return activeTextures;
    }

    private Texture GetActiveTextureFromRenderer(Renderer Renderer, int TriangleIndex)
    {
        if (Renderer.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
        {
            Mesh mesh = meshFilter.mesh;

            if (mesh.subMeshCount > 1)
            {
                int[] hitTriangleIndices = new int[]
                {
                    mesh.triangles[TriangleIndex * 3],
                    mesh.triangles[TriangleIndex * 3 + 1],
                    mesh.triangles[TriangleIndex * 3 + 2]
                };

                for (int i = 0; i < mesh.subMeshCount; i++)
                {
                    int[] submeshTriangles = mesh.GetTriangles(i);
                    for (int j = 0; j < submeshTriangles.Length; j += 3)
                    {
                        if (submeshTriangles[j] == hitTriangleIndices[0]
                            && submeshTriangles[j + 1] == hitTriangleIndices[1]
                            && submeshTriangles[j + 2] == hitTriangleIndices[2])
                        {
                            return Renderer.sharedMaterials[i].mainTexture;
                        }
                    }
                }
            }
            else
            {
                return Renderer.sharedMaterial.mainTexture;
            }
        }

        Debug.LogError($"{Renderer.name} has no MeshFilter! Using default impact effect instead of texture-specific one because we'll be unable to find the correct texture!");
        return null;
    }

    private void PlayEffects(Vector3 HitPoint, Vector3 HitNormal, SurfaceEffect SurfaceEffect, float SoundOffset)
    {
        foreach (SpawnObjectEffect spawnObjectEffect in SurfaceEffect.SpawnObjectEffects)
        {
            if (spawnObjectEffect.Probability > Random.value)
            {
                if (!ObjectPools.ContainsKey(spawnObjectEffect.Prefab))
                {
                    ObjectPools.Add(spawnObjectEffect.Prefab, new ObjectPool<GameObject>(() => Instantiate(spawnObjectEffect.Prefab)));
                }

                GameObject instance = ObjectPools[spawnObjectEffect.Prefab].Get();

                if (instance.TryGetComponent(out PoolableObject poolable))
                {
                    poolable.Parent = ObjectPools[spawnObjectEffect.Prefab];
                }
                instance.SetActive(true);
                instance.transform.position = HitPoint + HitNormal * 0.001f;
                instance.transform.forward = HitNormal;

                if (spawnObjectEffect.RandomizeRotation)
                {
                    Vector3 offset = new Vector3(
                        Random.Range(0, 180 * spawnObjectEffect.RandomizedRotationMultiplier.x),
                        Random.Range(0, 180 * spawnObjectEffect.RandomizedRotationMultiplier.y),
                        Random.Range(0, 180 * spawnObjectEffect.RandomizedRotationMultiplier.z)
                    );

                    instance.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + offset);
                }
            }
        }

        foreach (PlayAudioEffect playAudioEffect in SurfaceEffect.PlayAudioEffects)
        {
            if (!ObjectPools.ContainsKey(playAudioEffect.AudioSourcePrefab.gameObject))
            {
                ObjectPools.Add(playAudioEffect.AudioSourcePrefab.gameObject, new ObjectPool<GameObject>(() => Instantiate(playAudioEffect.AudioSourcePrefab.gameObject)));
            }

            AudioClip clip = playAudioEffect.AudioClips[Random.Range(0, playAudioEffect.AudioClips.Count)];
            GameObject instance = ObjectPools[playAudioEffect.AudioSourcePrefab.gameObject].Get();
            instance.SetActive(true);
            AudioSource audioSource = instance.GetComponent<AudioSource>();

            audioSource.transform.position = HitPoint;
            audioSource.PlayOneShot(clip, SoundOffset * Random.Range(playAudioEffect.VolumeRange.x, playAudioEffect.VolumeRange.y));
            StartCoroutine(DisableAudioSource(ObjectPools[playAudioEffect.AudioSourcePrefab.gameObject], audioSource, clip.length));
        }
    }

    private IEnumerator DisableAudioSource(ObjectPool<GameObject> Pool, AudioSource AudioSource, float Time)
    {
        yield return new WaitForSeconds(Time);

        AudioSource.gameObject.SetActive(false);
        Pool.Release(AudioSource.gameObject);
    }

    private class TextureAlpha
    {
        public float Alpha;
        public Texture Texture;
    }
}