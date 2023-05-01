using ForwardTask.Engines;

namespace ForwardTask.Engine_Tests
{

    public static class TempTest
    {
        public static double GetTimeForOverheat(Engine engine)
        {
            var maxTime = 1000;

            for (var t = 0; t < maxTime; t++)
            {
                engine.SimulatePerSecond();
                if (engine.TCurrent >= engine.TOverheat)
                {
                    return t;
                }
            }
            return -1;
        }
    }
}
