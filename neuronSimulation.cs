namespace c_;

using System.Security.Cryptography.X509Certificates;
using System.Windows;
public class neuronSimulation
{
    public class Neuron
    {
        public static Random random = new Random();//rand.NextDouble() * 2.0 - 1.0;

        public static List<inputNeuron> inputNeurons = new List<inputNeuron>();//список входжов
        public static List<List<hiddenNeuron>> hiddenNeurons = new List<List<hiddenNeuron>>();//список скрытых
        public static List<outputNeuron> outputNeurons = new List<outputNeuron>();//список выходов

        public static List<List<dopaminHiddenNeuron>> dopaminHiddenNeurons = new List<List<dopaminHiddenNeuron>>();//список дофаминовых скрытых нейронов
        public static List<dopaminOutputNeuron> dopaminOutputNeurons = new List<dopaminOutputNeuron>();//список дофаминовых скрытых нейронов
        public static List<dopaminInputNeuron> dopaminInputNeurons = new List<dopaminInputNeuron>();
        public static float maxDistance = 16f;//максимвльная дистанция между нейронами

        //размер поля
        public static float poleX = 16;
        public static float poleY = 16;
        public static float poleZ = 16;

        //ячейки для рандомных координат
        public static float randX;
        public static float randY;
        public static float randZ;  

        //типы нейронов
        public class inputNeuron
        {
            public float x;
            public float y;
            public float z;
            public double value;

            //конструктор
            public inputNeuron(float startX, float startY, float startZ)
            {
                x = startX;
                y = startY;
                z = startZ;
                value = 0;
            }
        }
        public class hiddenNeuron
        {
            public int IDhid;
            public float x;
            public float y;
            public float z;
            public List<double> inputsWeights = new List<double>();
            public List<double> hiddenWeights = new List<double>();
            public List<int> inputIdWeights = new List<int>();
            public List<List<int>> hiddenIdWeights = new List<List<int>>();
            public double value;
            public double delta = 0;
            public hiddenNeuron(int idhid, float startX, float startY, float startZ)
            {
                IDhid = idhid;
                x = startX;
                y = startY;
                z = startZ;
                value = 0;
            }
        }
        public class outputNeuron
        {
            public float x;
            public float y;
            public float z;
            public List<double> inputsWeights = new List<double>();
            public List<double> hiddenWeights = new List<double>();
            public List<double> dopaminWeights = new List<double>();
            public List<int> inputIdWeights = new List<int>();
            public List<List<int>> hiddenIdWeights = new List<List<int>>();
            public List<int> dopaminIdWeigths = new List<int>();
            public double value;
            public double delta = 0;
            public outputNeuron(float startX, float startY, float startZ)
            {
                x = startX;
                y = startY;
                z = startZ;
                value = 0;
            }
        }

        //дофаминовые нейрононы
        public class dopaminInputNeuron
        {
            public float x;
            public float y;
            public float z;
            public double value;

            public dopaminInputNeuron(float startX, float startY, float startZ)
            {
                x = startX;
                y = startY;
                z = startZ;
                value = 0;
            }
        }
        public class dopaminHiddenNeuron
        {
            public int IDhid;
            public float x;
            public float y;
            public float z;
            public List<double> inputsWeights = new List<double>();
            public List<double> hiddenWeights = new List<double>();
            public List<double> outputWeights = new List<double>();
            public List<int> inputIdWeights = new List<int>();
            public List<List<int>> hiddenIdWeights = new List<List<int>>();
            public List<int> outputIdWeights = new List<int>();
            public double value;
            public double delta = 0;
            public dopaminHiddenNeuron(int idhid, float startX, float startY, float startZ)
            {
                IDhid = idhid;
                x = startX;
                y = startY;
                z = startZ;
                value = 0;
            }
        }
        public class dopaminOutputNeuron
        {
            public float x;
            public float y;
            public float z;
            public List<double> inputsWeights = new List<double>();
            public List<double> dopaminInputsWeights = new List<double>();
            public List<double> hiddenWeights = new List<double>();
            public List<double> outputWeights = new List<double>();
            public List<int> inputIdWeights = new List<int>();
            public List<int> dopaminInputsIdWeight = new List<int>();
            public List<List<int>> hiddenIdWeights = new List<List<int>>();
            public List<int> outputsIdWeights = new List<int>();
            public double value;
            public double delta = 0;
            public dopaminOutputNeuron(float startX, float startY, float startZ)
            {
                x = startX;
                y = startY;
                z = startZ;
                value = 0;
            }
        }
        
        static void DrawProgressBar(int progress, int total, int barSize)
        {
            // Вычисляем проценты и количество заполненных блоков
            int percentage = progress * 100 / total;
            int filledBlocks = progress * barSize / total;

            // Создаем строку прогресс-бара
            string bar = new string('█', filledBlocks) + new string('░', barSize - filledBlocks);

            // Печатаем строку. \r возвращает курсор в начало строки
            Console.Write($"\r[{bar}] {percentage}%");
        }
        
        //поиск связей
        public static void seekConection(List<inputNeuron> inputNeurons, List<List<hiddenNeuron>> hiddenNeurons, List<outputNeuron> outputNeurons, List<dopaminInputNeuron> dopaminInputNeurons, List<List<dopaminHiddenNeuron>> dopaminHiddenNeurons, List<dopaminOutputNeuron> dopaminOutputNeurons)
        {
            //поиск связей для выходного слоя
            for(int i=0;i<outputNeurons.Count; i++)
            {
                //координаты выхода
                double outX = outputNeurons[i].x;
                double outY = outputNeurons[i].y;
                double outZ = outputNeurons[i].z;
                //входной слой
                for(int j=0; j<inputNeurons.Count; j++)
                {   
                    //координаты входа
                    double inX = inputNeurons[j].x;
                    double inY = inputNeurons[j].y;
                    double inZ = inputNeurons[j].z;

                    //разница между координатоми
                    double deltaX = outX-inX;
                    double deltaY = outY-inY;
                    double deltaZ = outZ-inZ;
                    
                    double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                    if(distance<=maxDistance)
                    {
                        outputNeurons[i].inputIdWeights.Add(j);
                        outputNeurons[i].inputsWeights.Add(random.NextDouble()*2-1);
                    }
                }
                //скрытый слой
                for(int j=0; j<hiddenNeurons.Count; j++)
                {
                    for(int n = 0; n<hiddenNeurons[j].Count; n++)
                    {
                        //координаты входа
                        double inX = hiddenNeurons[j][n].x;
                        double inY = hiddenNeurons[j][n].y;
                        double inZ = hiddenNeurons[j][n].z;

                        //разница между координатоми
                        double deltaX = outX-inX;
                        double deltaY = outY-inY;
                        double deltaZ = outZ-inZ;
                        
                        double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                        if(distance<=maxDistance)
                        {
                            outputNeurons[i].hiddenIdWeights.Add(new List<int>{j, n});
                            outputNeurons[i].hiddenWeights.Add(random.NextDouble()*2-1);
                        }
                    }  
                }
                
                for(int j = 0; j<dopaminOutputNeurons.Count; j++)
                {
                        //координаты входа
                        double inX = dopaminOutputNeurons[j].x;
                        double inY = dopaminOutputNeurons[j].y;
                        double inZ = dopaminOutputNeurons[j].z;

                        //разница между координатоми
                        double deltaX = outX-inX;
                        double deltaY = outY-inY;
                        double deltaZ = outZ-inZ;
                        
                        double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                        if(distance<=maxDistance)
                        {
                            outputNeurons[i].dopaminIdWeigths.Add(j);
                            outputNeurons[i].dopaminWeights.Add(random.NextDouble()*2-1);
                            dopaminOutputNeurons[j].outputsIdWeights.Add(i);
                            dopaminOutputNeurons[j].outputWeights.Add(1);
                        }
                }
                DrawProgressBar(i+1, outputNeurons.Count, 30);
            }
            Console.WriteLine(" ");
            //поиск связей для скрытых слоёв
            for(int i=0; i<hiddenNeurons.Count; i++)
            {
                for(int n = 0; n < hiddenNeurons[i].Count; n++)
                {
                    double outX = hiddenNeurons[i][n].x;
                    double outY = hiddenNeurons[i][n].y;
                    double outZ = hiddenNeurons[i][n].z;
                    for(int j = 0; j<inputNeurons.Count; j++)
                    {
                        double inX = inputNeurons[j].x;
                        double inY = inputNeurons[j].y;
                        double inZ = inputNeurons[j].z;

                        double deltaX = outX-inX;
                        double deltaY = outY-inY;
                        double deltaZ = outZ-inZ;

                        double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                        if(distance<maxDistance)
                        {
                            hiddenNeurons[i][n].inputIdWeights.Add(j);
                            hiddenNeurons[i][n].inputsWeights.Add(random.NextDouble()*2-1);
                        }
                    }
                    for(int j = n; j < hiddenNeurons.Count; j++)
                    {
                        for(int b = 0; b < hiddenNeurons[j].Count; b++)
                        {
                            if(hiddenNeurons[i][n].IDhid > hiddenNeurons[j][b].IDhid)
                            {
                                double inX = hiddenNeurons[j][b].x;
                                double inY = hiddenNeurons[j][b].y;
                                double inZ = hiddenNeurons[j][b].z;

                                double deltaX = outX-inX;
                                double deltaY = outY-inY;
                                double deltaZ = outZ-inZ;

                                double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                                if(distance<maxDistance)
                                {
                                    hiddenNeurons[i][n].hiddenIdWeights.Add(new List<int>{j, b});
                                    hiddenNeurons[i][n].hiddenWeights.Add(random.NextDouble()*2-1);
                                }
                            }
                        }  
                    }
                }
                
                DrawProgressBar(i+1, hiddenNeurons.Count, 30);
            }
            Console.WriteLine(" ");
            //поиск связей для дофаминовой системы
            for(int i = 0; i<dopaminOutputNeurons.Count; i++)
            {
                //координаты выхода
                double outX = dopaminOutputNeurons[i].x;
                double outY = dopaminOutputNeurons[i].y;
                double outZ = dopaminOutputNeurons[i].z;
                //входной слой
                for(int j=0; j<inputNeurons.Count; j++)
                {   
                    //координаты входа
                    double inX = inputNeurons[j].x;
                    double inY = inputNeurons[j].y;
                    double inZ = inputNeurons[j].z;

                    //разница между координатоми
                    double deltaX = outX-inX;
                    double deltaY = outY-inY;
                    double deltaZ = outZ-inZ;
                    
                    double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                    if(distance<=maxDistance)
                    {
                        dopaminOutputNeurons[i].inputIdWeights.Add(j);
                        dopaminOutputNeurons[i].inputsWeights.Add(random.NextDouble()*2-1);
                    }
                }
                //скрытый слой
                for(int j=0; j<dopaminHiddenNeurons.Count; j++)
                {
                    for(int n = 0; n<dopaminHiddenNeurons[j].Count; n++)
                    {
                        //координаты входа
                        double inX = dopaminHiddenNeurons[j][n].x;
                        double inY = dopaminHiddenNeurons[j][n].y;
                        double inZ = dopaminHiddenNeurons[j][n].z;

                        //разница между координатоми
                        double deltaX = outX-inX;
                        double deltaY = outY-inY;
                        double deltaZ = outZ-inZ;
                        
                        double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                        if(distance<=maxDistance)
                        {
                            dopaminOutputNeurons[i].hiddenIdWeights.Add(new List<int>{j, n});
                            dopaminOutputNeurons[i].hiddenWeights.Add(random.NextDouble()*2-1);
                        }
                    }  
                    
                }
                
                for(int j = 0; j < dopaminInputNeurons.Count; j++)
                {
                    //координаты входа
                    double inX = dopaminInputNeurons[j].x;
                    double inY = dopaminInputNeurons[j].y;
                    double inZ = dopaminInputNeurons[j].z;

                    //разница между координатоми
                    double deltaX = outX-inX;
                    double deltaY = outY-inY;
                    double deltaZ = outZ-inZ;
                    
                    double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                    if(distance<=maxDistance)
                    {
                        dopaminOutputNeurons[i].dopaminInputsIdWeight.Add(j);
                        dopaminOutputNeurons[i].dopaminInputsWeights.Add(random.NextDouble()*2-1);
                    }
                }

                DrawProgressBar(i+1, dopaminOutputNeurons.Count, 30);
            }
            Console.WriteLine(" ");
            for(int i = 0; i<dopaminHiddenNeurons.Count; i++)
            {

                for(int n = 0; n < dopaminHiddenNeurons[i].Count; n++)
                {
                    double outX = dopaminHiddenNeurons[i][n].x;
                    double outY = dopaminHiddenNeurons[i][n].y;
                    double outZ = dopaminHiddenNeurons[i][n].z;
                    for(int j = 0; j<inputNeurons.Count; j++)
                    {
                        double inX = inputNeurons[j].x;
                        double inY = inputNeurons[j].y;
                        double inZ = inputNeurons[j].z;

                        double deltaX = outX-inX;
                        double deltaY = outY-inY;
                        double deltaZ = outZ-inZ;

                        double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                        if(distance<maxDistance)
                        {
                            dopaminHiddenNeurons[i][n].inputIdWeights.Add(j);
                            dopaminHiddenNeurons[i][n].inputsWeights.Add(random.NextDouble()*2-1);
                        }
                    }
                    for(int j = n; j < dopaminHiddenNeurons.Count; j++)
                    {
                        for(int b = 0; b < dopaminHiddenNeurons[j].Count; b++)
                        {
                            if(dopaminHiddenNeurons[i][n].IDhid > dopaminHiddenNeurons[j][b].IDhid)
                            {
                                double inX = dopaminHiddenNeurons[j][b].x;
                                double inY = dopaminHiddenNeurons[j][b].y;
                                double inZ = dopaminHiddenNeurons[j][b].z;

                                double deltaX = outX-inX;
                                double deltaY = outY-inY;
                                double deltaZ = outZ-inZ;

                                double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                                if(distance<maxDistance)
                                {
                                    dopaminHiddenNeurons[i][n].hiddenIdWeights.Add(new List<int>{j, b});
                                    dopaminHiddenNeurons[i][n].hiddenWeights.Add(random.NextDouble()*2-1);
                                }
                            }
                        }  
                    }
                    for(int j = 0; j<outputNeurons.Count; j++)
                    {
                        double inX = outputNeurons[j].x;
                        double inY = outputNeurons[j].y;
                        double inZ = outputNeurons[j].z;

                        double deltaX = outX-inX;
                        double deltaY = outY-inY;
                        double deltaZ = outZ-inZ;

                        double distance = Math.Sqrt(deltaX*deltaX+deltaY*deltaY+deltaZ*deltaZ);
                        if(distance<maxDistance)
                        {
                            dopaminHiddenNeurons[i][n].outputIdWeights.Add(j);
                            dopaminHiddenNeurons[i][n].outputWeights.Add(random.NextDouble()*2-1);
                        }
                    }
                    DrawProgressBar(i+1, dopaminHiddenNeurons.Count, 30);
                }
                
                
            }

            Console.WriteLine(" Связи созданы");
        }

        //примой проход
        public static List<double> forwardNetwork(double[] inputs, List<inputNeuron> inputNeurons, List<List<hiddenNeuron>> hiddenNeurons, List<outputNeuron> outputNeurons)
        {
            List<double> output = new List<double>();//значения выходных нейронов после прямого прохода

            //приписываем значение каторые нам нужны
            for(int i = 0; i<inputs.Length; i++)
            {
                inputNeurons[i].value = inputs[i];
            }
            //прямой проход
            for(int i = 0; i < outputNeurons.Count; i++)
            {
                for(int j = 0; j < outputNeurons[i].inputIdWeights.Count; j++)
                {
                    outputNeurons[i].value += inputNeurons[outputNeurons[i].inputIdWeights[j]].value*outputNeurons[i].inputsWeights[j];
                }
            }
            //сркытый слой
            for(int i = 0; i < hiddenNeurons.Count; i++)
            {
                for(int n = 0; n < hiddenNeurons[i].Count; n++)
                {
                    //от входов к скрытому слою
                    for(int j = 0; j < hiddenNeurons[i][n].inputIdWeights.Count; j++)
                    {
                        hiddenNeurons[i][n].value += hiddenNeurons[i][n].inputsWeights[j]*inputNeurons[hiddenNeurons[i][n].inputIdWeights[j]].value;
                    }
                    //от скрытого к скрытому
                    for(int j = 0; j < hiddenNeurons[i][n].hiddenIdWeights.Count; j++)
                    {
                        hiddenNeurons[i][n].value += hiddenNeurons[i][n].hiddenWeights[j]*hiddenNeurons[hiddenNeurons[i][n].hiddenIdWeights[j][0]][hiddenNeurons[i][n].hiddenIdWeights[j][1]].value;
                    }
                }
                
            }

            //relu
            for(int i = 0; i < hiddenNeurons.Count; i++)
            {
                for(int j = 0; j < hiddenNeurons[i].Count; j++)
                {
                    hiddenNeurons[i][j].value = Sigmoid(hiddenNeurons[i][j].value);
                }
            }
                
            //выходной слой
            for(int i = 0; i < outputNeurons.Count; i++)
            {
                for(int j = 0; j < outputNeurons[i].inputIdWeights.Count; j++)
                {
                    outputNeurons[i].value += outputNeurons[i].inputsWeights[j]*inputNeurons[outputNeurons[i].inputIdWeights[j]].value;
                }
                //от скрытого к скрытому
                for(int j = 0; j < outputNeurons[i].hiddenIdWeights.Count; j++)
                {
                    outputNeurons[i].value += outputNeurons[i].hiddenWeights[j]*hiddenNeurons[outputNeurons[i].hiddenIdWeights[j][0]][outputNeurons[i].hiddenIdWeights[j][1]].value;
                }
            }
            for(int i = 0; i < outputNeurons.Count; i++)
            {
                output.Add(outputNeurons[i].value);
            }
            return output;
        }
        public static List<double> dopaminForwardNetwork(double[] inputs, List<inputNeuron> inputNeurons, List<List<dopaminHiddenNeuron>> dopaminHiddenNeurons, List<dopaminOutputNeuron> dopaminOutputNeurons)
        {
            List<double> result = new List<double>();
            for(int i = 0; i < inputs.Length; i++)
            {
                inputNeurons[i].value = inputs[i];
            }
            for(int i = 0; i < dopaminHiddenNeurons[0].Count; i++)
            {
                for(int j = 0; j < dopaminHiddenNeurons[0][i].inputIdWeights.Count; j++)
                {
                    dopaminHiddenNeurons[0][i].value += inputNeurons[dopaminHiddenNeurons[0][i].inputIdWeights[j]].value*dopaminHiddenNeurons[0][i].inputsWeights[j];
                }
                for(int j = 0; j < dopaminHiddenNeurons[0][i].outputIdWeights.Count; j++)
                {
                    dopaminHiddenNeurons[0][i].value += outputNeurons[dopaminHiddenNeurons[0][i].outputIdWeights[j]].value*dopaminHiddenNeurons[0][i].outputIdWeights[j];
                }
            }

            for(int i = 0; i < dopaminHiddenNeurons.Count; i++)
            {
                for(int j = 0; j < dopaminHiddenNeurons[i].Count; j++)
                {
                    for(int n = 0; n < dopaminHiddenNeurons[i][j].inputIdWeights.Count; n++)
                    {
                        dopaminHiddenNeurons[i][j].value += inputNeurons[dopaminHiddenNeurons[i][j].inputIdWeights[n]].value*dopaminHiddenNeurons[i][j].inputsWeights[n];
                    }
                    for(int n = 0; n < dopaminHiddenNeurons[i][j].hiddenIdWeights.Count; n++)
                    {
                        dopaminHiddenNeurons[i][j].value += dopaminHiddenNeurons[dopaminHiddenNeurons[i][j].hiddenIdWeights[n][0]][dopaminHiddenNeurons[i][j].hiddenIdWeights[n][0]].value*dopaminHiddenNeurons[i][j].hiddenWeights[n];
                    }
                    for(int n = 0; n < dopaminHiddenNeurons[i][j].outputIdWeights.Count; n++)
                    {
                        dopaminHiddenNeurons[i][j].value += outputNeurons[dopaminHiddenNeurons[i][j].outputIdWeights[n]].value*dopaminHiddenNeurons[i][j].outputIdWeights[n];
                    }
                }
            }

            for(int i = 0; i < dopaminOutputNeurons.Count; i++)
            {
                for(int j = 0; j < dopaminOutputNeurons[i].inputIdWeights.Count; j++)
                {
                    outputNeurons[i].value += inputNeurons[dopaminOutputNeurons[i].inputIdWeights[j]].value*dopaminOutputNeurons[i].inputsWeights[j];
                }
                for(int j = 0; j < dopaminOutputNeurons[i].hiddenIdWeights.Count; j++)
                {
                    dopaminOutputNeurons[i].value += dopaminHiddenNeurons[dopaminOutputNeurons[i].hiddenIdWeights[j][0]][dopaminOutputNeurons[i].hiddenIdWeights[j][0]].value*dopaminOutputNeurons[i].hiddenWeights[j];
                }
            }
            for(int i = 0; i < dopaminOutputNeurons.Count; i++)
            {
                result.Add(dopaminOutputNeurons[i].value);
            } 
            return result;
        }
        
        //обучение с учителем обычной нейронной сети
        public static void deebLearn(List<inputNeuron> inputNeurons, List<List<hiddenNeuron>> hiddenNeurons, List<outputNeuron> outputNeurons, double[] inputs, double[] target, double alpha = 0.01d) 
        {
            List<double> pred = forwardNetwork(inputs, inputNeurons, hiddenNeurons, outputNeurons);//предсказание

            for(int n = 0; n < pred.Count; n++)
            {
                outputNeurons[n].delta = pred[n]-target[n];
            }
            //создание дельт для выходов
            for(int n = 0; n < outputNeurons.Count; n++)
            {
                for(int b = 0; b < outputNeurons[n].hiddenWeights.Count; b++)
                {
                    hiddenNeurons[outputNeurons[n].hiddenIdWeights[b][0]][outputNeurons[n].hiddenIdWeights[b][1]].delta += outputNeurons[n].delta * outputNeurons[n].hiddenWeights[b];
                }
            }
            //создения дельт для скрытых слоёв
            for(int n = hiddenNeurons.Count-1; n >= 0; n--)
            {
                for(int b = 0; b < hiddenNeurons[n].Count; b++)
                {
                    for(int h = 0; h < hiddenNeurons[n][b].hiddenWeights.Count; h++)
                    {
                        hiddenNeurons[hiddenNeurons[n][b].hiddenIdWeights[h][0]][hiddenNeurons[n][b].hiddenIdWeights[h][1]].delta += hiddenNeurons[n][b].delta * hiddenNeurons[n][b].inputsWeights[h];
                    }
                }
            }

            //изменене веса

            //выход
            for(int n = 0; n < outputNeurons.Count; n++)
            {
                for(int b = 0; b < outputNeurons[n].inputIdWeights.Count; b++)
                {
                    outputNeurons[n].inputsWeights[b] -= outputNeurons[n].delta*inputNeurons[outputNeurons[n].inputIdWeights[b]].value*alpha;
                }
                for(int b = 0; b < outputNeurons[n].hiddenIdWeights.Count; b++)
                {
                    outputNeurons[n].hiddenWeights[b] -= outputNeurons[n].delta*hiddenNeurons[outputNeurons[n].hiddenIdWeights[b][0]][outputNeurons[n].hiddenIdWeights[b][1]].value*alpha;
                }
            }
            //скрытый слой
            for(int n = 0; n < hiddenNeurons.Count; n++)
            {
                for(int b = 0; b < hiddenNeurons[n].Count; b++)
                {
                    for(int h = 0; h < hiddenNeurons[n][b].inputIdWeights.Count; h++)
                    {
                        hiddenNeurons[n][b].inputsWeights[h] -= hiddenNeurons[n][b].delta*inputNeurons[hiddenNeurons[n][b].inputIdWeights[h]].value;
                    }

                    for(int h = 0; h < hiddenNeurons[n][b].hiddenIdWeights.Count; h++)
                    {
                        hiddenNeurons[n][b].hiddenWeights[h] -= hiddenNeurons[n][b].delta*hiddenNeurons[hiddenNeurons[n][b].hiddenIdWeights[h][0]][hiddenNeurons[n][b].hiddenIdWeights[h][1]].value;
                    }
                }
            }
            clearance(); 
        }
        //обучение с Учителем Критика
        public static void dopaminDeepLearn(List<inputNeuron> inputNeurons, List<List<dopaminHiddenNeuron>> dopaminHiddenNeurons, List<dopaminOutputNeuron> dopaminOutputNeurons, List<dopaminInputNeuron> dopaminInputNeurons, double [] inputs, double alpha = 0.01d)
        {
            List<double> pred = dopaminForwardNetwork(inputs, inputNeurons, dopaminHiddenNeurons, dopaminOutputNeurons);//предсказание

            for(int n = 0; n < dopaminOutputNeurons.Count; n++)
            {
                for(int b = 0; b < dopaminOutputNeurons[n].dopaminInputsIdWeight.Count; b++)
                {
                    dopaminOutputNeurons[n].delta += dopaminOutputNeurons[n].value - dopaminInputNeurons[dopaminOutputNeurons[n].dopaminInputsIdWeight[b]].value*dopaminOutputNeurons[n].dopaminInputsWeights[b];
                }
            }
            //создание дельт для выходов
            for(int n = 0; n < dopaminOutputNeurons.Count; n++)
            {
                for(int b = 0; b < dopaminOutputNeurons[n].hiddenWeights.Count; b++)
                {
                    dopaminHiddenNeurons[dopaminOutputNeurons[n].hiddenIdWeights[b][0]][dopaminOutputNeurons[n].hiddenIdWeights[b][1]].delta += dopaminOutputNeurons[n].delta * dopaminOutputNeurons[n].hiddenWeights[b];
                }
            }
            //создения дельт для скрытых слоёв
            for(int n = dopaminHiddenNeurons.Count-1; n >= 0; n--)
            {
                for(int b = 0; b < dopaminHiddenNeurons[n].Count; b++)
                {
                    for(int h = 0; h < dopaminHiddenNeurons[n][b].hiddenWeights.Count; h++)
                    {
                        dopaminHiddenNeurons[dopaminHiddenNeurons[n][b].hiddenIdWeights[h][0]][dopaminHiddenNeurons[n][b].hiddenIdWeights[h][1]].delta += dopaminHiddenNeurons[n][b].delta * dopaminHiddenNeurons[n][b].inputsWeights[h];
                    }
                }
            }

            //изменене веса

            //выход
            for(int n = 0; n < dopaminOutputNeurons.Count; n++)
            {
                for(int b = 0; b < dopaminOutputNeurons[n].inputIdWeights.Count; b++)
                {
                    dopaminOutputNeurons[n].inputsWeights[b] -= outputNeurons[n].delta*inputNeurons[outputNeurons[n].inputIdWeights[b]].value*alpha;
                }
                for(int b = 0; b < dopaminOutputNeurons[n].hiddenIdWeights.Count; b++)
                {
                    dopaminOutputNeurons[n].hiddenWeights[b] -= dopaminOutputNeurons[n].delta*dopaminHiddenNeurons[dopaminOutputNeurons[n].hiddenIdWeights[b][0]][dopaminOutputNeurons[n].hiddenIdWeights[b][1]].value*alpha;
                }
            }
            //скрытый слой
            for(int n = 0; n < dopaminHiddenNeurons.Count; n++)
            {
                for(int b = 0; b < dopaminHiddenNeurons[n].Count; b++)
                {
                    for(int h = 0; h < dopaminHiddenNeurons[n][b].inputIdWeights.Count; h++)
                    {
                        dopaminHiddenNeurons[n][b].inputsWeights[h] -= dopaminHiddenNeurons[n][b].delta*inputNeurons[dopaminHiddenNeurons[n][b].inputIdWeights[h]].value;
                    }

                    for(int h = 0; h < dopaminHiddenNeurons[n][b].hiddenIdWeights.Count; h++)
                    {
                        dopaminHiddenNeurons[n][b].hiddenWeights[h] -= dopaminHiddenNeurons[n][b].delta*dopaminHiddenNeurons[dopaminHiddenNeurons[n][b].hiddenIdWeights[h][0]][dopaminHiddenNeurons[n][b].hiddenIdWeights[h][1]].value;
                    }
                }
            }
            clearance();
        }
        public static void dopaminRL(List<inputNeuron> inputNeurons, List<List<hiddenNeuron>> hiddenNeurons, List<outputNeuron> outputNeurons, List<List<dopaminHiddenNeuron>> dopaminHiddenNeurons, List<dopaminOutputNeuron> dopaminOutputNeurons, List<dopaminInputNeuron> dopaminInputNeurons, double [] inputs, double alpha = 0.01d)
        {
            List<double> pred = forwardNetwork(inputs, inputNeurons, hiddenNeurons, outputNeurons);//предсказание
            List<double> reward = dopaminForwardNetwork(inputs, inputNeurons, dopaminHiddenNeurons, dopaminOutputNeurons);

            for(int n = 0; n < pred.Count; n++)
            {
                // 1. Собираем суммарный дофаминовый сигнал для этого конкретного нейрона
                double totalDopaminSignal = 0;
                
                for (int i = 0; i < outputNeurons[n].dopaminIdWeigths.Count; i++)
                {
                    // Находим индекс дофаминового нейрона, с которым есть связь
                    int dopaminIndex = outputNeurons[n].dopaminIdWeigths[i]; 
                    
                    // Умножаем значение дофамина на вес этой связи
                    totalDopaminSignal += reward[dopaminIndex] * outputNeurons[n].dopaminWeights[i];
                }

                // 2. Считаем дельту (умножаем суммарный сигнал на производную сигмоиды)
                outputNeurons[n].delta = totalDopaminSignal * (pred[n] * (1.0 - pred[n]));
            }
            //создание дельт для выходов
            for(int n = 0; n < outputNeurons.Count; n++)
            {
                for(int b = 0; b < outputNeurons[n].hiddenWeights.Count; b++)
                {
                    hiddenNeurons[outputNeurons[n].hiddenIdWeights[b][0]][outputNeurons[n].hiddenIdWeights[b][1]].delta += outputNeurons[n].delta * outputNeurons[n].hiddenWeights[b];
                }
            }
            //создения дельт для скрытых слоёв
            for(int n = hiddenNeurons.Count-1; n >= 0; n--)
            {
                for(int b = 0; b < hiddenNeurons[n].Count; b++)
                {
                    for(int h = 0; h < hiddenNeurons[n][b].hiddenWeights.Count; h++)
                    {
                        hiddenNeurons[hiddenNeurons[n][b].hiddenIdWeights[h][0]][hiddenNeurons[n][b].hiddenIdWeights[h][1]].delta += hiddenNeurons[n][b].delta * hiddenNeurons[n][b].inputsWeights[h];
                    }
                }
            }

            //изменене веса

            //выход
            for(int n = 0; n < outputNeurons.Count; n++)
            {
                for(int b = 0; b < outputNeurons[n].inputIdWeights.Count; b++)
                {
                    outputNeurons[n].inputsWeights[b] -= outputNeurons[n].delta*inputNeurons[outputNeurons[n].inputIdWeights[b]].value*alpha;
                }
                for(int b = 0; b < outputNeurons[n].hiddenIdWeights.Count; b++)
                {
                    outputNeurons[n].hiddenWeights[b] -= outputNeurons[n].delta*hiddenNeurons[outputNeurons[n].hiddenIdWeights[b][0]][outputNeurons[n].hiddenIdWeights[b][1]].value*alpha;
                }
            }
            //скрытый слой
            for(int n = 0; n < hiddenNeurons.Count; n++)
            {
                for(int b = 0; b < hiddenNeurons[n].Count; b++)
                {
                    for(int h = 0; h < hiddenNeurons[n][b].inputIdWeights.Count; h++)
                    {
                        hiddenNeurons[n][b].inputsWeights[h] -= hiddenNeurons[n][b].delta*inputNeurons[hiddenNeurons[n][b].inputIdWeights[h]].value;
                    }

                    for(int h = 0; h < hiddenNeurons[n][b].hiddenIdWeights.Count; h++)
                    {
                        hiddenNeurons[n][b].hiddenWeights[h] -= hiddenNeurons[n][b].delta*hiddenNeurons[hiddenNeurons[n][b].hiddenIdWeights[h][0]][hiddenNeurons[n][b].hiddenIdWeights[h][1]].value;
                    }
                }
            }
            clearance();
        }
        //отчистка
        public static void clearance()
        {
            for(int i = 0; i < inputNeurons.Count; i++)
            {
                inputNeurons[i].value = 0;
            }
            for(int i = 0; i < hiddenNeurons.Count; i++)
            {
                for(int j = 0; j < hiddenNeurons[i].Count; j++)
                {
                    hiddenNeurons[i][j].value = 0;
                    hiddenNeurons[i][j].delta = 0;  
                }
            }
            for(int i = 0; i < outputNeurons.Count; i++)
            {
                outputNeurons[i].value = 0;
                outputNeurons[i].delta = 0;
            }

            for(int i = 0; i < dopaminInputNeurons.Count; i++)
            {
                dopaminInputNeurons[i].value = 0;
            }
            for(int i = 0; i < dopaminHiddenNeurons.Count; i++)
            {
                for(int j = 0; j < dopaminHiddenNeurons[i].Count; j++)
                {
                    dopaminHiddenNeurons[i][j].value = 0;
                    dopaminHiddenNeurons[i][j].delta = 0;  
                }
            }
            for(int i = 0; i < dopaminOutputNeurons.Count; i++)
            {
                dopaminOutputNeurons[i].value = 0;
                dopaminOutputNeurons[i].delta = 0;
            }
        }
        
        //генерация нейронов
        public static List<inputNeuron> genericInputs(int enuIn)
        {
            List<inputNeuron> result = new List<inputNeuron>();
            for(int i = 0; i < enuIn; i++)
            {
                randX = (float)random.NextDouble() * poleX - poleX/2;
                randY = (float)random.NextDouble() * poleY - poleY/2;
                randZ = (float)random.NextDouble() * poleZ - poleZ/2;

                inputNeuron neuronG = new inputNeuron(randX, randY, randZ);
                result.Add(neuronG);
            }
            Console.WriteLine("Выходы созданы"); 
            return result;
        }
        public static List<List<hiddenNeuron>> genericHiddden(List<int> enuHid)
        {
            //генерация скрытых
            List<List<hiddenNeuron>> result = new List<List<hiddenNeuron>>();
            List<hiddenNeuron> neurons = new List<hiddenNeuron>();

            int idnum = 0;
            int vbor = 0;

            for(int i = 0; i < enuHid[0]; i++)
            {
                float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                randX = (float)(inputNeurons[vbor].x + deltaX);
                randZ = (float)(inputNeurons[vbor].z + delatZ);
                randY = (float)(inputNeurons[vbor].y + deltaY);

                hiddenNeuron neuronG = new hiddenNeuron(0, randX, randY, randZ);
                neurons.Add(neuronG);
                idnum++;
            }
            result.Add(neurons);
            for(int i = 1; i < enuHid.Count; i++)
            {
                neurons = new List<hiddenNeuron>{};
                for(int j = 0; j < enuHid[i]; j++)
                {

                    vbor = random.Next(enuHid[i-1]);

                    float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                    float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                    float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                    randX = (float)(result[i-1][vbor].x + deltaX);
                    randZ = (float)(result[i-1][vbor].z + delatZ);
                    randY = (float)(result[i-1][vbor].y + deltaY);

                    hiddenNeuron neuronG = new hiddenNeuron(i, randX, randY, randZ);
                    neurons.Add(neuronG);
                    idnum++;
                }
                result.Add(neurons);
            }
            Console.WriteLine("Скрытые слои созданы");
            return result;
        }
        public static List<outputNeuron> genericOutput(int enuOut)
        {
            int vbor1;
            int vbor2;
            List<outputNeuron> result = new List<outputNeuron>();

            for(int i = 0; i < enuOut; i++)
            {
                if(hiddenNeurons.Any())
                {

                    vbor1 = hiddenNeurons.Count-1;            
                    vbor2 = random.Next(hiddenNeurons[vbor1].Count);
                    float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                    float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                    float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                    randX = hiddenNeurons[vbor1][vbor2].x + deltaX;
                    randZ = hiddenNeurons[vbor1][vbor2].z + delatZ;
                    randY = hiddenNeurons[vbor1][vbor2].y + deltaY;

                    outputNeuron neuronG = new outputNeuron(randX, randY, randZ);
                    result.Add(neuronG);
                } else
                {
                    vbor1 = random.Next(enuIN);

                    float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                    float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                    float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                    randX = inputNeurons[vbor1].x + deltaX;
                    randZ = inputNeurons[vbor1].z + delatZ;
                    randY = inputNeurons[vbor1].y + deltaY;

                    outputNeuron neuronG = new outputNeuron(randX, randY, randZ);
                    result.Add(neuronG);
                }
                

                // Console.WriteLine($"вход {i} создан");
            }
            Console.WriteLine("Выходы созданы"); 
            return result;
        }
        public static List<dopaminInputNeuron> genericDopaminInputs(int enuIn)
        {
            List<dopaminInputNeuron> result = new List<dopaminInputNeuron>();
            for(int i = 0; i < enuIn; i++)
            {
                int vbor = random.Next(dopaminOutputNeurons.Count);
                float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                randX = dopaminOutputNeurons[vbor].x + deltaX;
                randY = dopaminOutputNeurons[vbor].y + deltaY;
                randZ = dopaminOutputNeurons[vbor].z + delatZ;

                dopaminInputNeuron neuronG = new dopaminInputNeuron(randX, randY, randZ);
                result.Add(neuronG);
            }
            return result;
        }
        public static List<List<dopaminHiddenNeuron>> genericDopaminHidden(List<int> enuHid)
        {
            //генерация скрытых
            List<List<dopaminHiddenNeuron>> result = new List<List<dopaminHiddenNeuron>>();
            List<dopaminHiddenNeuron> neurons = new List<dopaminHiddenNeuron>();

            int idnum = 0;
            int vbor1 = 0;
            int vbor2 = 0;

            for(int i = 0; i < enuHid[0]; i++)
            {
                vbor2 = random.Next(2);
                if(vbor2==1)
                {
                    vbor1 = random.Next(inputNeurons.Count);

                    float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                    float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                    float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                    randX = (float)(inputNeurons[vbor1].x + deltaX);
                    randZ = (float)(inputNeurons[vbor1].z + delatZ);
                    randY = (float)(inputNeurons[vbor1].y + deltaY);

                    dopaminHiddenNeuron neuronG = new dopaminHiddenNeuron(0, randX, randY, randZ);
                    neurons.Add(neuronG);
                    idnum++;
                } else
                {
                    vbor1 = random.Next(outputNeurons.Count);

                    float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                    float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                    float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                    randX = (float)(outputNeurons[vbor1].x + deltaX);
                    randZ = (float)(outputNeurons[vbor1].z + delatZ);
                    randY = (float)(outputNeurons[vbor1].y + deltaY);

                    dopaminHiddenNeuron neuronG = new dopaminHiddenNeuron(0, randX, randY, randZ);
                    neurons.Add(neuronG);
                    idnum++;
                }
                
            }
            result.Add(neurons);
            for(int i = 1; i < enuHid.Count; i++)
            {
                neurons = new List<dopaminHiddenNeuron>{};
                for(int j = 0; j < enuHid[i]; j++)
                {

                    vbor1 = random.Next(enuHid[i-1]);

                    float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                    float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                    float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                    randX = (float)(result[i-1][vbor1].x + deltaX);
                    randZ = (float)(result[i-1][vbor1].z + delatZ);
                    randY = (float)(result[i-1][vbor1].y + deltaY);

                    dopaminHiddenNeuron neuronG = new dopaminHiddenNeuron(i, randX, randY, randZ);
                    neurons.Add(neuronG);
                    idnum++;
                }
                result.Add(neurons);
            }
            return result;
        }
        public static List<dopaminOutputNeuron> genericDopaminOutput(int enuOut)
        {
            int vbor1;
            int vbor2;
            List<dopaminOutputNeuron> result = new List<dopaminOutputNeuron>();
            for(int i = 0; i < enuOut; i++)
            {
                
                if(dopaminHiddenNeurons.Any())
                {

                    vbor1 = dopaminHiddenNeurons.Count-1;            
                    vbor2 = random.Next(dopaminHiddenNeurons[vbor1].Count);
                    float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                    float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                    float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                    randX = dopaminHiddenNeurons[vbor1][vbor2].x + deltaX;
                    randZ = dopaminHiddenNeurons[vbor1][vbor2].z + delatZ;
                    randY = dopaminHiddenNeurons[vbor1][vbor2].y + deltaY;

                    dopaminOutputNeuron neuronG = new dopaminOutputNeuron(randX, randY, randZ);
                    result.Add(neuronG);
                } else
                {
                    vbor1 = random.Next(enuIN);

                    float deltaX = (float)random.NextDouble()*maxDistance-maxDistance/2;
                    float delatZ = (float)random.NextDouble()*MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)-MathF.Sqrt(maxDistance*maxDistance-deltaX*deltaX)/2;
                    float deltaY = (float)random.NextDouble()*MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)-MathF.Sqrt(MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)*MathF.Sqrt(deltaX*deltaX+delatZ*delatZ)+maxDistance*maxDistance)/2;

                    randX = inputNeurons[vbor1].x + deltaX;
                    randZ = inputNeurons[vbor1].z + delatZ;
                    randY = inputNeurons[vbor1].y + deltaY;

                    dopaminOutputNeuron neuronG = new dopaminOutputNeuron(randX, randY, randZ);
                    result.Add(neuronG);
                }
                

                // Console.WriteLine($"вход {i} создан");
            }
            Console.WriteLine("Критик создан"); 
            return result;
        }
        //сигмойда
        public static double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }
}

    public static int enuIN = 2;//количество входов
    public static List<int> enuHID = new List<int>  
    {
        3, 3, 3
    };//количество скрытых
    public static int enuOUT = 1;//количество выходов

    public static int enuDoIN = 1;
    public static List<int> enuDoHID = new List<int>
    {
        3, 2, 3
    };
    public static int enuDoOUT = 1;


   static void Main(string[] args)
    {
        //Console.Clear();
        //Console.Write("\x1b[3J");
        //генерация нейронов
        for(int Gen = 0; Gen < 100; Gen++)
        {
            Neuron.inputNeurons = Neuron.genericInputs(enuIN);
            Neuron.hiddenNeurons = Neuron.genericHiddden(enuHID);
            Neuron.outputNeurons = Neuron.genericOutput(enuOUT);

            Neuron.dopaminHiddenNeurons = Neuron.genericDopaminHidden(enuDoHID);
            Neuron.dopaminOutputNeurons = Neuron.genericDopaminOutput(enuDoOUT);
            Neuron.dopaminInputNeurons = Neuron.genericDopaminInputs(enuDoIN);

            Neuron.seekConection(Neuron.inputNeurons, Neuron.hiddenNeurons, Neuron.outputNeurons, Neuron.dopaminInputNeurons, Neuron.dopaminHiddenNeurons, Neuron.dopaminOutputNeurons);
            Console.WriteLine("Симуляция запущена!");
        }
        
            
    }
}
