// using UnityEngine;

// /// <summary>
// /// 時間記録支指示を行うためのクラス
// /// 
// /// </summary>
// public class TimerOperater : MonoBehaviour
// {
//     [SerializeField] StartGameInfo startGameInfo;
//     Timer timer => Timer.Instance;


//     public void StartTimer()
//     {
//         if (!startGameInfo.isObserver) return;
//         timer.StartTimer();
//     }

//     /// <summary>
//     /// Pause the timer
//     /// </summary>
//     public void Rpc_PauseTimer()
//     {
//         if (!startGameInfo.isObserver) return;
//         timer.PauseTimer();
//     }

//     /// <summary>
//     /// Reset the timer
//     /// </summary>
//     public void Rpc_ResetTimer()
//     {
//         if (!startGameInfo.isObserver) return;
//         timer.ResetTimer();
//     }

//     /// <summary>
//     ///  Stop the timer and reset time to 0
//     /// </summary>
//     public void Rpc_StopTimer()
//     {
//         if (!startGameInfo.isObserver) return;
//         timer.StopTimer();
//     }

//     /// <summary>
//     /// 実行者がタスク支持者かどうかをチェックする
//     /// </summary>
//     /// <returns></returns>
//     // bool CheckIsObserver()
//     // {
//     //     if (!startGameInfo.isObserver)
//     //     {
//     //         Debug.LogWarning("Timer: Only observer can pause the timer.");
//     //         return false;
//     //     }
//     //     return true;
//     // }
// }
