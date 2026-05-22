using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class EffectMeshManager : MonoBehaviour
{
    public EffectMesh effectMesh;
    public StartGameInfo startGameInfo;
    void Awake()
    {
        if (effectMesh != null)
        {
            effectMesh.HideMesh = startGameInfo.hideEffectMesh;
        }
    }
    public struct RoomSurface
    {
        public MeshRenderer meshRenderer; //描画に関わるmeshRenderer
        public Vector3 centerPos; //中心位置
        // public Vector3 rotation; //ワールド座標に対する回転
        public Vector3[] vert;//頂点
        public List<(Vector3, Vector3)> edgeList;//辺
    }

    /// <summary>
    /// 指定したラベルの面の情報を取得
    /// </summary>
    /// <param name="roomLabel"></param>
    /// <returns></returns>
    public RoomSurface FindRoomSurface(MRUKAnchor.SceneLabels roomLabel)
    {
        Debug.Log("effectMeshAnchors" + effectMesh.EffectMeshObjects.Count);
        if (effectMesh.EffectMeshObjects.Count <= 0)
        {
            Debug.Log("effectMesh.EffectMeshObjects.Count <= 0");
            return new RoomSurface();
        }

        RoomSurface roomSurface = new RoomSurface();

        // effectMeshが管理している全アンカーを走査
        foreach (var kv in effectMesh.EffectMeshObjects)
        {
            if (kv.Key == null) continue;
            var anchor = kv.Key;
            //アンカーを探す
            if (anchor.HasAnyLabel(roomLabel))
            {
                roomSurface.meshRenderer = anchor.GetComponentInChildren<MeshRenderer>();
            }
        }

        Vector3 center = roomSurface.meshRenderer.bounds.center;//中心
        roomSurface.centerPos = center;

        //バウンディングボックスの回転を求める
        // Vector3 ext = roomSurface.meshRenderer.bounds.extents;

        // Vector2 xMin = new Vector2(center.x - ext.x, center.z - ext.z);
        // Vector2 xMax = new Vector2(center.x + ext.x, center.z - ext.z);

        // Vector2 edgeX = xMax - xMin;
        // roomSurface.rotation.y = Vector2.SignedAngle(Vector2.right, edgeX);

        //頂点
        MeshFilter mf = roomSurface.meshRenderer.GetComponent<MeshFilter>();
        var vertices = GetMeshFilterVert(mf);
        roomSurface.vert = vertices;

        //辺
        var outlineEdges = GetMeshOutlineEdges(mf);
        roomSurface.edgeList = outlineEdges;

        return roomSurface;
    }


    /// <summary>
    /// 壁や床のメッシュの頂点を取得
    /// </summary>
    public Vector3[] GetMeshFilterVert(MeshFilter mf)
    {
        if (mf != null && mf.sharedMesh != null)
        {
            var mesh = mf.sharedMesh;
            Vector3[] vertices = mesh.vertices; // ローカル座標
                                                // ワールド座標に変換
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = mf.transform.TransformPoint(vertices[i]);
            }
            return vertices;
        }
        return null;
    }

    /// <summary>
    /// 壁や床の外周の辺を取得
    /// </summary>
    public List<(Vector3, Vector3)> GetMeshOutlineEdges(MeshFilter meshFilter)
    {
        var mesh = meshFilter.sharedMesh;
        var vertices = mesh.vertices;
        var triangles = mesh.triangles;

        // ワールド座標に変換
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = meshFilter.transform.TransformPoint(vertices[i]);

        // 辺ごとに出現回数をカウント
        var edgeCount = new Dictionary<(int, int), int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int[] idx = { triangles[i], triangles[i + 1], triangles[i + 2] };
            for (int e = 0; e < 3; e++)
            {
                int a = idx[e];
                int b = idx[(e + 1) % 3];
                var edge = (Mathf.Min(a, b), Mathf.Max(a, b));
                if (edgeCount.ContainsKey(edge))//既に辺が登録されていればカウントアップ
                    edgeCount[edge]++;
                else//未登録なら1
                    edgeCount[edge] = 1;
            }
        }

        // 出現回数が1の辺だけが「外周エッジ」
        var outlineEdges = new List<(Vector3, Vector3)>();
        foreach (var kv in edgeCount)
        {
            if (kv.Value == 1)
            {
                var v0 = vertices[kv.Key.Item1];
                var v1 = vertices[kv.Key.Item2];
                outlineEdges.Add((v0, v1));
            }
        }
        return outlineEdges;
    }

    public struct OutsideMeshes
    {
        public MeshRenderer x_Max;
        public MeshRenderer x_min;
        public MeshRenderer y_Max; //天井
        public MeshRenderer y_min; //床
        public MeshRenderer z_Max;
        public MeshRenderer z_min;
    }
    /// <summary>
    /// 壁の最も外側の4面を取得
    /// </summary>
    /// <returns></returns>
    public OutsideMeshes FindOutsideSurface()
    {
        Debug.Log("effectMeshAnchors" + effectMesh.EffectMeshObjects.Count);

        OutsideMeshes meshes = new OutsideMeshes();

        // effectMeshが管理している全アンカーを走査
        foreach (var kv in effectMesh.EffectMeshObjects)
        {
            var anchor = kv.Key;

            if (anchor.HasAnyLabel(MRUKAnchor.SceneLabels.WALL_FACE))
            {
                Vector3 newWallPos = anchor.transform.position;
                Debug.Log($"壁の位置（ワールド座標）: {newWallPos}");

                if (meshes.x_Max == null)
                {
                    meshes.x_Max = anchor.GetComponentInChildren<MeshRenderer>();
                    meshes.x_min = anchor.GetComponentInChildren<MeshRenderer>();
                    meshes.z_Max = anchor.GetComponentInChildren<MeshRenderer>();
                    meshes.z_min = anchor.GetComponentInChildren<MeshRenderer>();
                }

                if (meshes.x_Max.bounds.center.x < newWallPos.x) meshes.x_Max = anchor.GetComponentInChildren<MeshRenderer>();
                if (meshes.x_min.bounds.center.x > newWallPos.x) meshes.x_min = anchor.GetComponentInChildren<MeshRenderer>();
                if (meshes.z_Max.bounds.center.z < newWallPos.z) meshes.z_Max = anchor.GetComponentInChildren<MeshRenderer>();
                if (meshes.z_min.bounds.center.z > newWallPos.z) meshes.z_min = anchor.GetComponentInChildren<MeshRenderer>();

                Debug.Log($"壁の範囲 X: {meshes.x_Max.bounds.center} 〜 {meshes.x_min.bounds.center}, Z: {meshes.z_Max.bounds.center} 〜 {meshes.z_min.bounds.center}");
            }

            //床と天井のメッシュも取得
            if (anchor.HasAnyLabel(MRUKAnchor.SceneLabels.CEILING))
            {
                meshes.y_Max = anchor.GetComponentInChildren<MeshRenderer>();
            }
            if (anchor.HasAnyLabel(MRUKAnchor.SceneLabels.FLOOR))
            {
                meshes.y_min = anchor.GetComponentInChildren<MeshRenderer>();
            }
        }
        return meshes;
    }

    /// <summary>
    /// 部屋を直方体とする
    /// </summary>
    public struct RoomSize
    {
        public Vector3 x_MaxPos;
        public Vector3 x_minPos;
        public float xLength => x_MaxPos.x - x_minPos.x;//横の長さ
        public float xCenter => (x_minPos.x + x_MaxPos.x) / 2;//横の中心位置
        // public float inclination;//ワールド座標と部屋の傾き

        public Vector3 y_MaxPos;
        public Vector3 y_minPos;
        public float yLength => y_MaxPos.y - y_minPos.y;//高さ
        public float yCenter => (y_minPos.y + y_MaxPos.y) / 2;//高さの中心位置

        public Vector3 z_MaxPos;
        public Vector3 z_minPos;
        public float zLength => z_MaxPos.z - z_minPos.z;//奥行きの長さ
        public float zCenter => (z_minPos.z + z_MaxPos.z) / 2;//奥行きの中心位置
    }
    public RoomSize SearchRoomSize()
    {
        RoomSize roomSize = new RoomSize();

        OutsideMeshes outside = FindOutsideSurface();
        roomSize.x_MaxPos = outside.x_Max.bounds.max;
        roomSize.x_minPos = outside.x_min.bounds.min;
        roomSize.y_MaxPos = outside.y_Max.bounds.max;
        roomSize.y_minPos = outside.y_min.bounds.min;
        roomSize.z_MaxPos = outside.z_Max.bounds.max;
        roomSize.z_minPos = outside.z_min.bounds.min;

        //傾き
        Vector3 VecX3D = roomSize.x_MaxPos - roomSize.x_minPos;//xの軸
        // Vector2 VecX2D = new Vector2(VecX3D.x, VecX3D.z);

        return roomSize;
    }
}