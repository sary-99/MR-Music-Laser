using UnityEngine;
using Fusion;
using TMPro;

public class AudioLinkSync : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI pcR;
    [SerializeField] TextMeshProUGUI mqR;
    [Networked] public Vector4 alpass_audioLink { get; set; }
    [Networked] public float value { get; set; }

    public override void Spawned()
    {
        base.Spawned();
        if (HasStateAuthority)
        {
            alpass_audioLink = Vector4.zero;
            value = 0f;
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_SetFloat(float val)
    {
        value = val;
    }

    /// <summary>
    /// Rpc_SetAlpass_audioLinkを外部から呼び出す用
    /// </summary>
    /// <param name="val"></param>
    public void SetAllpass_audioLink(Vector4 val)
    {
        if (Runner == null || Runner.IsRunning == false)
        {
            Debug.LogWarning("AudioLinkSync Runner is null in SetAllpass_audioLink.");
            return;
        }
        Rpc_SetAlpass_audioLink(val);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_SetAlpass_audioLink(Vector4 val)
    {
        alpass_audioLink = val;
        // DebugTxtManager.Instance.DebugTxtSetR($"Alpass {val}\n at AudioLinkSync");
    }
}
