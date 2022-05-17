using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.States
{
    public class GameLoopState : IPayloadedState<string>
    {
        private string _sceneName;
        private string _levelResult;
        public GameLoopState(GameStateMachine gameStateMachine) { }
        
        public void Enter(string sceneName)
        {
            _levelResult = "win";
            _sceneName = sceneName;
        }

        public void Exit()
        {
            SendEvent();
        }

        private  void SendEvent()
        {
            var data = new Dictionary<string, object>()
            {
                {"level_name",  _sceneName}
                ,{"result", _levelResult}
            };
      
            AppMetrica.Instance.ReportEvent("level_finish", data);
            AppMetrica.Instance.SendEventsBuffer();
        }
    }
}