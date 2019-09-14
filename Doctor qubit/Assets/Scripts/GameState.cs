using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    public Image _xButton;
    [SerializeField]
    public Image _zButton;
    [SerializeField]
    public Image _hButton;

    private QuBitMovement _currentQubit;
    private IBit[,] _board;
    private ClassicBit[,] _initialBoard;
    private float _currentMovementColdDown;
    private QuGatesList _gatesList;

    public Canvas winCanvas;
    public Canvas loseCanvas;

    [Header("Sounds")]
    public AudioSource music;
    public AudioSource down;
    public AudioSource move;
    public AudioSource clearSound;
    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource gate;

    private float movementColdDown {
        get { return 1f / stepsPerBlock; }
    }

    private bool finished;

    private void Start() {
        _gatesList = GetComponent<QuGatesList>();
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


        winCanvas.gameObject.SetActive(false);
    }

    private void Update() {
        try {
            _currentMovementColdDown -= Time.deltaTime * speed;

            if (Input.GetKeyDown(KeyCode.A)) {
                move.Play();
                MoveLeft();
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                move.Play();
                MoveRight();
            }
            if (Input.GetKey(KeyCode.S)) {
                down.Play();
                _currentMovementColdDown = 0;
            }

            if (Input.GetKeyDown(KeyCode.X)) {
                _currentQubit.GetComponent<QuBit>().ApplyGate(_gatesList.quGatesList[0]);
                _xButton.color = Color.white;
                _zButton.color = Color.gray;
                _hButton.color = Color.gray;
                gate.Play();
            }
            if (Input.GetKeyDown(KeyCode.Z)) {
                _currentQubit.GetComponent<QuBit>().ApplyGate(_gatesList.quGatesList[1]);
                _xButton.color = Color.gray;
                _zButton.color = Color.white;
                _hButton.color = Color.gray;
                gate.Play();
            }
            if (Input.GetKeyDown(KeyCode.H)) {
                _currentQubit.GetComponent<QuBit>().ApplyGate(_gatesList.quGatesList[2]);
                _xButton.color = Color.gray;
                _zButton.color = Color.gray;
                _hButton.color = Color.white;
                gate.Play();
            }

            while (_currentMovementColdDown <= 0) {
                _currentMovementColdDown += movementColdDown;
                _currentQubit.MoveVertical(1f / stepsPerBlock);
                Vector2Int boardPosition = new Vector2Int(Mathf.FloorToInt(_currentQubit.position.x), Mathf.FloorToInt(_currentQubit.position.y));

                if (boardPosition.y < 0 || _board[boardPosition.x, boardPosition.y] != null) {
                    _currentQubit.MoveVertical(-1f / stepsPerBlock);

                    Vector2Int newBoardPosition = new Vector2Int(Mathf.FloorToInt(_currentQubit.position.x), Mathf.FloorToInt(_currentQubit.position.y));
                    try {
                        _board[newBoardPosition.x, newBoardPosition.y] = _currentQubit.GetComponent<QuBit>();
                    } catch  (IndexOutOfRangeException ex) {
                        loseSound?.Play();
                        loseCanvas.gameObject.SetActive(true);
                        enabled = false;
                        music.Stop();
                    }

                    _currentQubit.StopMoving();
                    EvaluateGameState();
                    if (!finished) {
                        SpawnNextQubit();
                    }
                }
            }
        } catch (IndexOutOfRangeException ex) {
            loseSound?.Play();
            loseCanvas.gameObject.SetActive(true);
            enabled = false;
            music.Stop();
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

        QuBit q = _currentQubit.GetComponent<QuBit>();

        float rnd1 = UnityEngine.Random.value;
        float rnd2 = UnityEngine.Random.value;
        float rnd3 = UnityEngine.Random.value;

        Debug.Log($"{rnd1}   {rnd2}  {rnd3}");
        if (rnd1 > 0.5) {
            q.ApplyGate(_gatesList.quGatesList[0]);
        } else if (rnd2 > 0.5f) {
            q.ApplyGate(_gatesList.quGatesList[2]);
            if (rnd3 > 0.5f) {
                q.ApplyGate(_gatesList.quGatesList[1]);
            }
        }
    }
    
    private void EvaluateGameState() {
        bool destruction = VerticalDestruction();
        destruction |= HorizontalDestruction();

        if (destruction) {
            clearSound.Play();
        }

        EvaluateWinCondition();
        EvaluateLooseCondition();
    }

    private void EvaluateLooseCondition() {
        bool lose = true;
        for (int i = 0; i < _boardSize.x; i++) {
            if (_board[i, 3] == null) {
                lose = false;
            }
        }

        if (lose) {
            loseSound.Play();
            loseCanvas.gameObject.SetActive(true);
            enabled = false;
            music.Stop();
        }
    }

    private void EvaluateWinCondition() {
        bool win = true;
        for (int i = 0; i < _boardSize.x; i++) {
            for (int j = 0; j < _boardSize.y; j++) {
                IBit currBit = _board[i, j];
                if (currBit is ClassicBit) {
                    win = false;
                }
            }
        }

        if (win) {
            winSound.Play();
            winCanvas.gameObject.SetActive(true);
            enabled = false;
        }
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
