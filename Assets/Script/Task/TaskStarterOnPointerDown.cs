using UnityEngine;
using UnityEngine.EventSystems;

public class TaskStarterOnPointerDown : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] TaskRpcSender requestTaskStart;
    [SerializeField] int taskNum;
    public void OnPointerDown(PointerEventData eventData)
    {
        requestTaskStart.Rpc_StartTask(taskNum);
    }
}
