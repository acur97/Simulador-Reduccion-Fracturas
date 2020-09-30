using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraMovementSerial : MonoBehaviour {

    private double sensor1_angle;
    private double sensor1_distance;
    public double sensor1_distance_Cal;
    private double sensor1_distance_Cal_LV;
    private double sensor1_angle_Cal_LV;
    public double sensor1_angle_Cal;
    public double sensor2_angle;
    private double sensor2_angle_Cal_LV;

    private bool isStaticCamera = true;

    public Serial serialPort;
    public Text textControlC;
    public Text tipoDeVista;
    public Toggle btnPlanoSuperior;
    public Text TextPlanoSuperior;
    public Toggle btnPlanoLateral;
    public Text TextPlanoLateral;
    public Button btnCambioControl;
    private int btnPlanos = 1;
    [Space]
    public GameObject GO_CArmDeltaX;
    public GameObject GO_CArmAlphaDegree;
    public GameObject GO_CArmBetaDegree;
    private float smooth = 20.0f;
    private Vector3 initialPos;

    [Space]
    public bool calibracion = false;
    public GameObject canvasLado0;
    public GameObject canvasLado90;
    public GameObject canvasLado180;
    public GameObject canvasLado270;

    private int count = 1;
    private int count2 = 1;

    public string someText;
    //public string[] dataSource;
    public string[] dataSource = new string[3];
    public string[] dataSourceB;

    private float num1;
    private float num2;
    private float num3;
    
    private Vector3 tGO_CArmAlphaDegree;
    private Quaternion qGO_CArmAlphaDegree;
    private Quaternion qGO_CArmBetaDegree;
    private Vector3 t2GO_CArmAlphaDegree;
    private Quaternion q2GO_CArmAlphaDegree;
    private Quaternion q2GO_CArmBetaDegree;

    public RectTransform xray;
    public Transform Cxray;

    //private float rotateKey = 0.0f;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("ArcoC") == 1)
        {
            serialPort = FindObjectOfType<Serial>();
        }
        //dataSource = new string[3];

        if (calibracion)
        {
            canvasLado0.SetActive(true);
            canvasLado90.SetActive(false);
            canvasLado180.SetActive(false);
            canvasLado270.SetActive(false);
        }
        else
        {
            btnPlanoLateral.isOn = true;
            btnPlanoSuperior.isOn = false;
        }
    }

    // Use this for initialization
    void Start ()
    {
        //System.Diagnostics.Process.Start(Application.dataPath + "/Read_CArm_Serial.exe");

        //initialPos = GO_CArmDeltaX.transform.position;
        //initialPos = GO_CArmAlphaDegree.transform.position;
        sensor1_angle_Cal_LV = 0.0f;
        sensor2_angle_Cal_LV = 0.0f;
        sensor1_distance_Cal_LV = 0.0f;

        if (PlayerPrefs.GetInt("ArcoC") == 1)
        {
            InvokeRepeating("SerialUpdate", 0, 0.005f);
            btnCambioControl.interactable = true;
        }
        else
        {
            btnCambioControl.interactable = false;
        }
        StartCoroutine(StartDelay());

        tGO_CArmAlphaDegree = GO_CArmAlphaDegree.transform.localPosition;
        qGO_CArmAlphaDegree = GO_CArmAlphaDegree.transform.localRotation;
        qGO_CArmBetaDegree = GO_CArmBetaDegree.transform.localRotation;
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1);
        if (!calibracion)
        {
            CamPlanoLateral();
        }
    }

    private void SerialUpdate()
    {
        if (serialPort != null)
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
    }

    private void LateUpdate()
    {
        if (serialPort != null)
        {
            num1 = Convert.ToInt16(dataSource[0]);
            num2 = Convert.ToInt16(dataSource[1]);
            num3 = Convert.ToInt16(dataSource[2]);

            sensor1_angle = num1;
            sensor1_distance = num2;
            sensor2_angle = num3;
        }

        if (isStaticCamera == false)
        {
            ApplyMovements();
        }

        //Debug.Log(GO_CArmAlphaDegree.transform.localRotation);
    }

    // Update is called once per frame
    /*void Update()
    {
        //Debug.Log(GO_CArmAlphaDegree.transform.localRotation);
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            someText = serialPort.GetAngle();
            //someText = someText.Replace('/', ' ');
            //someText = someText.Replace('*', ' ');
            someText = someText.Replace("$", "");
            someText = someText.Replace("#", "");

            //print("Aqui va la info " + someText);

            dataSource = someText.Split(',');

            if (dataSource.Length > 1)
            {
                //print("Data first position: " + dataSource[0]);

                if (dataSource.Length > 2)
                {
                    sensor1_angle = Convert.ToDouble(dataSource[1]);
                    sensor1_distance = Convert.ToDouble(dataSource[2]);

                    //print("Sensor 1 distance " + sensor1_distance.ToString());

                    sensor1_distance_Cal = 0.2163 * sensor1_distance - 37.019;
                    sensor1_angle_Cal = 0.6801 * sensor1_angle - 62.985;
                    //print("Sensor 1 angle" + sensor1_angle.ToString());
                    //print("Sensor 1 distance Cal" + sensor1_distance_Cal.ToString());

                    //print("Sensor 1 angle Cal" + sensor1_angle_Cal.ToString());
                }
                if (dataSource.Length > 1)
                {
                    sensor2_angle = Convert.ToDouble(dataSource[1]);

                    //print("Sensor 2 angle" + sensor2_angle.ToString());
                }

                if (isStaticCamera == false)
                {
                    ApplyMovements();
                }
            }

            /*if (dataSource.Length > 1)
            {
                //print("Data first position: " + dataSource[0]);

                if (dataSource[0].Contains("1"))
                {
                    if (dataSource.Length > 2)
                    {
                        sensor1_angle = Convert.ToDouble(dataSource[1]);
                        sensor1_distance = Convert.ToDouble(dataSource[2]);

                        //print("Sensor 1 distance " + sensor1_distance.ToString());

                        sensor1_distance_Cal = 0.2163 * sensor1_distance - 37.019;
                        sensor1_angle_Cal = 0.6801 * sensor1_angle - 62.985;
                        //print("Sensor 1 angle" + sensor1_angle.ToString());
                        //print("Sensor 1 distance Cal" + sensor1_distance_Cal.ToString());

                        //print("Sensor 1 angle Cal" + sensor1_angle_Cal.ToString());
                    }
                }
                else if (dataSource[0].Contains("2"))
                {
                    if (dataSource.Length > 1)
                    {
                        sensor2_angle = Convert.ToDouble(dataSource[1]);

                        //print("Sensor 2 angle" + sensor2_angle.ToString());
                    }
                }

                if (isStaticCamera == false)
                {
                    ApplyMovements();
                }
            }
}

        /*if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rotateKey += 3.0f;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            rotateKey -= 3.0f;
        }

        /*using (var fileStream = new FileStream(@"C:\Users\labrv_g24uyfv\Documents\CArm_Data.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var streamReader = new StreamReader(fileStream))
        {
            string someText = streamReader.ReadToEnd();
        
            someText = someText.Replace('/', ' ');
            someText = someText.Replace('*', ' ');

            //print("Aqui va la info " + someText);

            string[] dataSource = someText.Split(',');

            if (dataSource.Length > 1)
            {
                //print("Data first position: " + dataSource[0]);

                if (dataSource[0].Contains("1"))
                {
                    if (dataSource.Length > 2)
                    {
                        sensor1_angle = Convert.ToDouble(dataSource[1]);
                        sensor1_distance = Convert.ToDouble(dataSource[2]);

                        //print("Sensor 1 distance " + sensor1_distance.ToString());

                        sensor1_distance_Cal = 0.2163 * sensor1_distance - 37.019;
                        sensor1_angle_Cal = 0.6801 * sensor1_angle - 62.985;
                        //print("Sensor 1 angle" + sensor1_angle.ToString());
                        //print("Sensor 1 distance Cal" + sensor1_distance_Cal.ToString());

                        //print("Sensor 1 angle Cal" + sensor1_angle_Cal.ToString());
                    }
                }
                else if (dataSource[0].Contains("2"))
                {
                    if (dataSource.Length > 1)
                    {
                        sensor2_angle = Convert.ToDouble(dataSource[1]);

                        //print("Sensor 2 angle" + sensor2_angle.ToString());
                    }
                }

				if (isStaticCamera == false) {
					ApplyMovements ();
				}
            }
        }
    }*/

    public void ToogleStaticCamera(bool iStaticCamera){
		if (iStaticCamera == true) {
			isStaticCamera = true;
		} else if (iStaticCamera == false) {
			isStaticCamera = false;
		}
	}

    public void CambiarControlCamaraC()
    {
        count2 += 1;
        if (count2 == 3)
        {
            count2 = 1;
        }

        if (count2 == 1)
        {
            isStaticCamera = true;
            textControlC.text = "Planos estaticos:";
            btnPlanoLateral.interactable = true;
            btnPlanoSuperior.interactable = true;

            if (btnPlanos == 1)
            {
                btnPlanoLateral.isOn = true;
                btnPlanoSuperior.isOn = false;
            }
            if (btnPlanos == 2)
            {
                btnPlanoLateral.isOn = false;
                btnPlanoSuperior.isOn = true;
            }

            t2GO_CArmAlphaDegree = GO_CArmAlphaDegree.transform.localPosition;
            q2GO_CArmAlphaDegree = GO_CArmAlphaDegree.transform.localRotation;
            q2GO_CArmBetaDegree = GO_CArmBetaDegree.transform.localRotation;

            GO_CArmAlphaDegree.transform.localPosition = tGO_CArmAlphaDegree;
            GO_CArmAlphaDegree.transform.localRotation = qGO_CArmAlphaDegree;
            GO_CArmBetaDegree.transform.localRotation = qGO_CArmBetaDegree;
        }
        if (count2 == 2)
        {
            isStaticCamera = false;
            textControlC.text = "Control manual";
            btnPlanoLateral.interactable = false;
            btnPlanoSuperior.interactable = false;

            GO_CArmAlphaDegree.transform.localPosition = t2GO_CArmAlphaDegree;
            GO_CArmAlphaDegree.transform.localRotation = q2GO_CArmAlphaDegree;
            GO_CArmBetaDegree.transform.localRotation = q2GO_CArmBetaDegree;
        }
    }

    public void CamPlanoSuperior()
    {
        GO_CArmAlphaDegree.transform.localRotation = new Quaternion(-0.7f, 0.0f, 0.0f, -0.7f);
        qGO_CArmAlphaDegree = new Quaternion(-0.7f, 0.0f, 0.0f, -0.7f);
        //GO_CArmAlphaDegree.transform.Rotate(Vector3.right, 90);
        //btnPlanoLateral.interactable = true;
        //btnPlanoSuperior.interactable = false;
        btnPlanos = 2;
        xray.localScale = new Vector3(1, 1, 1);
        Cxray.localEulerAngles = new Vector3(0, -1440, -90);
        tipoDeVista.text = "Vista:" + "\n" + "AP";
        TextPlanoSuperior.text = "Plano" + "\n" + "AP - Seleccionado";
        TextPlanoLateral.text = "Plano" + "\n" + "Lat";
    }

    public void CamPlanoLateral()
    {
        GO_CArmAlphaDegree.transform.localRotation = new Quaternion(-1.0f, 0.0f, 0.0f, 0.0f);
        //qGO_CArmAlphaDegree = new Quaternion(-1.0f, 0.0f, 0.0f, 0.0f);
        qGO_CArmAlphaDegree = new Quaternion(0.0f, 0.0f, 0.0f, -1.0f);
        //GO_CArmAlphaDegree.transform.Rotate(Vector3.right, -90);
        //btnPlanoLateral.interactable = false;
        //btnPlanoSuperior.interactable = true;
        btnPlanos = 1;
        xray.localScale = new Vector3(1, -1, 1);
        Cxray.localEulerAngles = new Vector3(0, -1440, 0);
        tipoDeVista.text = "Vista:" + "\n" + "Lat";
        TextPlanoSuperior.text = "Plano" + "\n" + "AP";
        TextPlanoLateral.text = "Plano" + "\n" + "Lat - Seleccionado";
    }

	public void PlanoAnterior(){
		if (isStaticCamera == true)
        {
			GO_CArmAlphaDegree.transform.Rotate(Vector3.right, 90);
		}

        count += 1;
        if (count == 5)
        {
            count = 1;
        }

        if (count == 1)
        {
            canvasLado0.SetActive(true);
            canvasLado270.SetActive(false);
        }
        if (count == 2)
        {
            canvasLado0.SetActive(false);
            canvasLado90.SetActive(true);
        }
        if (count == 3)
        {
            canvasLado90.SetActive(false);
            canvasLado180.SetActive(true);
        }
        if (count == 4)
        {
            canvasLado180.SetActive(false);
            canvasLado270.SetActive(true);
        }
    }



    void ApplyMovements()
    {
        //GO_CArmDeltaX.transform.position = initialPos + new Vector3(fDistance, 0.0f, 0.0f);
        //GO_CArmBetaDegree.transform.rotation = Quaternion.Slerp (GO_CArmBetaDegree.transform.rotation, Quaternion.Euler(fNivel, 0.0f, 0.0f), Time.deltaTime*smooth);
        //GO_CArmAlphaDegree.transform.rotation = Quaternion.Slerp(GO_CArmAlphaDegree.transform.rotation, Quaternion.Euler(270.0f, 93.0f + fAngle, 0.0f), Time.deltaTime * smooth);

        //GO_CArmDeltaX.transform.position = initialPos + new Vector3(0.0f, 0.0f, -(float)sensor1_distance_Cal * 10.0f);

        //GO_CArmBetaDegree.transform.position = initialPos + new Vector3(0.0f, 0.0f, -(float)sensor1_distance_Cal * 10.0f);

        GO_CArmBetaDegree.transform.Rotate(Vector3.forward, (float)sensor1_angle_Cal - (float)sensor1_angle_Cal_LV);
        GO_CArmAlphaDegree.transform.Rotate(Vector3.right, (float)sensor2_angle - (float)sensor2_angle_Cal_LV);
        GO_CArmAlphaDegree.transform.Translate(new Vector3(0.0f, 0.0f, ((float)sensor1_distance_Cal - (float)sensor1_distance_Cal_LV))*10.0f);

        //GO_CArmAlphaDegree.transform.position = initialPos + new Vector3(0.0f, 0.0f, -(float)sensor1_distance_Cal * 10.0f);
        //GO_CArmAlphaDegree.transform.Rotate(Vector3.right, (float)rotateKey - (float)sensor2_angle_Cal_LV);

        //GO_CArmBetaDegree.transform.rotation = Quaternion.AngleAxis((float)sensor1_angle_Cal-(float)sensor1_angle_Cal_LV, Vector3.forward);



        //GO_CArmAlphaDegree.transform.rotation = Quaternion.Slerp(GO_CArmAlphaDegree.transform.rotation, Quaternion.Euler(0.0f, (float)rotateKey - 180.0f, 90.0f), Time.deltaTime * smooth);

        //GO_CArmBetaDegree.transform.rotation = Quaternion.Slerp(GO_CArmBetaDegree.transform.rotation, Quaternion.Euler(0.0f, 180.0f, (float)sensor1_angle_Cal +90.0f), Time.deltaTime * smooth);

        //GO_CArmBetaDegree.transform.rotation = Quaternion.Slerp(GO_CArmBetaDegree.transform.rotation, Quaternion.Euler(-1.8f,-180.0f,(float)sensor1_angle_Cal-270.0f), Time.deltaTime*smooth);
        //GO_CArmBetaDegree.transform.rotation = Quaternion.Slerp(GO_CArmBetaDegree.transform.rotation, Quaternion.Euler(-1.8f, 0.0f, (float)sensor1_angle_Cal - 270.0f), Time.deltaTime * smooth);

        //GO_CArmBetaDegree.transform.rotation = Quaternion.Slerp(GO_CArmBetaDegree.transform.rotation, Quaternion.AngleAxis((float)sensor1_angle_Cal,Vector3.forward), Time.deltaTime * smooth);

        //GO_CArmBetaDegree.transform.rotation = Quaternion.Slerp(GO_CArmBetaDegree.transform.rotation, Quaternion.AngleAxis((float)rotateKey, Vector3.back), Time.deltaTime * smooth);

        //GO_CArmBetaDegree.transform.Rotate(Vector3.forward, (float)sensor1_angle_Cal);

        //GO_CArmBetaDegree.transform.rotation = Quaternion.AngleAxis(rotateKey, Vector3.forward);

        //print("Rotation key " + rotateKey.ToString());
        //

        //GO_CArmAlphaDegree.transform.rotation = Quaternion.Slerp(GO_CArmAlphaDegree.transform.rotation, Quaternion.Euler(0.0f, (float)rotateKey-180.0f, 90.0f), Time.deltaTime * smooth);


        //GO_CArmAlphaDegree.transform.rotation = Quaternion.Slerp(GO_CArmAlphaDegree.transform.rotation, Quaternion.Euler(0.0f, 0.0f, (float)sensor1_angle), Time.deltaTime * smooth);

        //sensor2_angle_Cal_LV = rotateKey;


        ///COMENTADOS EN PRUEBA
        sensor1_angle_Cal_LV = sensor1_angle_Cal;
        sensor2_angle_Cal_LV = sensor2_angle;
        sensor1_distance_Cal_LV = sensor1_distance_Cal;
    }
}
