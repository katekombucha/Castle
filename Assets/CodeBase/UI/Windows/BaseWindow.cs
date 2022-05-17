using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        public Button CloseButton;
        protected LevelStaticData _data;

        public void Construct(LevelStaticData data)
        {
            _data = data;
        }

        private void Awake() =>
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdate();
        }

        private void OnDestroy() =>
            Cleanup();

        protected virtual void OnAwake() =>
            CloseButton.onClick.AddListener(() => Destroy(gameObject));

        protected virtual void Initialize()
        {
        }

        protected virtual void SubscribeUpdate()
        {
        }

        protected virtual void Cleanup()
        {
        }
    }
}