using UnityEngine;

public class TaskCubeHitChecker : MonoBehaviour
{
    [SerializeField] TaskManager taskManager;

    TaskCube _leftTaskComponent;
    TaskCube _centerTaskComponent;
    TaskCube _rightTaskComponent;

    void GetTaskCubeComponents()
    {
        _leftTaskComponent = taskManager.LeftCube.GetComponent<TaskCube>();
        _centerTaskComponent = taskManager.CenterCube.GetComponent<TaskCube>();
        _rightTaskComponent = taskManager.RightCube.GetComponent<TaskCube>();
    }
    /// <summary>
    /// 第１引数のPosと第２引数のレーザーのタスクキューブPosがヒット判定になるか
    /// </summary>
    /// <param name="chaserPos">Sphereを想定</param>
    /// <param name="type">タスクキューブ</param>
    /// <returns></returns>
    public bool CheckHit(Vector3 chaserPos, EffectLists.EffectType type)
    {
        if (_leftTaskComponent == null || _centerTaskComponent == null || _rightTaskComponent == null)
        {
            GetTaskCubeComponents();
        }
        switch (type)
        {
            case EffectLists.EffectType.leftLaser:
                return _leftTaskComponent.CheckCollision(chaserPos);
            case EffectLists.EffectType.centerLaser:
                return _centerTaskComponent.CheckCollision(chaserPos);
            case EffectLists.EffectType.rightLaser:
                return _rightTaskComponent.CheckCollision(chaserPos);
            default:
                Debug.LogError("TaskCubeHitChecker: Invalid laser type.");
                return false;
        }
    }
}
