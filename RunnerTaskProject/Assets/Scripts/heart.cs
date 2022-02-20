using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class heart : MonoBehaviour
{
    private Image kHeart, leftPart, rightPart;
    private void Start()
    {
        kHeart = transform.GetChild(0).GetComponent<Image>();
        leftPart = transform.GetChild(1).GetComponent<Image>();
        rightPart = transform.GetChild(2).GetComponent<Image>();
    }
    public void decrease()
    {
        StartCoroutine(breaking());
    }
    IEnumerator breaking()
    {
        float counter = 0;
        while(counter < 1f)
        {
            counter += 2 * Time.deltaTime;
            if(counter > 1)
            {
                counter = 1;
            }
            leftPart.fillAmount = counter;
            rightPart.fillAmount = counter;
            yield return null;
        }
        kHeart.enabled = false;
        StartCoroutine(fallingHerat());
    }
    IEnumerator fallingHerat()
    {
        float counter = 0;
        float speed = 0;
        Vector2 firstPos = kHeart.GetComponent<RectTransform>().anchoredPosition;
        while (counter < Mathf.PI / 2)
        {
            counter += Time.deltaTime;
            speed = Mathf.Cos(counter);
            speed = 1 - speed;
            speed *= 20;
            //transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 1), Time.deltaTime * speed);
            leftPart.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(leftPart.GetComponent<RectTransform>().anchoredPosition, new Vector2(firstPos.x - 200, -1500), 100 * speed * Time.deltaTime);
            rightPart.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(rightPart.GetComponent<RectTransform>().anchoredPosition, new Vector2(firstPos.x + 200, -1500), 100 * speed * Time.deltaTime);
            leftPart.GetComponent<Image>().color = new Color(1, 1, 1, 1 - 0.1f * (counter / (Mathf.PI / 2)));
            rightPart.GetComponent<Image>().color = new Color(1, 1, 1, 1 - 0.1f * (counter / (Mathf.PI / 2)));

            leftPart.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, speed * 3);
            rightPart.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -speed * 3);
            yield return null;
        }
        leftPart.enabled = false;
        rightPart.enabled = false;
    }

    IEnumerator pointUp()
    {

        float counter = 0;
        float speed = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += 1.5f * Time.deltaTime;
            speed = Mathf.Cos(counter);
            speed *= 10;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 1), Time.deltaTime * speed);
            //transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1 - (counter / (Mathf.PI / 2)));

            yield return null;
        }
        Destroy(gameObject);
    }
}
