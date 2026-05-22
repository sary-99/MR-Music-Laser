using UnityEngine;
using TMPro;
using Fusion;

public class DebugTxtManager : MonoBehaviour
{
    public static DebugTxtManager Instance { get; private set; }

    public TextMeshProUGUI mqDebugTextR;
    public TextMeshProUGUI mqDebugTextL;
    public TextMeshProUGUI pcDebugTextR;
    public TextMeshProUGUI pcDebugTextL;
    public StartGameInfo startGameInfo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("DebugTxtManager Awake", this);
    }
    public void DebugTxtPlusL(string msg)
    {
        if (startGameInfo.isMetaQuest)
        {
            mqDebugTextL.text += msg;
        }
        else
        {
            pcDebugTextL.text += msg;
        }
    }
    public void DebugTxtPlusR(string msg)
    {

        if (startGameInfo.isMetaQuest)
        {
            mqDebugTextR.text += msg;
        }
        else
        {
            pcDebugTextR.text += msg;
        }
    }
    public void DebugTxtSetL(string msg)
    {
        if (startGameInfo.isMetaQuest)
        {
            mqDebugTextL.text = msg;
        }
        else
        {
            pcDebugTextL.text = msg;
        }
        if (startGameInfo.isMetaQuest)
        {
            mqDebugTextL.text = msg;
        }
        else
        {
            pcDebugTextL.text = msg;
        }
    }
    public void DebugTxtSetR(string msg)
    {

        if (startGameInfo.isMetaQuest)
        {
            mqDebugTextR.text = msg;
        }
        else
        {
            pcDebugTextR.text = msg;
        }
    }
}