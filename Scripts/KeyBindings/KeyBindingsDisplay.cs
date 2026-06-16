using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class KeyBindingsDisplay : MonoBehaviour
{
    public Transform Grid;
    public GameObject KeyBindingUI;

    public Action ApplyChanges;


    private KeyRebinder keyRebinder;


    void Awake()
    {
        DisplayKeyBindings();
        keyRebinder = gameObject.GetComponent<KeyRebinder>();
        ApplyChanges += SaveKeyBindings;
    }

    public void SaveKeyBindings()
    {
        Serialization.KeyBindings.Save(InputHandler.bindings);
    }

    public void DisplayKeyBindings()
    {
        ClearGrid();
        foreach (KeyBindingAction action in InputHandler.bindings.list.Keys)
        {
            DisplayKeyBinding(action, InputHandler.bindings.list[action]);
        }
    }

    private void ClearGrid()
    {
        for (int i = Grid.childCount - 1; i >= 0; i--)
        {
            Destroy(Grid.GetChild(i).gameObject);
        }
    }

    private void DisplayKeyBinding(KeyBindingAction action, KeyBinding kb)
    {
        GameObject kbUI = Instantiate(KeyBindingUI, Grid);
        TMP_Text[] texts = kbUI.GetComponentsInChildren<TMP_Text>();
        EventTrigger[] eventTriggers = kbUI.GetComponentsInChildren<EventTrigger>();
        //Button[] buttons = kbUI.GetComponentsInChildren<Button>();

        AssignTexts(texts, kb);
        AssignEventTriggers(eventTriggers, texts, action);
    }

    private void AssignTexts(TMP_Text[] texts, KeyBinding kb)
    {
        texts[0].text = kb.Name;
        texts[1].text = (kb.PrimaryKey == KeyCode.None)
            ? ""
            : kb.PrimaryKey.ToString().InsertSpaces();
        texts[2].text = (kb.SecondaryKey == KeyCode.None)
            ? ""
            : kb.SecondaryKey.ToString().InsertSpaces();
    }

    private void AssignEventTriggers(EventTrigger[] eventTriggers, TMP_Text[] texts, KeyBindingAction action)
    {
        eventTriggers[0].AddListener(EventTriggerType.PointerEnter, /*AudioManager.Instance.PlaySFXButtonHover*/ null);
        eventTriggers[1].AddListener(EventTriggerType.PointerEnter, /*AudioManager.Instance.PlaySFXButtonHover*/ null);

        eventTriggers[0].AddListener(EventTriggerType.Submit, delegate { OnClickKeyBinding(texts[1], action, true); });
        eventTriggers[1].AddListener(EventTriggerType.Submit, delegate { OnClickKeyBinding(texts[2], action, false); });

        eventTriggers[0].AddListener(EventTriggerType.PointerClick, delegate { OnClickKeyBinding(texts[1], action, true); });
        eventTriggers[1].AddListener(EventTriggerType.PointerClick, delegate { OnClickKeyBinding(texts[2], action, false); });
    }

    private void OnClickKeyBinding(TMP_Text text, KeyBindingAction action, bool isPrimaryKey)
    {
        /*AudioManager.Instance.PlaySFXButtonClick();*/
        StartCoroutine(keyRebinder.ListenForKeyDownEvent(text, action, isPrimaryKey));
    }

    public void Reset()
    {
        InputHandler.bindings.Reset();
        DisplayKeyBindings();
    }

}