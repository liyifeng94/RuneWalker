using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FibonacciSequence
{
    private List<int> _fibonacciSequence;

    public FibonacciSequence()
    {
        _fibonacciSequence = new List<int>();
        _fibonacciSequence.Add(0);
        _fibonacciSequence.Add(1);
    }

    public int this[int key]
    {
        get
        {
            if (key >= _fibonacciSequence.Count)
            {
                for (int i = _fibonacciSequence.Count; i < key + 1; ++i)
                {
                    int next = _fibonacciSequence[i - 1] + _fibonacciSequence[i - 2];

                    _fibonacciSequence.Add(next);
                }
            }
            return _fibonacciSequence[key];
        }
        set { return; }
    }
}
