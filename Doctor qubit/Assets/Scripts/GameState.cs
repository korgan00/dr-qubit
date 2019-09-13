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
    private QubitMovement _instanciableQubit;
    [SerializeField]
    private Transform _spawnPoint;

    private QubitMovement _currentQubit;
    private GameObject[,] _board;
    private float _currentMovementColdDown;
    private float movementColdDown {
        get { return 1f / stepsPerBlock; }
    }

    private bool finished;

    private void Start() {
        _currentMovementColdDown = movementColdDown;
        SpawnNextQubit();
        _board = new GameObject[8,16];
    }


    // Update is called once per frame
    private void Update() {
        _currentMovementColdDown -= Time.deltaTime * speed;

        
        if (Input.GetKeyDown(KeyCode.A)) {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            MoveRight();
        }
        //if (Input.GetKeyDown)

        

        while (_currentMovementColdDown < 0) {
            _currentMovementColdDown += movementColdDown;
            _currentQubit.MoveVertical(1f / stepsPerBlock);
            Vector2Int boardPosition = new Vector2Int(Mathf.FloorToInt(_currentQubit.position.x), Mathf.FloorToInt(_currentQubit.position.y));
            if (boardPosition.y < 0 || _board[boardPosition.x, boardPosition.y] != null) {
                _currentQubit.MoveVertical(-1f / stepsPerBlock);
                Vector2Int newBoardPosition = new Vector2Int(Mathf.FloorToInt(_currentQubit.position.x), Mathf.FloorToInt(_currentQubit.position.y));
                _board[newBoardPosition.x, newBoardPosition.y] = _currentQubit.gameObject;
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

    }
}
