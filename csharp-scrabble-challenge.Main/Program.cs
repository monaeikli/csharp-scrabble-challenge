using csharp_scrabble_challenge.Main;

while (true)
{
    Console.Write("Write a word (or 'qqq' to quit): ");
    string input = Console.ReadLine();

    if (input == "qqq")
    {
        break;
    }

    Scrabble s = new Scrabble(input);
    Console.WriteLine($"Score: {s.score()}\n");
}
