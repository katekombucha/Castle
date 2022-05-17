using CodeBase.Infrastructure.Services;
using CodeBase.UI.Elements;
using UnityEngine;

namespace CodeBase.UI.Services.Pointers
{
    public interface IPointersService : IService
    {
        void AddVillagePointer(Vector3 targetPosition);
        void DeleteVillagePointer(Vector3 targetToDelete);
        Pointer CreatePointer(Vector3 target);
    }
}