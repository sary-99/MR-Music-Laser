using UnityEngine;
using Oculus.Interaction;

/// <summary>
/// LaserTargetParentにアタッチ
/// </summary>

[RequireComponent(typeof(PointableUnityEventWrapper))]
public class GrabEvent : MonoBehaviour
{
    [Header("References")]
    // [SerializeField] GrabInteractorFinder grabInteractorFinder;
    [SerializeField] TargetSphereController sphereController;

    [Header("Properties")]
    GameObject _grabedObj;//掴まれるオブジェクト
    bool _isGrabbed = false;
    public bool IsGrabbed { get { return _isGrabbed; } }

    void Awake()
    {
        _grabedObj = this.gameObject;
        if (_grabedObj == null)
        {
            Debug.LogError("GrabEvent: _grabedObj is null");
        }
        var eventWrapper = GetComponent<PointableUnityEventWrapper>();
        eventWrapper.WhenSelect.AddListener(WhenSelect);
        eventWrapper.WhenUnselect.AddListener(WhenUnselect);
    }

    /// <summary>
    /// このオブジェクトが掴まれたとき
    /// </summary>
    /// <param name="evt"></param>
    public void WhenSelect(PointerEvent evt)
    {
        _isGrabbed = true;
        sphereController?.WhenGrabbed(_grabedObj, evt.Identifier);
        Debug.Log("Grabbed: " + _grabedObj.name + "\nidentifier: " + evt.Identifier, this);
    }

    /// <summary>
    /// このオブジェクトが離されたとき
    /// </summary>
    /// <param name="evt"></param>
    public void WhenUnselect(PointerEvent evt)
    {
        _isGrabbed = false;
        sphereController?.WhenReleased(_grabedObj, evt.Identifier);
        Debug.Log("Released: " + _grabedObj.name + "\nidentifier: " + evt.Identifier, this);
    }
}
