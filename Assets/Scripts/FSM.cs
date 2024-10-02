using System;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private readonly Dictionary<Type, IState> _typeStateMap = new();

    private IState _currentState;

    public void AddState(IState state)
    {
        Type typeOfState = state.GetType();

        if (_typeStateMap.ContainsKey(typeOfState))
        {
            return;
        }

        state.SetFSM(this);
        _typeStateMap.Add(typeOfState, state);
    }

    public IState AddNewState<T>() where T : IState, new()
    {
        Type typeOfState = typeof(T);
        
        if (_typeStateMap.ContainsKey(typeOfState))
        {
            Debug.LogError("Trying to add an already existing state.");
            return null;
        }

        T newState = new();
        newState.SetFSM(this);
        _typeStateMap.Add(typeOfState, newState);

        return newState;
    }

    public void OnUpdate()
    {
        _currentState?.OnExecute();
    }

    public void SetInitialState<T>() where T : IState
    {
        Type typeOfState = typeof(T);

        if (!_typeStateMap.ContainsKey(typeOfState)) return;

        _currentState = _typeStateMap[typeOfState];
        _currentState.OnAwake();
    }

    public void SetState<T>() where T : IState
    {
        Type typeOfState = typeof(T);

        if (!_typeStateMap.ContainsKey(typeOfState))
        {
            Debug.LogError("The state " + typeOfState + " is not in the dictionary");
            return;
        }

        _currentState.OnSleep();
        _currentState = _typeStateMap[typeOfState];
        _currentState.OnAwake();
    }



    #region Funciones Helpers
    // Helpers

    public bool IsCurrentState<T>()
    {
        return _currentState.GetType() == typeof(T);
    }

    public void ResetCurrentState()
    {
        _currentState.OnSleep();
        _currentState.OnAwake();
    }
    public void ClearStates()
    {
        _typeStateMap.Clear();
    }
    #endregion


}
