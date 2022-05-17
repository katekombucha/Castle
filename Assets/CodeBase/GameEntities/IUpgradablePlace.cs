namespace CodeBase.GameEntities
{
    public interface IUpgradablePlace
    {
        bool CheckIfMaxLevel();
        int GetUpgradeCost();
        void Upgrade(int level);
        int ResourceShortage { get; set; }
    }
}