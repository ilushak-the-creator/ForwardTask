using ForwardTask.Engines;
using System.Linq;

namespace ForwardTask.Engine_Tests
{

    public static class MaxPowerTest
    {
        public static (double MaxPower, double Velocity) GetMaxPowerOfEngine(Engine engine)
        {
            var maxPower = double.MinValue;
            var velocityForMaxPower = 0.0;

            while (true)
            {
                if (engine.Velocity == engine.TorqueVelocityCurve.Last().V)
                {
                    break;
                }
                engine.SimulatePerSecond();
                if (engine.Power > maxPower)
                {
                    maxPower = engine.Power;
                    velocityForMaxPower = engine.Velocity;
                }
            }
            return (maxPower, velocityForMaxPower);
        }
    }
}
