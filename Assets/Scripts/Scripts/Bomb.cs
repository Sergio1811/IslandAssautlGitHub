using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float range;
    public GameObject player;
    Movement playerMovement;
    public GameObject rockPrefab, treePrefab;
    GameObject rock, tree;
    public bool knockback = false;
    bool canDestroyEnemies, canDestroyVillage, canDestroyDecoration;

    private void Start()
    {
        playerMovement = player.GetComponent<Movement>();

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

            if (colliders[k].tag == "Rock2")
            {
                rock = Instantiate(rockPrefab);
                rock.transform.position = colliders[k].transform.position;
                Destroy(colliders[k].transform.parent.gameObject);
            }

            if (colliders[k].tag == "Enemy" && canDestroyEnemies)
            {
                EnemyScript enemyScript = colliders[k].transform.parent.GetComponent<EnemyScript>();
                if (playerMovement.bomberKnockBack)
                    enemyScript.KnockBackActivated(this.gameObject.transform);
                enemyScript.GetAttackedByBomb();
            }
            if (colliders[k].tag == "Decoration" && canDestroyDecoration)
            {
                Destroy(colliders[k].transform.parent.gameObject);
            }
            if (colliders[k].tag == "Village" && canDestroyVillage)
            {
                Destroy(colliders[k].gameObject);
            }
            if (colliders[k].tag == "Tree" && playerMovement.bombPolivalente)
            {
                tree = Instantiate(treePrefab);
                tree.transform.position = colliders[k].transform.position;
                tree.transform.position = new Vector3(tree.transform.position.x, 5f, tree.transform.position.z);
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
