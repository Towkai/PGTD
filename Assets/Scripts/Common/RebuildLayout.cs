using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class RebuildLayout : MonoBehaviour
    {
        // IEnumerator Start()
        void Start()
        {
            LayoutGroup[] layouts = this.GetComponentsInChildren<LayoutGroup>();
            // yield return null;
            for (int i = layouts.Length - 1; i >= 0; i--)
                LayoutRebuilder.ForceRebuildLayoutImmediate(layouts[i].transform as RectTransform);
        }
    }
}