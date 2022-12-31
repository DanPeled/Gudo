# GUDO

# Files Explanation

## [Movement.cs](https://github.com/DanPeled/Gudo/blob/master/Scripts/Player/Movement.cs)

  defines a number of variables and functions that are used to control the character's movement, animation, and interactions with the game world.

  The **Start()** function is called when the script is first run, and is used to initialize the character's state. In this function, the health bar is set to the character's maximum health, the character's sprite is enabled and its Animator component is set up, and the TypesRun component is initialized.

  The **Update()** function is called every frame and is used to update the character's state and handle user input. In this function, the character's inventory is checked, a raycast is performed to determine what the character is currently pointing at, and the character's animation state is set based on the player's actions.

  The **FixedUpdate()** function is called every fixed frame and is used to handle the character's movement. In this function, the character's velocity is set based on the player's input and the character's current run speed.

## [Building.cs](https://github.com/DanPeled/Gudo/blob/master/Scripts/Player/Building.cs)

 It has several functions including handling the player's inventory, placing blocks, and switching between items in the inventory using the number keys.

The Start() function initializes the player's inventory by adding four tools to the first four slots and setting the rest of the slots to an empty slot object. It also populates a dictionary with the IDs of blocks as the keys and the corresponding block GameObjects as the values.

The Update() function handles player input to place and drop blocks, as well as switch between items in the inventory using the number keys. When the player right-clicks, it will try to place the current block in their inventory at the mouse position. If the player left-clicks, it will try to dig the block at the mouse position. If the player scrolls the mouse wheel, it will switch to the next or previous item in the inventory.

The AddBlock() function creates a new Slot object with the given parameters and returns it. A Slot object contains information about an item in the inventory such as its name, description, rarity, amount, and the GameObject of the item.

The none_ and none variables are used to store an empty GameObject and the empty variable is an instance of the Slot class with default values for an empty slot. The blocks dictionary is used to look up the GameObject of a block based on its ID.

The script also has a RaycastHit2D object named hit which is used to check for collisions with blocks when the player tries to place or dig a block. It also has several arrays for storing different types of GameObjects such as placeable blocks, tools, and the inventory UI. The blockIndex variable is used to keep track of which item in the inventory is currently selected. The creative boolean determines whether the player is in creative mode, which allows them to place an unlimited amount of blocks. The player variable is a reference to the Movement script attached to the player GameObject. The items array stores information about all the items in the game.

## [PerlinNoiseMap.cs](https://github.com/DanPeled/Gudo/blob/master/Scripts/Generation/PerlinNoiseMap.cs)

The **Start()** function is called when the script is first run, and is used to initialize the map generation process. In this function, the tileset (a dictionary of tile prefabs) and tile groups (empty game objects used to group tiles of the same type) are created, and the **GenerateMap()** function is called as a coroutine.

The **GenerateMap()** function is responsible for actually generating the Perlin noise map. This function uses nested for loops to iterate over each tile position on the map, generating a Perlin noise value for each tile using the **GetIdUsingPerlin()** function. The Perlin noise value is then used to determine which tile prefab to use for the current tile position, and a tile game object is created using the **CreateTile()** function. The tile game object is then added to the tile grid (a 2D list of game objects), and the function yields for a short period before continuing to the next tile. This yields are used to spread out the map generation process over time, allowing the game to continue running and displaying progress to the player.

## [HealthBarScript.cs](https://github.com/DanPeled/Gudo/blob/master/Scripts/HealthBarScript.cs)

The health bar is represented by a Slider UI element, which is a horizontal bar that can be filled to show the current value within a specified range.

The SetMaxHealth function sets the maximum value for the health bar, which represents the maximum health of the game object. This function also sets the current value of the health bar to the maximum value, so that the health bar is fully filled when the game starts.

The SetHealth function sets the current value of the health bar to the specified health value. This can be used to update the health bar as the game object takes damage or recovers health.

The **MonoBehaviour** class is a base class for scripts in Unity. This script derives from **MonoBehaviour** and overrides two of its methods, Start and Update, to implement the behavior of the health bar. The Start method is called when the script is first loaded, and the Update method is called every frame, allowing the script to update the health bar as needed.

## [Time.cs](https://github.com/DanPeled/Gudo/blob/master/Scripts/Time.cs)

This script keeps track of in-game time and adjusts certain aspects of the game depending on the current time. The script is attached to a game object in the Unity scene.

The script starts by declaring several variables at the top:

currentTime: a float variable that keeps track of the current time in the game, with a default value of 0.
day: a boolean variable that indicates whether it is currently day or night in the game.
times: an enum (enumeration) type called Times with three possible values: day, noon, and night. Each value is assigned a number.
playerAround, global, and playerMiddle: three variables of type GameObject that represent objects in the Unity scene. These objects are not defined in this script, but they are likely assigned to these variables in the Unity editor or through other scripts.
The script also includes several functions:
setPlayerLightState: a function that takes a boolean value called state as an input and sets the active state of both playerAround and playerMiddle objects to state.
Start: a special function in Unity that is called when the script is first loaded. In this case, the setPlayerLightState function is called with false as an input, which deactivates both playerAround and playerMiddle objects.
Update: a special function in Unity that is called every frame of the game. The UpdateTime function is called within this function.
UpdateTime: a function that is called every frame, and updates the currentTime variable based on whether it is currently day or night. The function also adjusts the intensity of the light on the playerAround and playerMiddle objects based on the current time. If the current time is between 30 and 90, the light intensity of these objects is set to a value between 0 and 0.5, with a value of 0.5 at 90. If the current time is less than 30, the light intensity is set to 0. The function also has a yield return statement that waits for 1 second before continuing, to slow down the rate at which currentTime is updated.
# [GameHandler.cs](https://github.com/DanPeled/Gudo/blob/master/Scripts/GameHandler.cs)
This code is responsible for saving and loading the game state in a Unity project. It uses the JsonUtility class to serialize and deserialize the game data to and from a JSON string.

The SaveGame method is responsible for saving the game data. It does this by creating a SaveData object and filling it with the current state of the game. It then converts this object to a JSON string using JsonUtility.ToJson and saves it to a file with the specified saveFileName.

The LoadGame method is responsible for loading the game data. It first checks if the save file exists, and if it does, it reads the contents of the file and converts it back to a SaveData object using JsonUtility.FromJson. It then sets the game state to the values stored in the SaveData object.
