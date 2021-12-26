
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
    Scene1_Farm,
    Scene2_Field,
    Scene3_Cabin,
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
    none,
    count

}

