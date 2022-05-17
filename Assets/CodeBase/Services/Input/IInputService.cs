using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }
        float AxisY { get; }

        bool IsAttackButtonUp();
    }
}