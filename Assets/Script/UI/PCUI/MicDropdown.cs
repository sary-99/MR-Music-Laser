using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class MicDropdownManager : MonoBehaviour, IPointerDownHandler
{
    public Mic mic;
    public TMP_Dropdown micDropdownUI;
    List<string> _micNameList = new List<string>();

    void Awake()
    {
        micDropdownUI.ClearOptions();
    }

    public void ApplyMicNameToDropdown(string newMicName)
    {
        // Debug.Log("micDropdownUI" + micDropdownUI.options[micDropdownUI.value].text);
        if (newMicName == micDropdownUI.options[micDropdownUI.value].text) return;//現在のドロップダウンの値と同じなら何もしない
        if (_micNameList.Contains(newMicName))
        {
            micDropdownUI.value = _micNameList.IndexOf(newMicName);
            micDropdownUI.RefreshShownValue();
        }
        else
        {
            Debug.LogWarning("Mic name not found in the list: " + newMicName);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mic Dropdown Clicked");
        mic.FindMic(); // ドロップダウンがクリックされたときにマイクデバイスを探す
    }
    public void OnValueChanged()
    {
        int index = micDropdownUI.value;
        mic.MicChange(micDropdownUI.options[index].text);
    }

    //参考: https://hirokuma.blog/?p=941
    //   https://xr-hub.com/archives/12118

    /// <summary>
    ///   もしマイクドロップダウンの内容と_micDeviceNames_listが異なる場合ドロップダウンを更新
    /// </summary>
    public void UpdateDropdown(List<string> newMicList)
    {

        if (newMicList.Count != micDropdownUI.options.Count)
        {
            Debug.Log("Reset Miclist");
            ResetMicList(newMicList);
            return;
        }
        for (int i = 0; Math.Max(newMicList.Count, micDropdownUI.options.Count) > i; i++)
        {
            if (newMicList[i] != micDropdownUI.options[i].text)
            {//ドロップダウンの内容と相違があったら更新
                ResetMicList(newMicList);
                Debug.Log("Update Miclist");
                break;
            }
        }
    }

    /// <summary>
    /// 既存リストを全てクリアしてマイクリストを再設定（今後の拡張用）
    /// </summary>
    public void ResetMicList(List<string> newMicList)
    {
        Debug.Log("reset!" + newMicList);
        _micNameList = new List<string>(newMicList);
        micDropdownUI.ClearOptions();
        micDropdownUI.AddOptions(_micNameList);
        micDropdownUI.RefreshShownValue();
        if (micDropdownUI.options[micDropdownUI.value].text == null) micDropdownUI.value = 0;//リストの最初のデバイス
    }

    public void UpdateMicList(List<string> newMicList)
    {
        _micNameList = new List<string>(newMicList);
        UpdateDropdown(newMicList);
    }

    //リストに追加
    public void AddMicList(string newMicName)
    {
        _micNameList.Add(newMicName);
        micDropdownUI.options.Add(new TMP_Dropdown.OptionData { text = newMicName });//新たなマイクをドロップダウンメニューに追加
        micDropdownUI.RefreshShownValue();
        if (micDropdownUI.options[micDropdownUI.value].text == null) micDropdownUI.value = 0;//リストの最初のデバイス
    }
}


