namespace ExpressNET;

internal class JobFixer
{
    public bool isJobFixed { get; private set; } = false;
    public State state { get; private set; } =  State.NotStarted;

    public void FixJob() => Task.FromResult(isJobFixed = true);
    public void SetState(State newState) => Task.FromResult(state = newState);
}

internal enum State
{
    NotStarted,
    InProgress,
    Done
}