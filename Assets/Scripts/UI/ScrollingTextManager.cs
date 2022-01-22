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
    private Dictionary<NarrativeType, string> texts;
    private NarrativeType narrativeType;

    // Start is called before the first frame update
    void Awake()
    {
        //rectTransform = textMeshProUGUI.GetComponent<RectTransform>();
        scrollPosition = 0.0f;
        narrativeType = NarrativeType.rentism;
        texts = new Dictionary<NarrativeType, string>();
        texts.Add(NarrativeType.rentism, rentism);
        //int i = NarrativeType.count
    }

    // Update is called once per frame
    public void Start()
    {
        //CleanTexts();
        width = textMeshProUGUI.preferredWidth;
        startPos = rectTransform.localPosition;
        textMeshProUGUI.text = texts[narrativeType];
        StartCoroutine(Scroll());
    }

    public void CleanTexts()
    {
        foreach (var text in texts)
        {
            //string t = text.Value.Replace(/\n / g, " ");
            //texts[text.Key] = t;
        }
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

    private string rentism = string.Join(
      "A series of catastrophic events at shores across the globe destroyed many homes and livelihoods. Families were separated, as all attempted to find refuge in states least affected by the events. However, these politically-unstable states were ill-equipped to deal with refugees due to ‘bickering’ between and within them. Although, this fracture in power was by design. The techno-corps had long conspired their downfall. This left a vacuum in which they proposed a solution to this problem, a smart city for the displaced.     After the #PROTESTHASHTAG movement, the ‘ai for all’ consortium of techno-corps was formed to create a new society in an uninhabited desert.     A society where rules were enshrined in code, open source for all to see and contested using decentralised technologies. A society where all production was automated.However, humans were required to maintain machinery required for their prosperity. The techno-corps collaborated to work on the technological infrastructure for the smart city, weaving in different stories of the past into the fabric of the datasets and code.They created a welcoming ceremony to implant a device in their earlobe. This device monitored the emotions and regulated their memories which were stored in the smart city distributed memory storage.",
"Since Day 0, [] has run the city, controlling the resource-extracting surveillance drones, allocating jobs to the citizens to clean and maintain the machines, allocating time for them to care and maintain their environment.However, conflicts soon emerged between groups who were for and against the fundamental principles of the smart city.",
"There was a great debate that swept the newly constructed smart city. The group against the principles of the city attempted to destroy the supposed mainframe. A small group of the citizens suddenly had vivid visions and multiple conflicting narratives enlodged with their memory. However, their attempts were short-lived as [] re-distributed the memories to other devices in the city.",
"The rebels fled the city into the desert with some of those who had the visions. They marched the desert for 40 days, following a faint glow leading to  a strange bush.",
"They met the sentient plants who were being exploited for their ashe. This ashe was a resource that had many uses.The drones from the smart city scoured the bush to locate the resource, damaging the sentient plants in the process. The plants also had agency in this relationship, they used the drones to clone themselves but also poisoned the ashe collected by the drones. The poison created a strange pink/purple smog which slowly killed the citizens and damaged all the buildings in the smart city.",
"The rebels slowly started to tend to the plants and protect them from the drones.The sentient plants began to trust the rebels and opened up a portal to consult the quantum egregores. The egregores were playful creatures who opened up different possibilities for the futures of all. They recited a new story which overrode the current memories of the origin story located in the shared memory system.");
}
