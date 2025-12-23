using ExitGames.Client.Photon;
using UnityEngine;

public struct ActivateCardAction : IMainPhaseAction
{
    int CardIndex;
    int SkillIndex;

    public IMainPhaseAction.Type type => IMainPhaseAction.Type.ActivateCard;

    public ActivateCardAction(int cardIndex, int skillIndex)
    {
        CardIndex = cardIndex;
        SkillIndex = skillIndex;
    }

    public ActivateCardAction(byte[] bytes)
    {
        CardIndex = -1;
        SkillIndex = -1;
        Deserialize(bytes);
    }

    public void Execute(TurnStateMachine stateMachine)
    {
        stateMachine.SetActCardSkill(CardIndex, SkillIndex);
    }

    public void Deserialize(byte[] bytes)
    {
        int index = 0;

        Protocol.Deserialize(out CardIndex, bytes, ref index);
        Protocol.Deserialize(out SkillIndex, bytes, ref index);
    }

    public byte[] Serialize()
    {
        byte[] bytes = new byte[sizeof(int) * 2];
        int index = 0;

        Protocol.Serialize(CardIndex, bytes, ref index);
        Protocol.Serialize(SkillIndex, bytes, ref index);

        return bytes;
    }
}
