using ForwardTask.Engines;
using ForwardTask.Engine_Tests;
using ForwardTask.Engine_Config;

var cfg = new EngineConfig();
cfg.I = 10;
cfg.TorqueVelocityCurve = new()
{
    (20, 0),
    (75, 75),
    (100, 150),
    (105, 200),
    (75, 250),
    (0, 300)
};
cfg.TOverheat = 110;
cfg.heatingCoefByM = 0.01;
cfg.heatingCoefByV = 0.0001;
cfg.coolingCoef = 0.1;

Console.WriteLine("Enter outside temperature: ");
var outsideTemp = int.Parse(Console.ReadLine());

Console.WriteLine("[Test 1]");
var engine = new Engine(cfg, outsideTemp);
var resultTime = TempTest.GetTimeForOverheat(engine);
if (resultTime == -1) 
    Console.WriteLine("Time out, the engine has not overheated ");
else 
    Console.WriteLine("Engine overheated on {0} second", resultTime);

Console.WriteLine("[Test 2]");
engine = new Engine(cfg, outsideTemp);
var resultPower = MaxPowerTest.GetMaxPowerOfEngine(engine);
Console.WriteLine("Maximum power is {0} kWt at {1} radSec", resultPower.MaxPower, resultPower.Velocity);

