using CodeBase.Infrastructure.Factory;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.UI.Services.Pointers
{
    public class PointerService : IPointersService
    {
        private Pointer _villagePointer;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;

        public PointerService(IUIFactory uiFactory, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
        }

        public void AddVillagePointer(Vector3 targetPosition)
        {
            if (!_villagePointer)
                _villagePointer = _uiFactory.CreatePointer();

            _villagePointer.AddTarget(targetPosition);
        }

        public void DeleteVillagePointer(Vector3 targetToDelete)
        {
            _villagePointer.RemoveTarget(targetToDelete);
        }

        public Pointer CreatePointer(Vector3 target)
        {
            Pointer pointer = _uiFactory.CreatePointer(); 
            pointer.AddTarget(target);
            return pointer;
        }
    }
}