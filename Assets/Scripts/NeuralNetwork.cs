using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NeuralNetwork 
{
    public float[,,] weights;
    public float[] Biases;
    public int HiddenLayers;
    public int HiddenLayerSize;
    
    public NeuralNetwork(int HiddenLayersI,int HiddenLayerSizeI,float[,,] WeightsI, float[] BiasesI)
    {
        weights = WeightsI;
        HiddenLayerSize = HiddenLayerSizeI;
        HiddenLayers = HiddenLayersI;
        Biases = BiasesI;
    }

    public float Run(float DistanceUntilNext, float YDifference)
    {
        //DistanceUntilNext = UnityEngine.Random.Range(-5f, 5f);
        //YDifference = UnityEngine.Random.Range(-50f, 50f);
        //Debug.Log(YDifference + "YDIFF RANDOM SUPPOSEDLY");

        //WeightsNow [i][j] means the weight from neuron I to neuron J
        // DistanceUntilNext = 0;
        DistanceUntilNext /= 3;
        YDifference *= 5;
        //Debug.Log(YDifference);

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
                    Sum = Relu(Sum);
                    SumsCalculated[i] = Sum;
                }
                //Debug.Log(SumsCalculated[0] + "  SUM");
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
                    Sum = Relu(Sum);
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
