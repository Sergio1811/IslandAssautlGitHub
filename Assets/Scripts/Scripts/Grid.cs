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

    private void Awake()
    {
        //GenerateGrid();
    }

    private void Update()
    {

    }

    public void GenerateGrid()
    {
        grid = new Node[(int)size_x, (int)size_y];

        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                Vector3 nodePosition = new Vector3(node_size * 0.5f + i * node_size - size_x/2 *node_size, 0, node_size * 0.5f + j * node_size - size_y / 2 * node_size);
                Vector3 worldNodePosition = transform.position + nodePosition;

                Collider[] colliders = Physics.OverlapSphere(worldNodePosition, node_size * 0.2f);

                Node.Type objectType = Node.Type.water;
                bool isTransitable = false;

                for (int k = 0; k < colliders.Length; k++)
                {
                    if (colliders[k].tag == "Island")
                    {
                        objectType = Node.Type.floor;
                        isTransitable = true;
                    }
                    if (colliders[k].tag == "Tree")
                    {
                        objectType = Node.Type.tree;
                        isTransitable = true;
                    }
                    if (colliders[k].tag == "Rock")
                    {
                        objectType = Node.Type.rock;
                        isTransitable = true;
                    }
                    if (colliders[k].tag == "Enemy")
                    {
                        objectType = Node.Type.enemy;
                        isTransitable = true;
                    }
                    if (colliders[k].tag == "Decoration")
                    {
                        objectType = Node.Type.decoration;
                        isTransitable = true;
                    }
                    if (colliders[k].tag == "Village")
                    {
                        objectType = Node.Type.village;
                        isTransitable = true;
                    }
                    if (colliders[k].tag == "Shore")
                    {
                        objectType = Node.Type.shore;
                        isTransitable = true;
                    }
                    if (colliders[k].tag == "Entry")
                    {
                        objectType = Node.Type.entry;
                        isTransitable = true;
                    }
                    if (colliders[k].tag == "Exit")
                    {
                        objectType = Node.Type.exit;
                        isTransitable = true;
                    }
                }

                grid[i, j] = new Node(i, j, node_size, worldNodePosition, objectType, isTransitable);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (grid != null)
        {

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    switch(grid[i,j].currentType)
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
}



public class Node
{
    public enum Type { water, rock, tree, village, enemy, decoration, shore, floor, entry, exit}
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

