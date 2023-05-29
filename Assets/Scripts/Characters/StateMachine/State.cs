using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Player player;
    protected Vector3 playerPosition => player.transform.position;
    protected StateMachine stateMachine;
    protected Animator animator;

    public State(StateMachine stateMachine, Animator animator, Player player)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.player = player;
    }

    public virtual void OnStateUpdate()
    {

    }
    public virtual void OnStateEnter()
    {

    }
    public virtual void OnStateExit()
    {

    }
}
