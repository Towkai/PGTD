using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
