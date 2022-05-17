using System;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        public string NextLevelKey;
        
        [Header("Hero Settings")]
        public Vector3 InitialHeroPosition;
        
        [Header("Gates Settings")]
        public Vector3 GatesPosition;
        
        [Header("Villages Settings")]
        public VillageData[] Villages;

        [Header("Castle Settings")] 
        public Vector3 CastlePosition;
        public int CastleNumber;
        public int StartingNumberOfCitizens;
        public float CoinsOffset = 7.5f;

        [Header("Dragon Settings")] 
        public DragonSpawnInfo DragonInfo;
        public Vector3 DragonBlockPosition;
        
        [Header("Tutorial")] 
        public string[] TutorialMessages;
    }
    
    [Serializable]
    public class DragonSpawnInfo
    {
        public Vector3 DragonPosition;
        public string DragonName = "Red";
        public int DragonRotation = 90;
    }
}