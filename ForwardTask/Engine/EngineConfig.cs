namespace ForwardTask.Engine_Config
{
    public class EngineConfig
    {
        public double I;
        public List<(double M, double V)> TorqueVelocityCurve;
        public double TOverheat;
        public double heatingCoefByM;
        public double heatingCoefByV;
        public double coolingCoef;
    }
}
