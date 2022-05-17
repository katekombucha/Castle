using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class Pointer : MonoBehaviour
    {
        public Image ArrowImage;
        private Vector3 _targetPosition;
        private RectTransform _pointerRectTransform;
        private float borderSize = 100f;
        private Camera _mainCamera;
        private Vector3 _screenCenter;
        private List<Vector3> _targets = new List<Vector3>();
        private Transform _playerTransform;
        
        public void AddTarget(Vector3 targetWorldPos) => 
            _targets.Add(targetWorldPos);

        public void RemoveTarget(Vector3 targetToRemove)
        {
            _targets.Remove(targetToRemove);

            if (_targets.Count == 0)
                Destroy(gameObject);
        }

        private void Awake()
        {
            _playerTransform = AllServices.Container.Single<IGameFactory>().HeroGameObject.transform;
            _pointerRectTransform = GetComponent<RectTransform>();
            _mainCamera = Camera.main;
            _screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        }

        private void Update()
        {
            DefineTarget();

            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(_targetPosition);

            LookOnTarget(screenPosition);
            
            bool isOffScreen = IsOffScreen(screenPosition);

            if (isOffScreen && !ArrowImage.enabled)
            {
                ArrowImage.enabled = true;
                _pointerRectTransform.DOPunchScale(Vector3.one * 1.01f, 0.3f, 1);
            }
            
            if (isOffScreen && ArrowImage.enabled)
            {
                Vector3 cappedTargetScreenPosition = screenPosition;
                if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
                if (cappedTargetScreenPosition.x >= Screen.width - borderSize)
                    cappedTargetScreenPosition.x = Screen.width - borderSize;
                if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
                if (cappedTargetScreenPosition.y >= Screen.height - borderSize)
                    cappedTargetScreenPosition.y = Screen.height - borderSize;
                
                _pointerRectTransform.position = cappedTargetScreenPosition;
                _pointerRectTransform.localPosition = new Vector3(_pointerRectTransform.localPosition.x,
                    _pointerRectTransform.localPosition.y, 0);
            }
            
            else
            {
                ArrowImage.enabled = false;
                _pointerRectTransform.position = screenPosition;
                _pointerRectTransform.localPosition = new Vector3(_pointerRectTransform.localPosition.x,
                    _pointerRectTransform.localPosition.y, 0);
            }
        }

        private bool IsOffScreen(Vector3 screenPosition)
        {
            return screenPosition.x <= borderSize
                   || screenPosition.x >= Screen.width - borderSize
                   || screenPosition.y <= borderSize
                   || screenPosition.y >= Screen.height - borderSize;
        }

        private void LookOnTarget(Vector3 targetPositionScreenPoint)
        {
            float angle = float.MinValue;
            GetArrowAngle(targetPositionScreenPoint, ref angle, _screenCenter);
            _pointerRectTransform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }

        private void GetArrowAngle(Vector3 screenPosition, ref float angle, Vector3 screenCentre)
        {
            screenPosition -= screenCentre;
            angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        }

        private void DefineTarget()
        {
            Vector3 closestTarget = _targets[0];

            foreach (var potentialTarget in _targets)
            {
                float newDistance = Vector3.Distance(_playerTransform.position, potentialTarget);
                float oldDistance = Vector3.Distance(_playerTransform.position, closestTarget);

                if (newDistance < oldDistance)
                    closestTarget = potentialTarget;
            }

            _targetPosition = closestTarget;
        }
    }
}