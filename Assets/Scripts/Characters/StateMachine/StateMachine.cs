using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine
{
    public State currentState;
    private Dictionary<State, List<Transition>> transitions = new Dictionary<State, List<Transition>>();
    private List<Transition> anyTransition = new List<Transition>();

    public void Initialize(State initialState)
    {
        currentState = initialState;
    }

    public void AddTransition(State fromState, State toState, Func<bool> condition)
    {
        if (!transitions.ContainsKey(fromState))
        {
            transitions.Add(fromState, new List<Transition>());
        }
        transitions[fromState].Add(new Transition(toState, condition));       
    }

    public void AddAnyTransition(State toState, Func<bool> condition)
    {
        anyTransition.Add(new Transition(toState, condition));
    }

    public void OnFSMUpdate()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.ToState);
        currentState.OnStateUpdate();
    }
    public void SetState(State nextState)
    {
        if (nextState == currentState)
        {
            return;
        }

        currentState?.OnStateExit();
        currentState = nextState;
        currentState.OnStateEnter();
    }

    private Transition GetTransition()
    {
        foreach (var transition in anyTransition)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        foreach (var transition in transitions.GetValueOrDefault(currentState))
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        return null;
    }

    private class Transition
    {
        public State ToState { get; }
        public Func<bool> Condition { get; }

        public Transition(State toState, Func<bool> condition)
        {
            ToState = toState;
            Condition = condition;
        }
    }
}
