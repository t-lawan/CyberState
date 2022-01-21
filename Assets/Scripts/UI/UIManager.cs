using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{

    private bool _pauseMenuOn = false;
    private bool _storyMenuOn = false;
    [SerializeField] private UIInventoryBar uiInventoryBar = null;
    [SerializeField] private PauseMenuInventoryManagement pauseMenuInventoryManagement = null;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject storyMenu = null;
    [SerializeField] private GameObject[] menuTabs = null;
    [SerializeField] private Button[] menuButtons = null;

    public bool PauseMenuOn { get => _pauseMenuOn; set => _pauseMenuOn = value; }
    public bool StoryMenuOn { get => _storyMenuOn; set => _storyMenuOn = value; }

    protected override void Awake()
    {
        base.Awake();

        pauseMenu.SetActive(false);
        storyMenu.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        //EnableStoryMenu();

        PauseMenu();
        StoryMenu();
    }

    private void PauseMenu()
    {
        // Toggle pause menu if escape is pressed

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenuOn)
            {
                DisablePauseMenu();
            }
            else
            {
                EnablePauseMenu();
            }
        }
    }

    private void StoryMenu()
    {
        // Toggle pause menu if escape is pressed

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (StoryMenuOn)
            {
                DisableStoryMenu();
            }
            else
            {
                EnableStoryMenu();
            }
        }
    }

    public void EnableStoryMenu()
    {
        StoryMenuOn = true;
        storyMenu.SetActive(true);
        Player.Instance.PlayerInputIsDisabled = true;
        //Time.timeScale = 0;
    }

    public void DisableStoryMenu()
    {
        StoryMenuOn = false;
        storyMenu.SetActive(false);
        Player.Instance.PlayerInputIsDisabled = false;
        Time.timeScale = 1;
        SceneControllerManager.Instance.FadeAndLoadScene(SceneName.Scene_Bush.ToString(), new Vector3(0f,0f,0f));
    }

    private void EnablePauseMenu()
    {
        //// Destroy any currently dragged items
        uiInventoryBar.DestroyCurrentlyDraggedItems();

        //// Clear currently selected items
        uiInventoryBar.ClearCurrentlySelectedItems();

        PauseMenuOn = true;
        Player.Instance.PlayerInputIsDisabled = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

        // Trigger garbage collector
        System.GC.Collect();

        // Highlight selected button
        HighlightButtonForSelectedTab();
    }

    public void DisablePauseMenu()
    {
        //// Destroy any currently dragged items
        pauseMenuInventoryManagement.DestroyCurrentlyDraggedItems();

        PauseMenuOn = false;
        Player.Instance.PlayerInputIsDisabled = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    private void HighlightButtonForSelectedTab()
    {
        for (int i = 0; i < menuTabs.Length; i++)
        {
            if (menuTabs[i].activeSelf)
            {
                SetButtonColorToActive(menuButtons[i]);
            }

            else
            {
                SetButtonColorToInactive(menuButtons[i]);
            }
        }
    }

    private void SetButtonColorToActive(Button button)
    {
        ColorBlock colors = button.colors;

        colors.normalColor = colors.pressedColor;

        button.colors = colors;

    }

    private void SetButtonColorToInactive(Button button)
    {
        ColorBlock colors = button.colors;

        colors.normalColor = colors.disabledColor;

        button.colors = colors;

    }

    public void SwitchPauseMenuTab(int tabNum)
    {
        for (int i = 0; i < menuTabs.Length; i++)
        {
            if (i != tabNum)
            {
                menuTabs[i].SetActive(false);
            }
            else
            {
                menuTabs[i].SetActive(true);

            }
        }

        HighlightButtonForSelectedTab();

    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
