namespace AnimalSimulation
{
    public interface IWorldSettings
    {
        public int FieldSize { get; }
        public int AnimalsCount { get; }
        public float AnimalSpeed { get; }
    }
}