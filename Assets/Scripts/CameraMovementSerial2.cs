using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CameraMovementSerial2 : MonoBehaviour {

    public Transform objeto1;
    public Transform objeto2;
    public Transform objeto3;
    private int valor1;
    public Text val1;
    private int valor2;
    public Text val2;
    private int valor3;
    public Text val3;
    public Serial serialPort;
    public string someText;
    public string[] dataSource;
    public string[] dataSourceB;

    private float num1;
    private float num2;
    private float num3;

    private void Awake()
    {
        serialPort = FindObjectOfType<Serial>();
        dataSource = new string[3];
    }

    private void Start()
    {
        InvokeRepeating("SerialUpdate", 0, 0.005f);
    }

    private void SerialUpdate()
    {
        someText = serialPort.GetAngle();

        if (someText.Contains("$"))
        {
            someText = someText.Replace("$", "");
            if (someText.Contains(","))
            {
                dataSourceB = someText.Split(',');
                if (dataSourceB.Length == 2)
                {
                    if (!dataSourceB[0].Contains("$") && !dataSourceB[0].Contains("#") && !dataSourceB[0].Contains(",") && !dataSourceB[0].Contains(" ") && dataSourceB[0] != null && dataSourceB[0].Length < 4 && dataSourceB[0].Length != 0)
                    {
                        dataSource[0] = dataSourceB[0];
                    }
                    if (!dataSourceB[1].Contains("$") && !dataSourceB[1].Contains("#") && !dataSourceB[1].Contains(",") && !dataSourceB[1].Contains(" ") && dataSourceB[1] != null && dataSourceB[1].Length < 4 && dataSourceB[1].Length != 0)
                    {
                        dataSource[1] = dataSourceB[1];
                    }
                }
            }
        }
        if (someText.Contains("#"))
        {
            someText = someText.Replace("#", "");

            if (!someText.Contains("$") && !someText.Contains("#") && !someText.Contains(",") && !someText.Contains(" ") && someText != null && someText.Length < 4 && someText.Length != 0)
            {
                dataSource[2] = someText;
            }
        }
    }

    private void LateUpdate()
    {
        num1 = Convert.ToInt16(dataSource[0]);
        num2 = Convert.ToInt16(dataSource[1]);
        num3 = Convert.ToInt16(dataSource[2]);

        val1.text = num1.ToString();
        val2.text = num2.ToString();
        val3.text = num3.ToString();

        objeto1.localPosition = new Vector3(50, num1, 0);
        objeto2.localPosition = new Vector3(0, num2, 0);
        objeto3.localPosition = new Vector3(-50, num3, 0);
    }
}