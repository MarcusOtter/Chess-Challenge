using System;
using System.Collections.Generic;
using System.Linq;
using ChessChallenge.API;

public class MyBot : IChessBot
{
	private readonly Dictionary<PieceType, int> _pieceValues = new()
	{
		{ PieceType.None, 0 },
		{ PieceType.Pawn, 1 },
		{ PieceType.Knight, 3 },
		{ PieceType.Bishop, 3 },
		{ PieceType.Rook, 5 },
		{ PieceType.Queen, 9 },
		{ PieceType.King, 100 },
	};

	public Move Think(Board board, Timer timer)
	{
		// board = Board.CreateBoardFromFEN("r1bqk2r/1pp3pp/p1nb1n2/1N1p1p2/3Pp3/2P1P3/PP1QNPPP/1RB1KB1R w Kkq - 0 1");
		
		var random = new Random();
		var moves = board.GetLegalMoves();
		foreach (var move in moves)
		{
			// Take free pieces
			if (move.IsCapture && !board.SquareIsAttackedByOpponent(move.TargetSquare))
			{
				Console.WriteLine("Free piece: " + move);
				return move;
			}

			// If we are attacking a piece that is worth more than us
			var attackedPiece = board.GetPiece(move.TargetSquare);
			if (_pieceValues[move.MovePieceType] < _pieceValues[attackedPiece.PieceType])
			{
				Console.WriteLine("Attacking piece worth more than us: " + move);
				return move;
			}
			
			board.TrySkipTurn();
			var isDefended = board.SquareIsAttackedByOpponent(move.StartSquare);
			var lowestValuePieceAttackingUs = int.MaxValue;
			foreach (var opponentMove in board.GetLegalMoves())
			{
				if (opponentMove.TargetSquare != move.StartSquare) continue;
				if (_pieceValues[opponentMove.MovePieceType] < lowestValuePieceAttackingUs)
				{
					lowestValuePieceAttackingUs = _pieceValues[opponentMove.MovePieceType];
				}
			}
			board.UndoSkipTurn();
			
			// Defend undefended & attacked pieces, or if we are being attacked by a piece with lower value than us
			if (!isDefended && lowestValuePieceAttackingUs < int.MaxValue || lowestValuePieceAttackingUs < _pieceValues[move.MovePieceType])
			{
				// TODO This crashes the bot for some reason
				// board.MakeMove(move);
				// board.TrySkipTurn();
				if (board.SquareIsAttackedByOpponent(move.TargetSquare)) continue;
				// board.UndoSkipTurn();
				// board.UndoMove(move);
				//
				Console.WriteLine("Defending without thinking much: " + move);
				return move;
			}
			
			// If we can get out of an attack by moving to a safe square
			if (board.SquareIsAttackedByOpponent(move.StartSquare) &&
			    !board.SquareIsAttackedByOpponent(move.TargetSquare))
			{
				// TODO: Check value of us vs the attacker here
				
				// And we are not defended
				if (!isDefended)
				{
					Console.WriteLine("Getting out of attack: " + move);
					return move;
				}
			}
		}

		var randomMoves = moves.OrderBy(x => random.Next());
		foreach (var move in randomMoves)
		{
			if (board.SquareIsAttackedByOpponent(move.TargetSquare)) continue;

			Console.WriteLine("Random move: " + move);
			return move;
		}

		Console.WriteLine("Probably terrible random move: " + randomMoves.FirstOrDefault());
		return randomMoves.FirstOrDefault();
	}
}
