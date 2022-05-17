using Cinemachine;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CinemachineSwitcher : MonoBehaviour
    {
        public CinemachineVirtualCamera[] Cameras;
        private const string cameraName = "Follow";
        private Animator _animator;


        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Subscribe(BaseWindow tutorialWindow)
        {
            tutorialWindow.CloseButton.onClick.AddListener(() => ChangeCamera());
        }

        public void ChangeCamera()
        {
            _animator.Play(cameraName);
        }
    }
}