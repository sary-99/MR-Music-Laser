using UnityEngine;

/// <summary>
/// レーザーの色が目標の色と一致しているかチェックするクラス
/// </summary>
public class LaserColorChecker : MonoBehaviour
{
    Material[] _leftMat = new Material[2];
    Material[] _centerMat = new Material[2];
    Material[] _rightMat = new Material[2];
    public Material[] LeftMat => _leftMat;
    public Material[] CenterMat => _centerMat;
    public Material[] RightMat => _rightMat;

    /// <summary>
    /// レーザーのマテリアルを取得する
    /// </summary>
    void GetLasers()
    {
        _leftMat[0] = EffectLists.Instance.LeftLaserObjectList[0].GetComponent<Renderer>().material;
        _leftMat[1] = EffectLists.Instance.LeftLaserObjectList[1].GetComponent<Renderer>().material;
        _centerMat[0] = EffectLists.Instance.CenterLaserObjectList[0].GetComponent<Renderer>().material;
        _centerMat[1] = EffectLists.Instance.CenterLaserObjectList[1].GetComponent<Renderer>().material;
        _rightMat[0] = EffectLists.Instance.RightLaserObjectList[0].GetComponent<Renderer>().material;
        _rightMat[1] = EffectLists.Instance.RightLaserObjectList[1].GetComponent<Renderer>().material;
    }

    /// <summary>
    /// レーザーの色が目標の色と一致しているかチェックする
    /// </summary>
    /// <param name="laserType">レーザーの種類</param>
    /// <param name="targetColor">目標の色</param>
    /// <returns>一致していればtrue、それ以外はfalse</returns>
    public bool CheckLaserColor(EffectLists.EffectType laserType, Color targetColor)
    {
        for (int i = 0; i < _leftMat.Length; i++)
        {
            if (_leftMat[i] == null || _centerMat[i] == null || _rightMat[i] == null)
            {
                try
                {
                    GetLasers();
                }
                catch (System.Exception e)
                {
                    Debug.LogError("LaserColorChecker:Maybe No Laser:\n" + e.Message);
                    return false;
                }
                break;
            }
        }

        switch (laserType)
        {
            case EffectLists.EffectType.leftLaser:
                Debug.Log($"Left Laser Color1: {_leftMat[0].GetColor("_Color1")}, Color2: {_leftMat[1].GetColor("_Color1")}, TargetColor: {targetColor}");
                return (_leftMat[0].GetColor("_Color1") == targetColor && _leftMat[1].GetColor("_Color1") == targetColor);
            case EffectLists.EffectType.centerLaser:
                Debug.Log($"Center Laser Color1: {_centerMat[0].GetColor("_Color1")}, Color2: {_centerMat[1].GetColor("_Color1")}, TargetColor: {targetColor}");
                return (_centerMat[0].GetColor("_Color1") == targetColor && _centerMat[1].GetColor("_Color1") == targetColor);
            case EffectLists.EffectType.rightLaser:
                Debug.Log($"Right Laser Color1: {_rightMat[0].GetColor("_Color1")}, Color2: {_rightMat[1].GetColor("_Color1")}, TargetColor: {targetColor}");
                return (_rightMat[0].GetColor("_Color1") == targetColor && _rightMat[1].GetColor("_Color1") == targetColor);
            default:
                Debug.LogError("LaserColorChecker: Invalid laser type.");
                return false;
        }
    }
}
