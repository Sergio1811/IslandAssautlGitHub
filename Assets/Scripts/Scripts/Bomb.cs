using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float range;
    public GameObject player, psRockHit, psExplosion;
    PlayerScript playerMovement;
    public GameObject rockPrefab, treePrefab, rock2Prefab, tree2Prefab;
    GameObject rock, tree;
    public bool knockback = false;
    bool canDestroyEnemies, canDestroyVillage, canDestroyDecoration;

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerScript>();

        canDestroyEnemies = true;
        canDestroyVillage = false;
        canDestroyDecoration = false;
    }

    public void Explode()
    {
        Instantiate(psExplosion, this.gameObject.transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        for (int k = 0; k < colliders.Length; k++)
        {
            if (colliders[k].tag == "Rock")
            {
                rock = Instantiate(rockPrefab);
                Instantiate(psRockHit, colliders[k].transform.position, Quaternion.identity);
                rock.transform.position = colliders[k].transform.position;
                Destroy(colliders[k].transform.parent.gameObject);
            }

            if (colliders[k].tag == "Rock2")
            {
                rock = Instantiate(rock2Prefab);
                Instantiate(psRockHit, colliders[k].transform);
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
                tree.transform.position = new Vector3(tree.transform.position.x, 7.5f, tree.transform.position.z);
                Destroy(colliders[k].transform.parent.gameObject);
            }

            if (colliders[k].tag == "Tree2" && playerMovement.bombPolivalente)
            {
                tree = Instantiate(tree2Prefab);
                tree.transform.position = colliders[k].transform.position;
                tree.transform.position = new Vector3(tree.transform.position.x, 7.5f, tree.transform.position.z);
                Destroy(colliders[k].transform.parent.gameObject);
            }

            if (colliders[k].tag == "Player")
            {
                playerMovement.Damage(player.transform.forward - transform.position.normalized, false);
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
