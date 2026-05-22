using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class LaserColorResetter : NetworkBehaviour
{
    [SerializeField] LaserColorManager laserColorManager;
    [SerializeField] LaserMatUIManager laserMatUIManager;
    // [SerializeField] ColorButtonOnPointerDown whiteButton;
    [SerializeField] StartGameInfo startGameInfo;
    [SerializeField] Button[] buttons = new Button[3];

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_ResetLasersColor(Color col)
    {
        if (startGameInfo.isAuthority)
        {
            //全てのレーザーの色を白にする
            var currentLaserList = laserColorManager.LaserList;//現在のレーザーリストを保存
                                                               //全レーザーを色変更の対象にする
            laserColorManager.LaserList.Clear();
            laserColorManager.LaserList.AddRange(EffectLists.Instance.LeftLaserObjectList);
            laserColorManager.LaserList.AddRange(EffectLists.Instance.CenterLaserObjectList);
            laserColorManager.LaserList.AddRange(EffectLists.Instance.RightLaserObjectList);

            laserMatUIManager.On_ChangeLaserColor(Color.white);
            // laserColorManager.Rpc_ChangeLaserColor(Color.white);
            laserColorManager.LaserList = currentLaserList;//リストを元に戻す
        }
        else if (startGameInfo.isPc)
        {
            foreach (var btn in buttons)
            {
                btn.image.color = Color.white;
            }
        }
    }
}
