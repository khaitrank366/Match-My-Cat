using System.Collections.Generic;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    public GameObject prefab;
    public int initialSize = 15;

    private readonly List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewCard();
        }
    }

    GameObject CreateNewCard()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    public GameObject Get()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeSelf)
                return obj;
        }
        return CreateNewCard();
    }

    public void ReleaseAll()
    {
        foreach (var obj in pool)
            obj.SetActive(false);
    }
}
