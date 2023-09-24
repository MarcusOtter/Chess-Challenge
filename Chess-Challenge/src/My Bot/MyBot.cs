using System;
using ChessChallenge.API;

public static class Thingy
{
    public static Piece DefendedBy(this Board board)
    {
        // return board.
        return new Piece();
    }
} 

public class MyBot : IChessBot
{
    private ulong largeNumber = 12345678901234567890;
    
    public MyBot()
    {
        // Console.WriteLine(largeDoubles[0].ToString("0.0"));
        // Console.WriteLine(largeNumber.ToString("0.0"));
    }
    
    public Move Think(Board board, Timer timer)
    {
        var random = new Random();
        var moves = board.GetLegalMoves();
        foreach (var move in moves)
        {
            // Take free pieces
            if (move.IsCapture && !board.SquareIsAttackedByOpponent(move.TargetSquare))
            {
                return move;
            }

            // If we can get out of an attack by moving
            if (board.SquareIsAttackedByOpponent(move.StartSquare) &&
                !board.SquareIsAttackedByOpponent(move.TargetSquare))
            {
                // And we are the king
                if (move.MovePieceType == PieceType.King)
                {
                    return move;
                }

                // Or we are not already defended
                board.TrySkipTurn();
                var isDefended = board.SquareIsAttackedByOpponent(move.StartSquare);
                board.UndoSkipTurn();

                if (!isDefended)
                {
                    return move;
                }
            }
        }

        var count = 0;
        var randomMove = moves[random.Next(0, moves.Length)];
        while (board.SquareIsAttackedByOpponent(randomMove.TargetSquare) && count < moves.Length)
        {
            randomMove = moves[random.Next(0, moves.Length)];
            count++;
            // Console.WriteLine("Random move: " + randomMove + "count: " + count);
        }
        
        return randomMove;
    }
}
