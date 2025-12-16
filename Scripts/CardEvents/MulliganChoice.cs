using System.Collections.Generic;

namespace DCGO.CardEvent
{
    public struct MulliganChoice : ICardEvent
    {
        public List<CardSource> Hand;

        public Player player;
        public ChoiceData Resolve(bool value)
        {
            return new ChoiceData() { value ? 1 : 0 };
        }
    }
}
