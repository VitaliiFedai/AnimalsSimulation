namespace AnimalSimulation
{
    public class EntityPrefabs
    {
        public Animal Animal { get; private set; }
        public Food Food { get; private set; }
        public Effect GatherEffect { get; private set; }

        public EntityPrefabs(Animal animalPrefab, Food foodPrefab, Effect gatherEffectPrefab)
        {
            Animal = animalPrefab;
            Food = foodPrefab;
            GatherEffect = gatherEffectPrefab;
        }
    }
}