// https://www.youtube.com/watch?v=nxJFGmFA9wM&t=338s
// #if FUSION2
using UnityEngine;
using Fusion;
using System;
using Meta.XR.MRUtilityKit;
using TMPro;

public class SpaceSharingManager : NetworkBehaviour
{
    [Networked] private NetworkString<_512> NetworkedRoomUuid { get; set; }
    [Networked] private NetworkString<_256> NetworkedRemoteFloorPose { get; set; }

    private Guid _SharedAnchorGroupId;
    public TextMeshProUGUI debugText;
    public StartGameInfo startGameInfo;

    public override void Spawned()
    {
        base.Spawned();
        DebugTxtManager.Instance.DebugTxtPlusR("SpaceSharingManager: Spawned\n");
        if (!startGameInfo.isMetaQuest) return;
        // #if UNITY_EDITOR
        //         return;
        // #endif
        PrepareColocation();
        debugText.text += "SpaceSharingManager: Spawned\n";
        Debug.Log("SpaceSharingManager: Spawned and prepared for colocation.");
    }

    private void PrepareColocation()
    {
        if (HasStateAuthority)
        {
            AdvertiseColocationSession();
            debugText.text += " AdvertiseColocationSession();\n";
            Debug.Log(" AdvertiseColocationSession();");
        }
        else
        {
            DiscoverNearbySession();
            debugText.text += " DiscoverNearbySession();\n";
            Debug.Log(" DiscoverNearbySession();");
        }
    }

    private async void AdvertiseColocationSession()
    {
        if (!startGameInfo.isMetaQuest) return;
        try
        {
            var result = await OVRColocationSession.StartAdvertisementAsync(null);
            if (!result.Success)
            {
                Debug.LogError($"SpaceSharingManager: Failed to start advertisement. Error: {result.Status}");
                debugText.text += ($"SpaceSharingManager: Failed to start advertisement. Error: {result.Status}");
                return;
            }
            _SharedAnchorGroupId = result.Value;
            print($"SpaceSharingManager: Advertisement started successfully with Group ID: {_SharedAnchorGroupId}");
            debugText.text += ($"SpaceSharingManager: Advertisement started successfully with Group ID: {_SharedAnchorGroupId}\n");
            ShareMrukRooms();
        }
        catch (Exception e)
        {
            Debug.LogError($"ColocationManager: Exception during advertisement: {e.Message}");
        }
    }

    private async void ShareMrukRooms()
    {
        var room = MRUK.Instance.GetCurrentRoom();
        NetworkedRoomUuid = room.Anchor.Uuid.ToString();
        print($"SpaceSharingManager: Sharing MRUK room with UUID: {NetworkedRoomUuid}");
        debugText.text += ($"SpaceSharingManager: Sharing MRUK room with UUID: {NetworkedRoomUuid}\n");
        var result = await room.ShareRoomAsync(_SharedAnchorGroupId);

        if (!result.Success)
        {
            Debug.LogError($"SpaceSharingManager: Failed to share MRUK room. Error: {result.Status}");
            debugText.text += ($"SpaceSharingManager: Failed to share MRUK room. Error: {result.Status}\n");
            return;
        }

        print($"SpaceSharingManager: MRUK room shared successfully with Group ID: {_SharedAnchorGroupId}");

        var pose = room.FloorAnchor.transform;
        NetworkedRemoteFloorPose =
            $"{pose.position.x},{pose.position.y},{pose.position.z},{pose.rotation.x}," +
            $"{pose.rotation.y},{pose.rotation.z},{pose.rotation.w}";
        print($"SpaceSharingManager: Remote floor pose shared: {NetworkedRemoteFloorPose}");
        debugText.text += ($"SpaceSharingManager: Remote floor pose shared: {NetworkedRemoteFloorPose}\n");
    }


    private async void DiscoverNearbySession()
    {
        if (!startGameInfo.isMetaQuest) return;
        OVRColocationSession.ColocationSessionDiscovered += OnColocationSessionDiscovered;
        var result = await OVRColocationSession.StartDiscoveryAsync();
        if (!result.Success)
        {
            Debug.LogError($"SpaceSharingManager: Failed to start discovery. Error: {result.Status}");
            debugText.text += ($"SpaceSharingManager: Failed to start discovery. Error: {result.Status}");
            return;
        }
    }

    private void OnColocationSessionDiscovered(OVRColocationSession.Data session)
    {
        if (!startGameInfo.isMetaQuest) return;
        OVRColocationSession.ColocationSessionDiscovered -= OnColocationSessionDiscovered;
        _SharedAnchorGroupId = session.AdvertisementUuid;
        LoadShareRoom(_SharedAnchorGroupId);
    }

    private static Pose ParsePose(string poseString)
    {
        var parts = poseString.Split(',');
        if (parts.Length == 7)
        {
            return new Pose(
                new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2])),
                new Quaternion(float.Parse(parts[3]), float.Parse(parts[4]), float.Parse(parts[5]), float.Parse(parts[6])));
        }

        Debug.LogError($"SpaceSharingManager: Invalid pose string format: {poseString}");

        return default;
    }

    private async void LoadShareRoom(Guid groupUuid)
    {
        if (!startGameInfo.isMetaQuest) return;
        var roomUuid = Guid.Parse(NetworkedRoomUuid.ToString());
        var remotePoseStr = NetworkedRemoteFloorPose.ToString();
        var remoteFloorWorldPose = ParsePose(remotePoseStr);

        var result = await MRUK.Instance.LoadSceneFromSharedRooms(null, groupUuid, (roomUuid, remoteFloorWorldPose));
        if (result == MRUK.LoadDeviceResult.Success)
        {
            Debug.Log("SpaceSharingManager: Room loaded successfully.");
            debugText.text += "SpaceSharingManager: Room loaded successfully.";
        }
        else
        {
            Debug.LogError($"SpaceSharingManager: Failed to load room. Error: {result}");
            debugText.text += $"SpaceSharingManager: Failed to load room. Error: {result}";
        }
    }
}
// #endif