using EventDispatcher;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Setting
{
    public class OpenSettingMinionPanel : MonoBehaviour
    {
        public SelectMinion selectMinion = null;
        public void OnButtonClick(Button button)
        {
            selectMinion.transform.parent.gameObject.SetActive(true);
            selectMinion.transform.position = button.transform.position;
            selectMinion.SetNowSelect(button);
        }
    }
}