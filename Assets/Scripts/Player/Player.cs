using System;

public class Player : Target
{  
    public Vec2 MouseDelta { get; private set; }
    public Player(int id)
    {
        Id = id;
        MaxHealth = 100;
        Health = 100;
        Name = GenerateName();
    }

    

    internal void Move(Vec2 input)
    {
        InputMove = input;
    }

    internal void MoveCamera(Vec2 mouseDelta)
    {
        MouseDelta = mouseDelta;
    }

    
}