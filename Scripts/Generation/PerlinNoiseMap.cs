using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class PerlinNoiseMap : MonoBehaviour
{
    public static PerlinNoiseMap instance;
    public GameObject txt;
    public GameObject[] enable;
    Dictionary<int, GameObject> tileset;
    public GameHandler gameHandler;
    Dictionary<int, GameObject> tile_groups;
    public GameObject[] blocks;
    public int map_width = 160;
    public int map_height = 90;
    [Range(-100000000f, 100000000f)] public float seed;
    public bool randomWorld;
    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();
    // recommend 4 to 20
    [Range(0f, 200f)] public float magnification = 14.5f;

    int x_offset = 0; // <- +>
    int y_offset = 0; // v- +^

    void Start()
    {
        if(randomWorld) seed = Random.Range(-Random.Range(0,100000000), Random.Range(0,100000000));
        if(seed == 63804990) seed = Random.Range(-Random.Range(0,100000000), Random.Range(0,100000000));
        
        GameObject.Find("Inventory").SetActive(false);
        GameObject.Find("HealthBar").SetActive(false);
        GameObject.Find("HealthBarIcon").SetActive(false);
        Camera.main.orthographicSize = 100;
        GameObject.Find("Player").GetComponent<Movement>().playerActive = false;
        CreateTileset();
        CreateTileGroups();
        StartCoroutine(GenerateMap((float)seed));
        instance = this;
        
    }
    void CreateTileset()
    {
        /** Collect and assign ID codes to the tile prefabs, for ease of access.
            Best ordered to match land elevation. **/

        tileset = new Dictionary<int, GameObject>();
        for (int i = 0; i < blocks.Length; i++)
        {
            tileset.Add(i, blocks[i]);
        }
    }

    void CreateTileGroups()
    {
        /** Create empty gameobjects for grouping tiles of the same type, ie
            forest tiles **/

        tile_groups = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, GameObject> prefab_pair in tileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0, 0, 0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }

    IEnumerator GenerateMap(float seed)
    {
        Camera.main.orthographicSize = 100;
        /** Generate a 2D grid using the Perlin noise fuction, storing it as
            both raw ID values and tile gameobjects **/
        int index = 0;
        int precentage;
        for (int x = 0; x < map_width; x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());
            yield return new WaitForSeconds(0.00001f);
            for (int y = 0; y < map_height; y++)
            {
                int tile_id = GetIdUsingPerlin(x, y,seed);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y);
                index++;
            }
            precentage = (index / 100) / 8;
            txt.GetComponent<Text>().text = string.Format("Loading - {0}%", precentage);
            GameObject.Find("loadingBar").GetComponent<Slider>().value = precentage;
            if (precentage == 100)
            {
                for(int i = 0; i < enable.Length; i++)
                {
                    enable[i].SetActive(true);
                }
                txt.transform.parent.gameObject.SetActive(false);
                for(int i = 100; i >= 5; i-=3){
                    Camera.main.orthographicSize = i;
                    yield return new WaitForSeconds(0.01f);
                }
                Camera.main.orthographicSize = 6;
            }
        }
        Debug.Log("Finsihed generating");
        GameObject.Find("Player").GetComponent<Movement>().playerActive = true;
    }

    int GetIdUsingPerlin(int x, int y, float seed)
    {
        /** Using a grid coordinate input, generate a Perlin noise value to be
            converted into a tile ID code. Rescale the normalised Perlin value
            to the number of tiles available. **/

        float raw_perlin = Mathf.PerlinNoise(
            (x - x_offset + seed) / magnification,
            (y - y_offset + seed) / magnification
        );
        float clamp_perlin = Mathf.Clamp01(raw_perlin);
        float scaled_perlin = clamp_perlin * tileset.Count;

        if (scaled_perlin == tileset.Count)
        {
            scaled_perlin = (tileset.Count - 1);
        }
        return Mathf.FloorToInt(scaled_perlin);
    }

    void CreateTile(int tile_id, int x, int y)
    {

        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);
        if (tile.name == "Water") tile.tag = "Water";

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);

        tile_grid[x].Add(tile);
    }
}