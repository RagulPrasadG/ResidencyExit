using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEventService",menuName = "Data/NewEventService")]
public class EventServiceScriptableObject : ScriptableObject
{
    public EventController OnCollectCoin = new EventController();
    public EventController OnReachGoal = new EventController();
  

}
