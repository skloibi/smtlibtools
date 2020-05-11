using System.Numerics;
using smtsharp.Expressions;
using smtsharp.Expressions.Types;
using smtsharp.Expressions.Conditional;
using smtsharp.Expressions.Conversion;
using smtsharp.Expressions.Logic;
using smtsharp.Expressions.Arithmetic;
using smtsharp.Expressions.Comparison;
using smtsharp.Generator.Error;

using static smtsharp.Generator.Error.ErrorType;
using static smtsharp.Expressions.Types.FixedSizeBitVector;

using Type = smtsharp.Expressions.Types.Type;



using System;

namespace smtsharp.Generator {



public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _number = 2;
	public const int _option = 3;
	public const int _bool = 4;
	public const int maxT = 53;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public delegate IVariable<Type>? VariableSource(string name);
    public delegate bool Declaration(Type type, string name);
    public delegate void AssertionSink(IExpression<Bool> assertion);
    
    public VariableSource GetDeclaredVariable { get; set; }
    public Declaration Declare { get; set; }
    public AssertionSink Assert { get; set; }
     
    private uint ParseInt() {
        if (uint.TryParse(t.val, out var intVal))
            return intVal;
        Error(InvalidNumericLiteral, t.val);
        return 0;
    }
    
    private BigInteger ParseBV() {
        var str = t.val;
        
        if (!str.StartsWith("bv")) {
            Error(InvalidBitVectorLiteral, str);
            return -1;
        }
        
        var cleared = str.Substring(2);
        
        if (BigInteger.TryParse(cleared, out var bv))
            return bv;
        Error(InvalidBitVectorLiteral, str);
        return -1;
    }
    
    private bool CheckBool(IExpression<Type> expression) {
        if (expression is IExpression<Bool>)
            return true;
        Error(TypeMismatch, expression.Type, Bool.Type);
        return false;
    }
    
    private bool CheckBV(IExpression<Type> expression) {
        if (expression is IExpression<FixedSizeBitVector>)
            return true;
        Error(TypeMismatch, expression.Type, "FixedSizeBitVectors");
        return false;
    }
    
    private bool CheckFP(IExpression<Type> expression) {
        if (expression is IExpression<FloatingPoint>)
            return true;
        Error(TypeMismatch, expression.Type, "FloatingPoint");
        return false;
    }
    
    private bool CheckSameType(IExpression<Type> expected, IExpression<Type> actual) {
        if (expected.Type == actual.Type)
            return true;
        Error(TypeMismatch, $"{actual} : {actual.Type}", $"{expected} : {expected.Type}");
        return false;
    }
    
    private void Error(ErrorType err, params object[] args) {
        SemErr(err.Format(args));
    }
    
    private IVariable<Type> GetVariable(string name) {
        var variable = GetDeclaredVariable(name);
        if (variable == null) 
            Error(MissingVariableDeclaration, name);
        return variable;
    }
    
    private IExpression<Type> GenerateConditional(IExpression<Bool> condition, IExpression<Type> trueExpr, IExpression<Type> falseExpr) {
        if (!CheckSameType(trueExpr, falseExpr)) {
            return null;
        }
        
        return (trueExpr, falseExpr) switch {
            (IExpression<Bool> trueB, IExpression<Bool> falseB) => new Ite<Bool>(condition, trueB, falseB),
            (IExpression<FixedSizeBitVector> trueB, IExpression<FixedSizeBitVector> falseB) => new Ite<FixedSizeBitVector>(condition, trueB, falseB),
            (IExpression<FloatingPoint> trueB, IExpression<FloatingPoint> falseB) => new Ite<FloatingPoint>(condition, trueB, falseB),
            _ => new Ite<Type>(condition, trueExpr, falseExpr)
        };
    }

/*------------------------------------------------------------------------*
 *----- SCANNER DESCRIPTION ----------------------------------------------*
 *------------------------------------------------------------------------*/



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void SMTLIB() {
		while (la.kind == 5) {
			Get();
			switch (la.kind) {
			case 6: {
				Get();
				Expect(1);
				var name = t.val; 
				Expect(5);
				Expect(7);
				Type(out var type);
				if (type != null && !Declare(type, name)) 
				Error(VariableAlreadyDeclared, name); 
				break;
			}
			case 8: {
				Get();
				BoolExpr(out var assertion);
				if (assertion != null) 
				Assert(assertion); 
				break;
			}
			case 9: {
				Get();
				Expect(1);
				break;
			}
			case 10: {
				Get();
				Expect(3);
				Expect(1);
				break;
			}
			case 11: {
				Get();
				break;
			}
			case 12: {
				Get();
				break;
			}
			case 13: {
				Get();
				break;
			}
			case 14: {
				Get();
				break;
			}
			case 15: {
				Get();
				break;
			}
			case 16: {
				Get();
				break;
			}
			case 17: {
				Get();
				break;
			}
			default: SynErr(54); break;
			}
			Expect(7);
		}
	}

	void Type(out Type type) {
		type = null; 
		if (la.kind == 5) {
			Get();
			Expect(18);
			if (la.kind == 19) {
				BitVecType(out var bvType);
				type = bvType; 
			} else if (la.kind == 20) {
				FloatingPointType(out var fpType);
				type = fpType; 
			} else SynErr(55);
		} else if (la.kind == 4) {
			Get();
			type = Bool.Type; 
		} else SynErr(56);
	}

	void BoolExpr(out IExpression<Bool> expression) {
		expression = null; 
		Expr(out var expr);
		if (expr is IExpression<Bool> boolExpr) 
		expression = boolExpr; 
		else 
		Error(TypeMismatch, expr.Type, Bool.Type); 
		Console.WriteLine(expression); 
	}

	void BitVecType(out FixedSizeBitVector type) {
		Expect(19);
		Expect(2);
		type = Bv(ParseInt()); 
		Expect(7);
	}

	void FloatingPointType(out FloatingPoint type) {
		Expect(20);
		Expect(2);
		var eb = ParseInt();
		Expect(2);
		var sb = ParseInt(); 
		type = new FloatingPoint(eb, sb); 
		Expect(7);
	}

	void Expr(out IExpression<Type> expression) {
		expression = null; 
		if (la.kind == 1) {
			Get();
			expression = GetVariable(t.val); 
		} else if (la.kind == 5) {
			Get();
			if (la.kind == 18 || la.kind == 22 || la.kind == 23) {
				Literal(out var constExpr);
				expression = constExpr; 
			} else if (StartOf(1)) {
				UnaryExpr(out expression);
			} else if (StartOf(2)) {
				BinaryExpr(out expression);
			} else if (la.kind == 21) {
				Get();
				BoolExpr(out var cond);
				Expr(out var trueExpr);
				Expr(out var falseExpr);
				expression = GenerateConditional(cond, trueExpr, falseExpr); 
			} else if (la.kind == 50 || la.kind == 51 || la.kind == 52) {
				VariadicExpr(out expression);
			} else SynErr(57);
			Expect(7);
		} else SynErr(58);
	}

	void Literal(out IExpression<Type> constant) {
		constant = null; 
		if (la.kind == 18) {
			Get();
			Expect(1);
			var value = ParseBV(); 
			Expect(2);
			var width = ParseInt(); 
			var type = Bv(width); 
			constant = new Expressions.BitVector.Const(type, value); 
		} else if (la.kind == 22 || la.kind == 23) {
			Const lConst;
			if (la.kind == 22) {
				Get();
				lConst = true; 
			} else {
				Get();
				lConst = false; 
			}
			constant = lConst; 
		} else SynErr(59);
	}

	void UnaryExpr(out IExpression<Type> expression) {
		expression = null; 
		if (la.kind == 24) {
			Get();
			BoolExpr(out var op);
			expression = new Not(op); 
		} else if (la.kind == 25) {
			Get();
			BVExpr(out var op);
			expression = new Bitwise.Not(op); 
		} else if (la.kind == 26) {
			Get();
			BVExpr(out var op);
			expression = new Neg<FixedSizeBitVector>(op); 
		} else if (la.kind == 5) {
			Get();
			if (la.kind == 27) {
				Get();
				Expect(2);
				var endIdx = ParseInt(); 
				Expect(2);
				var startIdx = ParseInt(); 
				FixedSizeBitVector toType = null; 
				if (startIdx >= endIdx) 
				Error(InvalidOperand, endIdx, "extract"); 
				else 
				toType = Bv(endIdx - startIdx + 1); 
				Expect(7);
				BVExpr(out var op);
				if (toType != null) 
				expression = new Extract(toType, op, startIdx); 
			} else if (la.kind == 18) {
				Get();
				if (la.kind == 28) {
					Get();
					Expect(2);
					var toBits = ParseInt(); 
					var toType = Bv(toBits); 
					Expect(7);
					BVExpr(out var op);
					expression = new SignExtend(toType, op); 
				} else if (la.kind == 29) {
					Get();
					Expect(2);
					var toBits = ParseInt(); 
					var toType = Bv(toBits); 
					Expect(7);
					BVExpr(out var op);
					expression = new SignExtend(toType, op); 
				} else SynErr(60);
			} else SynErr(61);
		} else SynErr(62);
	}

	void BinaryExpr(out IExpression<Type> expression) {
		expression = null; 
		switch (la.kind) {
		case 30: {
			Get();
			BoolExpr(out var a);
			BoolExpr(out var b);
			if (CheckSameType(a, b)) expression = new Implies(a, b); 
			break;
		}
		case 31: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Bitwise.And(a, b); 
			break;
		}
		case 32: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Bitwise.Or(a, b); 
			break;
		}
		case 33: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Bitwise.XOr(a, b); 
			break;
		}
		case 34: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Add<FixedSizeBitVector>(a, b); 
			break;
		}
		case 35: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Mul<FixedSizeBitVector>(a, b); 
			break;
		}
		case 36: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Div<FixedSizeBitVector>(a, b); 
			break;
		}
		case 37: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new UDiv(a, b); 
			break;
		}
		case 38: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Rem<FixedSizeBitVector>(a, b); 
			break;
		}
		case 39: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new URem(a, b); 
			break;
		}
		case 40: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Bitwise.Shl(a, b); 
			break;
		}
		case 41: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Bitwise.LShr(a, b); 
			break;
		}
		case 42: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Lt<FixedSizeBitVector>(a, b); 
			break;
		}
		case 43: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Not(new Lt<FixedSizeBitVector>(b, a)); 
			break;
		}
		case 44: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Lt<FixedSizeBitVector>(b, a); 
			break;
		}
		case 45: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Not(new Lt<FixedSizeBitVector>(a, b)); 
			break;
		}
		case 46: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new ULt(a, b); 
			break;
		}
		case 47: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Not(new ULt(b, a)); 
			break;
		}
		case 48: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new ULt(b, a); 
			break;
		}
		case 49: {
			Get();
			BVExpr(out var a);
			BVExpr(out var b);
			if (CheckSameType(a, b)) expression = new Not(new ULt(a, b)); 
			break;
		}
		default: SynErr(63); break;
		}
	}

	void VariadicExpr(out IExpression<Type> expression) {
		expression = null; 
		if (la.kind == 50 || la.kind == 51) {
			IExpression<Bool> expr; 
			if (la.kind == 50) {
				Get();
				BoolExpr(out expr);
				BoolExpr(out var op);
				expr = new And(expr, op); 
				while (la.kind == 1 || la.kind == 5) {
					BoolExpr(out op);
					expr = new And(expr, op); 
				}
			} else {
				Get();
				BoolExpr(out expr);
				BoolExpr(out var op);
				expr = new And(expr, op); 
				while (la.kind == 1 || la.kind == 5) {
					BoolExpr(out op);
					expr = new And(expr, op); 
				}
			}
			expression = expr; 
		} else if (la.kind == 52) {
			Get();
			Expr(out var expr);
			Expr(out var op);
			if (CheckSameType(expr, op)) expr = new Eq<Type>(expr, op); 
			while (la.kind == 1 || la.kind == 5) {
				Expr(out op);
				if (CheckSameType(expr, op)) expr = new Eq<Type>(expr, op); 
			}
			expression = expr; 
		} else SynErr(64);
	}

	void BVExpr(out IExpression<FixedSizeBitVector> expression) {
		expression = null; 
		Expr(out var expr);
		if (expr is IExpression<FixedSizeBitVector> bvExpr) 
		expression = bvExpr; 
		else 
		Error(TypeMismatch, expr.Type, "FixedSizeBitVector"); 
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		SMTLIB();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_x,_x, _x,_x,_x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "number expected"; break;
			case 3: s = "option expected"; break;
			case 4: s = "bool expected"; break;
			case 5: s = "\"(\" expected"; break;
			case 6: s = "\"declare-fun\" expected"; break;
			case 7: s = "\")\" expected"; break;
			case 8: s = "\"assert\" expected"; break;
			case 9: s = "\"set-logic\" expected"; break;
			case 10: s = "\"set-option\" expected"; break;
			case 11: s = "\"push\" expected"; break;
			case 12: s = "\"pop\" expected"; break;
			case 13: s = "\"reset\" expected"; break;
			case 14: s = "\"check-sat\" expected"; break;
			case 15: s = "\"get-model\" expected"; break;
			case 16: s = "\"get-value\" expected"; break;
			case 17: s = "\"exit\" expected"; break;
			case 18: s = "\"_\" expected"; break;
			case 19: s = "\"BitVec\" expected"; break;
			case 20: s = "\"FloatingPoint\" expected"; break;
			case 21: s = "\"ite\" expected"; break;
			case 22: s = "\"true\" expected"; break;
			case 23: s = "\"false\" expected"; break;
			case 24: s = "\"not\" expected"; break;
			case 25: s = "\"bvnot\" expected"; break;
			case 26: s = "\"bvneg\" expected"; break;
			case 27: s = "\"extract\" expected"; break;
			case 28: s = "\"sign_extend\" expected"; break;
			case 29: s = "\"zero_extend\" expected"; break;
			case 30: s = "\"=>\" expected"; break;
			case 31: s = "\"bvand\" expected"; break;
			case 32: s = "\"bvor\" expected"; break;
			case 33: s = "\"bvxor\" expected"; break;
			case 34: s = "\"bvadd\" expected"; break;
			case 35: s = "\"bvmul\" expected"; break;
			case 36: s = "\"bvsdiv\" expected"; break;
			case 37: s = "\"bvudiv\" expected"; break;
			case 38: s = "\"bvsrem\" expected"; break;
			case 39: s = "\"bvurem\" expected"; break;
			case 40: s = "\"bvshl\" expected"; break;
			case 41: s = "\"bvlshr\" expected"; break;
			case 42: s = "\"bvslt\" expected"; break;
			case 43: s = "\"bvsle\" expected"; break;
			case 44: s = "\"bvsgt\" expected"; break;
			case 45: s = "\"bvsge\" expected"; break;
			case 46: s = "\"bvult\" expected"; break;
			case 47: s = "\"bvule\" expected"; break;
			case 48: s = "\"bvugt\" expected"; break;
			case 49: s = "\"bvuge\" expected"; break;
			case 50: s = "\"and\" expected"; break;
			case 51: s = "\"or\" expected"; break;
			case 52: s = "\"=\" expected"; break;
			case 53: s = "??? expected"; break;
			case 54: s = "invalid SMTLIB"; break;
			case 55: s = "invalid Type"; break;
			case 56: s = "invalid Type"; break;
			case 57: s = "invalid Expr"; break;
			case 58: s = "invalid Expr"; break;
			case 59: s = "invalid Literal"; break;
			case 60: s = "invalid UnaryExpr"; break;
			case 61: s = "invalid UnaryExpr"; break;
			case 62: s = "invalid UnaryExpr"; break;
			case 63: s = "invalid BinaryExpr"; break;
			case 64: s = "invalid VariadicExpr"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}