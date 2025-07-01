using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace StopwatchUiWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private IDispatcherTimer? _dispatcherTimerRunningStopTime;
        private DateTime _startTime;
        private DateTime _currentRunningStopTime;
        private DateTime _currentSplitStartTime;
        private TimeSpan _currentStopTime;
        private TimeSpan _currentSplitTime;
        private readonly List<(TimeSpan, TimeSpan)> _splitTimesList;

        public MainViewModel()
        {
            _splitTimesList = [];
            StartButtonVisible = true;
        }

        [ObservableProperty]
        public string _runningStopTime = string.Empty;

        [ObservableProperty]
        private string _splitTimes = string.Empty;

        [ObservableProperty]
        private bool _resetButtonVisible;

        [ObservableProperty]
        private bool _startButtonVisible;

        [ObservableProperty]
        private bool _pauseButtonVisible;

        [ObservableProperty]
        private bool _splitTimeButtonVisible;

        [RelayCommand]
        public void StartStopProcess()
        {
            StartDispatcherTimerRunningStopTime();

            StartButtonVisible = false;
            PauseButtonVisible = true;
            SplitTimeButtonVisible = true;
            ResetButtonVisible = false;
        }

        [RelayCommand]
        public void PauseStopProcess()
        {
            if (_dispatcherTimerRunningStopTime == null)
                return;

            _dispatcherTimerRunningStopTime.Stop();
            StartButtonVisible = true;
            PauseButtonVisible = false;
            SplitTimeButtonVisible = false;
            ResetButtonVisible = true;
        }

        [RelayCommand]
        public void StopSplitTime()
        {
            if (_dispatcherTimerRunningStopTime == null)
                return;

            _splitTimesList.Add((_currentSplitTime, _currentStopTime));
            _currentSplitStartTime = DateTime.Now;
            TimeSpan benchmarkSplitTime = _splitTimesList.OrderBy(t => t.Item1).ToList()[^1].Item1,
                     benchmarkWholeTime = _splitTimesList[^1].Item2;
            int numberWidth = _splitTimesList.Count.ToString().Length;
            SplitTimes = string.Join('\n', _splitTimesList.Select((t, i) => $"{(i + 1).ToString().PadLeft(numberWidth, '0')}:  {FormatTimeSpan(t.Item1, benchmarkSplitTime)} - {FormatTimeSpan(t.Item2, benchmarkWholeTime)}")
                                                                   .Reverse());
        }

        [RelayCommand]
        public void Reset()
        {
            RunningStopTime = string.Empty;
            _splitTimesList.Clear();
            SplitTimes = string.Empty;
            StartButtonVisible = true;
            PauseButtonVisible = false;
            SplitTimeButtonVisible = false;
            ResetButtonVisible = false;
        }

        private void StartDispatcherTimerRunningStopTime()
        {
            if (_dispatcherTimerRunningStopTime == null)
            {
                _startTime = DateTime.Now;
                IDispatcher? dispatcher = Dispatcher.GetForCurrentThread();
                _dispatcherTimerRunningStopTime = dispatcher.CreateTimer();
                _dispatcherTimerRunningStopTime.Tick += UpdateRunningStopTimeTick;
                _dispatcherTimerRunningStopTime.Interval = new TimeSpan(0, 0, 0, 0, 1);
            }
            else
            {
                _startTime = DateTime.Now - _currentStopTime;
            }

            _currentSplitStartTime = _startTime;
            _dispatcherTimerRunningStopTime.Start();
        }

        private void UpdateRunningStopTimeTick(object? sender, object e) => UpdateRunningStopTime();

        private void UpdateRunningStopTime()
        {
            _currentRunningStopTime = DateTime.Now;
            _currentStopTime = _currentRunningStopTime - _startTime;
            _currentSplitTime = _currentRunningStopTime - _currentSplitStartTime;
            RunningStopTime = FormatTimeSpan(_currentStopTime, _currentStopTime);
        }

        private static string FormatTimeSpan(TimeSpan ts, TimeSpan? benchmark)
        {
            TimeSpan check = benchmark == null ? ts : benchmark.Value;

            if (check.Days > 0)
            {
                if (check.Days > 9)
                    return ts.ToString(@"dd:\hh\:mm\:ss\.fff");

                return ts.ToString(@"d:\hh\:mm\:ss\.fff");
            }

            if (check.Hours > 0)
            {
                if (check.Hours > 9)
                    return ts.ToString(@"hh\:mm\:ss\.fff");

                return ts.ToString(@"h\:mm\:ss\.fff");
            }

            if (check.Minutes > 0)
            {
                if (check.Minutes > 9)
                    return ts.ToString(@"mm\:ss\.fff");

                return ts.ToString(@"m\:ss\.fff");
            }

            if (check.Seconds > 0)
            {
                if (check.Seconds > 9)
                    return ts.ToString(@"ss\.fff");

                return ts.ToString(@"s\.fff");
            }

            return ts.ToString(@"s\.fff");
        }
    }
}
