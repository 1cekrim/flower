using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InputManager : MonoBehaviour
{
    public enum InputTypeEnum
    {
        invalid,
        gimbal_left,
        gimbal_right
    }

    [System.Serializable]
    public struct InputEvent
    {
        public InputTypeEnum inputType;
        public UnityEvent inputEvent;
    }

    public InputEvent[] inputEvents;
    private Dictionary<InputTypeEnum, UnityEvent> inputEventDictionary;

    private static InputManager _instance;
    public static InputManager Instance
    {
        get {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(InputManager)) as InputManager;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        UpdateInputEventDictionary();
    }

    public void UpdateInputEventDictionary()
    {
        inputEventDictionary = new Dictionary<InputTypeEnum, UnityEvent>();
        foreach (var e in inputEvents)
        {
            if (e.inputType != InputTypeEnum.invalid)
            {
                inputEventDictionary[e.inputType] = e.inputEvent;
            }
        }
        UnityEvent invalidEvent = new UnityEvent();
        invalidEvent.AddListener(delegate {
            Debug.LogError("invalid input!!");
        });
        inputEventDictionary[InputTypeEnum.invalid] = invalidEvent;
    }

    public void RaiseEvent(InputTypeEnum inputType)
    {
        inputEventDictionary[inputType].Invoke();
    }

    void Update()
    {

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor 
{
    public InputManager.InputTypeEnum selected;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector ();
        EditorGUILayout.LabelField("Raise Event");
        InputManager inputManager = (InputManager)target;
        selected = (InputManager.InputTypeEnum)EditorGUILayout.EnumPopup("selected event", selected);
        if (GUILayout.Button("raise"))
        {
            inputManager.RaiseEvent(selected);
        }
    }
}
#endif