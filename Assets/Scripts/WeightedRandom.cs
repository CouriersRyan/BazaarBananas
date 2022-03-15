using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedRandom
{
    private float[] _weightDistribution;

    public WeightedRandom(float[] weightDistribution)
    {
        SetWeightDistribution(weightDistribution);
    }

    public void SetWeightDistribution(float[] weights)
    {
        _weightDistribution = weights;
    }

    public int GetRandomIndex()
    {
        float totalWeight = GetTotalWeight();

        var target = Random.Range(0f, totalWeight);

        for (int i = 0; i < _weightDistribution.Length; i++)
        {
            totalWeight -= _weightDistribution[i];
            if (totalWeight <= 0)
            {
                return i;
            }
        }

        return _weightDistribution.Length - 1;
    }

    public void SetWeight(int index, float weight)
    {
        if (index < _weightDistribution.Length && index > -1)
        {
            _weightDistribution[index] = weight;
        }
    }

    public float GetTotalWeight()
    {
        var total = 0f;
        foreach (var weight in _weightDistribution)
        {
            total += weight;
        }

        return total;
    }
}
