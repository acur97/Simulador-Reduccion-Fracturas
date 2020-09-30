using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Serial : MonoBehaviour {

    public static string ComSeleccionado;
    
    public static SerialPort _serialPort;

    public bool MenuInicio = false;
    public GameObject PrefabBotonCom;
    public Transform content;
    [Space]
    public string message="";
    public Text ComprobanteConfiguracion;
    public Text ComprobanteCom;

    private static bool seleccionado = false;
    private static string comS;
    private string some;
    private bool comErroneo = false;

    private object[] single;
    private Toggle[] toggles;

    private void Awake()
    {
        //Singlenton
        single = FindObjectsOfType(typeof(Serial));
        int len = single.Length;
        if (len == 2)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        //

        if (seleccionado && comErroneo == false)
        {
            ComprobanteConfiguracion.text = "Arco en C configurado";
            PlayerPrefs.SetInt("ArcoC", 1);
            ComprobanteConfiguracion.color = Color.white;

            _serialPort.PortName = comS;
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            Debug.Log("Abierto puerto " + comS);
        }
        else
        {
            ComprobanteConfiguracion.text = "Arco en C no configurado";
            PlayerPrefs.SetInt("ArcoC", 0);
            ComprobanteConfiguracion.color = Color.red;
        }

        //reemplazar para compropbar que la escena sea la build index 0
        if (MenuInicio)
        {
            ComprobanteCom.text = "Selecciona un puerto COM compatible";
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }

            string[] ports = SerialPort.GetPortNames();
            toggles = new Toggle[ports.Length];
            for (int i = 0; i < ports.Length; i++)
            {
                GameObject boton = Instantiate(PrefabBotonCom, content);
                Text texto = boton.GetComponentInChildren<Text>();
                texto.text = ports[i];
                //Button btn = boton.GetComponent<Button>();
                Toggle btn2 = boton.GetComponent<Toggle>();
                toggles[i] = btn2;
                btn2.group = content.GetComponent<ToggleGroup>();
                string selected = ports[i];
                /*btn.onClick.AddListener(delegate
                {
                    SeleccionarCom(selected);

                });*/
                btn2.onValueChanged.AddListener(delegate
                {
                    SeleccionarCom2(btn2.isOn, selected);
                });
            }
        }
    }

    public Thread readThread;

    void Start ()
    {
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        // Create a new SerialPort object with default settings.
        _serialPort = new SerialPort();
    }

    public string GetAngle() {
        try
        {
            message = _serialPort.ReadLine();
            //_serialPort.DiscardOutBuffer();
            //_serialPort.DiscardInBuffer();
            //Debug.Log(message);
            comErroneo = false;
        }
        catch (InvalidOperationException)
        {
            Debug.Log("Debe seleccionar un puerto COM compatible");
            message = "Selecciona un puerto COM compatible";
        }
        catch(TimeoutException e){
            Debug.Log("La operación no se pudo completar");
            message = "La operación no se pudo completar";
            comErroneo = true;
        }
        return message;
    }

    /*public void SeleccionarCom(string com)
    {
        _serialPort.PortName = com;
        _serialPort.BaudRate = 9600;
        _serialPort.Parity = Parity.None;
        _serialPort.DataBits = 8;
        _serialPort.StopBits = StopBits.One;
        _serialPort.Handshake = Handshake.None;

        _serialPort.ReadTimeout = 500;
        _serialPort.WriteTimeout = 500;

        _serialPort.Open();
        Debug.Log("Abierto puerto " + com);
        comS = com;
        seleccionado = true;
        ComprobanteConfiguracion.text = "Arco en C configurado";
        ComprobanteConfiguracion.color = Color.white;
    }*/

    public void SeleccionarCom2(bool active, string com)
    {
        if (active && comErroneo == false)
        {
            _serialPort.Close();
            _serialPort.PortName = com;
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            //_serialPort.ReadTimeout = 25;

            _serialPort.Open();
            Debug.Log("Abierto puerto " + com);
            comS = com;
            seleccionado = true;
            ComprobanteConfiguracion.text = "Arco en C configurado";
            PlayerPrefs.SetInt("ArcoC", 1);
            ComprobanteConfiguracion.color = Color.white;
        }
        else
        {
            _serialPort.Close();
            Debug.Log("Cerrado puerto " + com);
            comErroneo = false;
            seleccionado = false;
            ComprobanteConfiguracion.text = "Arco en C no configurado";
            PlayerPrefs.SetInt("ArcoC", 0);
            ComprobanteConfiguracion.color = Color.red;
            ComprobanteCom.text = "Selecciona un puerto COM compatible";
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (seleccionado && comErroneo == false && ComprobanteCom.IsActive())
            {
                some = GetAngle();
                ComprobanteCom.text = some;
            }

            if (comErroneo)
            {
                for (int i = 0; i < toggles.Length; i++)
                {
                    toggles[i].isOn = false;
                    _serialPort.Close();
                    //Debug.Log("Cerrado puerto " + com);
                    comErroneo = false;
                    seleccionado = false;
                    ComprobanteConfiguracion.text = "Arco en C no configurado";
                    ComprobanteConfiguracion.color = Color.red;
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        _serialPort.Close();
        Debug.Log("Cerrado puerto " + comS);
    }

    /*private void OnGUI()
    {
        if (GUI.Button(new Rect(0,0,200,30), slectedItem))
        {
            editing = true;
        }

        if (editing)
        {
            String[] ports = SerialPort.GetPortNames();

            for (int x = 0; x < ports.Length; x++)
            {
                if(GUI.Button(new Rect (0, (30 * x) + 30, 200, 30), ports[x]))
                {
                    slectedItem = ports[x];
                    editing = false;
                    _serialPort.PortName = slectedItem;
                    _serialPort.BaudRate = 9600;
                    _serialPort.Parity = Parity.None;
                    _serialPort.DataBits = 8;
                    _serialPort.StopBits = StopBits.One;
                    _serialPort.Handshake = Handshake.None;

                    // Set the read/write timeouts
                    _serialPort.ReadTimeout = 500;
                    _serialPort.WriteTimeout = 500;

                    _serialPort.Open();
					_continue = true;
                    Debug.Log("Abrí el puerto");
                    editing = false;
                }
            }

        }
        
        textFieldString = GUI.TextField(new Rect(210, 2, 130, 30), textFieldString);
        if (GUI.Button(new Rect(345, 2, 50, 30), "Enviar"))
        {
            _serialPort.Write(textFieldString);
        }

        if (GUI.Button(new Rect(450, 2, 50, 30), "Cerrar"))
        {
            _serialPort.Close();
        }
    }*/
}
