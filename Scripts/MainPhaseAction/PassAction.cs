using ExitGames.Client.Photon;
using Unity.VisualScripting;
using UnityEngine;

public struct PassAction : IMainPhaseAction
{
    public IMainPhaseAction.Type type => IMainPhaseAction.Type.Pass;

    public PassAction(byte[] bytes)
    {
        Deserialize(bytes);
    }

    public void Execute(TurnStateMachine stateMachine)
    {
        stateMachine.PassTurn();
    }

    public void Deserialize(byte[] bytes)
    {
    }


    public byte[] Serialize()
    {
        return null;
    }
}
