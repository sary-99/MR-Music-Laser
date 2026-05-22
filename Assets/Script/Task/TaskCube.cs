using UnityEngine;

/// <summary>
/// TaskCubeに直接アタッチ
/// </summary>
public class TaskCube : MonoBehaviour
{
    GameObject _cube;
    GameObject _laserTarget;
    GameObject _laserTargetMesh;
    // public GrabEvent grabEvent;//Sphereが掴まれているかチェックするため
    float _hitDistance = 0.5f;

    void Awake()
    {
        _cube = this.gameObject;
    }

    public void SetLaserTarget(GameObject targetmesh)
    {
        _laserTarget = targetmesh;
        _laserTargetMesh = _laserTarget.transform.GetChild(0).gameObject;
        // Debug.Log("Laser Target Set: " + _laserTarget.name);
    }

    public void VisibleCube(bool isVisible)
    {
        _cube.GetComponent<Renderer>().enabled = isVisible;
    }

    /// <summary>
    /// このキューブのpositionと引数が規定の距離以内であればtrueを返す
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckCollision(Vector3 pos)
    {
        Vector3 cubePos = _cube.transform.position;
        float distance = Vector3.Distance(pos, cubePos);
        // Debug.Log("Cube" + cubePos + "Sphere" + pos + " Distance: " + distance);
        if (distance < _hitDistance && _laserTarget.GetComponent<GrabEvent>().IsGrabbed == false)
        {
            //離された時にヒットを返す
            Debug.Log("Hit the target!", _cube);
            return true;

        }
        return false;
    }
}
