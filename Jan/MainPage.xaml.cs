using ExtendedNumerics;
using System.Collections.ObjectModel;

namespace Jan
{
    public partial class MainPage : ContentPage
    {
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

        private static List<BigDecimal> Calculate(string input)
        {
            Stack<BigDecimal> stack = new();

            foreach (string token in input.Split().Select(i => i.Trim()).Where(i => !string.IsNullOrWhiteSpace(i)))
            {
                if (BigDecimal.TryParse(token, out BigDecimal dec))
                {
                    stack.Push(dec);
                }
                else if (TryGetValue(Constant, token, out BigDecimal? val) && val != null)
                {
                    stack.Push(val.Value);
                }
                else if (stack.Count >= 1 && TryGetValue(UnaryOperator, token, out Func<BigDecimal, BigDecimal>? oper) && oper != null)
                {
                    stack.Push(oper(stack.Pop()));
                }
                else if (stack.Count >= 2 && TryGetValue(BinaryOperator, token, out Func<BigDecimal, BigDecimal, BigDecimal>? oper2) && oper2 != null)
                {
                    BigDecimal first = stack.Pop();
                    BigDecimal second = stack.Pop();
                    stack.Push(oper2(second, first));
                }
            }

            List<BigDecimal> numbers = stack.ToList();
            numbers.Reverse();
            return numbers;
        }

        private static bool TryGetValue<X, Y>(Func<X, Y?> func, X input, out Y? output)
        {
            output = func(input);
            return output != null;
        }

        private static BigDecimal? Constant(string name) => name switch
        {
            "pi" => BigDecimal.Pi,
            "e" => BigDecimal.E,
            _ => null
        };

        private static Func<BigDecimal, BigDecimal>? UnaryOperator(string name) => name switch
        {
            "sin" => BigDecimal.Sin,
            "cos" => BigDecimal.Cos,
            "tan" => BigDecimal.Tan,
            _ => null,
        };

        private static Func<BigDecimal, BigDecimal, BigDecimal>? BinaryOperator(string name) => name switch
        {
            "+" => (a, b) => a + b,
            "-" => (a, b) => a - b,
            "*" => (a, b) => a * b,
            "/" => (a, b) => a / b,
            "^" => (a, b) => BigDecimal.Pow(a, b, 10),
            //"log" => (a, b) => a.LogN(b),
            _ => null,
        };
    }
}
