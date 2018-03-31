using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelectionScript : MonoBehaviour {

    public const string A_STAR = "A*";
    public const string AVARO = "Avaro";
    public const string AMPLITUD = "busqueda por amplitud";
    public const string PROFUNDIDAD = "busqueda por profundidad";
    public const string COSTO_UNIFORME = "busqueda por costo uniforme";

    public Dropdown dropdown;
    public Button startButton;
    List<string> opcionesInformada;
    List<string> opcionesNoInformadas;
    FileManager fileManager;
    // Use this for initialization
    void Start() {
        fileManager = GameObject.FindGameObjectWithTag("manager").GetComponent<FileManager>();

        opcionesInformada = new List<string>();
        opcionesInformada.Add(A_STAR);
        opcionesInformada.Add(AVARO);

        opcionesNoInformadas = new List<string>();
        opcionesNoInformadas.Add(AMPLITUD);
        opcionesNoInformadas.Add(PROFUNDIDAD);
        opcionesNoInformadas.Add(COSTO_UNIFORME);
        dropdown.AddOptions(opcionesInformada);

        fileManager.searchType = dropdown.options[dropdown.value].text;


        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });

    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
        int index = dropdown.value;
        Debug.Log(dropdown.options[index].text);
        fileManager.searchType = dropdown.options[index].text;
    }


    public void setDropdown(bool value) {
        if (!value) {
            dropdown.ClearOptions();
            dropdown.AddOptions(opcionesInformada);
        } else
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(opcionesNoInformadas);
        }
        fileManager.searchType = dropdown.options[dropdown.value].text;

    }



}
