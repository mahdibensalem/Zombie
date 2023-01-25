//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using DG.Tweening;
//public class CoinAnimator : MonoBehaviour
//{
//    public static CoinAnimator Instance;
//    public GameObject CoinPrefab;
//    public Canvas Canvas;
//    public float animationDuration = 1f;
//    public Ease animationEase = Ease.Linear;
//    public GameObject coinpos;

//    void Awake()
//    {
//        Instance = this;

//    }





//    public void Animate(GameObject coin, int Amount)
//    {
//        for (int i = 0; i <= Amount; i++)
//        {
//            RectTransform clone = Instantiate(CoinPrefab, Canvas.transform, false).GetComponent<RectTransform>();
//            clone.anchorMin = Camera.main.WorldToViewportPoint(coin.transform.position);
//            clone.anchorMax = clone.anchorMin;

//            clone.anchoredPosition = clone.localPosition;

//            clone.anchorMin = new Vector3(0.5f, 0.5f);
//            clone.anchorMax = clone.anchorMin;

//            clone.SetParent(coinpos.transform);
//            float duration = Random.Range(animationDuration, animationDuration + 1f);

//            clone.DOAnchorPos(Vector3.zero, duration).OnComplete(() =>
//            {
//                Destroy(clone.gameObject);
//            });
//        }
//    }



//    public void AnimateCanvasToCanvas(GameObject coin, GameObject objectToGoTo)
//    {
//        RectTransform myRect = coin.transform.GetComponent<RectTransform>();
//        RectTransform clone = Instantiate(CoinPrefab, Canvas.transform, false).GetComponent<RectTransform>();
//        clone.position = myRect.localPosition;

//        clone.DOMove(objectToGoTo.GetComponent<RectTransform>().position, animationDuration).OnComplete(() =>
//        {
//            Destroy(clone.gameObject);
//        }).Play();

//    }
//}