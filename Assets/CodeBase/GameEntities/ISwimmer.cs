namespace CodeBase.GameEntities
{
    public interface ISwimmer
    {
        public bool SwimCondition { get; }
        public void StopSwim();
    }
}