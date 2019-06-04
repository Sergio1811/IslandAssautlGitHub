using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid instance { set; get; }
    public int size_x;
    public int size_y;
    public float node_size;
    [HideInInspector] public Node[,] grid;

    public int minPrimaryPers, maxPrimaryPers;
    public int minSecundaryPers, maxSecundaryPers;
    int numberOfFloor;

    Node entryNode;

    public ObjectsInstantiation instantiationScript;
    [HideInInspector] public Transform islandParent;


    public void GenerateGrid(int characterType)
    {
        instance = this;
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
                randomNumber = Random.Range(minPrimaryPers * numberOfFloor / 100, maxPrimaryPers * numberOfFloor / 100);
                AssaignTrees(randomNumber);

                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignRocks(randomNumber);

                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignVillages(randomNumber, false);

                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignDecoration(randomNumber);
                break;

            case 1:
                randomNumber = Random.Range(minPrimaryPers * numberOfFloor / 100, maxPrimaryPers * numberOfFloor / 100);
                AssaignRocks(randomNumber);

                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignVillages(randomNumber, false);

                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignTrees(randomNumber);

                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignDecoration(randomNumber);
                break;

            case 2:
                randomNumber = Random.Range(minPrimaryPers * numberOfFloor / 100, maxPrimaryPers * numberOfFloor / 100);
                AssaignVillages(randomNumber, true);

                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignRocks(randomNumber);

                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignTrees(randomNumber);

                randomNumber = Random.Range(minSecundaryPers * numberOfFloor / 100, maxSecundaryPers * numberOfFloor / 100);
                AssaignDecoration(randomNumber);
                break;
        }

        instantiationScript.islandParent = islandParent;
        instantiationScript.InstantiateObjectInGrid();
    }



    //MËTODOS DE ASIGNACIÓN DE TIPOS

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
                    if (colliders[k].tag != "" && colliders[k].tag != "IslandCollision")
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

    //Método que escoge un nodo como entrada
    void AssaignEnter()
    {
        List<Node> shoreNodes = AvailableNodesType(Node.Type.shore, 1, 1, Node.Type.shore);
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

        entryNode = availableShoreNode[Random.Range(0, availableShoreNode.Count)];
        entryNode.currentType = Node.Type.entry;
    }
    //Método que escoge un nodo como salida
    void AssaignExit()
    {
        List<Node> shoreNodes = AvailableNodesType(Node.Type.shore, 1, 1, Node.Type.shore);
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
        int smallRocks = Random.Range(10 * cellsNumber / 100, 20 * cellsNumber / 100);
        cellsNumber -= smallRocks;
        int rocks1x2 = Random.Range(0, 15 * cellsNumber / 100);
        cellsNumber -= rocks1x2;
        int rocks2x1 = Random.Range(0, 15 * cellsNumber / 100);
        cellsNumber -= rocks2x1;

        int bigRocks = cellsNumber / 4;
        cellsNumber -= bigRocks * 4;
        smallRocks += cellsNumber;
        
        if (GameManager.Instance.rockTier2)
        {
            int smallRocks_T2 = Random.Range(30 * smallRocks / 100, 40 * smallRocks / 100);
            smallRocks -= smallRocks_T2;

            int rocks1x2_T2 = Random.Range(30 * rocks1x2 / 100, 40 * rocks1x2 / 100);
            rocks1x2 -= rocks1x2_T2;

            int rocks2x1_T2 = Random.Range(30 * rocks2x1 / 100, 40 * rocks2x1 / 100);
            rocks2x1 -= rocks2x1_T2;

            int bigRocks_T2 = Random.Range(30 * bigRocks / 100, 40 * bigRocks / 100);
            bigRocks -= bigRocks_T2;

            ChangeNodesAvailables(bigRocks_T2, Node.Type.floor, Node.Type.rock2, Node.Size.s2x2, 2, 2);
            ChangeNodesAvailables(rocks1x2_T2, Node.Type.floor, Node.Type.rock2, Node.Size.s1x2, 1, 2);
            ChangeNodesAvailables(rocks2x1_T2, Node.Type.floor, Node.Type.rock2, Node.Size.s2x1, 2, 1);
            ChangeNodesAvailables(smallRocks_T2, Node.Type.floor, Node.Type.rock2, Node.Size.s1x1, 1, 1);
        }

        ChangeNodesAvailables(bigRocks, Node.Type.floor, Node.Type.rock, Node.Size.s2x2, 2, 2);
        ChangeNodesAvailables(rocks1x2, Node.Type.floor, Node.Type.rock, Node.Size.s1x2, 1, 2);
        ChangeNodesAvailables(rocks2x1, Node.Type.floor, Node.Type.rock, Node.Size.s2x1, 2, 1);
        ChangeNodesAvailables(smallRocks, Node.Type.floor, Node.Type.rock, Node.Size.s1x1, 1, 1);
    }
    //Método que a partir del número de celas de árboles que se quieran crear va a cambiar los nodos a tipo árbol
    void AssaignTrees(int cellsNumber)
    {
        int smallTrees = Random.Range(10 * cellsNumber / 100, 20 * cellsNumber / 100);
        cellsNumber -= smallTrees;
        int trees1x2 = Random.Range(0, 15 * cellsNumber / 100);
        cellsNumber -= trees1x2;
        int trees2x1 = Random.Range(0, 15 * cellsNumber / 100);
        cellsNumber -= trees2x1;

        int bigTrees = cellsNumber / 4;
        cellsNumber -= bigTrees * 4;
        smallTrees += cellsNumber;

        if (GameManager.Instance.treeTier2)
        {
            int smallTrees_T2 = Random.Range(30 * smallTrees / 100, 40 * smallTrees / 100);
            smallTrees -= smallTrees_T2;

            int trees1x2_T2 = Random.Range(30 * trees1x2 / 100, 40 * trees1x2 / 100);
            trees1x2 -= trees1x2_T2;

            int trees2x1_T2 = Random.Range(30 * trees2x1 / 100, 40 * trees2x1 / 100);
            trees2x1 -= trees2x1_T2;

            int bigTrees_T2 = Random.Range(30 * bigTrees / 100, 40 * bigTrees / 100);
            bigTrees -= bigTrees_T2;

            ChangeNodesAvailables(bigTrees_T2, Node.Type.floor, Node.Type.tree2, Node.Size.s2x2, 2, 2);
            ChangeNodesAvailables(trees1x2_T2, Node.Type.floor, Node.Type.tree2, Node.Size.s1x2, 1, 2);
            ChangeNodesAvailables(trees2x1, Node.Type.floor, Node.Type.tree2, Node.Size.s2x1, 2, 1);
            ChangeNodesAvailables(smallTrees_T2, Node.Type.floor, Node.Type.tree2, Node.Size.s1x1, 1, 1);
        }

        ChangeNodesAvailables(bigTrees, Node.Type.floor, Node.Type.tree, Node.Size.s2x2, 2, 2);
        ChangeNodesAvailables(trees1x2, Node.Type.floor, Node.Type.tree, Node.Size.s1x2, 1, 2);
        ChangeNodesAvailables(trees2x1, Node.Type.floor, Node.Type.tree, Node.Size.s2x1, 2, 1);
        ChangeNodesAvailables(smallTrees, Node.Type.floor, Node.Type.tree, Node.Size.s1x1, 1, 1);
    }
    //Método que a partir del número de celas de aldea que se quieran crear va a cambiar los nodos a tipo aldea
    void AssaignVillages(int cellsNumber, bool isSworder)
    {
        int smallVillages = Random.Range(10 * (cellsNumber / 4) / 100, 20 * (cellsNumber / 4) / 100);
        cellsNumber -= smallVillages * 4;
        
        int mediumVillages = Random.Range(30 * (cellsNumber / 9) / 100,  50 * (cellsNumber / 9) / 100);
        cellsNumber -= mediumVillages * 9;

        int bigVillages = cellsNumber / 16;
        cellsNumber -= bigVillages * 16;

        if (cellsNumber > 4)
            smallVillages += 1;

        if (GameManager.Instance.enemyTier2)
        {
            int smallVillages_T2 = Random.Range(50 * smallVillages / 100, 70 * smallVillages / 100);
            smallVillages -= smallVillages_T2;

            int mediumVillages_T2 = Random.Range(50 * mediumVillages / 100, 70 * mediumVillages / 100);
            mediumVillages -= mediumVillages_T2;

            int bigVillages_T2 = Random.Range(50 * bigVillages / 100, 70 * bigVillages / 100);
            bigVillages -= bigVillages_T2;

            ChangeNodesAvailables(bigVillages_T2, Node.Type.floor, Node.Type.village2, Node.Size.s4x4, 4, 4);
            ChangeNodesAvailables(mediumVillages_T2, Node.Type.floor, Node.Type.village2, Node.Size.s3x3, 3, 3);
            ChangeNodesAvailables(smallVillages_T2, Node.Type.floor, Node.Type.village2, Node.Size.s2x2, 2, 2);

            if (isSworder)
                ChangeNodesAvailables(bigVillages_T2 + mediumVillages_T2 + smallVillages_T2, Node.Type.floor, Node.Type.enemy2, Node.Size.s1x1, 1, 1);
        }

        ChangeNodesAvailables(bigVillages, Node.Type.floor, Node.Type.village, Node.Size.s4x4, 4, 4);
        ChangeNodesAvailables(mediumVillages, Node.Type.floor, Node.Type.village, Node.Size.s3x3, 3, 3);
        ChangeNodesAvailables(smallVillages, Node.Type.floor, Node.Type.village, Node.Size.s2x2, 2, 2);

        if (isSworder && !GameManager.Instance.enemyTier2)
            ChangeNodesAvailables(bigVillages + mediumVillages + smallVillages, Node.Type.floor, Node.Type.enemy, Node.Size.s1x1, 1, 1);
    }

    //Método que a partir del número de celas de decoración crea nodos tipo decoración con distintas formas como posibilidad
    void AssaignDecoration(int cellsNumber)
    {
        int decoration1x1 = 0, decoration1x2 = 0, decoration2x1 = 0, decoration1x3 = 0, decoration3x1 = 0, decoration1x4 = 0, decoration4x1 = 0;
        int decoration2x2 = 0, decoration2x3 = 0, decoration3x2 = 0, decoration2x4 = 0, decoration4x2 = 0;

        while (cellsNumber > 0)
        {
            int randomMax = 12;
            if (cellsNumber < 2)
                randomMax = 0;
            else if (cellsNumber < 3)
                randomMax = 3;
            else if (cellsNumber < 4)
                randomMax = 5;
            else if (cellsNumber < 6)
                randomMax = 8;
            else if (cellsNumber < 8)
                randomMax = 10;

            switch (Random.Range(0, randomMax))
            {
                case 0:
                    decoration1x1++;
                    cellsNumber -= 1;
                    break;
                case 1:
                    decoration1x2++;
                    cellsNumber -= 2;
                    break;
                case 2:
                    decoration2x1++;
                    cellsNumber -= 2;
                    break;
                case 3:
                    decoration1x3++;
                    cellsNumber -= 3;
                    break;
                case 4:
                    decoration3x1++;
                    cellsNumber -= 3;
                    break;
                case 5:
                    decoration4x1++;
                    cellsNumber -= 4;
                    break;
                case 6:
                    decoration2x2++;
                    cellsNumber -= 4;
                    break;
                case 7:
                    decoration1x4++;
                    cellsNumber -= 4;
                    break;
                case 8:
                    decoration3x2++;
                    cellsNumber -= 6;
                    break;
                case 9:
                    decoration2x3++;
                    cellsNumber -= 6;
                    break;
                case 10:
                    decoration2x4++;
                    cellsNumber -= 8;
                    break;
                case 11:
                    decoration4x2++;
                    cellsNumber -= 8;
                    break;
            }
        }

        ChangeNodesAvailables(decoration2x4, Node.Type.floor, Node.Type.decoration, Node.Size.s2x4, 2, 4);
        ChangeNodesAvailables(decoration2x3, Node.Type.floor, Node.Type.decoration, Node.Size.s2x3, 2, 3);
        ChangeNodesAvailables(decoration2x2, Node.Type.floor, Node.Type.decoration, Node.Size.s2x2, 2, 2);
        ChangeNodesAvailables(decoration1x4, Node.Type.floor, Node.Type.decoration, Node.Size.s1x4, 1, 4);
        ChangeNodesAvailables(decoration1x3, Node.Type.floor, Node.Type.decoration, Node.Size.s1x3, 1, 3);
        ChangeNodesAvailables(decoration1x2, Node.Type.floor, Node.Type.decoration, Node.Size.s1x2, 1, 2);
        ChangeNodesAvailables(decoration1x1, Node.Type.floor, Node.Type.decoration, Node.Size.s1x1, 1, 1);
        ChangeNodesAvailables(decoration3x2, Node.Type.floor, Node.Type.decoration, Node.Size.s3x2, 3, 2);
        ChangeNodesAvailables(decoration4x2, Node.Type.floor, Node.Type.decoration, Node.Size.s4x2, 4, 2);
        ChangeNodesAvailables(decoration2x1, Node.Type.floor, Node.Type.decoration, Node.Size.s2x1, 2, 1);
        ChangeNodesAvailables(decoration3x1, Node.Type.floor, Node.Type.decoration, Node.Size.s3x1, 3, 1);
        ChangeNodesAvailables(decoration4x1, Node.Type.floor, Node.Type.decoration, Node.Size.s4x1, 4, 1);
    }



    //MÉTODOS PARA SIMPLIFICAR PROCESOS DE NODOS

    //Método que cambia el tipo y el tamaño de un nodo
    void ChangeNodeTypeAndSize(Node nodeToChange, Node.Type typeOfNode, Node.Size sizeOfNode, int sizeX, int sizeY)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                grid[nodeToChange.gridPositionX - i, nodeToChange.gridPositionY - j].currentType = typeOfNode;
                grid[nodeToChange.gridPositionX - i, nodeToChange.gridPositionY - j].currentSize = sizeOfNode;
            }
        }
    }

    //Método que a partir de un número busca si se puede colocar ese objeto y lo coloca tantas veces como número
    void ChangeNodesAvailables(int number, Node.Type nodeAvailableType, Node.Type nodeType, Node.Size nodeSize, int sizeX, int sizeY)
    {
        List<Node> availableNodes;

        for (int i = number; i > 0; i--)
        {
            availableNodes = AvailableNodesType(nodeAvailableType, sizeX, sizeY, nodeType);

            if (availableNodes.Count >= number)
            {
                Node selectedNode = availableNodes[Random.Range(0, availableNodes.Count)];
                ChangeNodeTypeAndSize(selectedNode, nodeType, nodeSize, sizeX, sizeY);
                //numberOfFloor -= sizeX * sizeY;
                number--;
                availableNodes.Clear();
            }
            else
                break;
        }
    }

    //Método que crea una lista de todos los nodos de un tipo determinado con una medida concreta (sizeX, sizeY)
    public List<Node> AvailableNodesType(Node.Type nodeType, int sizeX, int sizeY, Node.Type futureNodeType)
    {
        List<Node> availableNodes = new List<Node>();
        List<Node> neighbourNodes = new List<Node>();

        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                if (grid[i, j].currentType == nodeType)
                {
                    bool isAvailable = true;
                    if (futureNodeType != Node.Type.village && futureNodeType != Node.Type.enemy)
                        neighbourNodes = GetNeighboursBySize(grid[i, j], sizeX, sizeY);
                    else
                        neighbourNodes = GetNeighboursBySizeFarFromOriginNode(grid[i, j], sizeX, sizeY, entryNode, 3, 3);

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
                        availableNodes.Add(grid[i, j]);
                }
            }
        }
        return availableNodes;
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
            //Debug.LogWarning("Invalid Node" + x + ", " + y);
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

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
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

    public List<Node> GetNeighboursBySizeFarFromOriginNode(Node node, int x, int y, Node originNode, int howFarY, int howFarX)
    {
        List<Node> listaNodos = new List<Node>();

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if ((i == 0 && j == 0) && !(x == 1 && y == 1))
                    continue;

                Node vecino = GetNode(node.gridPositionX - i, node.gridPositionY - j);

                if (originNode != null)
                {
                    if ((node.gridPositionY - j <= originNode.gridPositionY + howFarY))
                    {
                        if (node.gridPositionX - i > originNode.gridPositionX + howFarX)
                            listaNodos.Add(vecino);
                        else
                            listaNodos.Add(null);
                    }
                    else
                        listaNodos.Add(vecino);
                }
                else if (vecino != null)
                    listaNodos.Add(vecino);
                else
                    listaNodos.Add(null);
            }
        }

        return listaNodos;
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
}



public class Node
{
    public enum Type { water, rock, rock2, tree, tree2, village, village2, enemy, enemy2, decoration, shore, floor, entry, exit }
    public Type currentType = Type.water;
    public bool isTransitable = false;
    public enum Size { s1x1, s1x2, s1x3, s1x4, s2x1, s2x2, s2x3, s2x4, s3x1, s3x2, s3x3, s4x1, s4x2, s4x4 };
    public Size currentSize = Size.s1x1;

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