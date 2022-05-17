using CodeBase.Infrastructure.States;
using Facebook.Unity;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        private void Awake()
        {
            if (FB.IsInitialized) 
            {
                FB.ActivateApp();
            } 
            else 
            {
                FB.Init(() => { FB.ActivateApp();});
            }
            
            _game = new Game(this);
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}