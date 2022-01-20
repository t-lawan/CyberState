using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
[RequireComponent(typeof(GenerateGUID))]
[RequireComponent(typeof(NPCDna))]
public class NPC : MonoBehaviour, ISaveable
{
    private WaitForSeconds afterUseToolAnimationPause;
    private WaitForSeconds afterLiftToolAnimationPause;
    private AnimationOverrides animationOverrides;
    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    private NPCMovement npcMovement;
    private NPCDna dna;

    private void OnEnable()
    {
        ISaveableRegister();
    }

    private void OnDisable()
    {
        ISaveableDeregister();
    }

    private void Awake()
    {
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
        EventHandler.CallNPCHasBeenBornEvent(ISaveableUniqueID);

    }

    private void Start()
    {
        // get npc movement component
        npcMovement = GetComponent<NPCMovement>();
        dna = GetComponent<NPCDna>();

    }

    private void Update()
    {
        if (dna.isDead)
        {
            Die();
        }
    }

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void Reproduce()
    {
        float val = UnityEngine.Random.Range(0.0f, 1.0f);

        if (Mathf.Abs((val + dna.value) / 2) > Settings.reproductionRate)
        {
            //Debug.Log("Repriduce");
            EventHandler.CallNPCIncubateEvent(dna.value);
            InstatiateItems.Instance.InstatiateHumanoidNPC();
            dna.Mutate();

        }
        // Will send message to produce 
    }

    public void Die()
    {
        //Debug.Log("Die");
        EventHandler.CallNPCHasDiedEvent(ISaveableUniqueID);
        Destroy(gameObject);
           
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        EventHandler.CallDataIncrementNumberOfInteractions();
        Reproduce();
    }


    public void ISaveableLoad(GameSave gameSave)
    {
        // Get game object save
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;

            // Get scene save
            if (GameObjectSave.sceneData.TryGetValue(Settings.PersistentScene, out SceneSave sceneSave))
            {
                // if dictionaries are not null
                if (sceneSave.vector3Dictionary != null && sceneSave.stringDictionary != null)
                {
                    // target grid position
                    if (sceneSave.vector3Dictionary.TryGetValue("npcTargetGridPosition", out Vector3Serializable savedNPCTargetGridPosition))
                    {
                        npcMovement.npcTargetGridPosition = new Vector3Int((int)savedNPCTargetGridPosition.x, (int)savedNPCTargetGridPosition.y, (int)savedNPCTargetGridPosition.z);
                        npcMovement.npcCurrentGridPosition = npcMovement.npcTargetGridPosition;
                    }

                    // target world position
                    if (sceneSave.vector3Dictionary.TryGetValue("npcTargetWorldPosition", out Vector3Serializable savedNPCTargetWorldPosition))
                    {
                        npcMovement.npcTargetWorldPosition = new Vector3(savedNPCTargetWorldPosition.x, savedNPCTargetWorldPosition.y, savedNPCTargetWorldPosition.z);
                        transform.position = npcMovement.npcTargetWorldPosition;
                    }

                    // target scene
                    if (sceneSave.stringDictionary.TryGetValue("npcTargetScene", out string savedTargetScene))
                    {
                        if (Enum.TryParse<SceneName>(savedTargetScene, out SceneName sceneName))
                        {
                            npcMovement.npcTargetScene = sceneName;
                            npcMovement.npcCurrentScene = npcMovement.npcTargetScene;

                        }
                    }

                    // Clear any current NPC movement
                    npcMovement.CancelNPCMovement();

                }

            }

        }
    }

    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        // Nothing required here since on persistent scene
    }

    public GameObjectSave ISaveableSave()
    {
        // Remove current scene save
        GameObjectSave.sceneData.Remove(Settings.PersistentScene);

        // Create scene save
        SceneSave sceneSave = new SceneSave();

        // Create vector 3 serialisable dictionary
        sceneSave.vector3Dictionary = new Dictionary<string, Vector3Serializable>();

        // Create string dictionary
        sceneSave.stringDictionary = new Dictionary<string, string>();

        // Store target grid position, target world position, and target scene
        sceneSave.vector3Dictionary.Add("npcTargetGridPosition", new Vector3Serializable(npcMovement.npcTargetGridPosition.x, npcMovement.npcTargetGridPosition.y, npcMovement.npcTargetGridPosition.z));
        sceneSave.vector3Dictionary.Add("npcTargetWorldPosition", new Vector3Serializable(npcMovement.npcTargetWorldPosition.x, npcMovement.npcTargetWorldPosition.y, npcMovement.npcTargetWorldPosition.z));
        sceneSave.stringDictionary.Add("npcTargetScene", npcMovement.npcTargetScene.ToString());

        // Add scene save to game object
        GameObjectSave.sceneData.Add(Settings.PersistentScene, sceneSave);

        return GameObjectSave;
    }

    public void ISaveableStoreScene(string sceneName)
    {
        // Nothing required here since on persistent scene
    }
}