using System;
using System.Collections.Generic;
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
	private Dictionary<PieceType, int> pieceValues = new()
	{
		{ PieceType.None, -1 },
		{ PieceType.Pawn, 1 },
		{ PieceType.Knight, 3 },
		{ PieceType.Bishop, 3 },
		{ PieceType.Rook, 5 },
		{ PieceType.Queen, 9 },
		{ PieceType.King, 100 },
	};

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

			// If we are attacking a piece that is worth more than us
			var attackedPiece = board.GetPiece(move.TargetSquare);
			if (pieceValues[move.MovePieceType] < pieceValues[attackedPiece.PieceType])
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
				
				board.TrySkipTurn();
				var isDefended = board.SquareIsAttackedByOpponent(move.StartSquare);
				board.UndoSkipTurn();
				
				
				// TODO: Check value of us vs the attacker here
				
				
				
				// Or we are defended by a less valuable piece, or not defended at all
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
