using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GenerateForestArea : MonoBehaviour
{
    public bool trees = true;
    public bool bushes = false;
    public bool grass = false;
    public bool addBillboardToBushes = false;
    public LayerMask groundMask;
    public float treeTileSize = 5f;
    public float grassTileSize = 5f;
    public float bushTileSize = 3f;
    public float scale = 2f;
    public float treeSpawnThreshold = 0.5f;
    public float grassSpawnThreshold = 0.2f;
    public float bushSpawnThreshold = 0.2f;
    public float treeScaleHeightMin = 0.7f;
    public float treeScaleHeightMax = 1.2f;
    public List<GameObject> treePrefabs;
    public List<GameObject> grassPrefabs;
    public List<GameObject> bushPrefabs;
    public Transform cameraTransform;
    public List<Sound> birdSounds;
    public float maxSoundVolume;
    public Transform Player;

    private float[,] tree_map;
    private float[,] grass_map;
    private float[,] bush_map;
    private float[,] ground_map;
    private float[,] height_map;

    private Vector3 center;
    private Vector2 size;
    private float noise_offsetX;
    private float noise_offsetY;

    private AudioSource source;

    void Start()
    {
        BoxCollider collider_area = GetComponent<BoxCollider>();
        center = collider_area.center;
        size = new Vector2(collider_area.size.x, collider_area.size.z);
        collider_area.enabled = false;

        if (trees)
            GenerateTrees();
        if (bushes)
            Invoke(nameof(GenerateBushes), 0.5f);
        if (grass)
            Invoke(nameof(GenerateGrass), 0.5f);

        source = GetComponent<AudioSource>();
    }

    private void CreateImage(float[,] arr, string filename)
    {
        int width = arr.GetUpperBound(0) + 1;
        int height = arr.GetUpperBound(1) + 1;
        Texture2D tx = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++){
                float v = arr[x, y];
                tx.SetPixel(x, y, new Color(v, v, v));
            }
        }
        var path = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllBytes(path, tx.EncodeToPNG());
    }

    private void Update()
    {
        if (center.x - size.x / 2 + this.transform.position.x < Player.position.x && Player.position.x <= center.x + size.x / 2 + this.transform.position.x &&
            center.z - size.y / 2 + this.transform.position.z < Player.position.z && Player.position.z <= center.z + size.y / 2 + this.transform.position.z)
        {
            int numStepsX = (int)(size.x / treeTileSize);
            float stepX = size.x / numStepsX;
            int numStepsZ = (int)(size.y / treeTileSize);
            float stepZ = size.y / numStepsZ;

            float[] relationsThingsSheeesh = { Player.transform.position.x - (center.x - size.x / 2 + this.transform.position.x),
                                                Player.transform.position.z - (center.z - size.y / 2 + this.transform.position.z) };
            int[] positions = { (int)(relationsThingsSheeesh[0] / stepX), (int)(relationsThingsSheeesh[1] / stepZ) };

            if (tree_map[positions[0], positions[1]] > 0.4f)
            {
                if (!source.isPlaying)
                {
                    int soundIndex = Random.Range(0, birdSounds.Count);
                    Sound sound = birdSounds[soundIndex];
                    source.clip = sound.clip;
                    source.Play();
                }
                source.volume = (tree_map[positions[0], positions[1]] - 0.4f) / 0.6f * maxSoundVolume;
            }
            else
                source.volume = Mathf.Lerp(source.volume, 0f, Time.deltaTime);
        }
    }

    void GenerateTrees()
    {
        int numStepsX = (int)(size.x / treeTileSize);
        float stepX = size.x / numStepsX;
        int numStepsZ = (int)(size.y / treeTileSize);
        float stepZ = size.y / numStepsZ;

        tree_map = new float[numStepsX, numStepsZ];
        ground_map = new float[numStepsX, numStepsZ];
        height_map = new float[numStepsX, numStepsZ];

        // calculate top left corner on world coords
        float start_x = center.x - size.x / 2f + treeTileSize / 2f + this.transform.position.x;
        float start_z = center.z - size.y / 2f + treeTileSize / 2f + this.transform.position.z;

        // Initialize ground map
        for (int x = 0; x < numStepsX; x++)
        {
            float world_x = x * stepX + start_x;
            for (int z = 0; z < numStepsZ; z++)
            {
                float world_z = z * stepZ + start_z;

                RaycastHit hit;
                bool hit_ground = Physics.Raycast(new Vector3(world_x, 100f, world_z), Vector3.down,
                    out hit, 120f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

                if (hit_ground && hit.transform.tag == "VegetationGround" && Vector3.Angle(hit.normal, Vector3.up) < 30f)
                {
                    ground_map[x, z] = 1.0f;
                    height_map[x, z] = hit.point.y;
                }
                else
                {
                    ground_map[x, z] = 0.0f;
                    height_map[x, z] = 0.0f;
                }
            }
        }

        // Initialize noise map and set ground margins
        noise_offsetX = Random.Range(0f, 100f);
        noise_offsetY = Random.Range(0f, 100f);

        float max_noise_value = 0.0f;
        for (int x = 0; x < numStepsX; x++)
        {
            float noise_x = x / (float)numStepsX * scale + noise_offsetX;
            for (int z = 0; z < numStepsZ; z++)
            {
                float noise_y = z / (float)numStepsZ * scale + noise_offsetY;
                float noise_value = Mathf.PerlinNoise(noise_x, noise_y);

                if (noise_value > max_noise_value)
                    max_noise_value = noise_value;

                // ground margins
                float ground_map_value = ground_map[x, z];
                if (x > 0 && x < numStepsX - 1 && z > 0 && z < numStepsZ - 1
                    && (ground_map[x - 1, z] == 0.0f || ground_map[x, z - 1] == 0.0f ||
                        ground_map[x + 1, z] == 0.0f || ground_map[x, z + 1] == 0.0f ||
                        ground_map[x + 1, z - 1] == 0.0f || ground_map[x - 1, z + 1] == 0.0f ||
                        ground_map[x + 1, z + 1] == 0.0f || ground_map[x - 1, z - 1] == 0.0f))
                    ground_map_value = 0.0f;

                tree_map[x, z] = noise_value * ground_map_value;
            }
        }

        // Spawn trees
        // Only spawn with a value above 0.5. Change prefab linearly with the value above 0.5 and the number of types available. 
        // Small randomization on X, Z position and scale
        for (int x = 0; x < numStepsX; x++)
        {
            float world_x = x * stepX + start_x;
            for (int z = 0; z < numStepsZ; z++)
            {
                float world_z = z * stepZ + start_z;
                float noise_map_value = tree_map[x, z];

                if (noise_map_value > treeSpawnThreshold)
                {
                    /*float random = Random.Range(0.5f, max_noise_value);
                    if(random > noise_map_value)
                        noise_map_value = random;
                    */
                    float randomX = Random.Range(stepX / -3f, stepX / 3f);
                    float randomZ = Random.Range(stepZ / -3f, stepZ / 3f);

                    float randomScale = Random.Range(treeScaleHeightMin, treeScaleHeightMax);

                    int index = (int)((max_noise_value - noise_map_value) / (max_noise_value - treeSpawnThreshold) * treePrefabs.Count);
                    Vector3 position = new Vector3(world_x + randomX, height_map[x, z], world_z + randomZ);
                    GameObject tree = Instantiate(treePrefabs[index], position, Quaternion.identity);
                    tree.transform.localScale *= randomScale;
                    tree.transform.position = position;
                    tree.transform.parent = this.transform;
                }
            }
        }

        CreateImage(ground_map, "ground_map.png");
        CreateImage(height_map, "height_map.png");
        CreateImage(tree_map, "tree_map.png");
    }

    void GenerateGrass()
    {
        int numStepsX = (int)(size.x / grassTileSize);
        float stepX = size.x / numStepsX;
        int numStepsZ = (int)(size.y / grassTileSize);
        float stepZ = size.y / numStepsZ;

        grass_map = new float[numStepsX, numStepsZ];
        ground_map = new float[numStepsX, numStepsZ];
        height_map = new float[numStepsX, numStepsZ];

        // calculate top left corner on world coords
        float start_x = center.x - size.x / 2f + grassTileSize / 2f + this.transform.position.x;
        float start_z = center.z - size.y / 2f + grassTileSize / 2f + this.transform.position.z;

        // Initialize ground map
        for (int x = 0; x < numStepsX; x++)
        {
            float world_x = x * stepX + start_x;
            for (int z = 0; z < numStepsZ; z++)
            {
                float world_z = z * stepZ + start_z;

                RaycastHit hit;
                bool hit_ground = Physics.Raycast(new Vector3(world_x, 100f, world_z), Vector3.down,
                    out hit, 120f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                if (hit_ground && hit.transform.tag == "VegetationGround")
                {
                    ground_map[x, z] = 1.0f;
                    height_map[x, z] = hit.point.y;
                }
                else
                {
                    ground_map[x, z] = 0.0f;
                    height_map[x, z] = 0.0f;
                }
            }
        }

        // Initialize noise map and set ground margins
        noise_offsetX = Random.Range(0f, 100f);
        noise_offsetY = Random.Range(0f, 100f);

        for (int x = 0; x < numStepsX; x++)
        {
            float noise_x = x / (float)numStepsX * scale + noise_offsetX;
            for (int z = 0; z < numStepsZ; z++)
            {
                float noise_y = z / (float)numStepsZ * scale + noise_offsetY;
                float noise_value = Mathf.PerlinNoise(noise_x, noise_y);

                // ground margins
                float ground_map_value = ground_map[x, z];
                if (x > 0 && x < numStepsX - 1 && z > 0 && z < numStepsZ - 1
                    && (ground_map[x - 1, z] == 0.0f || ground_map[x, z - 1] == 0.0f ||
                        ground_map[x + 1, z] == 0.0f || ground_map[x, z + 1] == 0.0f ||
                        ground_map[x + 1, z - 1] == 0.0f || ground_map[x - 1, z + 1] == 0.0f ||
                        ground_map[x + 1, z + 1] == 0.0f || ground_map[x - 1, z - 1] == 0.0f))
                    ground_map_value = 0.0f;

                grass_map[x, z] = noise_value * ground_map_value;
            }
        }

        // Spawn grass
        // Only spawn with a value above grassSpawnThreshold. Small randomization on X and Z position
        for (int x = 0; x < numStepsX; x++)
        {
            float world_x = x * stepX + start_x;
            for (int z = 0; z < numStepsZ; z++)
            {
                float world_z = z * stepZ + start_z;
                float noise_map_value = grass_map[x, z];

                if (noise_map_value > grassSpawnThreshold)
                {
                    float randomX = Random.Range(stepX / -2f, stepX / 2f);
                    float randomZ = Random.Range(stepZ / -2f, stepZ / 2f);

                    int index = (int)(Random.Range(0f, 1f) * grassPrefabs.Count);
                    GameObject grass = Instantiate(grassPrefabs[index], new Vector3(world_x + randomX, height_map[x, z], world_z + randomZ), Quaternion.identity);
                    grass.transform.parent = this.transform;
                    grass.transform.GetChild(0).gameObject.SetActive(false);
                    grass.name = "Grass";
                }
            }
        }

        CreateImage(grass_map, "grass_map.png");
    }

    void GenerateBushes()
    {
        int numStepsX = (int)(size.x / bushTileSize);
        float stepX = size.x / numStepsX;
        int numStepsZ = (int)(size.y / bushTileSize);
        float stepZ = size.y / numStepsZ;

        bush_map = new float[numStepsX, numStepsZ];
        ground_map = new float[numStepsX, numStepsZ];
        height_map = new float[numStepsX, numStepsZ];

        // calculate top left corner on world coords
        float start_x = center.x - size.x / 2f + bushTileSize / 2f + this.transform.position.x;
        float start_z = center.z - size.y / 2f + bushTileSize / 2f + this.transform.position.z;

        // Initialize ground map
        for (int x = 0; x < numStepsX; x++)
        {
            float world_x = x * stepX + start_x;
            for (int z = 0; z < numStepsZ; z++)
            {
                float world_z = z * stepZ + start_z;

                RaycastHit hit;
                bool hit_ground = Physics.Raycast(new Vector3(world_x, 100f, world_z), Vector3.down,
                    out hit, 120f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

                if (hit_ground && hit.transform.tag == "VegetationGround")
                {
                    ground_map[x, z] = 1.0f;
                    height_map[x, z] = hit.point.y;
                }
                else
                {
                    ground_map[x, z] = 0.0f;
                    height_map[x, z] = 0.0f;
                }
            }
        }

        // Initialize noise map and set ground margins
        for (int x = 0; x < numStepsX; x++)
        {
            float noise_x = x / (float)numStepsX * scale + noise_offsetX;
            for (int z = 0; z < numStepsZ; z++)
            {
                float noise_y = z / (float)numStepsZ * scale + noise_offsetY;
                float noise_value = Mathf.PerlinNoise(noise_x, noise_y);

                // bushes have the opposite map from trees
                noise_value = 1f - noise_value;

                // ground margins
                float ground_map_value = ground_map[x, z];
                if (x > 0 && x < numStepsX - 1 && z > 0 && z < numStepsZ - 1
                    && (ground_map[x - 1, z] == 0.0f || ground_map[x, z - 1] == 0.0f ||
                        ground_map[x + 1, z] == 0.0f || ground_map[x, z + 1] == 0.0f ||
                        ground_map[x + 1, z - 1] == 0.0f || ground_map[x - 1, z + 1] == 0.0f ||
                        ground_map[x + 1, z + 1] == 0.0f || ground_map[x - 1, z - 1] == 0.0f))
                    ground_map_value = 0.0f;

                bush_map[x, z] = noise_value * ground_map_value;
            }
        }

        // Spawn bushes
        // Only spawn with a value above bushSpawnThreshold. Small randomization on X and Z position
        for (int x = 0; x < numStepsX; x++)
        {
            float world_x = x * stepX + start_x;
            for (int z = 0; z < numStepsZ; z++)
            {
                float world_z = z * stepZ + start_z;
                float noise_map_value = bush_map[x, z];

                if (noise_map_value > bushSpawnThreshold)
                {
                    float randomX = Random.Range(stepX / -3f, stepX / 3f);
                    float randomZ = Random.Range(stepZ / -3f, stepZ / 3f);

                    int index = (int)(Random.Range(0f, 1f) * bushPrefabs.Count);
                    Vector3 position = new Vector3(world_x + randomX, height_map[x, z], world_z + randomZ);
                    GameObject bush = Instantiate(bushPrefabs[index], position, Quaternion.identity);
                    if (addBillboardToBushes)
                        bush.AddComponent<BillboardClamped>().cam = cameraTransform;
                    bush.transform.parent = this.transform;
                }
            }
        }

        CreateImage(bush_map, "bush_map.png");
    }
}
