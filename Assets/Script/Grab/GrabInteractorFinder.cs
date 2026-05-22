using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

/// <summary>
/// GrabInteractorとidの対応を調べてお
/// MetaのGrabbableコンポーネントなどで掴むことができるオブジェクトが掴まれたとき,GrabInteractorのidを取得できる
/// あらかじめGrabInteractorとidの対応を調べておくことで、どのGrabInteractorで掴まれたかを識別できる
/// </summary>
public class GrabInteractorFinder : MonoBehaviour
{
    [Header("Other Objects")]
    // [SerializeField] GameObject cameraRig; //[BuildingBlock] Camera Rig, OVRCameraRigのゲームオブジェクト
    [SerializeField] DistanceGrabInteractor lControllerDistanceGrabInteractor; //左コントローラーのControllerDistanceGrabInteractor
    [SerializeField] DistanceHandGrabInteractor lDistanceHandGrabInteractor; //左手のDistanceHandGrabInteractor
    [SerializeField] DistanceGrabInteractor rControllerDistanceGrabInteractor; //右コントローラーのControllerDistanceGrabInteractor
    [SerializeField] DistanceHandGrabInteractor rDistanceHandGrabInteractor; //右手のDistanceHandGrabInteractor

    [Header("References")]
    GrabInteractorIdentifierFinder idFinder = new GrabInteractorIdentifierFinder();

    /// <summary>
    /// GrabInteractorは掴む動作の発生源(左手、右手、左コントローラー、右コントローラー)
    /// </summary>
    public enum GrabInteractor
    {
        none,
        leftController,
        leftHand,
        rightController,
        rightHand
    }

    /// <summary>
    /// GrabInteractor(手,コントローラなど)を識別するためのクラス
    /// </summary>
    public struct GrabInteractorIdentifier
    {
        public GrabInteractor grabInteractor;
        public long id;
    }
    [Header("GrabInteractors")]
    GrabInteractorIdentifier _leftController = new GrabInteractorIdentifier { grabInteractor = GrabInteractor.leftController };
    GrabInteractorIdentifier _leftHand = new GrabInteractorIdentifier { grabInteractor = GrabInteractor.leftHand };
    GrabInteractorIdentifier _rightController = new GrabInteractorIdentifier { grabInteractor = GrabInteractor.rightController };
    GrabInteractorIdentifier _rightHand = new GrabInteractorIdentifier { grabInteractor = GrabInteractor.rightHand };

    void Start()
    {
        try
        {
            FindInteractorByName();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in FindInteractorByName\n" + ex.Message, this);
            return;
        }
    }

    /// <summary>
    /// cameraRigの階層からGameObject名でInteractorを探す
    /// OVRCameraRigの構造に依存するため脆弱
    /// </summary>
    void FindInteractorByName()
    {
        // //左手と右手のInteractionsオブジェクトを探す
        // var leftInteractions_obj = cameraRig.transform.Find("[BuildingBlock] OVRInteractionComprehensive/LeftInteractions");
        // var rightInteractions_obj = cameraRig.transform.Find("[BuildingBlock] OVRInteractionComprehensive/RightInteractions");
        // Debug.Log("leftInteractions_obj: " + leftInteractions_obj, leftInteractions_obj);
        // Debug.Log("rightInteractions_obj: " + rightInteractions_obj, rightInteractions_obj);

        // //掴む動作に関するコンポーネントを持つGameObjectを探す
        // //Transform.Find()はパスを正しく指定する必要がある
        // //https://shibuya24.info/entry/unity-transform-find
        // var leftControllerDistanceGrabInteractor = leftInteractions_obj.transform.Find(
        //     "Interactors/Controller and No Hand/ControllerDistanceGrabInteractor");
        // var leftDistanceHandGrabInteractor = leftInteractions_obj.transform.Find(
        //     "Interactors/Hand and No Controller/DistanceHandGrabInteractor");
        // var rightControllerDistanceGrabInteractor = rightInteractions_obj.transform.Find(
        //     "Interactors/Controller and No Hand/ControllerDistanceGrabInteractor");
        // var rightDistanceHandGrabInteractor = rightInteractions_obj.transform.Find(
        //     "Interactors/Hand and No Controller/DistanceHandGrabInteractor");

        // // 目的のコンポーネントを取得する
        // // gobの階層から全てのMonoBehaviourを取得する
        // // コントローラ使用時,手が非アクティブになる状態などを考慮し,(includeInactive: true) で非アクティブなオブジェクトも検索対象にする
        // MonoBehaviour[] mbArray_left_CDGI =
        //         leftControllerDistanceGrabInteractor.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);
        // MonoBehaviour[] mbArray_left_DHGI =
        //         leftDistanceHandGrabInteractor.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);
        // MonoBehaviour[] mbArray_right_CDGI =
        //         rightControllerDistanceGrabInteractor.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);
        // MonoBehaviour[] mbArray_right_DHGI =
        //         rightDistanceHandGrabInteractor.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);

        // 直接オブジェクト名からIdentifierを探す
        GrabInteractorIdentifierFinder.PointerEventIdentifier result_left_CDGI =
            idFinder.FindId(lControllerDistanceGrabInteractor); //左コントローラー
        GrabInteractorIdentifierFinder.PointerEventIdentifier result_left_DHGI =
            idFinder.FindId(lDistanceHandGrabInteractor); //左手
        GrabInteractorIdentifierFinder.PointerEventIdentifier result_right_CDGI =
            idFinder.FindId(rControllerDistanceGrabInteractor); //右コントローラー
        GrabInteractorIdentifierFinder.PointerEventIdentifier result_right_DHGI =
            idFinder.FindId(rDistanceHandGrabInteractor); //右手

        //見つかったidをそれぞれのGrabInteractorに割り当てる
        _leftController.id = result_left_CDGI.identifier;
        _leftHand.id = result_left_DHGI.identifier;
        _rightController.id = result_right_CDGI.identifier;
        _rightHand.id = result_right_DHGI.identifier;
        Debug.Log("左ControllerDistanceGrabInteractor: " + _leftController.id, lControllerDistanceGrabInteractor);
        Debug.Log("左DistanceHandGrabInteractor: " + _leftHand.id, lDistanceHandGrabInteractor);
        Debug.Log("右ControllerDistanceGrabInteractor: " + _rightController.id, rControllerDistanceGrabInteractor);
        Debug.Log("右DistanceHandGrabInteractor: " + _rightHand.id, rDistanceHandGrabInteractor);
    }


    /// <summary>
    /// idからGrabInteractorを返す,見つからなければGrabInteractor.noneを返す
    /// </summary>
    /// <param name="id">Oculus.Interaction.PointerEvent.Identifier</param>
    /// <returns></returns>
    public GrabInteractor FindGrabInteractor(long id)
    {
        if (id == _leftController.id)
        {
            Debug.Log("FindGrabInteractor: Found left controller interactor for identifier=" + id);
            return GrabInteractor.leftController;
        }
        else if (id == _leftHand.id)
        {
            Debug.Log("FindGrabInteractor: Found left hand interactor for identifier=" + id);
            return GrabInteractor.leftHand;
        }
        else if (id == _rightController.id)
        {
            Debug.Log("FindGrabInteractor: Found right controller interactor for identifier=" + id);
            return GrabInteractor.rightController;
        }
        else if (id == _rightHand.id)
        {
            Debug.Log("FindGrabInteractor: Found right hand interactor for identifier=" + id);
            return GrabInteractor.rightHand;
        }
        else
        {
            Debug.Log("FindGrabInteractor: No matching interactor found for identifier=" + id);
            return GrabInteractor.none;
        }
    }
}
