using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public int size_x;
    public int size_y;
    public float node_size;
    [HideInInspector]
    public Node[,] grid;

    public int minPrimaryPers, maxPrimaryPers;
    public int minSecundaryPers, maxSecundaryPers;
    int numberOfFloor;



    public void GenerateGrid(int characterType)
    {
        grid = new Node[(int)size_x, (int)size_y];
        numberOfFloor = 0;

        AssaignWaterAndFloor();
        AssaignShore();
        AssaignEnter();
        AssaignExit();

        int randomNumber;
        switch (characterType)
        {
            case 0:
                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignRocks(randomNumber);
                break;

            case 1:
                randomNumber = Random.Range(minPrimaryPers * numberOfFloor / 100, maxPrimaryPers * numberOfFloor / 100);
                print("Number of floor nodes: " + numberOfFloor);
                print("Minimum rock nodes: " + minPrimaryPers * numberOfFloor / 100 + "       Max rock nodes: " + maxPrimaryPers * numberOfFloor / 100);
                print("Number of rock nodes: " + randomNumber);
                AssaignRocks(randomNumber);
                break;

            case 2:
                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignRocks(randomNumber);
                break;
        }
    }




    //Método que crea la grid a partir de la size dada y además si detecta un collider en ese nodo le cambia le tipo a suelo
    void AssaignWaterAndFloor()
    {
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                Vector3 nodePosition = new Vector3(node_size * 0.5f + i * node_size - size_x / 2 * node_size, 0, node_size * 0.5f + j * node_size - size_y / 2 * node_size);
                Vector3 worldNodePosition = transform.position + nodePosition;

                Collider[] colliders = Physics.OverlapSphere(worldNodePosition, node_size * 0.2f);

                bool isTransitable = false;
                Node.Type objectType = Node.Type.water;

                for (int k = 0; k < colliders.Length; k++)
                {
                    if (colliders[k].tag != "")
                    {
                        objectType = Node.Type.floor;
                        isTransitable = true;
                        numberOfFloor++;
                        break;
                    }
                }

                grid[i, j] = new Node(i, j, node_size, worldNodePosition, objectType, isTransitable);
            }
        }
    }

    //Método que cambia a tipo shore los nodos que forman parte de la orilla
    void AssaignShore()
    {
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                if (grid[i, j].currentType == Node.Type.floor)
                {
                    List<Node> neighbourNodes = GetNeighbours(grid[i, j], true);

                    for (int k = 0; k < neighbourNodes.Count; k++)
                    {
                        if (neighbourNodes[k].currentType == Node.Type.water)
                        {
                            grid[i, j].currentType = Node.Type.shore;
                            numberOfFloor--;
                            break;
                        }
                    }
                }
            }
        }
    }

    void AssaignEnter()
    {
        List<Node> shoreNodes = AvailableNodesType(Node.Type.shore, 1, 1);
        List<Node> availableShoreNode = new List<Node>();
        int minYNodes = size_y;

        for (int i = 0; i < shoreNodes.Count; i++)
        {
            if (minYNodes > shoreNodes[i].gridPositionY)
                minYNodes = shoreNodes[i].gridPositionY;
        }
        for (int i = 0; i < shoreNodes.Count; i++)
        {
            if (minYNodes == shoreNodes[i].gridPositionY)
                availableShoreNode.Add(shoreNodes[i]);
        }

        availableShoreNode[Random.Range(0, availableShoreNode.Count)].currentType = Node.Type.entry;
    }
    void AssaignExit()
    {
        List<Node> shoreNodes = AvailableNodesType(Node.Type.shore, 1, 1);
        List<Node> availableShoreNode = new List<Node>();
        int maxYNodes = 0;

        for (int i = 0; i < shoreNodes.Count; i++)
        {
            if (maxYNodes < shoreNodes[i].gridPositionY)
                maxYNodes = shoreNodes[i].gridPositionY;
        }
        for (int i = 0; i < shoreNodes.Count; i++)
        {
            if (maxYNodes == shoreNodes[i].gridPositionY)
                availableShoreNode.Add(shoreNodes[i]);
        }

        availableShoreNode[Random.Range(0, availableShoreNode.Count)].currentType = Node.Type.exit;
    }


    //Método que a partir del número de celas de rocas que se quieran crear va a cambiar los nodos a tipo roca
    void AssaignRocks(int cellsNumber)
    {
        int bigRocks = cellsNumber / 4;
        print("Number of big rocks (2x2): " + bigRocks);
        List<Node> availableNodes;

        for (int i = bigRocks; i > 0; i--)
        {
            availableNodes = AvailableNodesType(Node.Type.floor, 2, 2);

            if (availableNodes.Count >= bigRocks)
            {
                Node selectedNode = availableNodes[Random.Range(0, availableNodes.Count)];
                selectedNode.currentType = Node.Type.rock;
                grid[selectedNode.gridPositionX - 1, selectedNode.gridPositionY].currentType = Node.Type.rock;
                grid[selectedNode.gridPositionX, selectedNode.gridPositionY - 1].currentType = Node.Type.rock;
                grid[selectedNode.gridPositionX - 1, selectedNode.gridPositionY - 1].currentType = Node.Type.rock;
                numberOfFloor -= 4;
                bigRocks--;
                availableNodes.Clear();
            }
            else
                break;
        }
    }


    //Método que crea una lista de todos los nodos de un tipo determinado con una medida concreta (sizeX, sizeY)
    public List<Node> AvailableNodesType(Node.Type nodeType, int sizeX, int sizeY)
    {
        List<Node> availableNodes = new List<Node>();

        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                if (grid[i, j].currentType == nodeType)
                {
                    bool isAvailable = true;
                    List<Node> neighbourNodes = GetNeighboursBySize(grid[i, j], sizeX, sizeY);

                    for (int k = 0; k < neighbourNodes.Count; k++)
                    {
                        if (neighbourNodes[k] != null)
                        {
                            if (neighbourNodes[k].currentType != nodeType)
                            {
                                isAvailable = false;
                                break;
                            }
                        }
                        else
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        availableNodes.Add(grid[i, j]);
                    }
                }
            }
        }

        return availableNodes;
    }
    
    
    //Método que según el tag del collider que detecte cambiara el nodo a un tipo u otro -> Este método ya no lo usamos
    void AssaignByCollidersTag()
    {
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                Vector3 nodePosition = new Vector3(node_size * 0.5f + i * node_size - size_x / 2 * node_size, 0, node_size * 0.5f + j * node_size - size_y / 2 * node_size);
                Vector3 worldNodePosition = transform.position + nodePosition;

                Collider[] colliders = Physics.OverlapSphere(worldNodePosition, node_size * 0.2f);

                bool isTransitable = false;
                Node.Type objectType = Node.Type.water;

                for (int k = 0; k < colliders.Length; k++)
                {
                    if (colliders[k].tag == "Island")
                    {
                        objectType = Node.Type.floor;
                        isTransitable = true;
                    }
                    else if (colliders[k].tag == "Tree")
                    {
                        objectType = Node.Type.tree;
                        isTransitable = true;
                    }
                    else if (colliders[k].tag == "Rock")
                    {
                        objectType = Node.Type.rock;
                        isTransitable = true;
                    }
                    else if (colliders[k].tag == "Enemy")
                    {
                        objectType = Node.Type.enemy;
                        isTransitable = true;
                    }
                    else if (colliders[k].tag == "Decoration")
                    {
                        objectType = Node.Type.decoration;
                        isTransitable = true;
                    }
                    else if (colliders[k].tag == "Village")
                    {
                        objectType = Node.Type.village;
                        isTransitable = true;
                    }
                    else if (colliders[k].tag == "Shore")
                    {
                        objectType = Node.Type.shore;
                        isTransitable = true;
                    }
                    else if (colliders[k].tag == "Entry")
                    {
                        objectType = Node.Type.entry;
                        isTransitable = true;
                    }
                    else if (colliders[k].tag == "Exit")
                    {
                        objectType = Node.Type.exit;
                        isTransitable = true;
                    }
                }

                grid[i, j] = new Node(i, j, node_size, worldNodePosition, objectType, isTransitable);
            }
        }
    }


    //Método para dibujar la grid
    private void OnDrawGizmosSelected()
    {
        if (grid != null)
        {

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    switch (grid[i, j].currentType)
                    {
                        case (Node.Type.water):
                            Gizmos.color = new Color(0.2666667f, 0.57f, 0.9254902f, 0.5f);
                            break;
                        case (Node.Type.shore):
                            Gizmos.color = new Color(0.9254902f, 0.910089f, 0.26666f, 0.5f);
                            break;
                        case (Node.Type.floor):
                            Gizmos.color = new Color(0.9254902f, 0.7990713f, 0.26666f, 0.5f);
                            break;
                        case (Node.Type.tree):
                            Gizmos.color = new Color(0.09024564f, 0.490566f, 0.1355f, 0.5f);
                            break;
                        case (Node.Type.rock):
                            Gizmos.color = new Color(0.6415094f, 0.6415094f, 0.6415094f, 0.5f);
                            break;
                        case (Node.Type.enemy):
                            Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
                            break;
                        case (Node.Type.decoration):
                            Gizmos.color = new Color(1f, 0f, 0.5643387f, 0.5f);
                            break;
                        case (Node.Type.village):
                            Gizmos.color = new Color(0.7991002f, 0.4791296f, 0.9150943f, 0.5f);
                            break;
                        case (Node.Type.entry):
                            Gizmos.color = new Color(0.2666667f, 0.9254902f, 0.3395f, 0.5f);
                            break;
                        case (Node.Type.exit):
                            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                            break;

                    }

                    Vector3 scale = new Vector3(node_size, node_size, node_size);

                    Gizmos.DrawCube(grid[i, j].worldPosition, scale);
                }
            }
        }

    }



    public Node GetNode(int x, int y)
    {
        if (x < 0 || y < 0 || x >= size_x || y >= size_y)
        {
            Debug.LogWarning("Invalid Node" + x + ", " + y);
            return null;
        }
        return grid[x, y];
    }


    public Node GetNodeContainingPosition(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - transform.position;
        int x = Mathf.FloorToInt(localPosition.x / node_size);
        int y = Mathf.FloorToInt(localPosition.y / node_size);


        if (x < size_x && x >= 0 && y < size_y && y >= 0)
        {
            return grid[x, y];
        }

        return null;
    }



    //Método que devuelve una lista de los nodos que rodean al nodo enviado, extenden = true para que envie también los que están en diagonal.
    public List<Node> GetNeighbours(Node node, bool extended)
    {
        List<Node> listaNodos = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {

                if (!extended)
                {
                    if (Mathf.Abs(i) == Mathf.Abs(j))
                        continue;
                }
                else
                {
                    if (i == 0 && j == 0)
                        continue;
                }


                Node vecino = GetNode(node.gridPositionX + i, node.gridPositionY + j);


                if (vecino != null)
                {
                    listaNodos.Add(vecino);
                }
            }
        }

        return listaNodos;
    }

    //Método que devuelve una lista con los nodos que hay a distancia x e y, teniendo como referencia el nodo de abajo a la derecha
    public List<Node> GetNeighboursBySize(Node node, int x, int y)
    {
        List<Node> listaNodos = new List<Node>();
        x--;
        y--;

        for (int i = 0; i <= x; i++)
        {
            for (int j = 0; j <= y; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                Node vecino = GetNode(node.gridPositionX - i, node.gridPositionY - j);


                if (vecino != null)
                    listaNodos.Add(vecino);
                else
                    listaNodos.Add(null);
            }
        }

        return listaNodos;
    }


}



public class Node
{
    public enum Type { water, rock, tree, village, enemy, decoration, shore, floor, entry, exit }
    public Type currentType = Type.water;
    public bool isTransitable = false;

    public int gridPositionX;
    public int gridPositionY;
    public float size;
    public Vector3 worldPosition;

    public Node() { }

    public Node(int _gridPositionX, int _gridPositionY, float _size, Vector3 _worldPosition, Type _currentType, bool _isTransitable)
    {
        gridPositionX = _gridPositionX;
        gridPositionY = _gridPositionY;
        size = _size;
        worldPosition = _worldPosition;
        currentType = _currentType;
        isTransitable = _isTransitable;
    }
}

