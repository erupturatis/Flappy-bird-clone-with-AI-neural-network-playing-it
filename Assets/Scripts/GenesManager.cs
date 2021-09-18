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
    int K = 0;

    public int[] Actions;

    float[] YDifference;
    float[] DistanceUntilNext;


    void RandomizeWeights()
    {
        for(int i = 0; i <= 6; i++)
        {
            for(int j = 0; j <= 10; j++)
            {
                for(int t = 0; t <= 10; t++)
                {
                    Weights[i,j,t] = Random.Range(0f, 1f);
                }
            }
        }
    }
    void RandomizeBiases()
    {
        for (int i = 0; i <= 6; i++)
        {
            Biases[i] = Random.Range(-3f, 3f);
        }
    }
    float Mutation()
    {
        return Random.Range(-10f, 10f);
    }
    void SetDeath(int t)
    {
        Deaths[Dead] = t;
        Dead += 1;
    }
    void SpawnPlayers()
    {
        for(int i = 0; i <= 0; i++)
        {
            Vector3 position = new Vector3(0f, Random.Range(-3f,3f), 0f);
            GameObject bot = Instantiate(bird,position,Quaternion.identity);
            bot.GetComponent<Player>().Action = i;
            print("spawned");
        }
    }

    void Start()
    {
        DistanceUntilNext = new float[35];
        YDifference = new float[35];
        Actions = new int[35];
        Weights = new float[11,11,11];
        Biases = new float[11];
        anns = new NeuralNetwork[35];
        for (int i = 0; i <= 0; i++)
        {
            RandomizeWeights();
            RandomizeBiases();

            anns[i] = new NeuralNetwork(2,6,Weights,Biases);
            //print(i);
        }
        SpawnPlayers();
    }

    void CombineNetworks()
    {
        int Lng = Deaths.Length;
        //Breeds the last 3 birds
        for(int i = Lng; i >= Lng - 3; i--) 
        {
            for(int j = Lng; j >= Lng - 3; j--)
            {
                if (i != j) {
                    int a = Deaths[i];
                    int b = Deaths[j];

                    float[,,] w1 = anns[a].weights;
                    float[,,] w2 = anns[b].weights;

                    float[] b1 = anns[a].Biases;
                    float[] b2 = anns[b].Biases;
                }
            }
        }
    }

    void Flow()
    {
        K = 1;
        if (Dead < 30)
        {
            //runs neural networks to decide the birds next actions
            for (int i = 0; i <= 0; i++)
            {

                float Decision = anns[i].Run(DistanceUntilNext[i], YDifference[i]);
                print(Decision);
                if (Decision < 0.5f)
                {
                    Actions[i] = 0;
                }
                else
                {
                    Actions[i] = 1;
                }
            }
            StartCoroutine(Wait3Frames());
        }
        else
        {
            print("ALL DEAD");
        }

    }
    // Update is called once per frame
    void Update()
    {
        //10 Bred from the best
        //20 Mutate 
        if(K == 0)
        {
            Flow();
        }
        

    }
    IEnumerator Wait3Frames()
    {

        yield return new WaitForSeconds(1f);
        K = 0;
    }
}
