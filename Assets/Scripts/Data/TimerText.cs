using UnityEngine;
using TMPro;

namespace Data
{
    public class TimerText : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TimerManager timerManager;

        private int _roundedTime;
        private void Update()
        {
            _roundedTime = Mathf.CeilToInt(timerManager.Timer);
            timerText.text = _roundedTime.ToString();
        }
    }
}