using System;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Settings;

namespace GeneticProgramming.Server.Core.Initialization
{
    public class CreateBoostrapperFromWeights:IInitializationBootstrapper
    {
        public List<List<double>> WeightsToBootstrap { get; set; }
        public int HowMany { get; set; }

        public CreateBoostrapperFromWeights()
        {
            //var weights = new List<double> { 2.50882654352634, 2.2500110024332, 2.65000264745682, 1.1643209762577, 1.99788265064399, 2.014789451826, 2.51093104135099, 0.859734514604096, 2.50268037586882, 1.59176172122409, 0.630404212874736, 0.529969269021181, -0.243651366982152, 1.53288158689313, 0.477436094626347, -0.0171274156617519, 0.123010426927782, 0.479318079033369, -0.385829791004231, -0.266231136498312, 1.91563524302158, 0.135503843267627, -0.232429648393632, 2.04025224967171, 1.55213516455283, 0.245999657732813, 0.996436378064619, 1.17923741944797, 1.45227782660744, 0.971648022106067, 0.472217280892433, 0.0906945842448265, 1.32796523500348, 1.69987931408165, -0.290887888128444, 0.558507669861246, 0.166914658802492, 0.403680076573351, 0.776348386985468, 0.535874760284574, -0.052886045638966, 1.31054965258596, 2.17132834256506, 1.57067147152647, -0.131280048274297, 0.17040649449119, 0.224249618793466, 0.657414226295494, 0.332020605484819, 1.89669121947069, 0.346634288098787, 1.56460911470855, 1.85931286855301, 0.131605774059778, 1.37501307734502, 0.165896497142815, 0.652407786464859, 0.115395877731232, 1.93189463046125, 2.51101049536896, 1.7331673209969, 0.952095319868839, -0.0824695219098284, 0.215937272464096, 1.02384311003797, 1.31067396842126, 1.27399130543655, 0.117993447170412, 0.0364532452865351, 0.920483135169672, 1.88113875297668, 1.19554571231624, 0.131145132108955, 0.562650338807461 };
        }

        public void Bootstrap(Populations popsToBootstrap, IGpExperimentSettings settings)
        {
            for (int i = 0; i < HowMany; i++)
            {
                var weights = WeightsToBootstrap[i%WeightsToBootstrap.Count].Select(Math.Abs).ToList();
                double catWeight = weights[0];
                double numWeith = weights[1];
                var catCoefficients = 26;
                var categoricalWeights = weights.GetRange(2, catCoefficients);
                var numericalWeights = weights.GetRange(2 + 26, 46);
                var categoricalBootstrapper = new CategoricalBootstrapper(settings.PopulationsSettings[1].TypeSets[0], catWeight, categoricalWeights);
                var numericalBootstrapper = new NumericalBootstrapper(settings.PopulationsSettings[0].TypeSets[0], numWeith, numericalWeights);
                var a = numericalBootstrapper.CreatePrograms(5);
                var b = categoricalBootstrapper.CreatePrograms(5);
                popsToBootstrap.Islands[0].Individuals[i].Programs = a;
                popsToBootstrap.Islands[1].Individuals[i].Programs = b;
            }
        }
    }
}