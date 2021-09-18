using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenesManager : MonoBehaviour
{
    float[,,] Weights;
    float[] Biases;
    NeuralNetwork[] anns;
    public GameObject bird;
    float MutationPower;
    int[] Deaths;
    int Dead;
    int K = 1;

    public int[] Actions;

    public float[] YDifference;
    public float[] DistanceUntilNext;

    int NumberSpawned = 100;

    int BirdNumb = 0;


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
            Biases[i] = Random.Range(-5f, 5f);
        }
    }
    float Mutation()
    {
        return Random.Range(-1f, 1f);
    }
    public void SetDeath(int t)
    {
        Deaths[Dead] = t;
        Dead += 1;
    }
    void SpawnPlayers()
    {
        for(int i = 0; i <= NumberSpawned-1; i++)
        {
            Vector3 position = new Vector3(0f, Random.Range(-5f,5f), 0f);
            GameObject bot = Instantiate(bird,position,Quaternion.identity);
            bot.GetComponent<Player>().playerNumber = i;
            //print("spawned");
        }
    }

    void Start()
    {
        Deaths = new int[NumberSpawned + 1];
        DistanceUntilNext = new float[NumberSpawned+1];
        YDifference = new float[NumberSpawned+1];
        Actions = new int[NumberSpawned+1];
        Weights = new float[11,11,11];
        Biases = new float[11];
        anns = new NeuralNetwork[NumberSpawned+1];
        for (int i = 0; i <= NumberSpawned-1; i++)
        {
            RandomizeWeights();
            RandomizeBiases();

            anns[i] = new NeuralNetwork(3,9,Weights,Biases);
            //print(i);
        }
        SpawnPlayers();
        StartCoroutine(Wait3Frames());
    }

    void CombineNetworks()
    {
        int Lng = Deaths.Length-1;
        //Breeds the last 3 birds
        for(int i = Lng; i >= Lng - 4; i--) 
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

                    float[,,] w3 = w1;
                    float[] b3 = b1;

                    for(int r = 0; a <= 6; a++)
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
                    anns[BirdNumb].weights = w3;
                    anns[BirdNumb].Biases = b3;
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

                    float[,,] w3 = w1;
                    float[] b3 = b1;

                    for (int r = 0; a <= 6; a++)
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
                                if (Choice < 0.2f)
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
                        if (Choice < 0.2f)
                        {
                            b3[r] += Mutation();
                        }
                    }
                    anns[BirdNumb].weights = w3;
                    anns[BirdNumb].Biases = b3;
                    BirdNumb++;

                }
            }
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

                float Decision = anns[i].Run(DistanceUntilNext[i], YDifference[i]);
                //Debug.Log( "DECISION MADE" + Decision  + "    " + i);
                //print(Decision);
                if (Decision < 0.5f)
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
            CombineNetworks(); // 20 birds
            CombineWithMutations(); // 20 birds
            CombineWithMutations(); // 20 birds
            CombineWithMutations(); // 20 birds
            CombineWithMutations(); // 20 birds
            //100 birds 
            
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
