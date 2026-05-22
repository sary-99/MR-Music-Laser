using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using Photon.Voice.Unity;
//https://note.com/hikohiro/n/n2b756a0dd2b7
[System.Serializable]
public class Mic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MicDropdownManager micDropdown;//入力マイクを変更するドロップダウンメニュー

    [Header("Mic")]    //マイク(デバイス)に関するフィールド
    AudioSource _micAudioSource;//AudioSourceコンポーネント
    List<string> _micDeviceNames_list = new List<string>();//マイクデバイスの名前を格納する配列
    string _targetDevice = "";//マイクのデバイス名

    private void Awake()
    {
        _micAudioSource ??= GetComponent<AudioSource>();
    }

    void Start()
    {
        FindMic(); //マイクデバイスを探す
        MicChange(_micDeviceNames_list[0]);//最初に見つけたマイクデバイスを設定
    }
    /// <summary>
    /// マイクを探す
    /// </summary>
    public void FindMic()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("Mic is not find");
            return;
        }

        List<string> newDeviceList = Microphone.devices.ToList();//マイクデバイスの名前を格納する配列
        micDropdown.UpdateMicList(newDeviceList);//ドロップダウンメニューにマイクデバイスの名前を反映
                                                 // Debug.Log($"Mic Device Found: List {newDeviceList} devices found.");

        foreach (var device in newDeviceList)
        {
            Debug.Log($"Mic Device Found: {device}");
        }
        if (string.IsNullOrEmpty(_targetDevice)) _targetDevice = newDeviceList[0];//最初に見つけたデバイスをtargetDeviceとして設定

        //もし、_micDeveiceNames_listとnewDevice_listが異なる場合、_micDeveiceNames_listを更新
        if (!_micDeviceNames_list.SequenceEqual(newDeviceList))
        {
            _micDeviceNames_list = newDeviceList;
            Debug.Log($"Mic Device List Updated: {_micDeviceNames_list.Count} devices found.");
        }
    }

    float _t = 0;
    public string MicChange(string newDevice)
    {
        //newDeviceが_micDeveiceNames_listに含まれているか確認
        if (!_micDeviceNames_list.Contains(newDevice))
        {
            Debug.Log($"Device {newDevice} is not found in the list.");
            return null;
        }
        _micAudioSource?.Stop();//AudioSource(マイク)の再生を停止
        _micAudioSource.clip = null;
        _targetDevice = newDevice;//新しいマイクの名前に変更
        _micAudioSource.clip = Microphone.Start(_targetDevice, true, 1, 48000);//マイクの録音を開始
        while (!(Microphone.GetPosition(_targetDevice) > 0) || _t < 5)
        {
            _t += Time.deltaTime;
        } //マイクデバイスの準備ができるまで待つ

        _micAudioSource.Play();//AudioSource(マイク)の再生を開始
        micDropdown.ApplyMicNameToDropdown(_targetDevice);//最初に見つけたマイクをドロップダウンに適用
        Debug.Log($"=== New Device Set: {_targetDevice} ===");

        return _targetDevice;
    }
}
