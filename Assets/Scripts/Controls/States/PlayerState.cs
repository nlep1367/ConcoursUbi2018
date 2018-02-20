using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : IState {
    protected Player _player;

    public PlayerState(Player player)
    {
        this._player = player;
    }
    public virtual void ChangeState(PlayerState newState)
    {
        this.OnExit();
        newState.OnEnter();
        _player.State = newState;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }

    public abstract void InterpretInput();
}
