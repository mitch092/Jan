using PeterO.Numbers;
using System.Collections.ObjectModel;

namespace Jan
{
    public partial class MainPage : ContentPage
    {
        private static readonly EContext Context = new(10, ERounding.HalfUp, -1000, 1000, clampNormalExponents: true);

        public class Item
        {
            public string? Number { get; set; }
        }

        public MainPage()
        {
            InitializeComponent();
            StackDisplay.ItemsSource = new ObservableCollection<Item>();
            EditorText.TextChanged += EditorText_TextChanged;
        }

        private ObservableCollection<Item> Items => ((ObservableCollection<Item>)StackDisplay.ItemsSource);

        private void EditorText_TextChanged(object? sender, TextChangedEventArgs e)
        {
            Items.Clear();
            foreach (var item in Calculate(e.NewTextValue))
            {
                Items.Add(new Item() { Number = item.ToString() });
            }
        }

        //private static List<double> Calculate(string input)
        //{
        //    Stack<double> stack = new();

        //    foreach (string token in input.Split().Select(i => i.Trim()).Where(i => !string.IsNullOrWhiteSpace(i)))
        //    {
        //        if (double.TryParse(token, out double number))
        //        {
        //            stack.Push(number);
        //        }
        //        else if (TryGetValue(Constant, token, out double? val))
        //        {
        //            stack.Push(val!.Value);
        //        }
        //        else if (stack.Count >= 1 && TryGetValue(UnaryOperator, token, out Func<double, double>? oper))
        //        {
        //            stack.Push(oper!(stack.Pop()));
        //        }
        //        else if (stack.Count >= 2 && TryGetValue(BinaryOperator, token, out Func<double, double, double>? oper2))
        //        {
        //            double first = stack.Pop();
        //            double second = stack.Pop();
        //            stack.Push(oper2!(second, first));
        //        }
        //    }

        //    List<double> doubles = stack.ToList();
        //    doubles.Reverse();
        //    return doubles;
        //}

        //private static bool TryGetValue<X, Y>(Func<X, Y?> func, X input, out Y? output)
        //{
        //    output = func(input);
        //    return output != null;
        //}

        //private static double? Constant(string name) => name switch
        //{
        //    "e" => Math.E,
        //    "pi" => Math.PI,
        //    "tau" => Math.Tau,
        //    _ => null
        //};

        //private static Func<double, double>? UnaryOperator(string name) => name switch
        //{
        //    "sin" => Math.Sin,
        //    "cos" => Math.Cos,
        //    "tan" => Math.Tan,
        //    _ => null,
        //};

        //private static Func<double, double, double>? BinaryOperator(string name) => name switch
        //{
        //    "+" => (a, b) => a + b,
        //    "-" => (a, b) => a - b,
        //    "*" => (a, b) => a * b,
        //    "/" => (a, b) => a / b,
        //    "^" => Math.Pow,
        //    "log" => Math.Log,
        //    _ => null,
        //};

        private static List<EDecimal> Calculate(string input)
        {
            Stack<EDecimal> stack = new();

            foreach (string token in input.Split().Select(i => i.Trim()).Where(i => !string.IsNullOrWhiteSpace(i)))
            {
                if (TryGetEDecimal(token, out EDecimal dec))
                {
                    stack.Push(dec);
                }
                else if (TryGetValue(Constant, token, out EDecimal? val))
                {
                    stack.Push(val!);
                }
                //else if (stack.Count >= 1 && TryGetValue(UnaryOperator, token, out Func<double, double>? oper))
                //{
                //    stack.Push(oper!(stack.Pop()));
                //}
                else if (stack.Count >= 2 && TryGetValue(BinaryOperator, token, out Func<EDecimal, EDecimal, EDecimal>? oper2))
                {
                    EDecimal first = stack.Pop();
                    EDecimal second = stack.Pop();
                    stack.Push(oper2!(second, first));
                }
            }

            List<EDecimal> numbers = stack.ToList();
            numbers.Reverse();
            return numbers;
        }

        private static bool TryGetEDecimal(string number, out EDecimal dec)
        {
            if (double.TryParse(number, out var _))
            {
                dec = EDecimal.FromString(number, Context);
                return true;
            }
            else
            {
                dec = EDecimal.NaN;
                return false;
            }
        }

        private static bool TryGetValue<X, Y>(Func<X, Y?> func, X input, out Y? output)
        {
            output = func(input);
            return output != null;
        }

        private static EDecimal? Constant(string name) => name switch
        {
            "pi" => EDecimal.PI(Context),
            "e" => Euler,            
            _ => null
        };

        private static readonly EDecimal Epsilon = GetEpsilon();

        private static EDecimal GetEpsilon() 
        {
            EDecimal two = EDecimal.FromString("2.0", Context);
            EDecimal last = EDecimal.FromString("1.0", Context);
            EDecimal next = EDecimal.FromString("2.0", Context);

            while (EDecimals.CompareTotal(last, next, Context) != 0) 
            {
                last = next;
                next = next.Divide(two, Context);
            }

            return next;
        }

        private static readonly EDecimal Euler = EDecimal.One.Log(Context);

        //private static Func<EDecimal, EDecimal>? UnaryOperator(string name) => name switch
        //{
        //    "sin" => (EDecimal a, EDecimal b) => a.,
        //    "cos" => Math.Cos,
        //    "tan" => Math.Tan,
        //    _ => null,
        //};

        private static Func<EDecimal, EDecimal, EDecimal>? BinaryOperator(string name) => name switch
        {
            "+" => (a, b) => a + b,
            "-" => (a, b) => a - b,
            "*" => (a, b) => a * b,
            "/" => (a, b) => a / b,
            "^" => (a, b) => a.Pow(b, Context),
            "log" => (a, b) => a.LogN(b, Context),
            _ => null,
        };
    }
}
