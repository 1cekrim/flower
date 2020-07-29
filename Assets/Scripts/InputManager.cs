using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum InputTypeEnum
{
    invalid,
    gimbal_left,
    gimbal_right,
    camera_zoomin,
    camera_zoomout
}

public interface KeyBindInterface
{
    KeyBindInterface Init();
    void Update();
}

public class KeyboardMouseInput : KeyBindInterface
{
    private Dictionary<KeyCode, InputTypeEnum> keyDict;
    public KeyBindInterface Init()
    {
        keyDict = new Dictionary<KeyCode, InputTypeEnum>
        {
            { KeyCode.Q, InputTypeEnum.gimbal_left },
            { KeyCode.E, InputTypeEnum.gimbal_right }
        };

        return this;
    }

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in keyDict)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    InputManager.Instance.RaiseEvent(dic.Value);
                }
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            if (scroll < 0)
            {
                InputManager.Instance.RaiseEvent(InputTypeEnum.camera_zoomout);
            }
            else
            {
                InputManager.Instance.RaiseEvent(InputTypeEnum.camera_zoomin);
            }
        }
    }
}

public class InputManager : MonoBehaviour
{
    [System.Serializable]
    public struct InputEvent
    {
        public InputTypeEnum inputType;
        public UnityEvent inputEvent;
    }

    public InputEvent[] inputEvents;
    private Dictionary<InputTypeEnum, UnityEvent> inputEventDictionary;
    private KeyBindInterface keyBind;
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
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
        SelectKeyBind(new KeyboardMouseInput().Init());
    }

    public void SelectKeyBind(KeyBindInterface keyBind)
    {
        this.keyBind = keyBind;
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
        invalidEvent.AddListener(delegate
        {
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
        keyBind?.Update();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor 
{
    public InputTypeEnum selected;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector ();
        EditorGUILayout.LabelField("Raise Event");
        InputManager inputManager = (InputManager)target;
        selected = (InputTypeEnum)EditorGUILayout.EnumPopup("selected event", selected);
        if (GUILayout.Button("raise"))
        {
            inputManager.RaiseEvent(selected);
        }
    }
}
#endif