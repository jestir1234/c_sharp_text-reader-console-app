using static System.Math;
// 'using static' imports the methods from one 
// class whereas 'using' statement imports all 
// classes from a namespace

namespace TeleprompterConsole
{
    internal class TeleprompterConfig
        {
            public int DelayInMilliseconds { get; private set; } = 200;
            public void UpdateDelay(int increment) // negative to speed up
            {
                var newDelay = Min(DelayInMilliseconds + increment, 1000);
                newDelay = Max(newDelay, 20);
                DelayInMilliseconds = newDelay;
            }
            public bool Done { get; private set; }

            public void SetDone()
            {
                Done = true;
            }

        }
}