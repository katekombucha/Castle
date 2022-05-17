using TMPro;

namespace CodeBase.UI.Elements
{
    public class RespawnTimer : Timer
    {
        public TextMeshProUGUI TimerText;

        protected override float ReduceTime(float secondsToRespawn)
        {
            secondsToRespawn -= 1;
            TimerText.text = secondsToRespawn.ToString();
            return secondsToRespawn;
        }
    }
}