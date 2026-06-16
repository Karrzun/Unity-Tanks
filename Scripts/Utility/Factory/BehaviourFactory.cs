using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class BehaviourFactory
{
	private static Dictionary<string, Type> myBehavioursByName;

	static BehaviourFactory ()
	{
		var myTypes = Assembly.GetAssembly(typeof(MovementBehaviour)).GetTypes ().Where (myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(MovementBehaviour)));

		myBehavioursByName = new Dictionary<string, Type> ();

		foreach (var type in myTypes)
		{
			myBehavioursByName.Add (type.Name, type);
		}
	}

	public static Type GetBehaviour (string myType)
	{
		if (myBehavioursByName.ContainsKey (myType))
		{
			Type type = myBehavioursByName [myType];
			return type;
		}

		return null;
	}

}