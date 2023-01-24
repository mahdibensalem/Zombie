using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class characterScript : MonoBehaviour
{
    public Vector3 Destination;
    NavMeshAgent agent;

    private void Awake()
    {
        agent=GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            GetComponentInParent<characterSave>().enabled = false;
            GetComponentInParent<SphereCollider>().enabled = false;

            progressLVL.Instance.numberOfWave++;
            if (progressLVL.Instance.numberOfWave == 4)
            {
                /// you Win 
                Time.timeScale = 0;
            }
            else
            {
                progressLVL.Instance.OnKillMissionUpdate();
                progressLVL.Instance.OnFillProgressLVL();
                progressLVL.Instance.removePosSpawnCharacter();
            }
            Destroy(gameObject);            
        }
    }
}
