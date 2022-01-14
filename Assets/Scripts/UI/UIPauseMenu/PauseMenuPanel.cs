using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuPanel : MonoBehaviour
{
    [SerializeField] private Sprite bushMenuBackground;
    [SerializeField] private Sprite simulationMenuBackground;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        SetImageSprite();

    }
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }

    private void SetImageSprite()
    {
        SceneName sceneName = SceneControllerManager.Instance.GetCurrentScene();
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                image.sprite = bushMenuBackground;
                break;
            case SceneName.Scene_Simulator:
                image.sprite = simulationMenuBackground;
                
                break;
        }
    }
    private void AfterSceneLoad()
    {
        Debug.Log("SET PIC");
        SetImageSprite();
        //image.
    }

}
