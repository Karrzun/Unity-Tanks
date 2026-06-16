using UnityEngine;


[System.Serializable]
public class KeyBinding
{
    [SerializeField] private KeyCode primaryKey;
    public KeyCode PrimaryKey => primaryKey;

    [SerializeField] private KeyCode secondaryKey;
    public KeyCode SecondaryKey => secondaryKey;

    private KeyCode defaultPrimaryKey, defaultSecondaryKey;


    public KeyBindingAction Action { get; private set; }
    public string Name => Action.ToString().InsertSpaces();
    

    public bool GetKey => NonUIInput.GetKey(primaryKey) || NonUIInput.GetKey(secondaryKey);
    public bool GetKeyDown => NonUIInput.GetKeyDown(primaryKey) || NonUIInput.GetKeyDown(secondaryKey);
    public bool GetKeyUp => NonUIInput.GetKeyUp(primaryKey) || NonUIInput.GetKeyUp(secondaryKey);


    public KeyBinding(KeyBindingAction action, KeyCode primary, KeyCode secondary = KeyCode.None)
    {
        primaryKey = primary;
        defaultPrimaryKey = primary;
        secondaryKey = secondary;
        defaultSecondaryKey = secondary;
        Action = action;
    }


    public void SetPrimaryKey(KeyCode keyCode) => primaryKey = keyCode;
    public void SetSecondaryKey(KeyCode keyCode) => secondaryKey = keyCode;

    public virtual bool IsKeyCodeUsed(KeyCode keyCode)
    {
        return (keyCode == KeyCode.None)
            ? false
            : (keyCode == primaryKey || keyCode == secondaryKey);
    }

    public void Reset()
    {
        primaryKey = defaultPrimaryKey;
        secondaryKey = defaultSecondaryKey;
    }

}