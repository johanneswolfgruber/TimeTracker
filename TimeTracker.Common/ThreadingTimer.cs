namespace TimeTracker.Common;

public class TimerEventArgs : EventArgs
{
    public TimerEventArgs(object? state)
    {
        State = state;
    }

    public object? State { get; }
}

public interface ITimer : IDisposable
{
    void Start();

    void Stop();

    event EventHandler<TimerEventArgs>? Elapsed;
}

public class ThreadingTimer : ITimer
{
    private readonly TimeSpan _period;
    private readonly TimeSpan _dueTime;
    private readonly object? _state;
    private Timer? _timer;

    public event EventHandler<TimerEventArgs>? Elapsed;

    public ThreadingTimer(TimeSpan period, TimeSpan? dueTime = null, object? state = null)
    {
        _period = period;
        _dueTime = dueTime is null ? TimeSpan.Zero : dueTime.Value;
        _state = state;
    }

    public void Start()
    {
        _timer = new Timer(TimerCallback, _state, _dueTime, _period);
    }

    public void Stop()
    {
        _timer?.Dispose();
        _timer = null;
    }

    public void Dispose()
    {
        _timer?.Dispose();        
    }

    private void TimerCallback(object? state)
    {
        Elapsed?.Invoke(this, new TimerEventArgs(state));
    }
}

