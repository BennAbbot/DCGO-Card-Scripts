
namespace DCGO.CardEvent
{
    public struct BreedingChoice : ICardEvent
    {
        public enum BreedingType
        {
            Hatch,
            Move
        }

        public BreedingType type;

        public Player player;

        public ChoiceData Resolve(bool value)
        {
            return new ChoiceData() { value ? 1 : 0 };
        }
    }
}
