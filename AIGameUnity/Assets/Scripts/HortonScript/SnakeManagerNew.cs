using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class SnakeManagerNew : MonoBehaviour
{
    public class Enemy
    {
        private GameObject instance;
        private Vector3[] way;
        private int wayIndex;
        private int actionNumber;

        public Enemy(GameObject instance)
        {
            if (instance)
                this.instance = instance;
            else Debug.LogError("SnakeManager.Enemy.Enemy(): Enemy instance cannot be initialized");

            Transform[] trs = instance.GetComponentsInChildren<Transform>();
            List<GameObject> gos = new List<GameObject>(trs.Length);

            for (int i = 0; i < trs.Length; ++i)
                gos.Add(trs[i].gameObject);

            gos.Sort((a, b) => a.name.CompareTo(b.name));

            way = new Vector3[trs.Length];

            for (int i = 0; i < way.Length; ++i)
                way[i] = gos[i].transform.position;

            way = way.ToArray();

            this.instance.transform.position = way[0];
            print(way.Length);
        }

        public void Move()
        {
            wayIndex = (wayIndex + 1) % way.Length;

            instance.transform.position = way[wayIndex];

            ++actionNumber;
        }

        public void ReturnAction()
        {
            if (actionNumber == 0)
                return;

            --wayIndex;
            --actionNumber;

            if (wayIndex < 0)
                wayIndex = way.Length - 1;

            instance.transform.position = way[wayIndex];
        }

        public Vector3 CurrentPosition
        {
            get => instance.transform.position;
        }

        public GameObject Instance
        {
            get => instance;
        }
    }

    enum Direction { right, up, left, down }

    #region GameObjects
    private List<GameObject> snake; // element by index 0 is head
    private GameObject[] barriers;
    private GameObject[] emptyCells;
    private List<GameObject[]> buttons;
    private List<GameObject> doors; // index of buttons element and index of door element must be equal if connected (look CheckToPressButtons() function)
    private GameObject[] lamps;
    private List<Enemy> enemies; // gays
    private Direction direction; // of snake (right on default !!!)
    #endregion

    #region Variables
    private List<(Vector3[], Direction)> previousSnakePartsPositions;
    private List<bool[]> previousLampsPositions; // second element is the number of player action when lamp must be transported in cell with first element position
    private List<(int, bool)> previousDoorsStates; // first element is number of door group, second is state (true - opened)
    private Vector3 positionToNewSnakePart;
    private bool canSnakeMove;
    #endregion

    [Header("WIN CELLS")] [SerializeField] private List<GameObject> winCells; // cells which ending game when snake go to one of these

    [Header("SNAKE PART IMAGES")]
    [SerializeField] private Sprite snakeStick;
    [SerializeField] private Sprite snakeCorner, snakeHead;

    [Header("READ COMMENTS IN SCRIPT !")] // THIS VARIABLE MUST BE INIT BY INSPECTOR PANEL OF EDITOR. ELEMENT 0 IS SPRITE OF GAMEOBJECT WITH BUTTON1, ELEMENT 1 - BUTTON 2, AND NEXT
    [SerializeField] private Sprite[] buttonPressedSprites;
    [SerializeField] private Sprite[] buttonUnpressedSprites;

    public void Start()
    {
        InitVariables();
        ScanArea();
        InitGraphics();
    }

    private void InitGraphics() // Make sure ALL game objects has Sprite Renderer component
    {
        for (int i = 0; i < emptyCells.Length; ++i)
            emptyCells[i].GetComponent<SpriteRenderer>().sortingOrder = 0;

        for (int i = 0; i < buttons.Count; ++i)
            for (int j = 0; j < buttons[i].Length; ++j)
                buttons[i][j].GetComponent<SpriteRenderer>().sortingOrder = 1;

        for (int i = 0; i < lamps.Length; ++i)
            lamps[i].GetComponent<SpriteRenderer>().sortingOrder = 2;

        for (int i = 0; i < winCells.Count; ++i)
            winCells[i].GetComponent<SpriteRenderer>().sortingOrder = 3;

        for (int i = 0; i < snake.Count; ++i)
            snake[i].GetComponent<SpriteRenderer>().sortingOrder = 4;

        for (int i = 0; i < enemies.Count; ++i)
            enemies[i].Instance.GetComponent<SpriteRenderer>().sortingOrder = 4;

        snake[0].GetComponent<SpriteRenderer>().sprite = snakeHead;
    }

    private void InitVariables()
    {
        buttons = new List<GameObject[]>(3);
        doors = new List<GameObject>(3);
        winCells = new List<GameObject>(2);

        previousSnakePartsPositions = new List<(Vector3[], Direction)>(25 * 2);
        previousLampsPositions = new List<bool[]>(10);
        previousDoorsStates = new List<(int, bool)>(25 * 2);
        snake = new List<GameObject>(25);
        enemies = new List<Enemy>(5);

        winCells.Add(GameObject.Find("WinCell"));
        winCells.Add(GameObject.Find("LossCell"));

        for (int i = 2; i > -1; --i)
            snake.Add(GameObject.Find($"Snake{ (i == 0 ? "" : $" ({i})") }")); // number of brackets make my state as "No comments", but ternary operator must be placed inside of brackets because priority of operators say it

        print(direction = ComputeDirection());

        canSnakeMove = true;

        MoveCameraToSnake();
        DrawSnakeParts();
    }

    private Direction ComputeDirection()
    {
        if (snake[0].transform.position.x > snake[2].transform.position.x)
            return Direction.right;
        else if (snake[0].transform.position.x < snake[2].transform.position.x)
            return Direction.left;
        else if (snake[0].transform.position.y > snake[2].transform.position.y)
            return Direction.up;
        else return Direction.down;
    }

    private void ScanArea() // Make sure ALL tags used in the function contains in tag manager
    {
        barriers = GameObject.FindGameObjectsWithTag("Barrier");
        emptyCells = GameObject.FindGameObjectsWithTag("EmptyCell");

        lamps = GameObject.FindGameObjectsWithTag("Lamp");

        for (int i = 0; true; ++i)
        {
            try
            {
                GameObject enemy = GameObject.Find($"Enemy{ (i == 0 ? "" : $" ({i})") }");

                if (!enemy)
                    break;

                enemies.Add(new Enemy(enemy));
            }
            catch
            {
                print(i);
                break;
            }
        }

        for (int i = 1; true; ++i)
        {
            try
            {
                GameObject[] btns = GameObject.FindGameObjectsWithTag($"Button{i}");
                GameObject door = GameObject.FindGameObjectWithTag($"Door{i}");

                if (btns == null || btns.Length == 0 || !door) // no objects found (but it is may be exception)
                    break;
                else
                {
                    buttons.Add(btns);
                    doors.Add(door);
                }
            }
            catch
            {
                break;
            }
        }
    }

    public void ReturnAction(InputAction.CallbackContext c)
    {
        if (c.phase != InputActionPhase.Started) return; // prevent returning more than 1 step if key has been pressed in time more than time of one press only

        if (previousSnakePartsPositions.Count == 0) return;

        ReturnSnake();
        ReturnLamp();

        for (int i = 0; i < enemies.Count; ++i)
            enemies[i].ReturnAction();

        ReturnDoorState();
    }

    private void ReturnSnake()
    {
        if (snake.Count > previousSnakePartsPositions[previousSnakePartsPositions.Count - 1].Item1.Length)
        {
            Destroy(snake[snake.Count - 1]);
            snake.RemoveAt(snake.Count - 1);
        }

        for (int i = 0; i < snake.Count; ++i)
            snake[i].transform.position = previousSnakePartsPositions[previousSnakePartsPositions.Count - 1].Item1[i];

        direction = previousSnakePartsPositions[previousSnakePartsPositions.Count - 1].Item2;

        previousSnakePartsPositions.RemoveAt(previousSnakePartsPositions.Count - 1);

        if (!canSnakeMove)
            canSnakeMove = true;

        EventAggregator.snakeMoving.Publish();

        DrawSnakeParts();
        MoveCameraToSnake();
        DrawButtons();
    }

    private void ReturnLamp()
    {
        if (previousLampsPositions.Count == 0)
            return;

        for (int i = 0; i < lamps.Length; ++i)
            lamps[i].SetActive(previousLampsPositions[previousLampsPositions.Count - 1][i]);

        previousLampsPositions.RemoveAt(previousLampsPositions.Count - 1);
    }

    private void ReturnDoorState()
    {
        for (int i = 0; i < buttons.Count; ++i)
        {
            try // maybe it can be deleted (try-catch)
            {
                if (previousDoorsStates[previousDoorsStates.Count - 1 - i].Item2)
                    doors[previousDoorsStates[previousDoorsStates.Count - 1 - i].Item1].SetActive(true);

                previousDoorsStates.RemoveAt(previousDoorsStates.Count - 1 - i);
            }
            catch
            {
                return;
            }
        }
    }

    private void MoveCameraToSnake() => transform.position = snake[0].transform.position + new Vector3(0, 0, transform.position.z); // To get more beauty image, camera background must be the same color as empty cell, so map will be bounded by barriers

    private void DrawSnakeParts()
    {
        DrawSnakeHead();
        DrawCenterSnakeParts();
        DrawEndSnakePart();
    }

    private void DrawSnakeHead()
    {
        switch (direction)
        {
            case Direction.down:
                snake[0].transform.rotation = Quaternion.Euler(0, 0, 180); break;
            case Direction.left:
                snake[0].transform.rotation = Quaternion.Euler(0, 0, 90); break;
            case Direction.right:
                snake[0].transform.rotation = Quaternion.Euler(0, 0, -90); break;
            case Direction.up:
                snake[0].transform.rotation = Quaternion.Euler(0, 0, 0); break;
        }
    }

    private void DrawEndSnakePart()
    {
        snake[snake.Count - 1].GetComponent<SpriteRenderer>().sprite = snakeStick;
        snake[snake.Count - 1].transform.rotation = Quaternion.Euler(0, 0, snake[snake.Count - 2].transform.position.x == snake[snake.Count - 1].transform.position.x ? 0 : 90);
    }

    private void DrawCenterSnakeParts() // this function check one part and its neighbours
    {
        for (int i = 1; i < snake.Count - 1; ++i)
        {
            // 3 in 1 in vertical
            if (snake[i - 1].transform.position.x != snake[i].transform.position.x && snake[i - 1].transform.position.y == snake[i].transform.position.y && snake[i + 1].transform.position.x != snake[i].transform.position.x && snake[i + 1].transform.position.y == snake[i].transform.position.y)
            {
                snake[i].GetComponent<SpriteRenderer>().sprite = snakeStick;
                snake[i].transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            // 3 in 1 in horizontal
            else if (snake[i - 1].transform.position.x == snake[i].transform.position.x && snake[i - 1].transform.position.y != snake[i].transform.position.y && snake[i + 1].transform.position.x == snake[i].transform.position.x && snake[i + 1].transform.position.y != snake[i].transform.position.y)
            {
                snake[i].GetComponent<SpriteRenderer>().sprite = snakeStick;
                snake[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                snake[i].GetComponent<SpriteRenderer>().sprite = snakeCorner;
                snake[i].transform.rotation = Quaternion.Euler(0, 0, GetCornerAngle(snake[i].transform.position, snake[i - 1].transform.position, snake[i + 1].transform.position));
            }
        }
    }

    private int GetCornerAngle(Vector3 c, Vector3 l, Vector3 r) // my favourite function
    {
        if ((c.x == l.x && c.y < l.y && c.x < r.x && c.y == r.y) || (c.x == r.x && c.y < r.y && c.x < l.x && c.y == l.y))
            return 90;
        else if ((c.x < r.x && c.y == r.y && c.x == l.x && c.y > l.y) || (c.x < l.x && c.y == l.y && c.x == r.x && c.y > r.y))
            return 0;
        else if ((c.x == r.x && c.y > r.y && c.x > l.x && c.y == l.y) || (c.x == l.x && c.y > l.y && c.x > r.x && c.y == r.y))
            return -90;
        else return 180;
    }


    public void MoveSnakeUp(InputAction.CallbackContext c)
    {
        if (c.phase != InputActionPhase.Started) return;

        if (direction == Direction.down) return;

        if (!canSnakeMove) return;

        SaveSnakeState();

        direction = Direction.up;

        Vector3 pos = snake[0].transform.position;

        ++pos.y;

        snake[0].transform.position = pos;

        MoveSnakeBody(new Vector3(pos.x, pos.y - 1, pos.z));
    }

    public void MoveSnakeDown(InputAction.CallbackContext c)
    {
        if (c.phase != InputActionPhase.Started) return;

        if (direction == Direction.up) return;

        if (!canSnakeMove) return;

        SaveSnakeState();

        direction = Direction.down;

        Vector3 pos = snake[0].transform.position;

        --pos.y;

        snake[0].transform.position = pos;

        MoveSnakeBody(new Vector3(pos.x, pos.y + 1, pos.z));
    }

    public void MoveSnakeLeft(InputAction.CallbackContext c)
    {
        if (c.phase != InputActionPhase.Started) return;

        if (direction == Direction.right) return;

        if (!canSnakeMove) return;

        SaveSnakeState();

        direction = Direction.left;

        Vector3 pos = snake[0].transform.position;

        --pos.x;

        snake[0].transform.position = pos;

        MoveSnakeBody(new Vector3(pos.x + 1, pos.y, pos.z));
    }

    public void MoveSnakeRight(InputAction.CallbackContext c)
    {
        if (c.phase != InputActionPhase.Started) return;

        if (direction == Direction.left) return;

        if (!canSnakeMove) return;

        SaveSnakeState();

        direction = Direction.right;

        Vector3 pos = snake[0].transform.position;

        ++pos.x;

        snake[0].transform.position = pos;

        MoveSnakeBody(new Vector3(pos.x - 1, pos.y, pos.z));
    }

    private void MoveSnakeBody(Vector3 p1)
    {
        MoveCameraToSnake();

        Vector3 p2;

        for (int i = 1; i < snake.Count; ++i)
        {
            p2 = snake[i].transform.position;
            snake[i].transform.position = p1;
            p1 = p2;
        }

        positionToNewSnakePart = p1;

        DrawSnakeParts();

        for (int i = 0; i < enemies.Count; ++i)
            enemies[i].Move();

        CheckSnake();

        EventAggregator.snakeMoving.Publish();
    }

    private void SaveLampsState()
    {
        bool[] state = new bool[lamps.Length];

        for (int i = 0; i < state.Length; ++i)
            state[i] = lamps[i].activeInHierarchy;

        previousLampsPositions.Add(state);
    }

    private void SaveSnakeState()
    {
        Vector3[] poss = new Vector3[snake.Count];

        for (int i = 0; i < snake.Count; ++i)
            poss[i] = snake[i].transform.position;

        previousSnakePartsPositions.Add((poss, direction));
    }

    private void IncreaseSnake()
    {
        GameObject newPart = Instantiate(snake[snake.Count - 1]);
        newPart.transform.position = positionToNewSnakePart;
        snake.Add(newPart);

        DrawSnakeParts();

        EventAggregator.getLight.Publish();
    }

    private void CheckSnake() // Order of calling functions must be sured
    {
        if (CheckToKilledByEnemy() || CheckToEatSelf() || CheckToReachBound() || CheckToReachDoors())
            canSnakeMove = false;

        SaveLampsState();

        if (CheckToEatLamp())
            IncreaseSnake();

        CheckToPressButtons();

        CheckToReachWinCell();
    }

    private bool CheckToEatSelf()
    {
        for (int i = 1; i < snake.Count; ++i)
            if (snake[0].transform.position == snake[i].transform.position)
                return true;

        return false;
    }

    private bool CheckToReachBound()
    {
        for (int i = 0; i < barriers.Length; ++i)
            if (snake[0].transform.position == barriers[i].transform.position)
                return true;

        return false;
    }

    private bool CheckToReachDoors()
    {
        for (int i = 0; i < doors.Count; ++i)
            if (doors[i].activeInHierarchy && snake[0].transform.position == doors[i].transform.position)
                return true;

        return false;
    }

    private bool CheckToKilledByEnemy()
    {
        for (int i = 0; i < snake.Count; ++i)
            for (int j = 0; j < enemies.Count; ++j)
                if (snake[i].transform.position == enemies[j].CurrentPosition)
                    return true;

        return false;
    }

    private bool CheckToEatLamp()
    {
        for (int i = 0; i < lamps.Length; ++i)
            if (lamps[i].activeInHierarchy && snake[0].transform.position == lamps[i].transform.position)
            {
                lamps[i].SetActive(false);
                return true;
            }

        return false;
    }

    private void CheckToPressButtons() // Shoot my leg
    {
        for (int i = 0; i < buttons.Count; ++i) // throw list of buttons by their color
        {
            if (!doors[i].activeInHierarchy)
            {
                for (int j = 0; j < buttons[i].Length; ++j)
                    buttons[i][j].GetComponent<SpriteRenderer>().sprite = buttonPressedSprites[i];

                continue;
            }

            int collisions = 0;

            for (int j = 0; j < buttons[i].Length; ++j) // throw all buttons of one color
                for (int k = 0; k < snake.Count; ++k) // throw snake
                    if (snake[k].transform.position == buttons[i][j].transform.position)
                    {
                        buttons[i][j].GetComponent<SpriteRenderer>().sprite = buttonPressedSprites[i];
                        ++collisions;
                        if (k == 0)
                            EventAggregator.pushButton.Publish();
                        break;
                    }
                    else if (k == snake.Count - 1)
                        buttons[i][j].GetComponent<SpriteRenderer>().sprite = buttonUnpressedSprites[i];

            previousDoorsStates.Add((i, doors[i].activeInHierarchy && collisions == buttons[i].Length));

            if (collisions == buttons[i].Length) // snake pressed all buttons of same color
            {
                if (i > doors.Count - 1) // ???
                    continue;

                doors[i].SetActive(false);
                EventAggregator.doorOpened.Publish();
            }
        }
    }

    private void DrawButtons()
    {
        for (int i = 0; i < buttons.Count; ++i) // throw list of buttons by their color
        {
            if (!doors[i].activeInHierarchy)
            {
                for (int j = 0; j < buttons[i].Length; ++j)
                    buttons[i][j].GetComponent<SpriteRenderer>().sprite = buttonPressedSprites[i];

                continue;
            }

            for (int j = 0; j < buttons[i].Length; ++j) // throw all buttons of one color
                for (int k = 0; k < snake.Count; ++k) // throw snake
                    if (snake[k].transform.position == buttons[i][j].transform.position)
                    {
                        buttons[i][j].GetComponent<SpriteRenderer>().sprite = buttonPressedSprites[i];
                        break;
                    }
                    else if (k == snake.Count - 1)
                        buttons[i][j].GetComponent<SpriteRenderer>().sprite = buttonUnpressedSprites[i];
        }
    }

    private void CheckToReachWinCell() // This function can be optimized
    {
        Debug.Log(winCells.Count);
        for (int i = 0; i < winCells.Count; ++i)
        {
            Debug.Log(winCells[i].name);
            if (snake[0].transform.position == winCells[i].transform.position)
            {
                if (winCells[i].name.Contains("Win"))
                    EventAggregator.puzzleEnded.Publish(PuzzleEnd.Algorithm);
                else if (winCells[i].name.Contains("Loss"))
                    EventAggregator.puzzleEnded.Publish(PuzzleEnd.Sabotage);

                return;
            }
        }
    }
}