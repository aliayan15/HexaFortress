using CommandTerminal;
using System;
using UnityEngine;

public class Example : MonoBehaviour
{
    // Adding the Command
    [RegisterCommand(Help = "Adds 2 numbers", MinArgCount = 2, MaxArgCount = 2, Name = "Addd")]
    private static void CommandAdd(CommandArg[] args)
    {
        int a = args[0].Int;
        int b = args[1].Int;

        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        int result = a + b;
        Terminal.Log("{0} + {1} = {2}", a, b, result);
        AddResult?.Invoke(result);
    }

    // Send Information
    public static Action<int> AddResult;

    private void OnEnable()
    {
        AddResult += Test;
    }

    private void OnDisable()
    {
        AddResult -= Test;
    }

    public void Test(int result)
    {
        Debug.Log("Result: " + result);
    }
}