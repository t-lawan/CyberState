using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Astral : SingletonMonoBehaviour<Scene_Astral>
{
    [SerializeField] private Egregore[] egregores;
    private int numberOfEgregoresActivated = 0;
    private bool egregoesActivated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AllEgregoresActivated() && !egregoesActivated)
        {
            Debug.Log("AllEgregoresActivated");
            egregoesActivated = true;
            UIManager.Instance.EnableStoryMenu();
        }
    }

    public void ActivateEgregore()
    {
        numberOfEgregoresActivated++;
        UIManager.Instance.EnableStoryMenu();


    }

    public bool AllEgregoresActivated()
    {
        return egregores.Length == numberOfEgregoresActivated;
    }
}
