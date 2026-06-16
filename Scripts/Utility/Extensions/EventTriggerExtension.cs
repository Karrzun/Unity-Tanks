using UnityEngine.EventSystems;


public static class EventTriggerExtension
{

	public static void AddListener (this EventTrigger trigger, EventTriggerType eventType, System.Action callback)
	{
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = eventType;
        if (callback != null)
        {
            entry.callback.AddListener(data => callback.Invoke());
        }
		trigger.triggers.Add (entry);
	}

}