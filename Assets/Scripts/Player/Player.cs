using System;

public class Player : Target
{
    private static readonly string[] name1 = { "Red", "Blue", "Green", "Yellow", "Brown", "Black", "Gray", "Purple" };
    private static readonly string[] name2 = { "Tiger", "Worm", "Cat", "Dog", "Bear", "Monkey", "Dragon", "Rat" };
    private static readonly Random rnd = new Random();

    public string Name;
    public float moveHorz;
    public float moveVert;

    public Player(int id)
    {
        Id = id;
        MaxHealth = 100;
        Health = 100;
        Name = GenerateName();
    }

    private static string GenerateName()
    {
        string part1 = name1[rnd.Next(name1.Length)];
        string part2 = name2[rnd.Next(name2.Length)];
        return $"{part1} {part2}";
    }

    internal void Move(float horz, float vert)
    {
        float targetMovingSpeed = isCrouch ? croachSpeed : (isRun ? runSpeed : normalSpeed);

        if (canWalk)
        {
            moveHorz = horz * targetMovingSpeed;
            moveVert = vert * targetMovingSpeed;
        }
    }
}