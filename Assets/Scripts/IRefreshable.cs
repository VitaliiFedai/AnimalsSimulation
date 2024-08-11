namespace AnimalSimulation
{
    public interface IRefreshable
    { 
        public float Speed { get; }
        public void Refresh(float timeScale);
    }
}