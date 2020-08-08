using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

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
    private GameObject windowPanel;
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
        windowPanel = GameObject.Find("WindowCanvas").transform.Find("WindowPanel").gameObject;
        return this;
    }

    public void Update()
    {
        // Window가 떠있는 동안에는 모든 기능 비활성화
        if (windowPanel.activeSelf)
        {
            return;
        }

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

        if (Input.GetMouseButtonDown(1) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var layerMask = 1 << 12;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                (int col, int row) = GetRayCastTargetCoord(hit);
                MoveToCoord(col, row);
            }
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            // 좌클릭 하면 상호작용
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var layerMask = 1 << 12;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                (int col, int row) = GetRayCastTargetCoord(hit);
                MoveToCoord(col, row);
                // TODO: NavMeshAgent에 도착 콜백이 없어서 직접 구현해야 함
                // RotateAgentToTarget(hit.transform);
                // GameManager.Instance.mapTile[row, col].State?.Interact();
            }
        }
    }

    private (int col, int row) GetRayCastTargetCoord(RaycastHit hit)
    {
        Vector3 position = hit.point;
        int col = convertGrid(position.x, GameManager.Instance.mapCols);
        int row = convertGrid(position.z, GameManager.Instance.mapRows);
        return (col, row);
    }

    private int convertWorld(float origin)
    {
        int result = Mathf.CeilToInt(origin);
        result = result % 2 == 0 ? result : result - 1;
        return result;
    }

    private int convertGrid(float origin, int size)
    {
        return convertWorld(origin) / 2 + size / 2;
    }

    private void RotateAgentToTarget(Transform target)
    {
        Transform player = GameManager.Instance.playerObject.transform;
        Vector3 direction = new Vector3(0, 0, (target.position - player.position.normalized).z);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        player.DORotate(lookRotation.eulerAngles, 0.5f);
    }

    private void MoveToCoord(int col, int row)
    {

        if (GameManager.Instance.mapTile[row, col] == null)
        {
            return;
        }

        if (!GameManager.Instance.mapTile[row, col].canMove)
        {
            // 해당 칸이 올라갈 수 없는 칸이면, 가장 가까운 곳부터 시계방향으로 8방향 탐색
            // TODO: 8방향 다 막혀있으면 다이얼로그 띄우기
            Func<int, int> Norm = k => (k == 0 ? 0 : (k > 0 ? 1 : -1));
            int playerCol = convertGrid(GameManager.Instance.playerObject.transform.position.x, GameManager.Instance.mapCols);
            int playerRow = convertGrid(GameManager.Instance.playerObject.transform.position.z, GameManager.Instance.mapRows);
            int dx = (int)(playerCol - col);
            int dz = (int)(playerRow - row);
            dx = Norm(dx);
            dz = Norm(dz);
            for (int i = 0; i < 8; ++i)
            {
                if (GameManager.Instance.mapTile[row + dz, col + dx].canMove)
                {
                    row += dz;
                    col += dx;
                    break;
                }
                int tx = Norm(dx + dz);
                dz = Norm(dz - dx);
                dx = tx;
            }
        }

        agent.SetDestination(GameManager.Instance.mapTile[row, col].transform.position + new Vector3(0, 1, 0));
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
        DrawDefaultInspector();
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