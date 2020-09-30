using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlRadiografias : MonoBehaviour {

    public Text horaInicial;
    public Text horaActual;
    public Text tiempoActual;
    public Text tiempoPic;

    private int Hora = 0;
    private int Minuto = 0;
    private int Segundo = 0;
    private int tiemp;

    private void Awake()
    {
        var tiempo = System.DateTime.Now;
        horaInicial.text = tiempo.Hour.ToString("00") + ":" + tiempo.Minute.ToString("00") + "." + tiempo.Second.ToString("00");
        InvokeRepeating("AumentarTiempo", 0, 1);
    }

    private void AumentarTiempo()
    {
        Segundo += 1;
        if (Segundo == 60)
        {
            Segundo = 0;
            Minuto += 1;
        }
        if (Minuto == 60)
        {
            Minuto = 0;
            Hora += 1;
        }
    }

    private void Update()
    {
        var tiempo = System.DateTime.Now;
        horaActual.text = tiempo.Hour.ToString("00") + ":" + tiempo.Minute.ToString("00") + "." + tiempo.Second.ToString("00");

        tiempoActual.text = Hora.ToString("00") + ":" + Minuto.ToString("00") + "." + Segundo.ToString("00");
        tiempoPic.text = Hora.ToString("00") + "h " + Minuto.ToString("00") + "m " + Segundo.ToString("00") + "s";
    }
}