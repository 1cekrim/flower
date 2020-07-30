using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    camera_zoomout,
    debug_1,
    debug_2,
    debug_3
}

public interface KeyBindInterface
{
    KeyBindInterface Init();
    void Update();
}

public class KeyboardMouseInput : KeyBindInterface
{
    private Dictionary<KeyCode, InputTypeEnum> keyDict;
    private NavMeshAgent agent;
    public KeyBindInterface Init()
    {
        keyDict = new Dictionary<KeyCode, InputTypeEnum>
        {
            { KeyCode.Q, InputTypeEnum.gimbal_left },
            { KeyCode.E, InputTypeEnum.gimbal_right },
            { KeyCode.Alpha1, InputTypeEnum.debug_1 },
            { KeyCode.Alpha2, InputTypeEnum.debug_2 },
            { KeyCode.Alpha3, InputTypeEnum.debug_3 },
        };

        agent = GameManager.Instance.playerObject.GetComponent<NavMeshAgent>();

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

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var layerMask = 1 << 12;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Vector3 position = hit.point;
                int x = Mathf.CeilToInt(position.x);
                x = x % 2 == 0 ? x : x - 1;
                int z = Mathf.CeilToInt(position.z);
                z = z % 2 == 0 ? z : z - 1;
                agent.SetDestination(new Vector3(x, 1, z));
                Debug.Log(position);
                // Transform target = hit.collider.gameObject.transform;
                // Debug.Log(target.position);
                // Debug.Log(hit.collider.gameObject.name);
                // agent.SetDestination(new Vector3(hit.transform.position.x, 1, hit.transform.position.y));
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