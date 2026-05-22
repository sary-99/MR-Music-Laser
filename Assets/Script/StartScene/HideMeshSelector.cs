using UnityEngine;
using UnityEngine.UI;
public class HideMeshSelector : MonoBehaviour
{
    [SerializeField] StartGameInfo startGameInfo;
    [SerializeField] Toggle hideMeshToggle;

    void Start()
    {
        startGameInfo.hideEffectMesh = hideMeshToggle.isOn;
    }

    public void OnSelect_HideMesh(bool hide)
    {
        Debug.Log("HideMeshSelector OnSelect_HideMesh: " + hide);
        startGameInfo.hideEffectMesh = hide;
    }
}
