using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonoBehaviour<SceneControllerManager>
{
    private bool isFading;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;
    private bool isStoryShowing = false;
    public SceneName startSceneName;

    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }
    }

    public SceneName GetCurrentScene()
    {
       Scene scene =  SceneManager.GetActiveScene();

        if(scene.name == SceneName.Scene_Bush.ToString())
        {
            return SceneName.Scene_Bush;
        }
        else if(scene.name == SceneName.Scene_Astral.ToString())
        {
            return SceneName.Scene_Astral;

        }
        else if (scene.name == SceneName.Scene_Simulator.ToString())
        {
            return SceneName.Scene_Simulator;

        }
        else if (scene.name == SceneName.Scene_Underground.ToString())
        {
            return SceneName.Scene_Underground;
        }

        return SceneName.Scene_Bush;
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
    {
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();
        yield return StartCoroutine(Fade(1f));

        SaveLoadManager.Instance.StoreCurrentSceneData();

        Player.Instance.gameObject.transform.position = spawnPosition;

        EventHandler.CallBeforeSceneUnloadEvent();


        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        EventHandler.CallAfterSceneLoadEvent();

        SaveLoadManager.Instance.RestoreCurrentSceneData();

        yield return StartCoroutine(Fade(0f));

        EventHandler.CallAfterSceneLoadFadeInEvent();
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);


        Scene newLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(newLoadedScene);
    }

    private IEnumerator Start()
    {
        faderImage.color = new Color(1f, 1f, 1f, 1f);
        faderCanvasGroup.alpha = 1f;

        yield return StartCoroutine(LoadSceneAndSetActive(startSceneName.ToString()));

        EventHandler.CallAfterSceneLoadEvent();

        SaveLoadManager.Instance.RestoreCurrentSceneData();

        StartCoroutine(Fade(0f));

    }

    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;

        faderCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        isFading = false;

        faderCanvasGroup.blocksRaycasts = false;
    }

    public void ShowStoryUI()
    {
        isStoryShowing = true;
    }

    public void HideStoryUI()
    {
        isStoryShowing = false;
    }

}
