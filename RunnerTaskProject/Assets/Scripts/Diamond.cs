using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Diamond : MonoBehaviour, IStartGameObserver
{
    Sequence sequence;
    GameObject target;
    //[SerializeField] Material firstMat;
    //Color FirstColour;
    //[SerializeField] Material mat;
    void Start()
    {
        target = GameObject.Find("DiamondTarget");
        GameManager.Instance.Add_StartObserver(this);
        //FirstColour = firstMat.GetColor("_EmissionColor");
        //StartCoroutine(collUpd());
    }
    public void StartGame()
    {
        StartCoroutine(tweenScale());
    }
    IEnumerator tweenScale()
    {
        float randomTime = Random.Range(0f, 1f);
        yield return new WaitForSeconds(randomTime);
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.one * 0.2f, 0.3f).SetLoops(-1, LoopType.Yoyo));

        sequence.AppendInterval(0f);
        sequence.SetLoops(-1, LoopType.Yoyo);
        sequence.SetRelative(true);
    }
    //sequence.Kill(this);
    public void collecting()
    {
        StartCoroutine(targetMotion());
        transform.GetComponent<Collider>().enabled = false;
    }
    IEnumerator targetMotion()
    {
        Debug.Log("deneme");
        while (Vector3.Distance(transform.position, target.transform.position) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, (5 / Vector3.Distance(transform.position, target.transform.position)) * 18 * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, target.transform.localScale, 40 * Time.deltaTime);
            yield return null;
        }
        GameObject diamond = gameObject;
        diamond.transform.parent = null;
        Destroy(diamond);
        Score.Instance.scoreUp();
    }

    //IEnumerator collUpd()
    //{
    //    while (true)
    //    {
    //        float counter = 1f;
    //        while (counter < 4f)
    //        {
    //            counter += 5 * Time.deltaTime;

    //            mat.SetColor("_EmissionColor", FirstColour * counter);

    //            yield return null;
    //        }
    //        while (counter > 1)
    //        {
    //            counter -= 5 * Time.deltaTime;

    //            mat.SetColor("_EmissionColor", FirstColour * counter);

    //            yield return null;
    //        }
    //    }
    //}
}
