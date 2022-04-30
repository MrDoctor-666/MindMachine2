using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int point = 1;

    private void OnMouseDown()
    {
        Die();
    }
    private void Die()
    {
        EventAggregator0.UnitDied.Publish(this);
        Destroy(this.gameObject);
    }

}
