using UnityEngine;

public class MqController : MonoBehaviour
{
    [Header("References")]
    public TargetSphereController sphereController;
    OVRInput.Controller _controllerL = OVRInput.Controller.LTouch;//左コントローラー
    OVRInput.Controller _controllerR = OVRInput.Controller.RTouch;//右コントローラー
    MqCntData _leftCont;
    MqCntData _rightCont;

    public struct MqCntData
    {
        public Vector3 pos;
        public Vector2 stick;
        public Quaternion rotation;
        public Vector3 direction => rotation * new Vector3(0, 0, 1).normalized;
    }

    void Update()
    {
        _leftCont.stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _controllerL);//スティックの入力角度と量
        if (_leftCont.stick == Vector2.zero)
        {
            _leftCont.stick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, _controllerL);//スティックの入力角度と量
            // Debug.Log("LStick Second: " + leftCnt.stick);
        }
        else
        {
            // Debug.Log("LStick Primary: " + leftCont.stick);
        }
        _rightCont.stick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);//スティックの入力角度と量

        if (_rightCont.stick == Vector2.zero)
        {
            //片方のコントローラーしか認識されていないときの対策
            _rightCont.stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _controllerR);//スティックの入力角度と量
            // Debug.Log("RStick Primary: " + rightCont.stick);
        }
        else
        {
            // Debug.Log("RStick Second: " + rightCnt.stick);
        }
        // Debug.Log("StickL: " + leftCnt.stick + ", stickR: " + rightCnt.stick);

        _leftCont.pos = OVRInput.GetLocalControllerPosition(_controllerL);
        _rightCont.pos = OVRInput.GetLocalControllerPosition(_controllerR);

        _leftCont.rotation = OVRInput.GetLocalControllerRotation(_controllerL);
        _rightCont.rotation = OVRInput.GetLocalControllerRotation(_controllerR);
    }

    public MqCntData GetLController()
    {
        // Debug.Log("leftContStick: " + leftCont.stick);
        return _leftCont;
    }

    public MqCntData GetRController()
    {
        // Debug.Log("rightContStick: " + rightCont.stick);
        return _rightCont;
    }



}