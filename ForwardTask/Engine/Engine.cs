using System;
using System.Collections.Generic;
using ForwardTask.Engine_Config;
using System.Linq;

namespace ForwardTask.Engines
{

    public class Engine
    {
        //init value
        public readonly double I;
        public readonly List<(double M, double V)> TorqueVelocityCurve;
        public readonly double TOverheat;
        public readonly double heatingCoefByM;
        public readonly double heatingCoefByV;
        public readonly double coolingCoef;
        public readonly double TOutside;

        //current value
        public double TCurrent { get; private set; }
        public double Acceleration { get; private set; }
        public double Velocity { get; private set; }
        public double Torque { get; private set; }
        public double Power { get; private set; }
        public int IndexOfTorqueVelocityCurve { get; private set; }

        public Engine(EngineConfig cfg, double outsideTemp)
        {
            I = cfg.I;
            TorqueVelocityCurve = cfg.TorqueVelocityCurve;
            TOverheat = cfg.TOverheat;
            heatingCoefByM = cfg.heatingCoefByM;
            heatingCoefByV = cfg.heatingCoefByV;
            coolingCoef = cfg.coolingCoef;

            TOutside = outsideTemp;
            TCurrent = TOutside;

            IndexOfTorqueVelocityCurve = 0;
            Acceleration = 0;
            Velocity = TorqueVelocityCurve[0].V;
            Torque = TorqueVelocityCurve[0].M;
            Power = 0;
        }

        public void SimulatePerSecond()
        {
            CalcAcceleration();
            CalcVelocity();
            CalcCurrentIndexOfTorqueByRpmCurve();
            CalcTorque();
            CalcPower();
            CalcTemperature();
        }

        private void CalcVelocity()
        {
            if (Math.Abs(Velocity + Acceleration - TorqueVelocityCurve.Last().V) < 1e2)
            {
                Velocity = TorqueVelocityCurve.Last().V;
                return;
            }
            Velocity += Acceleration;
        }

        private double CalcVHeating() =>
            Torque * heatingCoefByM + Velocity * Velocity * heatingCoefByV;

        private double CalcVCooling() => coolingCoef * (TOutside - TCurrent);

        private void CalcTemperature() => TCurrent += CalcVCooling() + CalcVHeating();

        private void CalcAcceleration() => Acceleration = Torque / I;

        private void CalcPower() => Power = Torque * Velocity / 1000;

        private void CalcTorque()
        {
            var x1 = TorqueVelocityCurve[IndexOfTorqueVelocityCurve].V;
            var x2 = TorqueVelocityCurve[IndexOfTorqueVelocityCurve + 1].V;
            var y1 = TorqueVelocityCurve[IndexOfTorqueVelocityCurve].M;
            var y2 = TorqueVelocityCurve[IndexOfTorqueVelocityCurve + 1].M;

            var k = (y2 - y1) / (x2 - x1);
            var b = y1 - k * x1;

            Torque = k * Velocity + b;
        }

        private void CalcCurrentIndexOfTorqueByRpmCurve()
        {
            var newIndex = 0;
            foreach (var item in TorqueVelocityCurve.Skip(1).SkipLast(1))
            {
                if (Velocity >= item.V)
                {
                    newIndex++;
                }
            }
            IndexOfTorqueVelocityCurve = newIndex;
        }

    }
}
