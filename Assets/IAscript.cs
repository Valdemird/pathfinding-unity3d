using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAscript : MonoBehaviour
{

    Node nodoInicial;
    Square casillaFinal;
    List<Node> nodosVisitados;
    MapCreatorScript mapCreatorScript;
    int[,] mapRepresentation;
    string searchType;
    float time;
    int Profundidad;
    int nodosExpandidos;

    // Use this for initialization




    void Start()
    {
        nodosExpandidos = 0;
        mapCreatorScript = gameObject.GetComponent<MapCreatorScript>();
        LoadMap();
        mapCreatorScript.CreateMap(mapRepresentation);
        nodosVisitados = new List<Node>();
        StartCoroutine(Busqueda());


    }

    IEnumerator Busqueda()
    {
        yield return new WaitForSeconds(2f);
        Node result = null;
        time = Time.realtimeSinceStartup;
        switch (searchType) {
            case MenuSelectionScript.AMPLITUD:
                result = BusquedaPorAmplitud();
                break;
            case MenuSelectionScript.PROFUNDIDAD:
                result = BusquedaPorProfundidad();
                break;
            case MenuSelectionScript.COSTO_UNIFORME:
                result = BusquedaPorCosteUniforme();
                break;
            case MenuSelectionScript.A_STAR:
                result = AStar();
                break;
            case MenuSelectionScript.AVARO:
                result = BusquedaAvara();
                break;

        }
        time = Time.realtimeSinceStartup - time;
        mapCreatorScript.setInfo(time * 1000, result.depth, nodosExpandidos);
        List<int> commands = new List<int>();
        while (result != null)
        {
            commands.Add(result.action);

            result = result.parent;
        }
        FixCommands(commands);
        mapCreatorScript.addCommands(commands);
        mapCreatorScript.showExpantions(nodosVisitados);
    }

    void FixCommands(List<int> commands)
    {
        commands.Reverse();
        for (int i = 0; i < commands.Count; i++)
        {
            commands[i] *= -1;
        }
    }

    void LoadMap()
    {

        GameObject manager = GameObject.FindGameObjectWithTag("manager");
        if (manager != null)
        {
            FileManager fileManager = manager.GetComponent<FileManager>();
            mapRepresentation = fileManager.array2D;
            nodoInicial = new Node(fileManager.root);
            casillaFinal = fileManager.goal;
            searchType = fileManager.searchType;
            mapCreatorScript.setText(searchType);


        }
        else
        {
            mapRepresentation = new int[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    mapRepresentation[x, y] = 0;
                }
            }
            mapRepresentation[5, 5] = 2;
            nodoInicial = new Node(new Square(5, 5, Square.AGENT));
            casillaFinal = new Square(9, 9, Square.PRICESS);
            mapRepresentation[9, 9] = 5;
        }
    }

    public Node BusquedaPorAmplitud()
    {
        Queue<Node> nodosFrontera = new Queue<Node>();
        Node currentNode = null;
        nodosFrontera.Enqueue(nodoInicial);
        while (nodosFrontera.Count != 0)
        {
            currentNode = nodosFrontera.Dequeue();
            if (currentNode.data.type == Square.PRICESS)
            {
                return currentNode;
            }
            else
            {
                nodosVisitados.Add(currentNode);
                nodosExpandidos++;
                for (int i = Square.ABAJO; i <= Square.ARRIBA; i++)
                {
                    Node son = Move(currentNode, i);
                    if (son != null && !nodosFrontera.Contains(son) && !nodosVisitados.Contains(son))
                    {
                        nodosFrontera.Enqueue(son);
                    }
                }
            }
        }
        return currentNode;
    }

    public Node BusquedaPorProfundidad()
    {
        Stack<Node> nodosFrontera = new Stack<Node>();
        Node currentNode = null;
        nodosFrontera.Push(nodoInicial);
        while (nodosFrontera.Count != 0)
        {
            currentNode = nodosFrontera.Pop();
            if (currentNode.data.type == Square.PRICESS)
            {
                return currentNode;
            }
            else
            {
                nodosVisitados.Add(currentNode);
                nodosExpandidos++;
                for (int i = Square.ABAJO; i <= Square.ARRIBA; i++)
                {
                    Node son = Move(currentNode, i);
                    if (son != null && !nodosFrontera.Contains(son) && !nodosVisitados.Contains(son))
                    {
                        nodosFrontera.Push(son);
                    }
                }
            }
        }
        return currentNode;
    }


    public Node BusquedaPorCosteUniforme()
    {
        List<Node> nodosFrontera = new List<Node>();
        Node currentNode = null;
        nodosFrontera.Add(nodoInicial);
        while (nodosFrontera.Count != 0)
        {
            currentNode = getLestCost(nodosFrontera);
            nodosFrontera.Remove(currentNode);
            if (currentNode.data.type == Square.PRICESS)
            {
                return currentNode;
            }
            else
            {
                nodosVisitados.Add(currentNode);
                nodosExpandidos++;
                for (int i = Square.ABAJO; i <= Square.ARRIBA; i++)
                {
                    Node son = Move(currentNode, i);
                    if (son != null && !nodosVisitados.Contains(son))
                    {
                        if (nodosFrontera.Contains(son))
                        {
                            int index = nodosFrontera.IndexOf(son);

                            if (nodosFrontera[index].totalCost > son.totalCost)
                            {
                                nodosFrontera[index] = son;
                            }

                        }
                        else
                        {
                            nodosFrontera.Add(son);
                        }

                    }
                }
            }
        }
        return currentNode;
    }


    public Node AStar()
    {
        List<Node> nodosFrontera = new List<Node>();
        Node currentNode = null;
        nodosFrontera.Add(nodoInicial);
        while (nodosFrontera.Count != 0)
        {
            currentNode = getLestCostAStar(nodosFrontera);
            nodosFrontera.Remove(currentNode);
            if (currentNode.data.type == Square.PRICESS)
            {
                return currentNode;
            }
            else
            {
                nodosExpandidos++;
                nodosVisitados.Add(currentNode);
                for (int i = Square.ABAJO; i <= Square.ARRIBA; i++)
                {
                    Node son = Move(currentNode, i);
                    if (son != null && !nodosVisitados.Contains(son))
                    {
                        if (nodosFrontera.Contains(son))
                        {
                            int index = nodosFrontera.IndexOf(son);
                            if (nodosFrontera[index].getTotalCostWithGoal(casillaFinal) > son.getTotalCostWithGoal(casillaFinal))
                            {
                                nodosFrontera[index] = son;
                            }

                        }
                        else
                        {
                            nodosFrontera.Add(son);
                        }

                    }
                }
            }
        }
        return currentNode;
    }


    public Node BusquedaAvara()
    {
        List<Node> nodosFrontera = new List<Node>();
        Node currentNode = null;
        nodosFrontera.Add(nodoInicial);
        while (nodosFrontera.Count != 0)
        {
            currentNode = getLestCostAvara(nodosFrontera);
            nodosFrontera.Remove(currentNode);
            if (currentNode.data.type == Square.PRICESS)
            {
                return currentNode;
            }
            else
            {
                nodosExpandidos++;
                nodosVisitados.Add(currentNode);
                for (int i = Square.ABAJO; i <= Square.ARRIBA; i++)
                {
                    Node son = Move(currentNode, i);
                    if (son != null && !nodosVisitados.Contains(son))
                    {
                        if (nodosFrontera.Contains(son))
                        {
                            int index = nodosFrontera.IndexOf(son);
                            if (nodosFrontera[index].calculateDistance(nodosFrontera[index].data,casillaFinal) > son.calculateDistance(son.data, casillaFinal))
                            {
                                nodosFrontera[index] = son;
                            }

                        }
                        else
                        {
                            nodosFrontera.Add(son);
                        }

                    }
                }
            }
        }
        return currentNode;
    }

    private Node getLestCost(List<Node> nodosFrontera)
    {
        Node result = null;
        int currentCost = 10000000;
        foreach (Node node in nodosFrontera)
        {
            if (node.totalCost < currentCost)
            {
                currentCost = node.totalCost;
                result = node;
            }
        }
        return result;
    }

    private Node getLestCostAStar(List<Node> nodosFrontera)
    {
        Node result = null;
        float currentCost = 10000000;
        foreach (Node node in nodosFrontera)
        {
            float tmpCost = node.getTotalCostWithGoal(casillaFinal);
            if (tmpCost < currentCost)
            {
                currentCost = tmpCost;
                result = node;
            }
        }
        return result;
    }

    private Node getLestCostAvara(List<Node> nodosFrontera)
    {
        Node result = null;
        float currentCost = 10000000;
        foreach (Node node in nodosFrontera)
        {
            float tmpCost = node.calculateDistance(node.data,casillaFinal);
            if (tmpCost < currentCost)
            {
                currentCost = tmpCost;
                result = node;
            }
        }
        return result;
    }

    Node Move(Node currentNode, int direccion)
    {
        int posX = currentNode.data.posX;
        int posY = currentNode.data.posY;
        int action = -1;
        switch (direccion)
        {
            case Square.IZQUIERDA:
                action = direccion;
                posX--;
                break;
            case Square.DERECHA:
                action = direccion;
                posX++;
                break;
            case Square.ABAJO:
                action = direccion;
                posY++;
                break;
            case Square.ARRIBA:
                action = direccion;
                posY--;
                break;
            default:
                break;
        }

        if (posX >= 0 && posX < mapRepresentation.GetLength(0) && posY >= 0 && posY < mapRepresentation.GetLength(1) && !(mapRepresentation[posX, posY] == Square.WALL))
        {
            Square newSquare = new Square(posX, posY, mapRepresentation[posX, posY]);
            Node moveNode = new Node(newSquare, currentNode);
            moveNode.action = action;
            return moveNode;
        }
        else
        {
            return null;
        }
    }
}


