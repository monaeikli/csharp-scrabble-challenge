using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;


namespace csharp_scrabble_challenge.Main
{
    public class Scrabble
    {
        private string word;

        private static Dictionary<char, int> letterScores = new()
        {
            ['A'] = 1, ['E'] = 1, ['I'] = 1, ['O'] = 1, ['U'] = 1, ['L'] = 1, ['N'] = 1, ['R'] = 1, ['S'] = 1, ['T'] = 1,
            ['D'] = 2, ['G'] = 2, 
            ['B'] = 3, ['C'] = 3, ['M'] = 3, ['P'] = 3,
            ['F'] = 4, ['H'] = 4, ['V'] = 4, ['W'] = 4, ['Y'] = 4,
            ['K'] = 5,
            ['J'] = 8, ['X'] = 8,
            ['Q'] = 10, ['Z'] = 10
        };

        public Scrabble(string word)
        {
            this.word = word;
        }

        public int score()
        {
            if (string.IsNullOrWhiteSpace(word))
                return 0;

            string input = word.Trim();
            int wordMultiplier = 1;

            // Sjekk for ytre { } rundt hele ordet (dobbel word)
            if (input.Length >= 2 && input[0] == '{' && input[^1] == '}')
            {
                string inner = input.Substring(1, input.Length - 2);

                // Hvis det ikke finnes andre { eller } inne i teksten, gjelder dobbel word
                if (!inner.Contains('{') && !inner.Contains('}'))
                {
                    wordMultiplier = 2;
                    input = inner;
                }
            }
            // Sjekk for ytre [ ] rundt hele ordet (trippel word)
            else if (input.Length >= 2 && input[0] == '[' && input[^1] == ']')
            {
                string inner = input.Substring(1, input.Length - 2);

                // Hvis det ikke finnes andre [ eller ] inne i teksten, gjelder trippel word
                if (!inner.Contains('[') && !inner.Contains(']'))
                {
                    wordMultiplier = 3;
                    input = inner;
                }
            }

            int total = 0;
            int i = 0;

            // Gå gjennom hver bokstav i ordet
            while (i < input.Length)
            {
                char c = input[i];

                // Hvis vi møter { eller [, prøver vi å finne riktig lukking (} eller ])
                if (c == '{' || c == '[')
                {
                    char expectedClose = c == '{' ? '}' : ']';
                    int multiplier = c == '{' ? 2 : 3;

                    // Finn posisjonen til tilhørende lukketegn
                    int closeIndex = input.IndexOf(expectedClose, i + 1);
                    if (closeIndex == -1)
                        return 0; // mangler lukketegn → ugyldig

                    // Innholdet mellom åpen og lukket tegn
                    string inside = input.Substring(i + 1, closeIndex - i - 1);

                    // Kun én gyldig bokstav tillatt inne i boost-par
                    if (inside.Length != 1 || !letterScores.ContainsKey(char.ToUpper(inside[0])))
                        return 0;

                    char letter = char.ToUpper(inside[0]);
                    total += letterScores[letter] * multiplier;

                    i = closeIndex + 1; // hopp videre
                }
                // Hvis vi finner en lukketegn uten åpen → ugyldig
                else if (c == '}' || c == ']')
                {
                    return 0;
                }
                // Vanlig bokstav
                else
                {
                    char letter = char.ToUpper(c);
                    if (!letterScores.ContainsKey(letter))
                        return 0;

                    total += letterScores[letter];
                    i++;
                }
            }

            return total * wordMultiplier;
        }


    }
}
