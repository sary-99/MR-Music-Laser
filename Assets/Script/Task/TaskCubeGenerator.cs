using UnityEngine;

public class TaskCubeGenerator : MonoBehaviour
{
    [Header("Other Objects")]
    public GameObject cubePrefab;
    public Material centerCubeMaterial;
    public Material leftCubeMaterial;
    public Material rightCubeMaterial;
    [Header("Laser targets")]
    public GameObject leftLaserTargetMesh;
    public GameObject centerLaserTargetMesh;
    public GameObject rightLaserTargetMesh;

    [Header("Properties")]
    GameObject _centerCube; //Yellow
    GameObject _leftCube; //Green
    GameObject _rightCube; //Red
    public GameObject CenterCube { get { return _centerCube; } }
    public GameObject LeftCube { get { return _leftCube; } }
    public GameObject RightCube { get { return _rightCube; } }


    void Awake()
    {
        // GenerateCubes();
    }


    public void GenerateCubes()
    {
        _centerCube = GenerateCube(centerCubeMaterial, new Vector3(0, 1.5f, 2f));
        _leftCube = GenerateCube(leftCubeMaterial, new Vector3(-0.5f, 1.5f, 2f));
        _rightCube = GenerateCube(rightCubeMaterial, new Vector3(0.5f, 1.5f, 2f));

        // 各キューブにレーザーターゲットメッシュをセット
        var center = _centerCube.GetComponent<TaskCube>();
        var left = _leftCube.GetComponent<TaskCube>();
        var right = _rightCube.GetComponent<TaskCube>();

        center.SetLaserTarget(centerLaserTargetMesh.transform.parent.gameObject);
        left.SetLaserTarget(leftLaserTargetMesh.transform.parent.gameObject);
        right.SetLaserTarget(rightLaserTargetMesh.transform.parent.gameObject);

        // 最初は非表示にする
        center.VisibleCube(false);
        left.VisibleCube(false);
        right.VisibleCube(false);

    }



    GameObject GenerateCube(Material cubeMaterial, Vector3 position)
    {
        var cubeInstance = Instantiate(cubePrefab, position, Quaternion.identity);
        // cubeMaterial.color = new Color(cubeMaterial.color.r + 0.2f, cubeMaterial.color.g + 0.2f, cubeMaterial.color.b + 0.2f, 0.7f);//薄くする
        //色を薄くする
        Material newMat = new Material(cubeMaterial);
        newMat.color = new Color(newMat.color.r + 0.5f, newMat.color.g + 0.5f, newMat.color.b + 0.5f, 0.8f);//薄くする
        cubeInstance.GetComponent<Renderer>().material = newMat;
        return cubeInstance;
    }
}
