using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.GameEntities.Villager;
using CodeBase.GameResources;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }  
        List<ISavedProgress> ProgressWriters { get; }
        GameObject HeroGameObject { get; }
        GameObject CreatePlayerObject(Vector3 at);
        GameObject CreateHud();
        GameObject CreateVillage(Vector3 at);
        GameObject CreateGates(Vector3 at);
        Unit CreateVillager(Vector3 at, GameEntities.Village village);
        GameObject CreateCastle(Vector3 at, int levelDataCastleNumber);
        Collectible CreateCoin(Vector3 at);
        void Cleanup();
        GameObject CreateGround();
        GameObject CreateDragon(DragonSpawnInfo info);
        GameObject CreateBlock(Vector3 at);
        void CreateSavedArmy(PlayerProgress progressServiceProgress, string sceneName);
        GameObject CreateGameManager();
    }
}