using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MapCreatorScript : MonoBehaviour
{
    public GameObject[] elements;
    public GameObject[,] map = new GameObject[10, 10];
    public GameObject mario;
    public GameObject princess;
    public List<GameObject> turttles;
    public List<GameObject> flowers;
    public Text levelTitle;

    private bool mapDone = false;
    private bool marioRequest = false;
    Queue<int> commands;
    // Use this for initialization



    /* 
     0 si es camino libre
     1 si es un muro
     2 si el punto donde inicia Mario
     3 si es una flor
     4 si es una tortuga
     5 si es la princesa
    */


    public void CreateMap(int[,] mapRepresentation)
    {
        commands = new Queue<int>();
        mapDone = false;
        turttles = new List<GameObject>();
        flowers = new List<GameObject>();
        StartCoroutine(makeWorld(mapRepresentation));
    }


    //Vector3(j, 0, i);   i = x;   j = y;
    IEnumerator makeWorld(int[,] mapRepresentation)
    {

        yield return new WaitForSeconds(0f);
        for (int i = 0; i < mapRepresentation.GetLength(0); i++)
        {
            yield return new WaitForSeconds(0f);
            for (int j = 0; j < mapRepresentation.GetLength(1); j++)
            {
                int value = mapRepresentation[i, j];
                Vector3 position = new Vector3(j, 0, i);
                GameObject elementToDraw;
                if (value == Square.WALL)
                {
                    elementToDraw = Instantiate(elements[1], position, transform.rotation) as GameObject;
                }
                else
                {
                    elementToDraw = Instantiate(elements[0], position, Quaternion.identity) as GameObject;
                    Debug.DrawRay(position, Vector3.up, Color.red, Mathf.Infinity);
                    if (value != Square.EMPTY)
                    {
                        GameObject dinamicObject = Instantiate(elements[value], position, transform.rotation) as GameObject;
                        switch (value)
                        {
                            case Square.AGENT:
                                mario = dinamicObject;
                                break;
                            case Square.FLOWER:
                                flowers.Add(dinamicObject);
                                break;
                            case Square.PRICESS:
                                princess = dinamicObject;
                                break;
                            case Square.TURTTLE:
                                turttles.Add(dinamicObject);
                                break;
                        }

                    }
                }
                map[i, j] = elementToDraw;
            }

        }
        mapDone = true;

    }



    public void showExpantions(List<Node> nodos) {
        StartCoroutine(Expand(nodos));
    }

    private IEnumerator Expand(List<Node> nodos)
    {
        Color[] colors = new Color[50];
        Color baseColor = new Color(0, 0, 0, 0);
        foreach (Node nodo in nodos) {
            map[nodo.data.posX, nodo.data.posY].GetComponent<Animator>().Play("pick");
             yield return new WaitForSeconds(0.01f);
        }
        
       

    }



    public void addCommands(List<int> commands) {
        this.commands = new Queue<int>(commands);
        marioRequest = true;
    }

    public IEnumerator executeMarioMove()
    {
        marioRequest = false;
        mario.GetComponent<SimpleCharacterControl>().commands = commands;
        yield return null;
    }

    //Vector3(j, 0, i);   i = x;   j = y;
    public void  Move(int command) {
        Vector3 moveDireccion = Vector3.zero;
        switch (command) {
        case Square.ABAJO:
                moveDireccion = new Vector3(-1, 0, 0);
            break;
        case Square.ARRIBA:
                moveDireccion = new Vector3(1, 0, 0);
                break;
        case Square.IZQUIERDA:
                moveDireccion = new Vector3(0, 0, 1);
                break;
        case Square.DERECHA:
                moveDireccion = new Vector3(0, 0, -1);
                break;
         
    }
        mario.transform.Translate(moveDireccion);
    }

    public void setText(string text) {
        levelTitle.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        if (marioRequest && mapDone) {
            StartCoroutine(executeMarioMove());
        }
    }
}
