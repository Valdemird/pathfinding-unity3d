using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class FileManager : MonoBehaviour {

    string path;
    public int[,] array2D = new int[10, 10];
    public Square root;
    public Square goal;
    public string searchType;

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void loadLevel() {
        SceneManager.LoadScene("level", LoadSceneMode.Single);
    }
	public void OpenExplorer () {
        path  = EditorUtility.OpenFilePanel("Archivos", "", "txt");
        if (path.Length != 0)
        {
            loadScene(path);


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


