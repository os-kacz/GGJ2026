using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class BossLogic : MonoBehaviour
{
    private Transform[] LeftRows;
    private Transform[] RightRows;
    private Transform[] AllRowsDefault;
    private List<int> AllRowsWorking = new List<int>();

    float timer;

    [SerializeField] private GameObject LeftRowContainer;
    [SerializeField] private GameObject RightRowContainer;
    [SerializeField] private GameObject Boss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < LeftRowContainer.gameObject.transform.childCount; i++)
        {
            AllRowsDefault.Append(LeftRowContainer.gameObject.transform.GetChild(i).transform);
        }
        for (int i = 0; i < RightRowContainer.gameObject.transform.childCount; i++)
        {
            AllRowsDefault.Append(RightRowContainer.gameObject.transform.GetChild(i).transform);
        }
        Debug.Log(AllRowsDefault.Length);
        //Debug.Log(LeftRowContainer.gameObject.transform.GetChild(0).name);
        //Debug.Log(LeftRowContainer.gameObject.transform.childCount);
        //Debug.Log(LeftRowContainer.gameObject.transform.GetChild(0).transform);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            int nextSpot = Random.Range(0, AllRowsWorking.Count + 1);
            Boss.transform.position = AllRowsDefault[nextSpot].position;
            AllRowsWorking.Remove(nextSpot);
            timer = 0;
        }
    }
}
