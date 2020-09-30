using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Historia Clinica Paciente", menuName = "Historia clinica Object", order = 1)]
public class HistoriaClinica : ScriptableObject {

    [Header("Datos sobre el paciente")]
    [Space]
    public string nombresDelPaciente;
    public string apellidosDelPaciente;
    public enum Sexo {Masculino, Femenino};
    public Sexo sexo;
    public int edad;
    [Space]
    [TextArea(5, 10)]
    public string datosImportantes;
    [Space]
    [TextArea(10, 20)]
    public string diagnostico;
    [Space]
    public Sprite[] radiografiasPrevias;
}
