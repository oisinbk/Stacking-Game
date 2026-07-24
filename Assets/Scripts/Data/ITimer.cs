namespace Data
{
    public interface ITimer
    {
        public float Timer { get; }
        
        public void ResetTimer();
        
        public void PauseTimer();
        
        public void ResumeTimer();
    }
}