using TikTokLiveUnity;
using UnityEngine;

public class TikTok : MonoBehaviour
{
    public string userName;
    async void Start()
    {
        TikTokLiveManager.Instance.OnLike += (liveClien, liveEvent) =>
        {
            Debug.Log($"Thank you for likes! {liveEvent.Count} {liveEvent.Sender.NickName}");
        };

        await TikTokLiveManager.Instance.ConnectToStream(userName);
    }
}
