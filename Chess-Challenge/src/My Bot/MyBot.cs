using System;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    
    private double[] largeDoubles = {
        1234567890123456789012345678901234567890123d,
        0x0000000000000000,
    };

    private ulong largeNumber = 12345678901234567890;
    
    public MyBot()
    {
        Console.WriteLine(largeDoubles[0].ToString("0.0"));
        Console.WriteLine(largeNumber.ToString("0.0"));
    }
    
    public Move Think(Board board, Timer timer)
    {
        var moves = board.GetLegalMoves();
        foreach (var move in moves)
        {
            if (move.IsCapture && !board.SquareIsAttackedByOpponent(move.TargetSquare))
            {
                return move;
            }
        }

        // var queenMoves = moves.Where(x => x.MovePieceType == PieceType.Queen);
        // var captureMove = moves.FirstOrDefault(x => x.IsCapture);
        return moves[new Random().Next(0, moves.Length)];
    }
}
