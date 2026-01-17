using ExtendedNumerics;
using Math;
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
            foreach (var item in Calc.Calculate(e.NewTextValue))
            {
                Items.Add(new Item() { Number = item.ToString() });
            }
        }
    }
}
