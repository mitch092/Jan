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

        private static List<double> Calculate(string input)
        {
            Stack<double> stack = new();

            foreach (string token in input.Split().Select(i => i.Trim()).Where(i => !string.IsNullOrWhiteSpace(i)))
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (TryGetValue(Constant, token, out double? val)) 
                {
                    stack.Push(val!.Value);
                }
                else if (stack.Count >= 1 && TryGetValue(UnaryOperator, token, out Func<double, double>? oper))
                {
                    stack.Push(oper!(stack.Pop()));
                }
                else if (stack.Count >= 2 && TryGetValue(BinaryOperator, token, out Func<double, double, double>? oper2))
                {
                    double first = stack.Pop();
                    double second = stack.Pop();
                    stack.Push(oper2!(second, first));
                }
            }

            List<double> doubles = stack.ToList();
            doubles.Reverse();
            return doubles;
        }

        private static bool TryGetValue<X, Y>(Func<X, Y?> func, X input, out Y? output)
        {
            output = func(input);
            return output != null;
        }

        private static double? Constant(string name) => name switch
        {
            "e" => Math.E,
            "pi" => Math.PI,
            "tau" => Math.Tau,
            _ => null
        };

        private static Func<double, double>? UnaryOperator(string name) => name switch
        {
            "sin" => Math.Sin,
            "cos" => Math.Cos,
            "tan" => Math.Tan,
            _ => null,
        };

        private static Func<double, double, double>? BinaryOperator(string name) => name switch
        {
            "+" => (a, b) => a + b,
            "-" => (a, b) => a - b,
            "*" => (a, b) => a * b,
            "/" => (a, b) => a / b,
            "^" => Math.Pow,
            "log" => Math.Log,
            _ => null,
        };
    }
}
