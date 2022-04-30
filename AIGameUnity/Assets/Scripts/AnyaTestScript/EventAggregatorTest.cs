using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAggregatorTest
{
    public static AddCurrencyEvent AddCurrency = new AddCurrencyEvent();
    public static AddCurrencyWithParametrsEvent AddCurrencyParam = new AddCurrencyWithParametrsEvent();
    public static CubesDestroyedEvent CubesDestroyed = new CubesDestroyedEvent();
}
