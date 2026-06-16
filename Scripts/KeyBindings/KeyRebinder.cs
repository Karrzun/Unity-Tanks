using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class KeyRebinder : MonoBehaviour
{
    public UnityEvent OnKeyBindingsChanged;
    private bool isBinding = false;


    public IEnumerator ListenForKeyDownEvent(TMP_Text text, KeyBindingAction action, bool isPrimaryKey)
    {
        string former = text.text;
        text.text = "...";
        isBinding = true;
        while (isBinding)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        KeyCode kc = keyCode;
                        isBinding = false;

                        if (kc.ToString().InsertSpaces().Equals(former)) // former key to unbind
                        {
                            UnbindKey(text, action, isPrimaryKey);
                            break;
                        }

                        if (InputHandler.bindings.IsKeyCodeUsed(kc))
                        {
                            text.text = former;
                            break;
                        }

                        text.text = kc.ToString().InsertSpaces();

                        if (isPrimaryKey)
                        {
                            InputHandler.bindings.list[action].SetPrimaryKey(kc);
                        }
                        else
                        {
                            InputHandler.bindings.list[action].SetSecondaryKey(kc);
                        }
                        OnKeyBindingsChanged.Invoke();
                    }
                }
            }
            yield return null;
        }
    }

    private void UnbindKey(TMP_Text text, KeyBindingAction action, bool isPrimaryKey)
    {
        if (isPrimaryKey)
        {
            InputHandler.bindings.list[action].SetPrimaryKey(KeyCode.None);
        }
        else
        {
            InputHandler.bindings.list[action].SetSecondaryKey(KeyCode.None);
        }

        text.text = "";
    }
}
