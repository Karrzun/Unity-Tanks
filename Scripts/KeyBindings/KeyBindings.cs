using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class KeyBindings
{
    public Dictionary<KeyBindingAction, KeyBinding> list { get; private set; }

    public KeyBindings()
    {
        list = new Dictionary<KeyBindingAction, KeyBinding>();

        // Default KeyBindings
        TryAddBinding(new KeyBinding(KeyBindingAction.Shoot, KeyCode.Mouse0));
        TryAddBinding(new KeyBinding(KeyBindingAction.DeployMine, KeyCode.Mouse1));

        TryAddBinding(new KeyBinding(KeyBindingAction.MoveUp, KeyCode.W, KeyCode.UpArrow));
        TryAddBinding(new KeyBinding(KeyBindingAction.MoveDown, KeyCode.S, KeyCode.DownArrow));
        TryAddBinding(new KeyBinding(KeyBindingAction.MoveLeft, KeyCode.A, KeyCode.LeftArrow));
        TryAddBinding(new KeyBinding(KeyBindingAction.MoveRight, KeyCode.D, KeyCode.RightArrow));

        TryAddBinding(new KeyBinding(KeyBindingAction.Pause, KeyCode.Escape, KeyCode.Space));
    }

    private bool TryAddBinding(KeyBinding binding)
    {
        if (list.ContainsKey(binding.Action))
        {
            Debug.LogWarning("A KeyBinding for this Action already exists within these KeyBindings.");
            return false;
        }

        list.Add(binding.Action, binding);
        return true;
    }

    public bool IsKeyCodeUsed(KeyCode keyCode)
    {
        foreach (KeyBinding kb in list.Values)
        {
            if (kb.IsKeyCodeUsed(keyCode))
            {
                return true;
            }
        }
        return false;
    }

    public void Reset()
    {
        foreach (KeyBinding kb in list.Values)
        {
            kb.Reset();
        }
    }

}