using UnityEngine;
using UnityEngine.Playables;
public class TimelinePlayer : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public void PlayTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
        }
        else
        {
            Debug.LogError("PlayableDirector is not assigned.");
        }
    }
    public void StopTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Stop();
        }
    }
    public bool IsTimelineFinished()
    {
        if (playableDirector == null)
            return true;

        // 停止状態 = アニメーション終了または手動停止
        return playableDirector.state == PlayState.Paused;
    }
}