using ExtendedNumerics;
using Math;
using System.Collections.ObjectModel;
using System.Threading.Channels;

namespace Jan
{
    public partial class MainPage : ContentPage
    {
        public class Item
        {
            public string? Number { get; set; }
        }

        private readonly Channel<string> InputChannel;
        private readonly Task ChannelReaderTask;

        public MainPage()
        {
            InitializeComponent();
            StackDisplay.ItemsSource = new ObservableCollection<Item>();

            BoundedChannelOptions options = new(1) { FullMode = BoundedChannelFullMode.DropOldest, SingleReader = true, SingleWriter = true };
            InputChannel = Channel.CreateBounded<string>(options);
            ChannelReaderTask = Task.Run(ProcessInputs);

            EditorText.TextChanged += EditorText_TextChanged;
        }

        public async Task ProcessInputs() 
        {
            await foreach (string input in InputChannel.Reader.ReadAllAsync()) 
            {
                Items.Clear();
                foreach (var item in Calc.Calculate(input))
                {
                    Items.Add(new Item() { Number = item.ToString() });
                }
            }
        }

        private ObservableCollection<Item> Items => ((ObservableCollection<Item>)StackDisplay.ItemsSource);

        private void EditorText_TextChanged(object? sender, TextChangedEventArgs e)
        {
            InputChannel.Writer.TryWrite(e.NewTextValue);
        }
    }
}
