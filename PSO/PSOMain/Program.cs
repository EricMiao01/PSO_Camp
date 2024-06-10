using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PSOLib;

///
/// 本版主要差異：
/// CheckParticle原本是Check fail就不走下一部, 本版依CEC2020RW所採CHT技術, 除了fitness外, g(x)及h(x)也是好壞比較的依據;
///
namespace PSOMain
{
    class ClassMain
    {
        static void Main(string[] args)
        {
            //Rosenbrock prob = new Rosenbrock(); // 0

            // *** CEC2020 series ***
            List<Problem> problems = new List<Problem>
            {
                //new RC15_SpeedReducer(),
                //new RC16_IndustRefrig(),
                //new RC17_SpringDesign(),
                //new RC18_PressureVessel(),
                //new RC19_WeldedBeam(),
                //new RC20_ThreebarTruss(),
                //new RC21_DiskClutch(),
                //new RC22_PlanetaryGear(),
                //new RC23_StepconePulley(),
                //new RC24_RobotGripper(),
                //new RC25_ThrustBearing(),
                //new RC26_GearBox(),
                //new RC27_10barTruss(),
                //new RC28_RollingBearing(),
                new RC29_GasCompressor(),
                new RC30_CompressionString(),
                new RC31_GearTrain(),
                new RC32_Himmelblau(),
                new RC33_Topology(),
            };
            //RC15_SpeedReducer prob = new RC15_SpeedReducer(); // 2994.4244, 2892.81316343172, Convx:0.0909 (OK)
            //RC16_IndustRefrig prob = new RC16_IndustRefrig(); // 0.163479, 0.011720461421122, Convx:0.0666 -> 0.032213
            //RC17_SpringDesign prob = new RC17_SpringDesign(); // 0.012719, 0.0128168905493021, Convx:0 (OK)
            //RC18_PressureVessel prob = new RC18_PressureVessel(); // 6410.08675, 6090.52620168586, Convx:0 -> 6060
            //RC19_WeldedBeam prob = new RC19_WeldedBeam(); // 1.670217 (OK)
            //RC20_ThreebarTruss prob = new RC20_ThreebarTruss(); // 263.93282, 263.895843376483, Convx:0 (OK)

            //RC21_DiskClutch prob = new RC21_DiskClutch(); // 0.235242 (OK)
            //RC22_PlanetaryGear prob = new RC22_PlanetaryGear(); // 0.53705, 0.526739926739927, Convx:0 -> 0.526
            //RC23_StepconePulley prob = new RC23_StepconePulley(); // infeasible, 4.2599910899947, Convx:0.2727 -> 16.1
            //RC24_RobotGripper prob = new RC24_RobotGripper();
            //RC25_ThrustBearing prob = new RC25_ThrustBearing(); // 1818.54755, 1714.05322531787, Convx:0 -> 1620

            //RC26_GearBox prob = new RC26_GearBox(); // infeasible; 29.7627305534893, Convx:0.0232 -> 35.3635
            //RC27_10barTruss prob = new RC27_10barTruss();
            //RC28_RollingBearing prob = new RC28_RollingBearing(); // 16958.2022869421 -> 14600
            //RC29_GasCompressor prob = new RC29_GasCompressor(); // 2967536.91129674 -> 2960000
            //RC30_CompressionString prob = new RC30_CompressionString(); // 2.658559 (OK)
            //RC31_GearTrain prob = new RC31_GearTrain(); // 0 (OK)
            //RC32_Himmelblau prob = new RC32_Himmelblau(); // -30665.5386 (OK)
            //RC33_Topology prob = new RC33_Topology();


            // PSO: 1998版的傳統PSO
            // NLP: 已發表論文
            // DENLP: DE+NLP的混合應用
            // Cyclic: NLP的進化版本(待證明效果)
            // DECyc: DE+Cyclic的混合應用

            int problemNumber = problems.Count();
            int probCounter = 0;
            int experimentNumber = 10;

            bool isTesting = true; // true: 測試; false: 實驗
            if (isTesting) experimentNumber = 1; // 測試就只跑一次

            //double[] MutateRateArray = { 0.005, 0.006, 0.007, 0.008, 0.009, 0.01, 0.011, 0.012, 0.013, 0.014, 0.015 };
            //double[] RestoreRateArray = { 0.0005, 0.0006, 0.0007, 0.0008, 0.0009, 0.001, 0.0011, 0.0012, 0.0013, 0.0014, 0.0015 };

            //double[] MutateRateArray = { 0.5};
            //double[] RestoreRateArray = { 0.01};


            foreach (var prob in problems)
            {
                probCounter++;
                //foreach (var mutationRate in MutateRateArray)
                //{
                //  foreach (var restoreRate in RestoreRateArray)
                    //{
                        for (int i = 0; i < experimentNumber; i++)
                        {
                            try
                            {
                                Console.WriteLine("\nProblem: " + prob.name() + ", Experiment " + (i).ToString() + ":");
                                //Console.WriteLine(string.Format("\nParameter -> Mutation Rate: {0}, Restore Rate: {1}\n", mutationRate, restoreRate));

                                // 絕對要有的, 在建構時傳入;
                                //PSO pso = new PSO(50, prob.x_u, prob.x_l, prob.v_u, prob.v_l, prob.GetFitness, prob.MaxFES);
                                //NLP pso = new NLP(50, prob.x_u, prob.x_l, prob.v_u, prob.v_l, prob.GetFitness, prob.MaxFES);
                                //Cyclic pso = new Cyclic(50, prob.x_u, prob.x_l, prob.v_u, prob.v_l, prob.GetFitness, prob.MaxFES);
                                DECyc pso = new DECyc(50, prob.x_u, prob.x_l, prob.v_u, prob.v_l, prob.GetFitness, prob.MaxFES);

                                //Cyclic_D pso = new Cyclic_D(50, prob.x_u, prob.x_l, prob.v_u, prob.v_l, prob.GetFitness, prob.MaxFES);
                                 //CycDE pso = new CycDE(50, prob.x_u, prob.x_l, prob.v_u, prob.v_l, prob.GetFitness, prob.MaxFES);

                                // Cyclic系列專用參數:
                                //pso.StagThreshold = 100;    // 幾個迭代無異動就視為停滯粒子;
                                //pso.MutateRate = mutationRate;        // 停滯粒子轉成NLP粒子的轉換率 (for 脫離停滯狀態);
                                //pso.RestoreRate = restoreRate;        // 從NLP狀態回到一般狀態的機率;
                                //pso.RestoreGap = 0.1;       // 重生位置在奇點(GB)位置的接近程度;

                                //pso.CheckParticle += prob.CheckParticle;              // 停用
                                pso.GetConstraintResult += prob.GetConstraintResult;    // CEC2020RW所採CHT 
                                pso.OnEvolute += ShowSwarm;

                                // *** START ***
                                pso.StartOptimize();
                                Console.WriteLine(string.Format("\n*** Final {0}, IsFeasible:{1}\n", pso.GetGroupBest().ToString(), Utils.Bool2Str(pso.GetGroupBest().Convx == 0)));
                                Console.WriteLine(string.Format("Elapsed generations: {0}", pso.ElapsedGenerations));
                                Console.WriteLine(string.Format("Elapsed seconds: {0}", pso.ElapsedSeconds));

                                if (isTesting == false) // 實驗的話進行存檔
                                {
                                    // 把不同題目的實驗數據分開比較好管理
                                    string filePath = string.Format("cyclic_parameter_experiment_results_{0}_test.txt", prob.name());
                                    WriteResultToFile(pso.GetGroupBest(), i, prob.name(), filePath);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                //    }
                //}
                if (probCounter == problemNumber)
                {
                    Console.WriteLine("\nPress any key ...");
                    Console.ReadKey();
                }
            }
        }

        static public void ShowSwarm(Swarm swarm, int iteration, TimeSpan span)
        {
            Console.WriteLine(String.Format("({0}) {2}", iteration, span, swarm.GroupBest.ToString()));
        }

        static void WriteResultToFile(PSOTuple result, int experimentNumber,string problemName, string filePath, double mutationRate = 0, double restoreRate = 0)
        {
            //string filePath = "cyclic_experiment_results.txt";

            using (StreamWriter sw = File.AppendText(filePath))
            {
                if (experimentNumber == 0)
                {
                    sw.WriteLine(string.Format("\nProblem: {0}", problemName)); // 新增一行表示數據來自哪一題
                    //sw.WriteLine(string.Format("\nMutation Rate: {0}, Restore Rate: {1}\n", mutationRate, restoreRate));
                }
                sw.WriteLine(string.Format("Experiment {0}: *** Final {1}, IsFeasible:{2}", experimentNumber, result.ToString(), Utils.Bool2Str(result.Convx == 0)));
            }
        }
    }
}
