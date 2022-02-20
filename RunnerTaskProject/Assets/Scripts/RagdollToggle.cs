using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody Rigidbody;
    protected CapsuleCollider capsuleCollider;
    //protected ScriptName script;
    protected Collider[] childrenCollider;
    protected Rigidbody[] childrenRigidbody;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        childrenCollider = GetComponentsInChildren<Collider>();
        childrenRigidbody = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        RagdollActivate(false);
    }
    public void RagdollJump(bool active)
    {
        foreach (var collider in childrenCollider)
            collider.enabled = active;
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.drag = 1;
            rigidb.constraints = RigidbodyConstraints.FreezePositionX;
            rigidb.detectCollisions = active;
            rigidb.isKinematic = !active;
            rigidb.AddForce(new Vector3(0f, 0.5f, 1f * (Globals.score * 0.15f / Globals.diamondMultiple + 1)) * 5000);
        }
        animator.enabled = !active;
        Rigidbody.detectCollisions = !active;
        Rigidbody.isKinematic = active;
        capsuleCollider.enabled = !active;
    }
    public void RagdollActivate(bool active)
    {
        foreach (var collider in childrenCollider)
            collider.enabled = active;
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.detectCollisions = active;
            rigidb.isKinematic = !active;
        }
        animator.enabled = !active;
        Rigidbody.detectCollisions = !active;
        Rigidbody.isKinematic = active;
        capsuleCollider.enabled = !active;
    }
}