using UnityEngine;

[CreateAssetMenu(fileName = "TaskUIPos", menuName = "ScriptableObject/TaskUIPos", order = 1)]
public class TaskUIPos : ScriptableObject
{
    public Vector3 pos = new Vector3(0.3f, -0.3f, 0.5f);
    public float fontSize = 36f;
}
