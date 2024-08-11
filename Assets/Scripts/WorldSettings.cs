namespace AnimalSimulation
{
   public class WorldSettings : IWorldSettings
    { 
        public int FieldSize { get; set; }
        public int AnimalsCount { get; set; }
        public float AnimalSpeed { get; set; }
    }
}