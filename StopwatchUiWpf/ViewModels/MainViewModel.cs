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
        private DateTime _currentSplitStartTime;
        private TimeSpan _currentStopTime;
        private TimeSpan _currentSplitTime;
        private List<(TimeSpan, TimeSpan)> _splitTimesList;

        public MainViewModel()
        {
            _splitTimesList = [];
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
            StartDispatcherTimerRunningStopTime();

            StartButtonVisible = false;
            PauseButtonVisible = true;
            SplitTimeButtonVisible = true;
            StopButtonVisible = true;
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
            StopButtonVisible = false;
            ResetButtonVisible = true;
        }

        [RelayCommand]
        public void StopStopProcess()
        {
            if (_dispatcherTimerRunningStopTime == null)
                return;

            _dispatcherTimerRunningStopTime.Stop();
            _dispatcherTimerRunningStopTime = null;
            FinalStopTime = _currentStopTime.ToString(@"hh\:mm\:ss\.fff");
        }

        [RelayCommand]
        public void StopSplitTime()
        {
            if (_dispatcherTimerRunningStopTime == null)
                return;

            _splitTimesList.Add((_currentSplitTime, _currentStopTime));
            _currentSplitStartTime = DateTime.Now;
            SplitTimes = string.Join('\n', _splitTimesList.Select((t, i) => $"{i + 1}  {t.Item1:hh\\:mm\\:ss\\.fff}  {t.Item2:hh\\:mm\\:ss\\.fff}"));
        }

        private void StartDispatcherTimerRunningStopTime()
        {
            if (_dispatcherTimerRunningStopTime == null)
            {
                _startTime = DateTime.Now;
                _dispatcherTimerRunningStopTime = new DispatcherTimer();
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
            _currentStopTime = _currentRunningStopTime - _startTime;
            _currentSplitTime = _currentRunningStopTime - _currentSplitStartTime;
            RunningStopTime = _currentStopTime.ToString(@"hh\:mm\:ss\.fff");
        }
    }
}
