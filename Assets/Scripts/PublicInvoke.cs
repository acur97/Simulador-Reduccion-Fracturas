using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PublicInvoke : MonoBehaviour {

    public UnityEvent[] publicInvoke;

    public void Invocar(int index)
    {
        publicInvoke[index].Invoke();
    }
}
