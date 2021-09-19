using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NeuralNetwork 
{
    public float[,,] weights ;
    public float[] Biases;
    public int HiddenLayers;
    public int HiddenLayerSize;
    public int NetworkNum;
    
    public NeuralNetwork(int HiddenLayersI,int HiddenLayerSizeI, int Num)
    {
        NetworkNum = Num;
        weights = new float[7, 11, 11];

        HiddenLayerSize = HiddenLayerSizeI;
        HiddenLayers = HiddenLayersI;
        Biases = new float[11];


    }
    public void SetW(float[,,] WeightsI)
    {
        for (int i = 0; i <= 6; i++)
        {
            for (int j = 0; j <= 10; j++)
            {
                for (int t = 0; t <= 10; t++)
                {
                    weights[i, j, t] = WeightsI[i, j, t];
                }
            }
        }
    }
    public void SetB(float[] BiasesI)
    {
        for (int i = 0; i <= 6; i++)
        {
            Biases[i] = BiasesI[i];
        }
    }
    public float Run(float DistanceUntilNext, float YDifference)
    {
        //DistanceUntilNext = UnityEngine.Random.Range(-5f, 5f);
        //YDifference = UnityEngine.Random.Range(-5f, 5f);
        //Debug.Log(YDifference + "YDIFF RANDOM SUPPOSEDLY");

        //WeightsNow [i][j] means the weight from neuron I to neuron J
        // DistanceUntilNext = 0;


        //Debug.Log(YDifference);
       // Debug.Log("IN NEURAL NETWORK " + weights[0,0,0] + "    " + NetworkNum);
        float[,] WeightsNow = new float[11, 11];
        for(int i = 0; i <= 10; i++)
        {
            for(int j = 0; j <= 10; j++)
            {
                WeightsNow[i,j] = weights[0,i,j];
                //Debug.Log(WeightsNow[i, j] + " " + i + " " + j);
            }
        }    
        
        
        float[] SumsPrevious = new float[50];
        float[] SumsCalculated = new float[50]; //The neural network shouldn't have layers bigger than 50 in height
        float Sum = 0;
        int Input = 0;
        int Output = 0;
        //Input -> Output
        for (int p = 0; p <= HiddenLayers; p++)
        {
            for (int i = 0; i <= 10; i++)
            {
                for (int j = 0; j <= 10; j++)
                {
                    WeightsNow[i, j] = weights[p, i, j];
                }
            }
            //Input is the input of the layer we are currently calculating
            //Output is the output of the layer we are calculating 
            if (p == 0)
            {
                Input = 2;
            }else
            {
                Input = HiddenLayerSize;
            }
            if(p == HiddenLayers)
            {
                Output = 1;
            }
            else
            {
                Output = HiddenLayerSize;
            }
            if (p == 0)
            {
                //Calculating the initial sums from the input
                for (int i = 0; i < HiddenLayerSize; i++)
                {
                    Sum = WeightsNow[0,i] * DistanceUntilNext + WeightsNow[1,i] * YDifference + Biases[p];
                    //Debug.Log("THE SUMMM" + Sum + "   " + i);
                    //Debug.Log(WeightsNow[0, i] + "THE F WEIGHT");
                    Sum = Sigmoid(Sum);
                    SumsCalculated[i] = Sum;
                }
                //Debug.Log(WeightsNow[0, 0] + "  SUM     " + NetworkNum);
            }
            else if (p == HiddenLayers)
            {
                for (int i = 0; i < HiddenLayerSize; i++)
                {
                    //Debug.Log("I " + i + "WEIGHTS  " + WeightsNow[i, 0] + " Distance");
                    Sum += WeightsNow[i,0] * SumsPrevious[i];
                }
                Sum = Sum + Biases[p];
                Sum = Sigmoid(Sum);
                SumsCalculated[0] = Sum;
            }
            else
            {
                for (int i = 0; i < HiddenLayerSize; i++)
                {
                    //General Calculation for the hidden Layers
                    Sum = 0;
                    for (int j = 0; j <= Input; j++)
                    {
                        Sum += WeightsNow[j,i] * SumsPrevious[j];
                    }
                    Sum += Biases[p];
                    Sum = Sigmoid(Sum);
                    SumsCalculated[i] = Sum;
                }
            }
            SumsPrevious = SumsCalculated;
            if (p == HiddenLayers)
            {
                return SumsCalculated[0];
            }
            
        }
        return 1f;
    }

    public float Relu(float x)
    {
        if (x > 0)
        {
            return x;
        }
        else
        {
            return 0;
        }
    }
    
    public static float Sigmoid(double value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value));
    }


}
