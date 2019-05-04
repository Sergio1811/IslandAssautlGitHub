using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float range;
    public GameObject player;
    public GameObject rockPrefab;
    GameObject rock;

    bool canDestroyTrees, canDestroyEnemies, canDestroyVillage, canDestroyDecoration;

    private void Start()
    {
        canDestroyTrees = false;
        canDestroyEnemies = true;
        canDestroyVillage = false;
        canDestroyDecoration = false;
    }

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        for (int k = 0; k < colliders.Length; k++)
        {
            if (colliders[k].tag == "Rock")
            {
                rock = Instantiate(rockPrefab);
                rock.transform.position = colliders[k].transform.position;
                Destroy(colliders[k].transform.parent.gameObject);
            }
            if (colliders[k].tag == "Enemy" && canDestroyEnemies)
            {
                Destroy(colliders[k].transform.parent.gameObject);
            }
            if (colliders[k].tag == "Decoration" && canDestroyDecoration)
            {
                Destroy(colliders[k].transform.parent.gameObject);
            }
            if (colliders[k].tag == "Village" && canDestroyVillage)
            {
                Destroy(colliders[k].gameObject);
            }
            if (colliders[k].tag == "Tree" && canDestroyTrees)
            {
                Destroy(colliders[k].transform.parent.gameObject);
            }
            if (colliders[k].tag == "Player")
            {
                player.GetComponent<Movement>().Damage(player.transform.forward - transform.position.normalized);
            }
        }

        gameObject.SetActive(false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, range);

    }
}
