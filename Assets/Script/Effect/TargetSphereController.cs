using UnityEngine;
using System.Collections.Generic;
using Oculus.Interaction;
public class TargetSphereController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<GameObject> targetSphereParentList;//left,center,right
    List<GameObject> _sphereMeshList = new List<GameObject>();
    [SerializeField] MqController mqController;
    [SerializeField] GrabInteractorFinder grabInteractorFinder;

    [Header("Other GameObjects")]
    public ConicalFrustum[] selectionFrustums;//ReticleLineと同じ階層に存在

    [Header("Properties")]
    float _grabableLength = 10f;
    GameObject _leftGrabObjParent;
    GameObject _leftGrabObjMesh;
    GameObject _rightGrabObjParent;
    GameObject _rightGrabObjMesh;
    float _ratio = 0.1f;//移動速度
    bool _isVisible = false;
    public bool IsVisible { get { return _isVisible; } }

    public GameObject LeftTarget => targetSphereParentList[0];
    public GameObject CenterTarget => targetSphereParentList[1];
    public GameObject RightTarget => targetSphereParentList[2];

    void Start()
    {
        _sphereMeshList.Clear();
        //targetSphereParentの子オブジェクトをsphereMeshに格納
        foreach (var targetSphere in targetSphereParentList)
        {
            var obj = targetSphere.GetComponentInChildren<MeshRenderer>().gameObject;
            // Debug.Log("sphere mesh: " + obj.name);
            _sphereMeshList.Add(obj);
        }
        //開始時は全て表示
        VisibleTargets();
        ChangeGrabableLength(_grabableLength);
    }

    void Update()
    {
        if (_leftGrabObjParent != null)
        {
            var moveObj = _leftGrabObjMesh;
            var cont = mqController.GetLController();
            // Debug.Log("ContL Pos: " + cont.pos);
            var direction = (_leftGrabObjParent.transform.position - cont.pos).normalized;
            MoveSphere(moveObj, direction * cont.stick.y);
            // Debug.Log("L direction: " + cont.direction + "stick: " + cont.stick.y);
        }
        if (_rightGrabObjParent != null)
        {
            var moveObj = _rightGrabObjMesh;
            var cont = mqController.GetRController();
            var direction = (_rightGrabObjParent.transform.position - cont.pos).normalized;
            MoveSphere(moveObj, direction * cont.stick.y);
            // Debug.Log("mesh: " + moveObj.name);
            // Debug.Log("R direction: " + cont.direction + "stick: " + cont.stick.y);
        }

        //SphereをReset
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("Push A Button: Reset Sphere Position", this);
            ResetSpherePos();
        }
    }
    /// <summary>
    /// Hostで無いならターゲット球体を非表示にする
    /// </summary>
    public void HideTargetSpheres()
    {
        Debug.Log("Not Host: Hide Target Spheres", this);
        foreach (var targetSphereParent in targetSphereParentList)
        {
            targetSphereParent.SetActive(false);
        }
    }

    public void VisibleTargets()
    {
        _isVisible = !_isVisible;
        if (_sphereMeshList == null)
        {
            Debug.Log("sphereMeshList is null", this);
            // return;
        }
        if (_sphereMeshList.Count == 0)
        {
            Debug.Log("sphereMeshList is null or empty " + _sphereMeshList.Count, this);
            // return;
        }
        foreach (var sphereMesh in _sphereMeshList)
        {
            var meshRenderer = sphereMesh.GetComponentInChildren<MeshRenderer>();
            meshRenderer.enabled = _isVisible;
        }
    }

    public void WhenGrabbed(GameObject grabbedObj, long grabSourceID)
    {
        Debug.Log("WhenGrabbed: " + grabbedObj.name + ", grabSourceid: " + grabSourceID);
        var grabSource = grabInteractorFinder.FindGrabInteractor(grabSourceID);
        if (grabSource == GrabInteractorFinder.GrabInteractor.leftController)
        {
            _leftGrabObjParent = grabbedObj;
            _leftGrabObjMesh = grabbedObj.GetComponentInChildren<MeshRenderer>().gameObject;
        }
        else if (grabSource == GrabInteractorFinder.GrabInteractor.rightController)
        {
            _rightGrabObjParent = grabbedObj;
            _rightGrabObjMesh = grabbedObj.GetComponentInChildren<MeshRenderer>().gameObject;
        }

    }

    public void WhenReleased(GameObject releasedObj, long grabSourceID)
    {
        Debug.Log("WhenReleased: " + releasedObj.name + ", grabSourceid: " + grabSourceID);
        var grabSource = grabInteractorFinder.FindGrabInteractor(grabSourceID);
        if (grabSource == GrabInteractorFinder.GrabInteractor.leftController)
        {
            TeleportParentSphere(_leftGrabObjParent);
            _leftGrabObjParent = null;
            _leftGrabObjMesh = null;
        }
        else if (grabSource == GrabInteractorFinder.GrabInteractor.rightController)
        {
            TeleportParentSphere(_rightGrabObjParent);
            _rightGrabObjParent = null;
            _rightGrabObjMesh = null;
        }
    }

    /// <summary>
    /// SphereをStickで移動させる
    /// </summary>
    /// <param name="move"></param>
    public void MoveSphere(GameObject obj, Vector3 move)
    {
        obj.transform.position += move * _ratio;
        // Debug.Log("MoveSphere: " + move);
    }

    /// <summary>
    /// Parentを離した時に呼ばれる
    /// </summary>
    void TeleportParentSphere(GameObject parentObj)
    {
        var childObj = parentObj.GetComponentInChildren<MeshRenderer>().gameObject;
        var pos = childObj.transform.position;
        var offset = pos - parentObj.transform.position;

        parentObj.transform.position = pos;
        childObj.transform.position -= offset;
    }

    int cnt = 0;
    public void ResetSpherePos()
    {
        cnt++;
        Debug.Log("ResetSpherePos cnt: " + cnt);

        float x = -0.2f;
        Debug.Log("ResetSpherePos SphereCount: " + targetSphereParentList.Count);
        for (int i = 0; i < targetSphereParentList.Count; i++)
        {
            var targetSphereParent = targetSphereParentList[i];
            targetSphereParent.transform.localPosition = new Vector3(x, 0.5f, 0.5f);
            x += 0.2f;
            Debug.Log("ResetSphere[" + i + "]Pos: " + targetSphereParent.name);
        }
    }

    /// <summary>
    /// オブジェクトを掴める最大距離を変更,デフォルト値は5f
    /// </summary>
    void ChangeGrabableLength(float length)
    {
        for (int i = 0; i < selectionFrustums.Length; i++)
        {
            selectionFrustums[i].MaxLength = length;
            Debug.Log("Left ConicalFrustum Length set to: " + length, selectionFrustums[i].gameObject);
        }
    }
    /// <summary>
    /// 確認用
    /// </summary>
    // public void ParentMeshRender()
    // {
    //     foreach (var targetSphereParent in targetSphereParentList)
    //     {
    //         var meshRenderer = targetSphereParent.GetComponentInChildren<MeshRenderer>();
    //         meshRenderer.enabled = !meshRenderer.enabled;
    //     }
    // }
}
