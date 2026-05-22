using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DanceData", menuName = "ScriptableObject/DanceData", order = 0)]
public class DanceData : ScriptableObject
{
    public float frameInterval;
    public List<Vector3> dancePositions;

}

