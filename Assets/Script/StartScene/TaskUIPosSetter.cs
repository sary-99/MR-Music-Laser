using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TaskUIPosSetter : MonoBehaviour
{
    [SerializeField] TaskUIPos taskUIPos_SO;
    [SerializeField] GameObject taskTextParent;
    TextMeshProUGUI _taskTMPro;
    [SerializeField] GameObject centerEyeAnchor;
    [SerializeField] Vector3 initTaskUIPos = new Vector3(0f, -0.2f, 0.5f);
    [SerializeField] float initFontSize = 50f;
    [SerializeField] Slider xSlider;
    [SerializeField] Slider ySlider;
    [SerializeField] Slider zSlider;
    [SerializeField] Slider fontsizeSlider;
    [SerializeField] string testText = "This is a test task text.\nPlease adjust the position\nand font size using the sliders.";

    void Awake()
    {
        taskUIPos_SO.pos = initTaskUIPos;
        taskUIPos_SO.fontSize = initFontSize;
    }
    void Start()
    {
        xSlider.value = taskUIPos_SO.pos.x;
        ySlider.value = taskUIPos_SO.pos.y;
        zSlider.value = taskUIPos_SO.pos.z;
        fontsizeSlider.value = taskUIPos_SO.fontSize;

        _taskTMPro = taskTextParent.GetComponentInChildren<TextMeshProUGUI>();
        if (_taskTMPro == null)
        {
            Debug.LogError("TaskUIManager: TextMeshProUGUI component not found in taskTextParent.");
            return;
        }

        _taskTMPro.text = testText;
    }
    void Update()
    {
        taskUIPos_SO.pos = new Vector3(xSlider.value, ySlider.value, zSlider.value);
        taskUIPos_SO.fontSize = fontsizeSlider.value;

        taskTextParent.transform.localPosition = taskUIPos_SO.pos;
        _taskTMPro.fontSize = taskUIPos_SO.fontSize;
        // Debug.Log("TaskUIPosSetter Update: " + centerEyeAnchor.transform.position + taskUIPos.pos, taskTextParent);
    }



}
