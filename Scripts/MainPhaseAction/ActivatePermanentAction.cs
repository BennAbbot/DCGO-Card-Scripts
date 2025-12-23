using ExitGames.Client.Photon;
using UnityEngine;

public struct ActivatePermanentAction : IMainPhaseAction
{
    int PermanentIndex;
    int SkillIndex;

    public IMainPhaseAction.Type type => IMainPhaseAction.Type.ActivatePermanent;

    public ActivatePermanentAction(int permanentIndex, int skillIndex)
    {
        PermanentIndex = permanentIndex;
        SkillIndex = skillIndex;
    }

    public ActivatePermanentAction(byte[] bytes)
    {
        PermanentIndex = -1;
        SkillIndex = -1;
        Deserialize(bytes);
    }

    public void Execute(TurnStateMachine stateMachine)
    {
        stateMachine.SetActSkill(PermanentIndex, SkillIndex);
    }

    public void Deserialize(byte[] bytes)
    {
        int index = 0;

        Protocol.Deserialize(out PermanentIndex, bytes, ref index);
        Protocol.Deserialize(out SkillIndex, bytes, ref index);
    }

    public byte[] Serialize()
    {
        byte[] bytes = new byte[sizeof(int) * 2];
        int index = 0;

        Protocol.Serialize(PermanentIndex, bytes, ref index);
        Protocol.Serialize(SkillIndex, bytes, ref index);

        return bytes;
    }
}
