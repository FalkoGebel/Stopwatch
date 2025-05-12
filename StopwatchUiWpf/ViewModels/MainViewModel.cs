using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Threading;

namespace StopwatchUiWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private DispatcherTimer? _dispatcherTimerCurrentTime;
        private DispatcherTimer? _dispatcherTimerRunningStopTime;
        private DateTime _startTime;
        private DateTime _currentRunningStopTime;

        public MainViewModel()
        {
            StartButtonVisible = true;

            UpdateCurrentTime();
            StartDispatcherTimerCurrentTime();
        }

        [ObservableProperty]
        private string _currentTime = string.Empty;

        [ObservableProperty]
        private string _runningStopTime = string.Empty;

        [ObservableProperty]
        private string _splitTimes = string.Empty;

        [ObservableProperty]
        private string _finalStopTime = string.Empty;

        [ObservableProperty]
        private bool _resetButtonVisible;

        [ObservableProperty]
        private bool _startButtonVisible;

        [ObservableProperty]
        private bool _pauseButtonVisible;

        [ObservableProperty]
        private bool _splitTimeButtonVisible;

        [ObservableProperty]
        private bool _stopButtonVisible;

        [RelayCommand]
        public void StartStopProcess()
        {
            _startTime = DateTime.Now;
            StartDispatcherTimerRunningStopTime();

            StartButtonVisible = false;
            PauseButtonVisible = true;
            SplitTimeButtonVisible = true;
            StopButtonVisible = true;
        }

        private void StartDispatcherTimerRunningStopTime()
        {
            _dispatcherTimerRunningStopTime = new DispatcherTimer();
            _dispatcherTimerRunningStopTime.Tick += UpdateRunningStopTimeTick;
            _dispatcherTimerRunningStopTime.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _dispatcherTimerRunningStopTime.Start();
        }

        private void StartDispatcherTimerCurrentTime()
        {
            _dispatcherTimerCurrentTime = new DispatcherTimer();
            _dispatcherTimerCurrentTime.Tick += UpdateCurrentTimeTick;
            _dispatcherTimerCurrentTime.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimerCurrentTime.Start();
        }

        private void UpdateCurrentTimeTick(object? sender, object e) => UpdateCurrentTime();

        private void UpdateCurrentTime()
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            CurrentTime = now.ToString("T");
        }

        private void UpdateRunningStopTimeTick(object? sender, object e) => UpdateRunningStopTime();

        private void UpdateRunningStopTime()
        {
            _currentRunningStopTime = DateTime.Now;
            TimeSpan ts = _currentRunningStopTime - _startTime;
            RunningStopTime = ts.ToString(@"hh\:mm\:ss\.fff");
        }
    }
}
