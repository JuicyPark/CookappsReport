using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Manager
{
    public enum State { Generate, Ready}
    public class StateManager : Singleton<StateManager>
    {
        public State state = State.Generate;
        void Start()
        {
            Screen.SetResolution(720, 1280, true);
        }
    }
}