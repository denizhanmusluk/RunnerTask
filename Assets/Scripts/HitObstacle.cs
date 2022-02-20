using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<IDamageble>() != null)
        {
            collision.transform.GetComponent<IDamageble>().obstacleHit();
            this.GetComponent<Rigidbody>().AddForce(((transform.position - collision.transform.position).normalized + collision.transform.forward )* 300);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.GetComponent<IDamageble>() != null)
    //    {
    //        other.GetComponent<IDamageble>().obstacleHit();
    //    }
    //}
}
