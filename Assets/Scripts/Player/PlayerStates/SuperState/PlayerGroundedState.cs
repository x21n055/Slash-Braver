using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animeBoolName) : base(player, stateMachine, playerData, animeBoolName)
    {
    }
}
