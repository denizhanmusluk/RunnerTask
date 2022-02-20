using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour, IStartGameObserver, IDamageble
{
    // Start is called before the first frame update
    [SerializeField] public GameObject followObject;
    [SerializeField] public GameObject characterPelvis;
    public float maxSteerAngle = 10f;


    float FollowDistance = 7f;
    float speed;
    float steeringSpeed;
    public bool followActive = true;
    int health = 3;
    [SerializeField] CinemachineVirtualCamera camMain, camFinish ;
    [SerializeField] GameObject heartsParent;
    [SerializeField] public GameObject MoveEffect, FlyEffect;
    void Start()
    {
        GameManager.Instance.Add_StartObserver(this);

        speed = FindObjectOfType<Controller>().moveSpeed;
        FollowDistance = speed / 5;
        if (Globals.isGameActive)
        {
            StartCoroutine(following());
        }

   
    }
    public void StartGame()
    {
        Debug.Log("satrting");
        StartCoroutine(following());
        transform.GetComponent<Animator>().SetTrigger("run");
        health = Globals.heartCount;
        MoveEffect.SetActive(true);
        FlyEffect.SetActive(false);
    }

    IEnumerator following()
    {
        yield return new WaitForSeconds(0.1f);
        while (followActive)
        {

            transform.position = Vector3.MoveTowards(transform.position, followObject.transform.position, 0.9f * speed * (Vector3.Distance(transform.position, followObject.transform.position)) / FollowDistance * Time.deltaTime);

            if (Vector3.Distance(transform.position, followObject.transform.position) > 1)
            {
                ApplySteer();
            }

            yield return null;
        }
    }
    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(followObject.transform.position);
        relativeVector /= relativeVector.magnitude;
        float newSteerY = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;

        transform.Rotate(0, newSteerY * Time.deltaTime * 100f, 0);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "diamond")
        {
            collision.transform.GetComponent<Diamond>().collecting();
        }
    }
    public void obstacleHit()
    {
        health--;
        heartsParent.transform.GetChild(health).GetComponent<heart>().decrease();
        if (health == 0)
        {
            //finish
            //transform.GetComponent<Animator>().SetTrigger("dying");
            transform.GetComponent<RagdollToggle>().RagdollActivate(true);
            followActive = false;
            Globals.isGameActive = false;
            GameManager.Instance.Notify_LoseObservers();
            MoveEffect.SetActive(false);
            FlyEffect.SetActive(false);

        }
        else
        {
            StartCoroutine(hitDelay());
        }
    }
    IEnumerator hitDelay()
    {
        MoveEffect.SetActive(false);
        transform.GetComponent<Animator>().SetBool("stagger", true);
        //transform.GetComponent<RagdollToggle>().RagdollActivate(true);
        yield return new WaitForSeconds(0.8f);
        //transform.GetComponent<RagdollToggle>().RagdollActivate(false);
        transform.GetComponent<Animator>().SetBool("stagger", false);
        MoveEffect.SetActive(true);
    }
    public void finish()
    {
        followActive = false;
        Globals.isGameActive = false;
        //GameManager.Instance.Notify_WinObservers();
        transform.GetComponent<Animator>().SetTrigger("jump");
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0.2f, 1) * 500);
   
    }
    public void jumpRagdoll()
    {
        MoveEffect.SetActive(false);
        FlyEffect.SetActive(true);
        transform.GetComponent<RagdollToggle>().RagdollJump(true);
        camMain.Follow = characterPelvis.transform;
        camMain.LookAt = characterPelvis.transform;
        camMain.Priority = 0;

        camFinish.Follow = characterPelvis.transform;
        camFinish.LookAt = characterPelvis.transform;
        camFinish.Priority = 10;
    }
}
