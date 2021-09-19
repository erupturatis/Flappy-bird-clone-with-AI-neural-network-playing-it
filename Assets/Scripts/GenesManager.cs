using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GenesManager : MonoBehaviour
{
    float[,,] Weights;
    float[] Biases;
    NeuralNetwork[] anns = new NeuralNetwork[102];
    NeuralNetwork[] annsNewGen = new NeuralNetwork[102];
    public GameObject bird;
    float MutationPower;
    int[] Deaths;
    int[] First5;
    int Dead;
    float[] DeathsFitness;
    int K = 1;
    //BUGS
    //OVERWRITING THE FIRST BIRDS IS A BIG NONO
    //CHANGE THE METHON NOT THE LAST TO DIE, THE MOST FAR
    //KEEP THE BEST 3
    public int[] Actions;

    public float[] YDifference;
    public float[] DistanceUntilNext;

    int NumberSpawned = 100;

    int BirdNumb = 0;

    public class birdi{
        public int birdN;
        public float fitness;
    }

    List<birdi> Nums = new List<birdi>();



    void RandomizeWeights()
    {
        for(int i = 0; i <= 6; i++)
        {
            for(int j = 0; j <= 10; j++)
            {
                for(int t = 0; t <= 10; t++)
                {
                    Weights[i,j,t] = Random.Range(-1f , 1f);
                    //print(Weights[i, j, t]);
                }
            }
        }
    }
    void RandomizeBiases()
    {
        for (int i = 0; i <= 6; i++)
        {
            Biases[i] = Random.Range(-2f, 2f);
        }
    }
    float Mutation()
    {
        return Random.Range(-4f, 4f);
    }
    public void SetDeath(int t, float fitness)
    {
        birdi y = new birdi();
        y.birdN = t;
        y.fitness = fitness;

        Deaths[Dead] = t;
        DeathsFitness[Dead] = fitness;

        Nums.Add(y);
        Dead += 1;
    }
    void SpawnPlayers()
    {
        for(int i = 0; i <= NumberSpawned-1; i++)
        {
            Vector3 position = new Vector3(-1f, 0f, 0f);
            GameObject bot = Instantiate(bird,position,Quaternion.identity);
            bot.GetComponent<Player>().playerNumber = i;
            //print("spawned");
        }
    }

    void Start()
    {
        Time.timeScale = 2f;
        Deaths = new int[NumberSpawned + 1];
        DeathsFitness = new float[NumberSpawned + 1];

        DistanceUntilNext = new float[NumberSpawned+1];
        YDifference = new float[NumberSpawned+1];
        Actions = new int[NumberSpawned+1];
        Weights = new float[11,11,11];
        Biases = new float[11];
        First5 = new int[7];
        
        for (int i = 0; i <= NumberSpawned-1; i++)
        {
            RandomizeWeights();
            RandomizeBiases();

            anns[i] = new NeuralNetwork(3,9,i);
            anns[i].NetworkNum = i;
            anns[i].SetW(Weights);
            anns[i].SetB(Biases);
        }

        SpawnPlayers();
        StartCoroutine(Wait3Frames());
    }

    void CombineNetworks()
    {
        //Take first 5 winner birds
        int Lng = NumberSpawned - 1;

        annsNewGen[BirdNumb] = new NeuralNetwork(3, 9, BirdNumb);
        annsNewGen[BirdNumb].SetW(anns[Deaths[Lng-BirdNumb]].weights);
        annsNewGen[BirdNumb].SetB(anns[Deaths[Lng - BirdNumb]].Biases);
        //print(Deaths[Lng - BirdNumb]);
        BirdNumb++;
        annsNewGen[BirdNumb] = new NeuralNetwork(3, 9, BirdNumb);
        annsNewGen[BirdNumb].SetW(anns[Deaths[Lng - BirdNumb]].weights);
        annsNewGen[BirdNumb].SetB(anns[Deaths[Lng - BirdNumb]].Biases);
        //print(Deaths[Lng - BirdNumb]);
        BirdNumb++;
        annsNewGen[BirdNumb] = new NeuralNetwork(3, 9, BirdNumb);
        annsNewGen[BirdNumb].SetW(anns[Deaths[Lng - BirdNumb]].weights);
        annsNewGen[BirdNumb].SetB(anns[Deaths[Lng - BirdNumb]].Biases);
        //print(Deaths[Lng - BirdNumb]);
        BirdNumb++;
        annsNewGen[BirdNumb] = new NeuralNetwork(3, 9, BirdNumb);
        annsNewGen[BirdNumb].SetW(anns[Deaths[Lng - BirdNumb]].weights);
        annsNewGen[BirdNumb].SetB(anns[Deaths[Lng - BirdNumb]].Biases);
        //print(Deaths[Lng - BirdNumb]);
        BirdNumb++;
        annsNewGen[BirdNumb] = new NeuralNetwork(3, 9, BirdNumb);
        annsNewGen[BirdNumb].SetW(anns[Deaths[Lng - BirdNumb]].weights);
        annsNewGen[BirdNumb].SetB(anns[Deaths[Lng - BirdNumb]].Biases);
        //print(Deaths[Lng - BirdNumb]);
        BirdNumb++;

        //print(Deaths[Lng] + " " + Deaths[Lng-1] + " " + Deaths[Lng-2] + " " + Deaths[Lng-3] + " " + Deaths[Lng-4] + " ");

        for (int o = 0; o <= 5; o++)
        {
            int a0 = Deaths[0];
            float[,,] w10 = anns[a0].weights;
            float[] b10 = anns[a0].Biases;

            for (int r = 0; r <= 6; r++)
            {
                for (int t = 0; t <= 10; t++)
                {
                    for (int z = 0; z <= 10; z++)
                    {
                        w10[r, t, z] += Random.Range(-0.1f, 0.1f);

                    }
                }
                b10[r] += Random.Range(-0.1f, 0.1f);
            }

            annsNewGen[BirdNumb] = new NeuralNetwork(3, 9, BirdNumb);
            annsNewGen[BirdNumb].SetW(w10);
            annsNewGen[BirdNumb].SetB(b10);
            BirdNumb++;
        }
        //Breeds the last 3 birds
        for (int i = Lng; i >= Lng - 4; i--) 
        {
            for(int j = Lng; j >= Lng - 4; j--)
            {
                if (i != j) {

                    int a = Deaths[i];
                    int b = Deaths[j];

                    float[,,] w1 = anns[a].weights;
                    float[,,] w2 = anns[b].weights;

                    float[] b1 = anns[a].Biases;
                    float[] b2 = anns[b].Biases;

                    float[,,] w3 = new float[7,11,11];
                    float[] b3 = new float[10];

                    for(int r = 0; r <= 6; r++)
                    {
                        for(int t = 0; t <= 10; t++)
                        {
                            for(int z = 0; z <= 10; z++)
                            {
                                float Choice = Random.Range(0f, 1f);
                                if (Choice > 0.5f) {
                                    w3[r, t, z] = w1[r, t, z];
                                }
                                else
                                {
                                    w3[r, t, z] = w2[r, t, z];
                                }
                            }
                        }
                    }
                    for (int r = 0; a <= 6; a++)
                    {
                        float Choice = Random.Range(0f, 1f);
                        if (Choice > 0.5f)
                        {
                            b3[r] = b1[r];
                        }
                        else
                        {
                            b3[r] = b2[r];
                        }
                    }

                    annsNewGen[BirdNumb] = new NeuralNetwork(3, 9, BirdNumb);
                    annsNewGen[BirdNumb].SetW(w3);
                    annsNewGen[BirdNumb].SetB(b3);
                    BirdNumb++;

                }
            }
        }

        
    }
    void CombineWithMutations()
    {
        int Lng = Deaths.Length - 1;
        for (int i = Lng; i >= Lng - 4; i--)
        {
            for (int j = Lng; j >= Lng - 4; j--)
            {
                if (i != j)
                {
                    int a = Deaths[i];
                    int b = Deaths[j];

                    float[,,] w1 = anns[a].weights;
                    float[,,] w2 = anns[b].weights;

                    float[] b1 = anns[a].Biases;
                    float[] b2 = anns[b].Biases;

                    float[,,] w3 = new float[7, 11, 11];
                    float[] b3 = new float[10];

                    for (int r = 0; r <= 6; r++)
                    {
                        for (int t = 0; t <= 10; t++)
                        {
                            for (int z = 0; z <= 10; z++)
                            {
                                float Choice = Random.Range(0f, 1f);
                                if (Choice > 0.5f)
                                {
                                    w3[r, t, z] = w1[r, t, z];
                                }
                                else
                                {
                                    w3[r, t, z] = w2[r, t, z];
                                }
                                if (Choice < 0.4f)
                                {
                                    w3[r, t, z] += Mutation();
                                }
                            }
                        }
                    }
                    for (int r = 0; a <= 6; a++)
                    {
                        float Choice = Random.Range(0f, 1f);
                        if (Choice > 0.5f)
                        {
                            b3[r] = b1[r];
                        }
                        else
                        {
                            b3[r] = b2[r];
                        }
                        if (Choice < 0.4f)
                        {
                            b3[r] += Mutation();
                        }
                    }
                    annsNewGen[BirdNumb] = new NeuralNetwork(3, 9, BirdNumb);
                    annsNewGen[BirdNumb].SetW(w3);
                    annsNewGen[BirdNumb].SetB(b3);
                    BirdNumb++;

                }
            }
        }
    }

    void NewBirds()
    {
        int Lng = Deaths.Length - 1;
        for (int i = Lng; i >= Lng - 4; i--)
        {
            for (int j = Lng; j >= Lng - 4; j--)
            {
                if (i != j)
                {
                    int a = Deaths[i];
                    int b = Deaths[j];

                    float[,,] w1 = anns[a].weights;
                    float[,,] w2 = anns[b].weights;

                    float[] b1 = anns[a].Biases;
                    float[] b2 = anns[b].Biases;

                    float[,,] w3 = new float[7, 11, 11];
                    float[] b3 = new float[10];

                    for (int r = 0; r <= 6; r++)
                    {
                        for (int t = 0; t <= 10; t++)
                        {
                            for (int z = 0; z <= 10; z++)
                            {
                                w3[r, t, z] = Random.Range(-1f, 1f);
                            }
                        }
                    }
                    for (int r = 0; a <= 6; a++)
                    {
                        b3[r] = Random.Range(-2f, 2f);

                    }
                    annsNewGen[BirdNumb] = new NeuralNetwork(3, 9, BirdNumb);
                    annsNewGen[BirdNumb].SetW(w3);
                    annsNewGen[BirdNumb].SetB(b3);
                    BirdNumb++;

                    if(BirdNumb == 100)
                    {
                        return;
                    }

                }
            }
        }
    }

    void SordDeaths()
    {

        float a = 0;
        Nums.Sort(delegate(birdi emp1,birdi emp2)
        {
            return emp1.fitness.CompareTo(emp2.fitness);
        });
        //print(Nums[0].fitness + "    " + Nums[0].birdN);
        //print(Nums[NumberSpawned - 1].fitness + "    " + Nums[NumberSpawned - 1].birdN);
        //take the first 5 in order

        for(int i=0;i<= NumberSpawned - 1; i++)
        {
            //print(Nums[i].fitness + "    " + Nums[i].birdN + "     I "+ i );
        }

        for (int i = NumberSpawned - 1; i >= NumberSpawned - 5; i--)
        {
            Deaths[i] = Nums[i].birdN;
        }
        Nums.Clear();
    }
    void TransferBirds()
    {
        for(int i = 0; i <= 99; i++)
        {
            anns[i].SetW(annsNewGen[i].weights);
            anns[i].SetB(annsNewGen[i].Biases);
        }
    }

    void Flow()
    {

        K = 1;
        StartCoroutine(Wait3Frames());
        if (Dead < NumberSpawned)
        {
            
            //runs neural networks to decide the birds next actions
            for (int i = 0; i <= NumberSpawned-1; i++)
            {

                float Decision = anns[i].Run(DistanceUntilNext[i], YDifference[i], 0f);

                //Debug.Log( "DECISION MADE     " + Decision  + "    " + i);

                //print(Decision);

                float Ds = Random.Range(0f, 1f);

                if (0.3f < Decision)
                {
                    Actions[i] = 0;
                }
                else
                {
                    Actions[i] = 1;
                }


            }
            
        }
        else
        {
            Dead = 0;
            print("ALL DEAD");
            //BirdNumb = 5; //leave the top 5 first as they are
            SordDeaths();
            CombineNetworks(); // 20 birds

            CombineWithMutations(); // 20 birds
            CombineWithMutations(); // 20 birds

            NewBirds(); // 20 birds
            NewBirds(); // 20 birds
            //100 birds 
            TransferBirds();
            BirdNumb = 0;
            SpawnPlayers();

        }

    }
    // Update is called once per frame
    void Update()
    {
        //20 Bred from the best
        //30 Mutate 
        if(K == 0)
        {
            Flow();
        }
        

    }
    IEnumerator Wait3Frames()
    {

        yield return new WaitForSeconds(0.1f);
        K = 0;
    }
}
