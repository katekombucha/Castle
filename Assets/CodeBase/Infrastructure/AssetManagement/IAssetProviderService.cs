using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProviderService : IService
    {
        GameObject Instantiate(string path, Vector3 at);
        GameObject Instantiate(string path, Vector3 at, Quaternion rotation);
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Transform parent);
    }
}