using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnsMenu : MonoBehaviour {

    public string escenaSimulacion;
    public float velocidadFade1 = 0.5f;
    [Space]
    public string escenaCalibracion;
    public float velocidadFade2 = 2;
    [Space]
    public string escenaMenu;
    public float velocidadFade3 = 2;

    public void scn_Iniciar()
    {
        Initiate.Fade(escenaSimulacion, Color.black, velocidadFade1);
    }

    public void scn_Calibracion()
    {
        Initiate.Fade(escenaCalibracion, Color.black, velocidadFade2);
    }

    public void scn_VolverMenu()
    {
        Initiate.Fade(escenaMenu, Color.white, velocidadFade3);
    }

    public void scn_Salir()
    {
        Debug.Log("Salir aplicacion");
        Application.Quit();
    }
}
