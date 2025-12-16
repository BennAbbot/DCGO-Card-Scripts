using System.Collections.Generic;

namespace DCGO.CardEvent
{
    public struct MainPhaseChoice : ICardEvent
    {
        public List<CardSource> Hand;

        public Player player;
        public ChoiceData ResolveAttack()
        {
            return null;
        }
    }
}
