public enum SoundName
{
    none = 0,
    effectFootstepSoftGround = 10,
    effectFootstepHardGround = 20,
    effectAxe = 30,
    effectPickaxe = 40,
    effectScythe = 50,
    effectHoe = 60,
    effectWateringCan = 70,
    effectBasket = 80,
    effectPickupSound = 90,
    effectRustle = 100,
    effectTreeFalling = 110,
    effectPlantingSound = 120,
    effectPluck = 130,
    effectStoneShatter = 140,
    effectWoodSplinters = 150,
    ambientCountryside1 = 1000,
    ambientCountryside2 = 1010,
    ambientIndoors1 = 1020,
    musicCalm3 = 2000,
    musicCalm1 = 2010,
    bushMusic = 2020
}

public enum HarvestActionEffect
{
    deciduousLeavesFalling,
    pineConesFalling,
    choppingTreeTrunk,
    breakingStone,
    reaping,
    none
}

public enum AnimationName
{
    idleDown,
    idleUp,
    idleRight,
    idleLeft,

    walkUp,
    walkDown,
    walkRight,
    walkLeft,

    runUp,
    runDown,
    runRight,
    runLeft,

    useToolUp,
    useToolDown,
    useToolRight,
    useToolLeft,

    swingToolUp,
    swingToolDown,
    swingToolRight,
    swingToolLeft,

    liftToolUp,
    liftToolDown,
    liftToolRight,
    liftToolLeft,

    holdToolUp,
    holdToolDown,
    holdToolRight,
    holdToolLeft,

    pickUp,
    pickDown,
    pickRight,
    pickLeft,

    count



}


public enum CharacterPartAnimator
{
    body,
    arms,
    hair,
    tool,
    hat,
    count
}


public enum PartVariantColour
{
    none,
    count
}

public enum SceneName
{
    Scene_Bush,
    Scene_Astral,
    Scene_Underground,
    Scene_Simulator
}

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter,
    none,
    count
}

public enum PartVariantType
{
    none,
    carry,
    hoe,
    pickaxe,
    axe,
    scythe,
    wateringCan,
    count
}

public enum GridBoolProperty
{
    diggable,
    canDropItem,
    canPlaceBuilding,
    isPath,
    isNPCObstacle
}

public enum InventoryLocation
{
    player,
    chest,
    count
}


public enum ToolEffect
{
    none,
    lubricate

}

public enum Direction
{
    up,
    down,
    left,
    right,
    none
}

public enum ItemType
{
    Seed,
    Commodity,
    Watering_Tool,
    Hoeing_Tool,
    Fixing_Tool,
    Breaking_Tool,
    Reaping_Tool,
    Collecting_Tool,
    Furniture,
    Building,
    Reapable_scenery,
    NPC,
    none,
    count

}

public enum Facing
{
    none,
    front,
    back,
    right
}

public enum Weather
{
    dry,
    raining,
    snowing,
    none,
    count
}
