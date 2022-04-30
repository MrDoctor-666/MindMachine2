using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnSceneTest : MonoBehaviour
{
    public int howMuchToAdd = 10;
    private void Awake()
    {
        EventAggregatorTest.CubesDestroyed.Subscribe(Die);
    }
    private void OnMouseDown()
    {
        //add curency when we click on the cube
        Debug.Log("on mouse down");
        EventAggregatorTest.AddCurrency.Publish(this);
    }

    private void OnMouseEnter()
    {
        //(i actually substract this random value but whatever, was testing how parameters worked)
        Debug.Log("on mouse enter");
        EventAggregatorTest.AddCurrencyParam.Publish(this, howMuchToAdd/Random.Range(1, 10));
    }

    void Die(MonoBehaviour item)
    {
        //when we get to 0 currency again
        Destroy(this.gameObject);
    }
}
