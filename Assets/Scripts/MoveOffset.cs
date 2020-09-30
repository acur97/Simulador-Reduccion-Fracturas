using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOffset : MonoBehaviour {

	public enum tipoCambio {Awake, Start, Update};
    public tipoCambio Metodo;
    public Transform referencia;
    public Transform aMover;
    public Vector3 offset;

    private void Awake()
    {
        if (Metodo == tipoCambio.Awake)
        {
            aMover.position = new Vector3(referencia.position.x + offset.x, referencia.position.y + offset.y, referencia.position.z + offset.z);
        }
    }

    private void Start()
    {
        if (Metodo == tipoCambio.Start)
        {
            aMover.position = new Vector3(referencia.position.x + offset.x, referencia.position.y + offset.y, referencia.position.z + offset.z);
        }
    }

    private void Update()
    {
        if (Metodo == tipoCambio.Update)
        {
            aMover.position = new Vector3(referencia.position.x + offset.x, referencia.position.y + offset.y, referencia.position.z + offset.z);
        }
    }
}
