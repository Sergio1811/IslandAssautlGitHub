using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInstantiation : MonoBehaviour
{
    public Grid gridScript;
    [HideInInspector] public Transform islandParent;

    public GameObject tree1x1, tree2x2, tree1x2, enemy, rock1x1, rock2x2, rock1x2, village2x2, village3x3, village4x4, portal, water;
    public GameObject tree1x1_T2, tree2x2_T2, tree1x2_T2, enemy_T2, rock1x1_T2, rock2x2_T2, rock1x2_T2, village2x2_T2, village3x3_T2, village4x4_T2;
    public GameObject decoration1x1, decoration1x2, decoration1x3, decoration1x4, decoration2x2, decoration2x3, decoration2x4;

    public float instantiationHeight = 100;
    private GameObject[] lights;

    public void InstantiateObjectInGrid()
    {
        GameObject objectInstantiation;

        for (int i = gridScript.grid.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = gridScript.grid.GetLength(1) - 1; j >= 0; j--)
            {
                Node actualNode = gridScript.grid[i, j];


                //ENTRANCE
                if (actualNode.isTransitable && actualNode.currentType == Node.Type.entry)
                {
                    GameManager.Instance.startNode = actualNode;
                    actualNode.isTransitable = false;
                }


                //EXIT
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.exit)
                {
                    GameManager.Instance.endNode = actualNode;
                    objectInstantiation = Instantiate(portal, islandParent);
                    ChangeTransitable(actualNode, false, 1, 1);
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, 10f, actualNode.worldPosition.z);
                    GameManager.Instance.portalExit = objectInstantiation;
                }



                //TREES T1
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.tree)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x1:
                            objectInstantiation = Instantiate(tree1x1, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(tree1x2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(tree1x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(tree2x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                    }
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    objectInstantiation.transform.eulerAngles = new Vector3(Random.Range(-6f, 6f), objectInstantiation.transform.eulerAngles.y, Random.Range(-6f, 6f));
                    GameManager.Instance.woodInMap += GameManager.Instance.woodByItem;
                }

                //TREES T2
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.tree2)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x1:
                            objectInstantiation = Instantiate(tree1x1_T2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(tree1x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(tree1x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(tree2x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                    }
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    objectInstantiation.transform.eulerAngles = new Vector3(Random.Range(-6f, 6f), objectInstantiation.transform.eulerAngles.y, Random.Range(-6f, 6f));
                    GameManager.Instance.wood2InMap += GameManager.Instance.woodByItem;
                }



                //ROCKS T1
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.rock)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x1:
                            objectInstantiation = Instantiate(rock1x1, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(rock1x2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(rock1x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(rock2x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                    }
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    objectInstantiation.transform.eulerAngles = new Vector3(Random.Range(-6f, 6f), objectInstantiation.transform.eulerAngles.y, Random.Range(-6f, 6f));
                    GameManager.Instance.rockInMap += GameManager.Instance.rockByItem;
                }

                //ROCKS T2
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.rock2)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x1:
                            objectInstantiation = Instantiate(rock1x1_T2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(rock1x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(rock1x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(rock2x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                    }
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                    objectInstantiation.transform.eulerAngles = new Vector3(Random.Range(-6f, 6f), objectInstantiation.transform.eulerAngles.y, Random.Range(-6f, 6f));
                    GameManager.Instance.rock2InMap += GameManager.Instance.rockByItem;
                }



                //VILLAGES T1
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.village)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s2x2:
                            objectInstantiation = Instantiate(village2x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            break;
                        case Node.Size.s3x3:
                            objectInstantiation = Instantiate(village3x3, islandParent);
                            ChangeTransitable(actualNode, false, 3, 3);
                            break;
                        default:
                            objectInstantiation = Instantiate(village4x4, islandParent);
                            ChangeTransitable(actualNode, false, 4, 4);
                            break;
                    }
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, 7.75f, actualNode.worldPosition.z);
                    objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
                    GameObject enemiesGroup = objectInstantiation.transform.GetChild(0).GetChild(2).gameObject;
                    enemiesGroup.SetActive(true);
                    if (GameManager.characterNumber == 0)
                    {
                        for (int k = 0; k < enemiesGroup.transform.childCount; k++)
                        {
                            enemiesGroup.transform.GetChild(k).GetComponent<EnemyScript>().lives = 3;
                        }
                    }
                    GameManager.Instance.fabricInMap += GameManager.Instance.enemiesByItem;
                }

                //VILLAGES T2
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.village2)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s2x2:
                            objectInstantiation = Instantiate(village2x2_T2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            break;
                        case Node.Size.s3x3:
                            objectInstantiation = Instantiate(village3x3_T2, islandParent);
                            ChangeTransitable(actualNode, false, 3, 3);
                            break;
                        default:
                            objectInstantiation = Instantiate(village4x4_T2, islandParent);
                            ChangeTransitable(actualNode, false, 4, 4);
                            break;
                    }
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, 7.75f, actualNode.worldPosition.z);
                    objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
                    GameObject enemiesGroup = objectInstantiation.transform.GetChild(0).GetChild(2).gameObject;
                    enemiesGroup.SetActive(true);
                    if (GameManager.characterNumber == 0)
                    {
                        for (int k = 0; k < enemiesGroup.transform.childCount; k++)
                        {
                            enemiesGroup.transform.GetChild(k).GetComponent<EnemyScript>().lives = 3;
                        }
                    }
                    GameManager.Instance.fabric2InMap += GameManager.Instance.enemiesByItem;
                }



                //ENEMIES T1
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.enemy)
                {
                    objectInstantiation = Instantiate(enemy, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    objectInstantiation.SetActive(true);
                    actualNode.isTransitable = false;
                }

                //ENEMIES T2
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.enemy2)
                {
                    objectInstantiation = Instantiate(enemy_T2, islandParent);
                    objectInstantiation.transform.position = actualNode.worldPosition;
                    objectInstantiation.SetActive(true);
                    actualNode.isTransitable = false;
                }


                //DECORATION
                else if (actualNode.isTransitable && actualNode.currentType == Node.Type.decoration)
                {
                    switch (actualNode.currentSize)
                    {
                        case Node.Size.s1x2:
                            objectInstantiation = Instantiate(decoration1x2, islandParent);
                            ChangeTransitable(actualNode, false, 1, 2);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.25f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s2x1:
                            objectInstantiation = Instantiate(decoration1x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        case Node.Size.s1x3:
                            objectInstantiation = Instantiate(decoration1x3, islandParent);
                            ChangeTransitable(actualNode, false, 1, 3);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.5f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s3x1:
                            objectInstantiation = Instantiate(decoration1x3, islandParent);
                            ChangeTransitable(actualNode, false, 3, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        case Node.Size.s1x4:
                            objectInstantiation = Instantiate(decoration1x4, islandParent);
                            ChangeTransitable(actualNode, false, 1, 4);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.75f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s4x1:
                            objectInstantiation = Instantiate(decoration1x4, islandParent);
                            ChangeTransitable(actualNode, false, 4, 1);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        case Node.Size.s2x2:
                            objectInstantiation = Instantiate(decoration2x2, islandParent);
                            ChangeTransitable(actualNode, false, 2, 2);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
                            break;
                        case Node.Size.s2x3:
                            objectInstantiation = Instantiate(decoration2x3, islandParent);
                            ChangeTransitable(actualNode, false, 2, 3);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(-0.25f, 0, -0.5f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s3x2:
                            objectInstantiation = Instantiate(decoration2x3, islandParent);
                            ChangeTransitable(actualNode, false, 3, 2);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        case Node.Size.s2x4:
                            objectInstantiation = Instantiate(decoration2x4, islandParent);
                            ChangeTransitable(actualNode, false, 2, 4);
                            objectInstantiation.transform.GetChild(0).localPosition = new Vector3(-0.25f, 0, -0.75f);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90, 0);
                            else
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        case Node.Size.s4x2:
                            objectInstantiation = Instantiate(decoration2x4, islandParent);
                            ChangeTransitable(actualNode, false, 4, 2);
                            if (Random.Range(0, 2) == 1)
                                objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                            break;
                        default:
                            objectInstantiation = Instantiate(decoration1x1, islandParent);
                            ChangeTransitable(actualNode, false, 1, 1);
                            objectInstantiation.transform.GetChild(0).localEulerAngles = new Vector3(0, 90 * Random.Range(0, 4), 0);
                            break;
                    }
                    objectInstantiation.transform.position = new Vector3(actualNode.worldPosition.x, instantiationHeight + actualNode.gridPositionY * 100, actualNode.worldPosition.z);
                }
            }
        }
    }


    void ChangeTransitable(Node nodeToChange, bool _isTransitable, int sizeX, int sizeY)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                gridScript.grid[nodeToChange.gridPositionX - i, nodeToChange.gridPositionY - j].isTransitable = _isTransitable;
            }
        }
    }
}
