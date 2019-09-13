using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class InitialBoardState : ScriptableObject {

    public Vector2Int boardSize;
    //public int[,] elements => new int[boardSize[0], boardSize[1]];

    [SerializeField]
    public int numClassiBits = 2;

    [SerializeField]
    public int maxToShowBits = 5;

    public ClassicBit[,] getInitialBoard(Vector2Int boardSize)
    {
        ClassicBit [,] board = new ClassicBit[boardSize[0], boardSize[1]];

        for (int i = 0; i < numClassiBits; i++)
        {

            Vector2Int randomCoord = calculateRandomCoord();

            while(board[randomCoord.x, randomCoord.y] || randomCoord.y > maxToShowBits)
            {
                randomCoord = calculateRandomCoord();
            }

            board[randomCoord.x, randomCoord.y] = new ClassicBit();

        }

        return board;
    }

    private Vector2Int calculateRandomCoord()
    {
        Random rnd = new Random();
        int randomX = rnd.Next(0, boardSize[0]);
        int randomY = rnd.Next(0, boardSize[1]);

        return new Vector2Int(randomX, randomY);
    }
}
