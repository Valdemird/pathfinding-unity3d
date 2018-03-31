using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{

    public const int ABAJO = -2;
    public const int IZQUIERDA = -1;
    public const int DERECHA = 1;
    public const int ARRIBA = 2;

    public const int EMPTY = 0;
    public const int WALL = 1;
    public const int AGENT = 2;
    public const int FLOWER = 3;
    public const int TURTTLE = 4;
    public const int PRICESS = 5;

    readonly int NORMAL_COST = 1;
    readonly int TURTTLE_COST = 7;

    public int posX;
    public int posY;
    public int type;
 

    public Square(int postX, int postZ, int type) {
        this.posX = postX;
        this.posY = postZ;
        this.type = type;
    }

    public override bool Equals(object obj)
    {
        var casilla = obj as Square;
        return casilla != null &&
               posX == casilla.posX &&
               posY == casilla.posY;
    }

    public int getCost(bool hasFlower) {
        int cost = 0;
        if (type == TURTTLE && !hasFlower)
        {
            cost += TURTTLE_COST;
        }
        cost += NORMAL_COST;
        return cost;
    }





}
