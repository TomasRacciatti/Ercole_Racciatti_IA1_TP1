using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateBoid
{

    void OnAwake();
    void OnExecute();
    void OnSleep();

    void SetFSM(FSM manager);
    void SetAgent(Boid agent);
}
