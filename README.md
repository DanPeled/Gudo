# Gudo
A 2D survival game

# Files explonation

## Movement.cs
  defines a number of variables and functions that are used to control the character's movement, animation, and interactions with the game world.

  The **Start()** function is called when the script is first run, and is used to initialize the character's state. In this function, the health bar is set to the character's maximum health, the character's sprite is enabled and its Animator component is set up, and the TypesRun component is initialized.

  The **Update()** function is called every frame and is used to update the character's state and handle user input. In this function, the character's inventory is checked, a raycast is performed to determine what the character is currently pointing at, and the character's animation state is set based on the player's actions.

  The **FixedUpdate()** function is called every fixed frame and is used to handle the character's movement. In this function, the character's velocity is set based on the player's input and the character's current run speed.
## Building.cs
defines a number of variables and functions that are used to manage the player's inventory, place and destroy blocks, and interact with the game world.

The **Start()** function is called when the script is first run, and is used to initialize the player's inventory. In this function, the player's inventory is filled with tools, and a dictionary of placeable blocks is created.

The **Update()** function is called every frame and is used to handle user input and update the game world. In this function, the player's inventory is checked for empty slots, and keyboard input is used to switch the player's currently selected block. The function also handles the placing and destroying of blocks based on the player's input.
## PerlinNoiseMap.cs
The **Start()** function is called when the script is first run, and is used to initialize the map generation process. In this function, the tileset (a dictionary of tile prefabs) and tile groups (empty game objects used to group tiles of the same type) are created, and the **GenerateMap()** function is called as a coroutine.

The **GenerateMap()** function is responsible for actually generating the Perlin noise map. This function uses nested for loops to iterate over each tile position on the map, generating a Perlin noise value for each tile using the **GetIdUsingPerlin()** function. The Perlin noise value is then used to determine which tile prefab to use for the current tile position, and a tile game object is created using the **CreateTile()** function. The tile game object is then added to the tile grid (a 2D list of game objects), and the function yields for a short period before continuing to the next tile. This yields are used to spread out the map generation process over time, allowing the game to continue running and displaying progress to the player.
## HealthBarScript.cs
The health bar is represented by a Slider UI element, which is a horizontal bar that can be filled to show the current value within a specified range.

The SetMaxHealth function sets the maximum value for the health bar, which represents the maximum health of the game object. This function also sets the current value of the health bar to the maximum value, so that the health bar is fully filled when the game starts.

The SetHealth function sets the current value of the health bar to the specified health value. This can be used to update the health bar as the game object takes damage or recovers health.

The **MonoBehaviour** class is a base class for scripts in Unity. This script derives from **MonoBehaviour** and overrides two of its methods, Start and Update, to implement the behavior of the health bar. The Start method is called when the script is first loaded, and the Update method is called every frame, allowing the script to update the health bar as needed.
