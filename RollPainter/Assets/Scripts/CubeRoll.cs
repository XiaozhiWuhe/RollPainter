using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeRoll : MonoBehaviour
{
    public Quaternion initialRotation;

    public float rollDuration = 0.25f;

    private bool isRolling = false;

    private Vector3 gridPosition;

    public event Action OnRollComplete;

    LevelManager levelManager;

    List<MoveState> history = new List<MoveState>();

    const int MAX_HISTORY = 5;

    public enum CubeFace
    {
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back,

    }

    public Dictionary<CubeFace, int> faceColors =new Dictionary<CubeFace, int>();

    void Start()
    {
        initialRotation = transform.rotation;
        gridPosition = transform.position;

        levelManager = FindObjectOfType<LevelManager>();

        faceColors[CubeFace.Top] = 0;
        faceColors[CubeFace.Front] = 1;
        faceColors[CubeFace.Right] = 2;
        faceColors[CubeFace.Back] = 3;
        faceColors[CubeFace.Left] = 4;
        faceColors[CubeFace.Bottom] = 5;

    }

    void Update()
    {
        if (levelManager.levelFinished) return;
        
        if (isRolling) return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.wasPressedThisFrame ||
            Keyboard.current.upArrowKey.wasPressedThisFrame)
            input = Vector2.up;

        if (Keyboard.current.sKey.wasPressedThisFrame ||
            Keyboard.current.downArrowKey.wasPressedThisFrame)
            input = Vector2.down;

        if (Keyboard.current.aKey.wasPressedThisFrame ||
            Keyboard.current.leftArrowKey.wasPressedThisFrame)
            input = Vector2.left;

        if (Keyboard.current.dKey.wasPressedThisFrame ||
            Keyboard.current.rightArrowKey.wasPressedThisFrame)
            input = Vector2.right;

        if (input != Vector2.zero)
        {
            Vector2Int dir = new Vector2Int((int)input.x, (int)input.y);

            if (CanMove(dir))
                StartCoroutine(Roll(dir));
        }
    }

    IEnumerator Roll(Vector2Int dir)
    {
        MoveState state = SaveState();

        isRolling = true;

        Vector3 moveDir = new Vector3(dir.x, 0, dir.y);

        Vector3 pivot =
            transform.position +
            (moveDir + Vector3.down) * 0.5f;

        Vector3 axis = Vector3.Cross(Vector3.up, moveDir);

        float angle = 0f;
        float target = 90f;

        while (angle < target)
        {
            float step = (target / rollDuration) * Time.deltaTime;

            if (angle + step > target)
                step = target - angle;

            transform.RotateAround(pivot, axis, step);

            angle += step;

            yield return null;
        }

        gridPosition += moveDir;
        transform.position = gridPosition;

        Vector3 euler = transform.eulerAngles;

        euler.x = Mathf.Round(euler.x / 90f) * 90f;
        euler.y = Mathf.Round(euler.y / 90f) * 90f;
        euler.z = Mathf.Round(euler.z / 90f) * 90f;

        transform.eulerAngles = euler;

        RotateFaces(dir);

        PaintTile(state);

        history.Add(state);
        TrimHistory();

        isRolling = false;

        levelManager.AddStep();

        OnRollComplete?.Invoke();
    }

    void RotateFaces(Vector2Int dir)
    {
        int top = faceColors[CubeFace.Top];
        int bottom = faceColors[CubeFace.Bottom];
        int left = faceColors[CubeFace.Left];
        int right = faceColors[CubeFace.Right];
        int front = faceColors[CubeFace.Front];
        int back = faceColors[CubeFace.Back];

        if (dir == Vector2Int.right)
        {
            faceColors[CubeFace.Top] = left;
            faceColors[CubeFace.Bottom] = right;
            faceColors[CubeFace.Left] = bottom;
            faceColors[CubeFace.Right] = top;
        }
        else if (dir == Vector2Int.left)
        {
            faceColors[CubeFace.Top] = right;
            faceColors[CubeFace.Bottom] = left;
            faceColors[CubeFace.Left] = top;
            faceColors[CubeFace.Right] = bottom;
        }
        else if (dir == Vector2Int.up)
        {
            faceColors[CubeFace.Top] = back;
            faceColors[CubeFace.Bottom] = front;
            faceColors[CubeFace.Front] = top;
            faceColors[CubeFace.Back] = bottom;
        }
        else if (dir == Vector2Int.down)
        {
            faceColors[CubeFace.Top] = front;
            faceColors[CubeFace.Bottom] = back;
            faceColors[CubeFace.Front] = bottom;
            faceColors[CubeFace.Back] = top;
        }
    }

    void PaintTile(MoveState state)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            Tile tile = hit.collider.GetComponent<Tile>();

            if (tile != null)
            {
                TileColor newColor = (TileColor)faceColors[CubeFace.Bottom];

                state.changedTile = tile;
                state.previousColor = tile.currentColor;

                tile.Paint(newColor);

                levelManager.CheckLevelComplete();
            }
        }
    }

    MoveState SaveState()
    {
        MoveState state = new MoveState();

        state.cubePosition = transform.position;
        state.cubeRotation = transform.rotation;

        state.faceColors = new int[6];

        for (int i = 0; i < 6; i++)
            state.faceColors[i] = faceColors[(CubeFace)i];

        return state;
    }

    public void UndoMove()
    {
        if (history.Count == 0)
            return;

        //取最后一个元素
        MoveState state = history[history.Count - 1];
        history.RemoveAt(history.Count - 1);  //移除

        //恢复状态
        transform.position = state.cubePosition;
        transform.rotation = state.cubeRotation;
        gridPosition = state.cubePosition;

        for (int i = 0; i < 6; i++)
            faceColors[(CubeFace)i] = state.faceColors[i];

        if (state.changedTile != null)
            state.changedTile.Paint(state.previousColor);

        levelManager.ReduceStep();
    }

    public void ClearHistory()
    {
        history.Clear();
    }

    bool CanMove(Vector2Int dir)
    {
        int targetX = (int)gridPosition.x + dir.x;
        int targetZ = (int)gridPosition.z + dir.y;

        if (targetX < 0 || targetZ < 0)
            return false;

        if (targetX >= levelManager.CurrentLevelData.width)
            return false;

        if (targetZ >= levelManager.CurrentLevelData.height)
            return false;

        return true;
    }

    public void ResetToInitial(Vector3 position, Quaternion rotation)
    {
        transform.rotation = initialRotation;//重置朝向

        //停止滚动动画
        StopAllCoroutines();
        isRolling = false;

        transform.position = position;
        transform.rotation = rotation;
        gridPosition = position;

        faceColors[CubeFace.Top] = 0;//√
        faceColors[CubeFace.Front] = 1;//√
        faceColors[CubeFace.Right] = 2;//√
        faceColors[CubeFace.Back] = 3;//√
        faceColors[CubeFace.Left] = 4;
        faceColors[CubeFace.Bottom] = 5;

        //清空历史记录
        ClearHistory();
    }

    void TrimHistory()
    {
        //如果超出最大容量，移除最早的记录
        while (history.Count > MAX_HISTORY)
        {
            history.RemoveAt(0);
        }
    }
}