using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InstructionsPanel : SingletonMonoBehaviour<InstatiateItems>
{
    private bool isFading;
    [SerializeField] private float fadeDuration = 10f;

    [SerializeField] private TextMeshProUGUI instructionsText = null;
    private bool isShowingText = false;

    // Start is called before the first frame update

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;

    }

    public void FadeText()
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndUpdateText());
        }
    }

    private IEnumerator FadeAndUpdateText()
    {
        yield return StartCoroutine(Fade(1f));


        yield return StartCoroutine(Fade(0f));

    }

    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;


        float fadeSpeed = Mathf.Abs(instructionsText.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(instructionsText.alpha, finalAlpha))
        {
            instructionsText.alpha = Mathf.MoveTowards(instructionsText.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        isFading = false;
    }

    private void AfterSceneLoad()
    {
        FadeText();
        SceneName sceneName = SceneControllerManager.Instance.GetCurrentScene();
        string text;
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                text = "Tend to your plants";
                break;
            case SceneName.Scene_Astral:
                text = "Collect all the cowrie shell";
                break;
            case SceneName.Scene_Simulator:
                text = "The non-aligned countries came together to create a cybernetic state in a remote uninhabited desert";
                break;
            case SceneName.Scene_Underground:
                text = "The non-aligned countries came together to create a cybernetic state in a remote uninhabited desert";
                break;
            default:
                text = "";
                break;

        }
        SetText(text);
    }

    public void SetText(string text)
    {
        isShowingText = true;
        instructionsText.SetText(text);
    }


}
