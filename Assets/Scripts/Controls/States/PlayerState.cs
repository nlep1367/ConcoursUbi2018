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
        ChangeState(newState, null);
    }

    public virtual void ChangeState(PlayerState newState, object o)
    {
        this.OnExit();
        newState.OnEnter(o);
        _player.State = newState;
    }

    public virtual void OnEnter() { OnEnter(null); }
    public virtual void OnExit() { OnEnter(null); }

    public virtual void OnEnter(object o) { }
    public virtual void OnExit(object o) { }

    public abstract void InterpretInput();
}
