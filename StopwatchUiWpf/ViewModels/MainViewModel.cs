using CommunityToolkit.Mvvm.ComponentModel;

namespace StopwatchUiWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            StartButtonVisible = true;
        }

        [ObservableProperty]
        private bool resetButtonVisible;

        [ObservableProperty]
        private bool startButtonVisible;

        [ObservableProperty]
        private bool pauseButtonVisible;

        [ObservableProperty]
        private bool splitTimeButtonVisible;

        [ObservableProperty]
        private bool stopButtonVisible;
    }
}
