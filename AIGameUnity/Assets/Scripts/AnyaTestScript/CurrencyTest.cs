using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyTest : MonoBehaviour
{
    [SerializeField] private int currency = 0;
    public void Awake()
    {
        Debug.Log(EventAggregatorTest.AddCurrency.ToString());
        EventAggregatorTest.AddCurrency.Subscribe(OnAddCurrency);
        EventAggregatorTest.AddCurrencyParam.Subscribe(OnAddCurrencyParam);
    }

    void OnAddCurrency(ObjectOnSceneTest item)
    {
        currency += item.howMuchToAdd;
        Debug.Log(item.ToString());
        if (currency == 0) EventAggregatorTest.CubesDestroyed.Publish(this);
    }

    void OnAddCurrencyParam(MonoBehaviour item, int intParam)
    {
        currency -= intParam;
        if (currency == 0) EventAggregatorTest.CubesDestroyed.Publish(this);
    }
}
