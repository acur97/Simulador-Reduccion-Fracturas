using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarHistorialClinico : MonoBehaviour {

    public HistoriaClinica historiaClinicaPaciente;
    [Space]
    public Text nombres;
    public Text apellidos;
    public Text sexo;
    public Text edad;
    public Text datos;
    public Text diagnostico;
    public Transform content;
    [Space]
    public GameObject PrefabRadiografiasPrevias;

    private void Awake()
    {
        nombres.text = "";
        apellidos.text = "";
        sexo.text = "";
        edad.text = "";
        datos.text = "";
        diagnostico.text = "";
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Start()
    {
        nombres.text = historiaClinicaPaciente.nombresDelPaciente;
        apellidos.text = historiaClinicaPaciente.apellidosDelPaciente;
        if (historiaClinicaPaciente.sexo.ToString() == "Masculino")
        {
            sexo.text = "Masculino";
        }
        if (historiaClinicaPaciente.sexo.ToString() == "Femenino")
        {
            sexo.text = "Femenino";
        }
        edad.text = historiaClinicaPaciente.edad.ToString();
        datos.text = historiaClinicaPaciente.datosImportantes;
        diagnostico.text = historiaClinicaPaciente.diagnostico;
        for (int i = 0; i < historiaClinicaPaciente.radiografiasPrevias.Length; i++)
        {
            GameObject radiografiaPrefab = Instantiate(PrefabRadiografiasPrevias, content);
            Image[] radiografiaImg = radiografiaPrefab.GetComponentsInChildren<Image>();
            radiografiaImg[2].sprite = historiaClinicaPaciente.radiografiasPrevias[i];
        }
    }
}