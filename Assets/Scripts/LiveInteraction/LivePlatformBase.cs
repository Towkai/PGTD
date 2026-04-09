using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LiveInteraction
{
    public abstract class LivePlatformBase : MonoBehaviour
    {
        [Header("Live Events")]
        protected UnityEvent<string> onChat;
        protected UnityEvent<string> onLike;
        protected UnityEvent<string> onGift;
        protected UnityEvent<string> onJoin;

        protected bool isConnected = false;
        public bool IsConnected => isConnected;

        /// <summary>
        /// 主執行緒事件佇列（重點🔥）
        /// </summary>
        private readonly Queue<Action> eventQueue = new Queue<Action>();

        /// <summary>
        /// 子類實作連線
        /// </summary>
        public virtual void Connect() {
            isConnected = false;
        }

        public virtual void Disconnect() { }

        /// <summary>
        /// 子類可覆寫（例如 Twitch 輪詢）
        /// </summary>
        protected virtual void Tick() { }

        protected virtual void Update()
        {
            // ✅ 統一在主執行緒處理所有事件
            while (eventQueue.Count > 0)
            {
                eventQueue.Dequeue()?.Invoke();
            }

            // 給輪詢型平台用
            if (isConnected)
                Tick();
        }
        public void SetEventListener(UnityEvent<string> onChat, UnityEvent<string> onLike, UnityEvent<string> onGift, UnityEvent<string> onJoin)
        {
            this.onChat = onChat;
            this.onLike = onLike;
            this.onGift = onGift;
            this.onJoin = onJoin;
        }

        #region Queue API（給子類用）
        protected void Enqueue(Action action)
        {
            eventQueue.Enqueue(action);
        }
        #endregion

        #region Helper（統一輸出）
        protected void EmitChat(string msg)
        {
            Enqueue(() => onChat?.Invoke(msg));
        }

        protected void EmitLike(string msg)
        {
            Enqueue(() => onLike?.Invoke(msg));
        }

        protected void EmitGift(string msg)
        {
            Enqueue(() => onGift?.Invoke(msg));
        }

        protected void EmitJoin(string msg)
        {
            Enqueue(() => onJoin?.Invoke(msg));
        }
        #endregion
    }
}