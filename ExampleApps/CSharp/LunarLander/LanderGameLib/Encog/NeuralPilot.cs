﻿using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Util.Arrayutil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanderGameLib
{

    public class NeuralPilot
    {
        private readonly NormalizedField _fuelStats;
        private readonly BasicNetwork _network;
        private readonly bool _track;
        private readonly NormalizedField _altitudeStats;
        private readonly NormalizedField _velocityStats;

        public static int CycleCount { get; set; } = 0;
        public static List<EncogSimOut> Outputs { get; set; } = new List<EncogSimOut>();

        public NeuralPilot(BasicNetwork network, bool track)
        {
            _fuelStats = new NormalizedField(NormalizationAction.Normalize, "fuel", 200, 0, -0.9, 0.9);
            _altitudeStats = new NormalizedField(NormalizationAction.Normalize, "altitude", 10000, 0, -0.9, 0.9);
            _velocityStats = new NormalizedField(NormalizationAction.Normalize, "velocity",
                                                LanderSimulator.TerminalVelocity, -LanderSimulator.TerminalVelocity,
                                                -0.9, 0.9);

            _track = track;
            _network = network;
        }

        public int ScorePilot()
        {
            var sim = new LanderSimulator();
            while (sim.Flying)
            {
                var input = new BasicMLData(3);
                input[0] = _fuelStats.Normalize(sim.Fuel);
                input[1] = _altitudeStats.Normalize(sim.Altitude);
                input[2] = _velocityStats.Normalize(sim.Velocity);
                IMLData output = _network.Compute(input);
                double value = output[0];

                bool thrust;

                if (value > 0)
                {
                    thrust = true;
                    if (_track)
                        Console.WriteLine(@"THRUST");
                }
                else
                    thrust = false;

                sim.Turn(thrust);
                if (_track)
                    Console.WriteLine(sim.Telemetry());
            }

            CycleCount++;

            Outputs.Add(new EncogSimOut { Session = CycleCount, Score = sim.Score });

            return (sim.Score);
        }
    }

    public class EncogSimOut
    {
        public int Session { get; set; }
        public double Score { get; set; }
    }
}
