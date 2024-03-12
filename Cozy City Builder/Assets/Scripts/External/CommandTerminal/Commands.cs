using CommandTerminal;
using Managers;
using System;
using UnityEngine;

public class Commands : MonoBehaviour
{
    // Adding the Command
    [RegisterCommand(Help = "Add gold", MinArgCount = 1, MaxArgCount = 1, Name = "addgold")]
    private static void AddGold(CommandArg[] args)
    {
        int a = args[0].Int;

        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log("Gold Added: " + a);
        GameManager.Instance.player.AddGold(a);
    }

    [RegisterCommand(Help = "Get new collection of tiles", MinArgCount = 0, MaxArgCount = 0, Name = "getnewtiles")]
    private static void GetTiles(CommandArg[] args)
    {

        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log("GetTiles");
        UIManager.Instance.gameCanvasManager.GetTiles();
    }
}