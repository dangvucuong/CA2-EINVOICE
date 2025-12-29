using System.Linq.Expressions;
using System.Text;

namespace Common
{
    public static partial class myExtension
    {
        public static double ConvertToExpression(this string input)
        {
            try
            {
                Scanner scanner = new Scanner(input);
                Parser parser = new Parser(scanner);
                Expression expr = parser.Parse();

                Delegate func = Expression.Lambda(expr).Compile();
                var result = func.DynamicInvoke();
                var typeName= result.GetType().Name;
                if (typeName== "Boolean")
                {
                    return result.ConvertToBoolean() ? 1 : 0;
                }
                return func.DynamicInvoke().ConvertToDouble();
            }
            catch (Exception ex)
            {
             
                throw ex;
            }
        }
    }
    class Parser
    {

        private Scanner _scanner;
        private Token _token;

        public Parser(Scanner scanner)
        {
            this._scanner = scanner;
        }

        public Expression Parse()
        {
            _token = _scanner.Next();

            Expression expr = CompareExpression();

            Check(Token.EOF);

            return expr;
        }

        private bool Read()
        {
            if (_token.Type == TokenType.EOF)
                return false;

            _token = _scanner.Next();

            return _token.Type != TokenType.EOF;
        }

        private bool Check(Token token)
        {
            if (!_token.Equals(token))
                throw new Exception("'" + token + "' expected.");

            return Read();
        }

        private Expression CompareExpression()
        {
            Expression lhs = AddExpression();

            if (_token.Type == TokenType.Equal ||
                _token.Type == TokenType.GreaterThan || _token.Type == TokenType.LessThan ||
                _token.Type == TokenType.GreaterThanOrEqual || _token.Type == TokenType.LessThanOrEqual)
            {
                TokenType type = _token.Type;

                Read();

                Expression rhs = CompareExpression();

                switch (type)
                {
                    case TokenType.Equal:
                        lhs = Expression.Equal(lhs, rhs);
                        break;
                    case TokenType.GreaterThan:
                        lhs = Expression.GreaterThan(lhs, rhs);
                        break;
                    case TokenType.GreaterThanOrEqual:
                        lhs = Expression.GreaterThanOrEqual(lhs, rhs);
                        break;
                    case TokenType.LessThan:
                        lhs = Expression.LessThan(lhs, rhs);
                        break;
                    case TokenType.LessThanOrEqual:
                        lhs = Expression.LessThanOrEqual(lhs, rhs);
                        break;
                }

            }

            return lhs;
        }

        private Expression AddExpression()
        {
            Expression lhs = MultiplyExpression();

            while (_token.Type == TokenType.Add || _token.Type == TokenType.Subtract)
            {
                TokenType type = _token.Type;

                Read();

                Expression rhs = MultiplyExpression();

                lhs = type == TokenType.Add ? Expression.Add(lhs, rhs) : Expression.Subtract(lhs, rhs);

            }

            return lhs;
        }

        private Expression MultiplyExpression()
        {
            Expression lhs = UnaryExpression();

            while (_token.Type == TokenType.Multiply || _token.Type == TokenType.Divide)
            {
                TokenType type = _token.Type;

                Read();

                Expression rhs = UnaryExpression();

                lhs = type == TokenType.Multiply ? Expression.Multiply(lhs, rhs) : Expression.Divide(lhs, rhs);

            }

            return lhs;
        }

        private Expression UnaryExpression()
        {
            Expression ret = null;

            if (_token.Type == TokenType.Add || _token.Type == TokenType.Subtract)
            {
                TokenType type = _token.Type;
                Read();

                Expression expr = UnaryExpression();

                ret = type == TokenType.Add ? Expression.UnaryPlus(expr) : Expression.Negate(expr);
            }
            else
                ret = ValueExpression();

            return ret;
        }

        private Expression ValueExpression()
        {
            Expression ret = null;

            if (_token.Type == TokenType.Number)
            {
                ret = Expression.Constant(double.Parse(_token.Value));

                Read();
            }
            else if (_token.Type == TokenType.LParen)
            {
                Read();

                ret = CompareExpression();

                Check(Token.RParen);
            }
            else
                throw new Exception("Unexpected token: " + _token);

            return ret;
        }
    }
    class Scanner
    {
        const char EOF = '\0';
        string _input;
        int _index = -1;
        char _ch;

        public Scanner(string input)
        {
            _input = input;
        }
        private void Read()
        {
            _index++;
            if (_index < _input.Length)
                _ch = _input[_index];
            else
                _ch = EOF;
        }

        public Token Next()
        {
            Read();

            if (_index < _input.Length)
            {
                while (Char.IsWhiteSpace(_ch))
                {
                    Read();
                }

                if (char.IsDigit(_ch)) // number
                {
                    StringBuilder str = new StringBuilder();

                    while (Char.IsDigit(_ch) || _ch == '.')
                    {
                        str.Append(_ch);
                        Read();
                    }
                    _index--;

                    return new Token(str.ToString(), TokenType.Number);
                }
                else
                {
                    switch (_ch)
                    {
                        case '+':
                            return Token.Add;
                        case '-':
                            return Token.Subtract;
                        case '*':
                            return Token.Multiply;
                        case '/':
                            return Token.Divide;
                        case '(':
                            return Token.LParen;
                        case ')':
                            return Token.RParen;
                        case '=':
                            Read();
                            if (_ch == '=') return Token.Equal;
                            throw new InvalidOperationException("equalily operator must be '=='");
                        case '>':
                            Read();
                            if (_ch == '=') return Token.GreaterThanOrEqual;
                            _index--;
                            return Token.GreaterThan;
                        case '<':
                            Read();
                            if (_ch == '=') return Token.LessThanOrEqual;
                            _index--;
                            return Token.LessThan;
                        default:
                            throw new Exception("Invalid character: " + _ch);
                    }

                }
            }
            return Token.EOF;
        }

    }
    enum TokenType
    {
        EOF = 0,
        Number,
        Add,
        Subtract,
        Multiply,
        Divide,
        LParen, // (
        RParen, // )
        Equal,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
    }
    struct Token
    {
        public string Value;
        public TokenType Type;

        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }
        public override string ToString()
        {
            return Value;
        }



        public static Token EOF
        {
            get { return new Token("EOF", TokenType.EOF); }
        }
        public static Token Add
        {
            get { return new Token("+", TokenType.Add); }
        }
        public static Token Subtract
        {
            get { return new Token("-", TokenType.Subtract); }
        }
        public static Token Multiply
        {
            get { return new Token("*", TokenType.Multiply); }
        }
        public static Token Divide
        {
            get { return new Token("/", TokenType.Divide); }
        }
        public static Token LParen
        {
            get { return new Token("(", TokenType.LParen); }
        }
        public static Token RParen
        {
            get { return new Token(")", TokenType.RParen); }
        }
        public static Token Equal
        {
            get { return new Token("==", TokenType.Equal); }
        }
        public static Token GreaterThan
        {
            get { return new Token(">", TokenType.GreaterThan); }
        }
        public static Token GreaterThanOrEqual
        {
            get { return new Token(">=", TokenType.GreaterThanOrEqual); }
        }
        public static Token LessThan
        {
            get { return new Token("<", TokenType.LessThan); }
        }
        public static Token LessThanOrEqual
        {
            get { return new Token("<=", TokenType.LessThanOrEqual); }
        }
    }
}
