using UnityEngine;

public interface IMainPhaseAction
{
    public enum Type
    {
        PlayCard,
        AttackPermanent,
        ActivateCard,
        ActivatePermanent
    }

    public Type type { get; }
    void Execute(TurnStateMachine stateMachine);
    byte[] Serialize();
    void Deserialize(byte[] bytes);
}

public static class MainPhaseActionUtils
{
    public static IMainPhaseAction FromBytes(IMainPhaseAction.Type type, byte[] bytes)
    {
        switch (type)
        {
            case IMainPhaseAction.Type.PlayCard:
                return new PlayCardAction(bytes);
            case IMainPhaseAction.Type.AttackPermanent:
                return new AttackPermanentAction(bytes);
            case IMainPhaseAction.Type.ActivateCard:
                return new ActivateCardAction(bytes);
            case IMainPhaseAction.Type.ActivatePermanent:
                return new ActivatePermanentAction(bytes);
        }

        return null;
    }
}