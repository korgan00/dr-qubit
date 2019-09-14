using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameState : MonoBehaviour {

    private static GameState _instance;

    public GameState instance {
        get { return _instance; }
    }

    [SerializeField]
    private Vector2Int _boardSize;
    [SerializeField]
    public float speed = 1.0f;
    [SerializeField]
    public float stepsPerBlock = 1.0f;
    [SerializeField]
    public int qubitsInRow = 3;
    [SerializeField]
    private QuBitMovement _instanciableQubit;
    [SerializeField]
    private ClassicBit _instanciableClassicBit;
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    public InitialBoardState _initialStateGenerator;

    private QuBitMovement _currentQubit;
    private IBit[,] _board;
    private ClassicBit[,] _initialBoard;
    private float _currentMovementColdDown;
    private QuGatesList _gatesList;

    private float movementColdDown {
        get { return 1f / stepsPerBlock; }
    }

    private bool finished;

    private void Start() {
        _currentMovementColdDown = movementColdDown;
        SpawnNextQubit();
        _board = new IBit[8,16];
        _initialBoard = _initialStateGenerator.getInitialBoard(_boardSize);

        for (int i = 0; i < _boardSize.x; i++) {
            for (int j = 0; j < _boardSize.y; j++) {
                if (_initialBoard[i, j] != null) {
                    ClassicBit cBit = Instantiate(_instanciableClassicBit, _spawnPoint.position, _spawnPoint.rotation);
                    cBit.value = _initialBoard[i, j].value;
                    cBit.transform.position = cBit.transform.position + (Vector3.right * i);
                    cBit.transform.position = cBit.transform.position - (Vector3.up * (_boardSize.y - j));
                    _board[i, j] = cBit;
                }
            }
        }

        _gatesList = GetComponent<QuGatesList>();
    }

    private void Update() {
        _currentMovementColdDown -= Time.deltaTime * speed;

        if (Input.GetKeyDown(KeyCode.A)) {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            MoveRight();
        }
        if (Input.GetKey(KeyCode.S)) {
            _currentMovementColdDown = 0;
        }


        if (Input.GetKeyDown(KeyCode.X)) {
            _currentQubit.GetComponent<QuBit>().ApplyGate(_gatesList.quGatesList[0]);
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            _currentQubit.GetComponent<QuBit>().ApplyGate(_gatesList.quGatesList[1]);
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            _currentQubit.GetComponent<QuBit>().ApplyGate(_gatesList.quGatesList[2]);
        }

        while (_currentMovementColdDown <= 0) {
            _currentMovementColdDown += movementColdDown;
            _currentQubit.MoveVertical(1f / stepsPerBlock);
            Vector2Int boardPosition = new Vector2Int(Mathf.FloorToInt(_currentQubit.position.x), Mathf.FloorToInt(_currentQubit.position.y));

            if (boardPosition.y < 0 || _board[boardPosition.x, boardPosition.y] != null) {
                _currentQubit.MoveVertical(-1f / stepsPerBlock);

                Vector2Int newBoardPosition = new Vector2Int(Mathf.FloorToInt(_currentQubit.position.x), Mathf.FloorToInt(_currentQubit.position.y));
                _board[newBoardPosition.x, newBoardPosition.y] = _currentQubit.GetComponent<QuBit>();

                _currentQubit.StopMoving();
                EvaluateGameState();
                if (!finished) {
                    SpawnNextQubit();
                }
            }
        }
    }

    public void MoveRight() {
        if (_currentQubit.position.x + 1 < _boardSize.x) {
            _currentQubit.MoveHorizontal(1f);
        }
    }

    public void MoveLeft() {
        if (_currentQubit.position.x - 1 >= 0) {
            _currentQubit.MoveHorizontal(-1f);
        }
    }

    private void SpawnNextQubit() {
        _currentQubit = Instantiate(_instanciableQubit, _spawnPoint.position, _spawnPoint.rotation);
        _currentQubit.position.y = _boardSize.y;
        _currentQubit.MoveHorizontal(_boardSize.x / 2);
    }
    
    private void EvaluateGameState() {
        VerticalDestruction();
        HorizontalDestruction();
    }

    private bool VerticalDestruction() {
        int consecutives;
        int lastBit = -1;
        bool destruction = false;
        for (int i = 0; i < _boardSize.x; i ++) {
            consecutives = 1;
            for (int j = 0; j < _boardSize.y; j++) {
                IBit currBit = _board[i, j];
                if (currBit == null) {
                    consecutives = 1;
                    lastBit = -1;
                    continue;
                }

                if (lastBit == currBit.Value()) {
                    if (currBit is QuBit) {
                        consecutives++;
                    }
                } else {
                    consecutives = 1;
                }

                lastBit = currBit.Value();

                if (consecutives >= qubitsInRow) {
                    DestroyCoalecentsVertical(new Vector2Int(i,j), lastBit);
                    destruction = true;
                }
            }
        }
        return destruction;
    }

    private void DestroyCoalecentsVertical(Vector2Int pos, int value) {
        int y = pos.y;
        while (_board[pos.x, y] != null && _board[pos.x, y].Value() == value) {
            Destroy((_board[pos.x, y] as MonoBehaviour).gameObject);
            _board[pos.x, y] = null;
            y++;
            if (y > _boardSize.y) {
                break;
            }
        }

        y = pos.y - 1;
        while (_board[pos.x, y] != null && _board[pos.x, y].Value() == value) {
            Destroy((_board[pos.x, y] as MonoBehaviour).gameObject);
            _board[pos.x, y] = null;
            y--;
            if (y < 0) {
                break;
            }
        }
    }


    private bool HorizontalDestruction() {
        int consecutives;
        int lastBit = -1;
        bool destruction = false;
        for (int j = 0; j < _boardSize.y; j++) {
            consecutives = 1;
            for (int i = 0; i < _boardSize.x; i++) {
                IBit currBit = _board[i, j];
                if (currBit == null) {
                    consecutives = 1;
                    lastBit = -1;
                    continue;
                }

                if (lastBit == currBit.Value()) {
                    if (currBit is QuBit) {
                        consecutives++;
                    }
                } else {
                    consecutives = 1;
                }

                lastBit = currBit.Value();

                if (consecutives >= qubitsInRow) {
                    DestroyCoalecentsHorizontal(new Vector2Int(i, j), lastBit);
                    destruction = true;
                }
            }
        }
        return destruction;
    }

    private void DestroyCoalecentsHorizontal(Vector2Int pos, int value) {
        int x = pos.x;
        while (_board[x, pos.y] != null && _board[x, pos.y].Value() == value) {
            Destroy((_board[x, pos.y] as MonoBehaviour).gameObject);
            _board[x, pos.y] = null;
            x++;
            if (x > _boardSize.x) {
                break;
            }
        }

        x = pos.x - 1;
        while (_board[x, pos.y] != null && _board[x, pos.y].Value() == value) {
            Destroy((_board[x, pos.y] as MonoBehaviour).gameObject);
            _board[x, pos.y] = null;
            x--;
            if (x < 0) {
                break;
            }
        }
    }
}
