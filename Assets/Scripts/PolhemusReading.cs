using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using UnityEditor;
using System.Collections;

public class PolhemusReading : MonoBehaviour {

    private string TrackerData = "";
    private float upLX;
    private float upLY;
    private float upLZ;
    private float loLX;
    private float loLY;
    private float loLZ;

    // 1.0f Traslation -- 1.0f Rotation
    private float flagTraslationRotation = 1.0f;
    private bool isRotation = true;
    private bool isUpperLimb = true;
    private bool isStaticCamera = true;

    //private float upRX;
    //private float upRY;
    //private float upRZ;

    //private float loRX;
    //private float loRY;
    //private float loRZ;

    private float upRQ1;
    private float upRQ2;
    private float upRQ3;
    private float upRQ4;

    private float loRQ1;
    private float loRQ2;
    private float loRQ3;
    private float loRQ4;

    private float smooth = 20.0f;

    public GameObject upLimb;
    public GameObject loLimb;
    private string polhemusData;

    //Calibration offsets traslation
    private float offSetUpLimbX;
    private float offSetUpLimbY;
    private float offSetUpLimbZ;
    private float offSetLoLimbX;
    private float offSetLoLimbY;
    private float offSetLoLimbZ;

    //Calibration offsets rotation
    private float GoffSetUpLimbX;
    private float GoffSetUpLimbY;
    private float GoffSetUpLimbZ;
    private float GoffSetLoLimbX;
    private float GoffSetLoLimbY;
    private float GoffSetLoLimbZ;


    //private float offSetUpLimbRotX = 0.0f;
    private float offSetUpLimbRotY;
    //private float offSetUpLimbRotZ = 0.0f;
    private float offSetLoLimbotX;
    private float offSetLoLimbRotY;
    private float offSetLoLimbRotZ;

    /* Variables added by Berna, para que dejen de ser priovadas*/
    float offSetLoLimbRotX;
    float offSetUpLimbRotZ;
    float offSetUpLimbRotX;

    private Vector3 InitPosSceneUpLimb;
    private Vector3 InitPosSceneLoLimb;

    private Vector3 StartPosUpLimb;
    private Vector3 StartPosLoLimb;

    private Vector3 InitOffSetUpLimb;
    private Vector3 InitOffSetLoLimb;

    //Scale factor tracker - Unity Scene (2.54 * 10)
    //float scaleFactor = 2.54f * 10.0f;
    public float scaleFactor = 3.937f;
    //float scaleFactor = 2.54f;
    //int counter = 60;

    [Space]
    public Offsets offsetScriptable;
    private int count = 1;
    [Space]
    public bool calibracion = false;
    public Button masMulti;
    public Button menosMulti;
    public Text textoMulti;
    private float MultiplicadorOffset = 2;

    private string pathA;
    private string pathB;

    private void Awake()
    {
        //InitPosSceneUpLimb = upLimb.transform.position;
        //InitPosSceneLoLimb = loLimb.transform.position;

        /*StreamWriter sw = new StreamWriter(Application.dataPath + "/Ejecutables_externos/Polhemus.txt");
        sw.WriteLine("");
        Debug.Log(Application.dataPath + "/Ejecutables_externos");*/
        
        pathA = Application.dataPath;

#if UNITY_STANDALONE && !UNITY_EDITOR
        pathB = pathA.Replace((Application.productName + "_Data"), "/Polhemus_DATA.txt");
        print(pathB);
        print("Standalone");

#elif UNITY_EDITOR
        pathB = pathA.Replace("/Assets", "/Polhemus_DATA.txt");
        print(pathB);
        print("Editor");

#endif

        /*Process proceso = new Process();
        //startInfo.CreateNoWindow = true;
        string path = pathA + "/Ejecutables_externos/TestingPolhemusTracker.exe";
        proceso.StartInfo.FileName = path;
        //startInfo.Arguments = path;
        proceso.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
        //startInfo.CreateNoWindow = true;
        proceso.StartInfo.UseShellExecute = true;
        //proceso.StartInfo.RedirectStandardOutput = true;
        //proceso.StartInfo.RedirectStandardError = true;
        proceso.Start();*/


        if (calibracion)
        {
            textoMulti.text = MultiplicadorOffset.ToString("F2");
        }
    }

    // Use this for initialization
    void Start ()
    {
        System.Diagnostics.Process.Start(Application.dataPath + "/Ejecutables_externos/TestingPolhemusTracker.exe");

        /*ProcessStartInfo proceso = new ProcessStartInfo();
        //startInfo.CreateNoWindow = true;
        //startInfo.UseShellExecute = false;
        string path = Application.dataPath + "/Ejecutables_externos/TestingPolhemusTracker.exe";
        proceso.FileName = path;
        //startInfo.Arguments = path;
        //startInfo.WindowStyle = ProcessWindowStyle.Minimized;
        //startInfo.CreateNoWindow = true;
        proceso.UseShellExecute = true;
        Process.Start(proceso);*/

        /*Process proceso = new Process();
        //startInfo.CreateNoWindow = true;
        string path = Application.dataPath + "/Ejecutables_externos/TestingPolhemusTracker.exe";
        proceso.StartInfo.FileName = path;
        //startInfo.Arguments = path;
        //startInfo.WindowStyle = ProcessWindowStyle.Minimized;
        //startInfo.CreateNoWindow = true;
        proceso.StartInfo.UseShellExecute = true;
        proceso.Start();*/

        //print(System.Environment.SpecialFolder.ProgramFiles);

        upRQ1 = 0.0f;
		upRQ2 = 0.0f;
		upRQ3 = 0.0f;
		upRQ4 = 0.0f;

		loRQ1 = 0.0f;
		loRQ2 = 0.0f;
		loRQ3 = 0.0f;
		loRQ4 = 0.0f;

        //Setting initial pos upper limb
        //offSetUpLimbX = 0.0f;
        //offSetUpLimbY = 0.0f;
        //offSetUpLimbZ = 0.0f;

        //Setting initial pos lower limb
        //offSetLoLimbX = 42.0f;
        //offSetLoLimbY = -261.0f;
        //offSetLoLimbZ = 36.0f;

        /*offSetLoLimbX = -12.0f;
        offSetLoLimbY = -9.0f;
        offSetLoLimbZ = 0.0f;*/

        //offSetLoLimbX = 0.0f;
        //offSetLoLimbY = 0.0f;
        //offSetLoLimbZ = 0.0f;

        //offSetUpLimbRotX = 0.0f;
        //offSetUpLimbRotY = 0.0f;
        //offSetUpLimbRotZ = 0.0f;

        //offSetLoLimbRotX = 0.0f;
        //offSetLoLimbRotY = 0.0f;
        //offSetLoLimbRotZ = 0.0f;

        //Setting Previous pos limb´s
        offSetUpLimbX = offsetScriptable.offSetUpLimbX;
        offSetUpLimbY = offsetScriptable.offSetUpLimbY;
        offSetUpLimbZ = offsetScriptable.offSetUpLimbZ;

        offSetLoLimbX = offsetScriptable.offSetLoLimbX;
        offSetLoLimbY = offsetScriptable.offSetLoLimbY;
        offSetLoLimbZ = offsetScriptable.offSetLoLimbZ;

        //Setting Previous rot limb´s
        offSetUpLimbRotX = offsetScriptable.offSetUpLimbRotX;
        offSetUpLimbRotY = offsetScriptable.offSetUpLimbRotY;
        offSetUpLimbRotZ = offsetScriptable.offSetUpLimbRotZ;

        offSetLoLimbRotX = offsetScriptable.offSetLoLimbRotX;
        offSetLoLimbRotY = offsetScriptable.offSetLoLimbRotY;
        offSetLoLimbRotZ = offsetScriptable.offSetLoLimbRotZ;
    }
	
	// Update is called once per frame
	void Update () {
	
		ReadingPolhemus();

        //CalibrationProcess();

        /*if (counter != 0){
            counter--;
        }
        else {

            StartPosUpLimb = upLimb.transform.position;
            StartPosLoLimb = loLimb.transform.position;

            InitOffSetUpLimb = StartPosUpLimb - InitPosSceneUpLimb;
            InitOffSetLoLimb = StartPosLoLimb - InitPosSceneLoLimb;

            counter--;
        }*/

		//SimulatingReading();
	}
	

	void ReadingPolhemus()
    {
//#if UNITY_EDITOR
        using (StreamReader reader = new StreamReader(new FileStream(pathB, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
		{
			TrackerData = reader.ReadLine();
			TrackerData = TrackerData + " " + reader.ReadLine();
			
			//print(TrackerData);
		}
//#endif
/*#if UNITY_STANDALONE
        using (StreamReader reader = new StreamReader(new FileStream(pathB, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
        {
            TrackerData = reader.ReadLine();
            TrackerData = TrackerData + " " + reader.ReadLine();

            //print(TrackerData);
        }
#endif*/

        string[] data = TrackerData.Split(' ');
		
		//print (data[3] + " " + data[4] + " " + data[5]);
		
		if(data.GetLength(0) >= 13)
		{
			upLX = float.Parse(data[0]) * scaleFactor;
			upLY = float.Parse(data[1]) * scaleFactor;
			upLZ = float.Parse(data[2]) * scaleFactor;
			
			upRQ1 = float.Parse(data[3]);
			upRQ2 = float.Parse(data[4]);
			upRQ3 = float.Parse(data[5]);
			upRQ4 = float.Parse(data[6]);

			loLX = float.Parse(data[7]) * scaleFactor;
			loLY = float.Parse(data[8]) * scaleFactor;
			loLZ = float.Parse(data[9]) * scaleFactor;
			
			loRQ1 = float.Parse(data[10]);
			loRQ2 = float.Parse(data[11]);
			loRQ3 = float.Parse(data[12]);
			loRQ4 = float.Parse(data[13]);

			//polhemusData = upLX + " " + upLY + " " + upLZ + " " + loLX + " " + loLY + " " + loLZ;
			//print(polhemusData);

			Quaternion qTemp1 = new Quaternion(upRQ1, upRQ2, upRQ3, upRQ4);
			Quaternion qTemp2 = new Quaternion(loRQ1, loRQ2, loRQ3, loRQ4);

            //THE POSITION OF THE UPPER LIMB IS NOT CHANGED BECAUSE IT IS LINKED TO THE UPPER LIMB
            upLimb.transform.position = new Vector3(-upLZ + offSetUpLimbZ, -upLY + offSetUpLimbY, upLX + offSetUpLimbX);

            //FOR RECOVERING OF THE INITIAL PROGRAM
            //loLimb.transform.position = new Vector3(-loLZ+offSetLoLimbZ-InitOffSetLoLimb.x, -loLY+offSetLoLimbY-InitOffSetLoLimb.y, loLX+offSetLoLimbX-InitOffSetLoLimb.z);
            loLimb.transform.position = new Vector3(-loLZ + offSetLoLimbZ, -loLY + offSetLoLimbY, loLX + offSetLoLimbX);

            //upLimb.transform.rotation = Quaternion.Slerp (upLimb.transform.rotation,qTemp1, Time.deltaTime*smooth);
            //loLimb.transform.rotation = Quaternion.Slerp (loLimb.transform.rotation,qTemp2, Time.deltaTime*smooth);

            Vector3 vTemp = upLimb.transform.rotation.ToEulerAngles();

            Vector3 vTemp1 = qTemp1.ToEulerAngles();
            Vector3 vTemp2 = qTemp2.ToEulerAngles();

            vTemp1.x = (vTemp1.x * 180.0f) / 3.14f;
            vTemp1.y = (vTemp1.y * 180.0f) / 3.14f;
            vTemp1.z = (vTemp1.z * 180.0f) / 3.14f;

            vTemp2.x = (vTemp2.x * 180.0f) / 3.14f;
            vTemp2.y = (vTemp2.y * 180.0f) / 3.14f;
            vTemp2.z = (vTemp2.z * 180.0f) / 3.14f;

            Quaternion tempQ1 = Quaternion.Euler(new Vector3(-vTemp1.x + offSetUpLimbRotZ, -vTemp1.y + offSetUpLimbRotY, -vTemp1.z + offSetUpLimbRotX));
            Quaternion tempQ2 = Quaternion.Euler(new Vector3(-vTemp2.x + offSetLoLimbRotZ, -vTemp2.y + offSetLoLimbRotY, -vTemp2.z + offSetLoLimbRotX));

			//Quaternion tempQ1 = Quaternion.Euler(new Vector3(-vTemp1.x, -vTemp1.y, -vTemp1.z));
			//Quaternion tempQ2 = Quaternion.Euler(new Vector3(-vTemp2.x, -vTemp2.y, -vTemp2.z));

            //upLimb.transform.rotation = Quaternion.Euler(new Vector3(-vTemp.x, -vTemp.y, -vTemp.z));

            upLimb.transform.rotation = Quaternion.Slerp(upLimb.transform.rotation, tempQ1, Time.deltaTime * smooth);
			loLimb.transform.rotation = Quaternion.Slerp(loLimb.transform.rotation, tempQ2, Time.deltaTime * smooth);
	
		}
	}

    /*public void ToogleUpperLimb(Text testoBtn)
    {
        count += 1;
        if (count == 3)
        {
            count = 1;
        }

		if (count == 1)
        {
			isUpperLimb = true;
            testoBtn.text = "Controlar Lower Limb";
        }
        else if (count == 2)
        {
			isUpperLimb = false;
            testoBtn.text = "Controlar Upper Limb";
        }
	}*/

	/*public void ToogleRotation(bool iRotation){
		if (iRotation == true) {
			isRotation = true;
			//print (isRotation);
		} else if (iRotation == false) {
			isRotation = false;
			//print (isRotation);
		}
	}*/

	/*public void ToogleStaticCamera(bool iStaticCamera){
		if (iStaticCamera == true) {
			isStaticCamera = true;
			//print (isStaticCamera);
		} else if (iStaticCamera == false) {
			isStaticCamera = false;
			//print (isStaticCamera);
		}
	}*/

    public void MasMultiplicador()
    {
        if (MultiplicadorOffset < 3)
        {
            MultiplicadorOffset += 0.25f;
        }

        textoMulti.text = MultiplicadorOffset.ToString("F2");

        if (MultiplicadorOffset == 3)
        {
            masMulti.interactable = false;
        }
        else
        {
            masMulti.interactable = true;
        }

        if (MultiplicadorOffset == 0.25f)
        {
            menosMulti.interactable = false;
        }
        else
        {
            menosMulti.interactable = true;
        }
    }

    public void MenosMultiplicador()
    {
        if (MultiplicadorOffset > 0.25)
        {
            MultiplicadorOffset -= 0.25f;
        }

        textoMulti.text = MultiplicadorOffset.ToString("F2");

        if (MultiplicadorOffset == 3)
        {
            masMulti.interactable = false;
        }
        else
        {
            masMulti.interactable = true;
        }

        if (MultiplicadorOffset == 0.25f)
        {
            menosMulti.interactable = false;
        }
        else
        {
            menosMulti.interactable = true;
        }
    }

    //calibraciones especificas por hueso

    //upper
    public void Calibra_Upper_Rot_X(bool mas)
    {
        if (mas)
        {
            offSetUpLimbRotX += MultiplicadorOffset;
        }
        else
        {
            offSetUpLimbRotX -= MultiplicadorOffset;
        }
    }

    public void Calibra_Upper_Rot_Y(bool mas)
    {
        if (mas)
        {
            offSetUpLimbRotY += MultiplicadorOffset;
        }
        else
        {
            offSetUpLimbRotY -= MultiplicadorOffset;
        }
    }

    public void Calibra_Upper_Rot_Z(bool mas)
    {
        if (mas)
        {
            offSetUpLimbRotZ += MultiplicadorOffset;
        }
        else
        {
            offSetUpLimbRotZ -= MultiplicadorOffset;
        }
    }

    //lower
    public void Calibra_Lower_Pos_X(bool mas)
    {
        if (mas)
        {
            offSetLoLimbX += MultiplicadorOffset;
        }
        else
        {
            offSetLoLimbX -= MultiplicadorOffset;
        }
    }

    public void Calibra_Lower_Pos_Y(bool mas)
    {
        if (mas)
        {
            offSetLoLimbY += MultiplicadorOffset;
        }
        else
        {
            offSetLoLimbY -= MultiplicadorOffset;
        }
    }

    public void Calibra_Lower_Pos_Z(bool mas)
    {
        if (mas)
        {
            offSetLoLimbZ += MultiplicadorOffset;
        }
        else
        {
            offSetLoLimbZ -= MultiplicadorOffset;
        }
    }

    public void Calibra_Lower_Rot_X(bool mas)
    {
        if (mas)
        {
            offSetLoLimbRotX += MultiplicadorOffset;
        }
        else
        {
            offSetLoLimbRotX -= MultiplicadorOffset;
        }
    }

    public void Calibra_Lower_Rot_Y(bool mas)
    {
        if (mas)
        {
            offSetLoLimbRotY += MultiplicadorOffset;
        }
        else
        {
            offSetLoLimbRotY -= MultiplicadorOffset;
        }
    }

    public void Calibra_Lower_Rot_Z(bool mas)
    {
        if (mas)
        {
            offSetLoLimbRotZ += MultiplicadorOffset;
        }
        else
        {
            offSetLoLimbRotZ -= MultiplicadorOffset;
        }
    }

    /*public void ButtonXPlus(){
		if (isUpperLimb == true) {
			if (isRotation == true) {
				offSetUpLimbRotX += 1.0f;
			} else {
				offSetUpLimbX += 3.0f;
			}
		} else {
			if (isRotation == true) {
				offSetLoLimbRotX += 1.0f;
			} else {
				offSetLoLimbX += 3.0f;
			}
		}
	}

	public void ButtonXMinus(){
		if (isUpperLimb == true) {
			if (isRotation == true) {
				offSetUpLimbRotX -= 1.0f;
			} else {
				offSetUpLimbX -= 3.0f;
			}
		} else {
			if (isRotation == true) {
				offSetLoLimbRotX -= 1.0f;
			} else {
				offSetLoLimbX -= 3.0f;
			}
		}
	}

	public void ButtonYPlus(){
		if (isUpperLimb == true) {
			if (isRotation == true) {
				offSetUpLimbRotY += 1.0f;
			} else {
				offSetUpLimbY += 3.0f;
			}
		} else {
			if (isRotation == true) {
				offSetLoLimbRotY += 1.0f;
			} else {
				offSetLoLimbY += 3.0f;
			}
		}
	}

	public void ButtonYMinus(){
		if (isUpperLimb == true) {
			if (isRotation == true) {
				offSetUpLimbRotY -= 1.0f;
			} else {
				offSetUpLimbY -= 3.0f;
			}
		} else {
			if (isRotation == true) {
				offSetLoLimbRotY -= 1.0f;
			} else {
				offSetLoLimbY -= 3.0f;
			}
		}
	}

	public void ButtonZPlus(){
		//print ("ButtonZPlus");
		if (isUpperLimb == true) {
			if (isRotation == true) {
				offSetUpLimbRotZ += 1.0f;
			} else {
				offSetUpLimbZ += 3.0f;
			}
		} else {
			if (isRotation == true) {
				offSetLoLimbRotZ += 1.0f;
			} else {
				offSetLoLimbZ += 3.0f;
			}
		}
	}

	public void ButtonZMinus(){
		if (isUpperLimb == true) {
			if (isRotation == true) {
				offSetUpLimbRotZ -= 1.0f;
			} else {
				offSetUpLimbZ -= 3.0f;
			}
		} else {
			if (isRotation == true) {
				offSetLoLimbRotZ -= 1.0f;
			} else {
				offSetLoLimbZ -= 3.0f;
			}
		}
	}*/



    /*void CalibrationProcess()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            flagTraslationRotation = 1;

            //print("Passing for here 1");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            flagTraslationRotation = 2;

            //print("Passing for here 2");
        }

        if (flagTraslationRotation == 1) //Calibration of Traslation
        {
            //Upper Limb
            if (Input.GetKeyDown(KeyCode.W))
            {
                offSetUpLimbX += 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                offSetUpLimbX -= 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                offSetUpLimbY += 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                offSetUpLimbY -= 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                offSetUpLimbZ += 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                offSetUpLimbZ -= 3.0f;
            }


            //Lower Limb
            if (Input.GetKeyDown(KeyCode.T))
            {
                offSetLoLimbX += 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                offSetLoLimbX -= 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                offSetLoLimbY += 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                offSetLoLimbY -= 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                offSetLoLimbZ += 3.0f;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                offSetLoLimbZ -= 3.0f;
            }
        }
        else if (flagTraslationRotation == 2) //Calibration of Rotation
        {
            //Upper Limb
            if (Input.GetKeyDown(KeyCode.W))
            {
                offSetUpLimbRotX += 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                offSetUpLimbRotX -= 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                offSetUpLimbRotY += 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                offSetUpLimbRotY -= 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                offSetUpLimbRotZ += 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                offSetUpLimbRotZ -= 1.0f;
            }


            //Lower Limb
            if (Input.GetKeyDown(KeyCode.T))
            {
                offSetLoLimbRotX += 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                offSetLoLimbRotX -= 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                offSetLoLimbRotY += 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                offSetLoLimbRotY -= 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                offSetLoLimbRotZ += 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                offSetLoLimbRotZ -= 1.0f;
            }
        }

        //print("Offset Traslation UpLimb: " + offSetUpLimbX + " " + offSetUpLimbY + " " + offSetUpLimbZ);
        //print("Offset Traslation LoLimb: " + offSetLoLimbX + " " + offSetLoLimbY + " " + offSetLoLimbZ);

        //print("Offset Rotation UpLimb: " + offSetUpLimbRotX + " " + offSetUpLimbRotY + " " + offSetUpLimbRotZ);
        //print("Offset Rotation LoLimb: " + offSetLoLimbRotX + " " + offSetLoLimbRotY + " " + offSetLoLimbRotZ);
    }*/

    public void GuardarOffsets()
    {
        offsetScriptable.offSetUpLimbX = offSetUpLimbX;
        offsetScriptable.offSetUpLimbY = offSetUpLimbY;
        offsetScriptable.offSetUpLimbZ = offSetUpLimbZ;

        offsetScriptable.offSetLoLimbX = offSetLoLimbX;
        offsetScriptable.offSetLoLimbY = offSetLoLimbY;
        offsetScriptable.offSetLoLimbZ = offSetLoLimbZ;

        offsetScriptable.offSetUpLimbRotX = offSetUpLimbRotX;
        offsetScriptable.offSetUpLimbRotY = offSetUpLimbRotY;
        offsetScriptable.offSetUpLimbRotZ = offSetUpLimbRotZ;

        offsetScriptable.offSetLoLimbRotX = offSetLoLimbRotX;
        offsetScriptable.offSetLoLimbRotY = offSetLoLimbRotY;
        offsetScriptable.offSetLoLimbRotZ = offSetLoLimbRotZ;
    }

    public void BorrarOffsets()
    {
        offsetScriptable.offSetUpLimbX = 0;
        offsetScriptable.offSetUpLimbY = 0;
        offsetScriptable.offSetUpLimbZ = 0;

        offsetScriptable.offSetLoLimbX = 0;
        offsetScriptable.offSetLoLimbY = 0;
        offsetScriptable.offSetLoLimbZ = 0;

        offsetScriptable.offSetUpLimbRotX = 0;
        offsetScriptable.offSetUpLimbRotY = 0;
        offsetScriptable.offSetUpLimbRotZ = 0;

        offsetScriptable.offSetLoLimbRotX = 0;
        offsetScriptable.offSetLoLimbRotY = 0;
        offsetScriptable.offSetLoLimbRotZ = 0;

        offSetUpLimbX = 0;
        offSetUpLimbY = 0;
        offSetUpLimbZ = 0;

        offSetLoLimbX = 0;
        offSetLoLimbY = 0;
        offSetLoLimbZ = 0;

        offSetUpLimbRotX = 0;
        offSetUpLimbRotY = 0;
        offSetUpLimbRotZ = 0;

        offSetLoLimbRotX = 0;
        offSetLoLimbRotY = 0;
        offSetLoLimbRotZ = 0;
    }
}
