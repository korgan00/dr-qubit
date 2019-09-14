using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "InitialBoardState", menuName = "Board")]
public class InitialBoardState : ScriptableObject {

    [SerializeField]
    public int numClassicBits = 2;

    [SerializeField]
    public int maxHeightBits = 5;

    private Random rnd;

    public ClassicBit[,] getInitialBoard(Vector2Int boardSize) {
        ClassicBit[,] board = new ClassicBit[boardSize.x, boardSize.y];
        rnd = new Random();

        for (int i = 0; i < numClassicBits; i++) {
            Vector2Int randomCoord = calculateRandomCoord(boardSize);

            while (board[randomCoord.x, randomCoord.y] != null) {
                randomCoord = calculateRandomCoord(boardSize);
            }
            
            board[randomCoord.x, randomCoord.y] = new GameObject().AddComponent<ClassicBit>();
            board[randomCoord.x, randomCoord.y].value = rnd.Next(0, 2) == 1;
        }

        return board;
    }

    private Vector2Int calculateRandomCoord(Vector2Int boardSize) {
        int randomX = rnd.Next(0, boardSize.x);
        int randomY = rnd.Next(0, Mathf.Min(boardSize.y, maxHeightBits));

        return new Vector2Int(randomX, randomY);
    }
}
