using UnityEngine;
using Crosstales.FB;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class FileManager : MonoBehaviour {

    public string path;
    public int[,] array2D = new int[10, 10];
    public Square root;
    public Square goal;
    public string searchType;
    public MenuSelectionScript menuSelectionScript;


    void Start() {
        DontDestroyOnLoad(gameObject);
        path = "";
        menuSelectionScript.startButton.interactable = false;


    }

    public void loadLevel() {
        SceneManager.LoadScene("level", LoadSceneMode.Single);
    }

    public void loadMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void OpenExplorer () {
        
        path  = FileBrowser.OpenSingleFile("Open File", "", "");
        if (path.Length != 0)
        {
            loadScene(path);


        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (SceneManager.GetActiveScene().name == "level")
            {
                loadMenu();
                GameObject.Destroy(gameObject);
            }
            else {
                Application.Quit();
            }
        }
    }

    private void loadScene(string path)
    {

        StreamReader reader = new StreamReader(path);
        try
        {
            int i = 0;
            while (!reader.EndOfStream)
            {
                string tmp = reader.ReadLine();
                string[] arrayString = tmp.Split(' ');
                tmp = "";
                for (int j = 0; j < arrayString.Length; j++)
                {
                    array2D[i, j] = Int32.Parse(arrayString[j]);
                    if (array2D[i, j] == Square.AGENT) {
                        root = new Square(i, j, Square.AGENT);
                    }
                    if (array2D[i, j] == Square.PRICESS)
                    {
                        goal = new Square(i, j, Square.PRICESS);
                    }
                }
                i++;
            }
            menuSelectionScript.RutaDelPath.text = path;
            menuSelectionScript.startButton.interactable = true;
        }
        catch (FormatException e)
        {
            Debug.LogError("formato no permitido " + e.Message);
        }
        finally {
            reader.Close();
        }
    }
}


