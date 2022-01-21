using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScrollingTextManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public float scrollSpeed = 10.0f;

    private TextMeshProUGUI m_clone_textMeshProUGUI;

    public RectTransform rectTransform;
    private string text;
    private string tempText;
    private float scrollPosition;
    private float width;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Awake()
    {
        //rectTransform = textMeshProUGUI.GetComponent<RectTransform>();
        scrollPosition = 0.0f;

    }

    // Update is called once per frame
    public void Start()
    {
        width = textMeshProUGUI.preferredWidth;
        startPos = rectTransform.localPosition;

        StartCoroutine(Scroll());


    }

    private IEnumerator Scroll()
    {
        while (true)
        {

            rectTransform.localPosition = new Vector3(-scrollPosition, startPos.y, startPos.z);
            float factor = scrollSpeed * 20 * Time.deltaTime;
            scrollPosition = scrollPosition + factor;

            yield return null;
        }
    }
}
