using UnityEngine;
public class FpsSetter : MonoBehaviour
{
    [SerializeField] int _fps = 72;
    void Awake()
    {
        Application.targetFrameRate = _fps;
    }
}
