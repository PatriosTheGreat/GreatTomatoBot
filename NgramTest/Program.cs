using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NTextCat;

namespace NgramTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var naiveBayesLanguageIdentifier = new NaiveBayesLanguageIdentifierFactory().Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("NgramTest.Core14.profile.xml"));



            while (true)
            {
                Console.WriteLine(naiveBayesLanguageIdentifier.Identify(Console.ReadLine()).First().Item1.Iso639_2T);
            }
            
            /*
            var text = File.ReadAllText(@"C:\LayoutProfile.txt");

            File.WriteAllText(@"C:\SwitchedLayoutProfile.txt", NgramTextRegex.Replace(text, match =>
            {
                return $"text=\"{string.Join("", match.Groups[1].ToString().Select(ch => Escape(SwitchLayout[ch].ToString())).ToArray())}\"";
            }));*/
        }

        private static string Escape(string str)
        {
            switch (str)
            {
                case "\"":
                    return "&quot;";
                case "\'":
                    return "&apos;";
                case "<":
                    return "&lt;";
                case ">":
                    return "&gt;";
                default:
                    return str;
            }
        }

        private static Regex NgramTextRegex = new Regex("text=\"([_a-zA-Zа-яА-Я]+)\"");

        private static Dictionary<char, char> SwitchLayout = new Dictionary<char, char>
        {
            ['q'] = 'й',
            ['w'] = 'ц',
            ['e'] = 'у',
            ['r'] = 'к',
            ['t'] = 'е',
            ['y'] = 'н',
            ['u'] = 'г',
            ['i'] = 'ш',
            ['o'] = 'щ',
            ['p'] = 'з',
            ['a'] = 'ф',
            ['s'] = 'ы',
            ['d'] = 'в',
            ['f'] = 'а',
            ['g'] = 'п',
            ['h'] = 'р',
            ['j'] = 'о',
            ['k'] = 'л',
            ['l'] = 'д',
            ['z'] = 'я',
            ['x'] = 'ч',
            ['c'] = 'с',
            ['v'] = 'м',
            ['b'] = 'и',
            ['n'] = 'т',
            ['m'] = 'ь',

            ['Q'] = 'Й',
            ['W'] = 'Ц',
            ['E'] = 'У',
            ['R'] = 'К',
            ['T'] = 'Е',
            ['Y'] = 'Н',
            ['U'] = 'Г',
            ['I'] = 'Ш',
            ['O'] = 'Щ',
            ['P'] = 'З',
            ['A'] = 'Ф',
            ['S'] = 'Ы',
            ['D'] = 'В',
            ['F'] = 'А',
            ['G'] = 'П',
            ['H'] = 'Р',
            ['J'] = 'О',
            ['K'] = 'Л',
            ['L'] = 'Д',
            ['Z'] = 'Я',
            ['X'] = 'Ч',
            ['C'] = 'С',
            ['V'] = 'М',
            ['B'] = 'И',
            ['N'] = 'Т',
            ['M'] = 'Ь',

            ['й'] = 'q',
            ['ц'] = 'w',
            ['у'] = 'e',
            ['к'] = 'r',
            ['е'] = 't',
            ['н'] = 'y',
            ['г'] = 'u',
            ['ш'] = 'i',
            ['щ'] = 'o',
            ['з'] = 'p',
            ['х'] = '[',
            ['ъ'] = ']',
            ['ф'] = 'a',
            ['ы'] = 's',
            ['в'] = 'd',
            ['а'] = 'f',
            ['п'] = 'g',
            ['р'] = 'h',
            ['о'] = 'j',
            ['л'] = 'k',
            ['д'] = 'l',
            ['ж'] = ';',
            ['э'] = '\'',
            ['я'] = 'z',
            ['ч'] = 'x',
            ['с'] = 'c',
            ['м'] = 'v',
            ['и'] = 'b',
            ['т'] = 'n',
            ['ь'] = 'm',
            ['б'] = ',',
            ['ю'] = '.',
            ['ё'] = '`',
            ['_'] = '_',
            ['_'] = '_',

            ['Й'] = 'Q',
            ['Ц'] = 'W',
            ['У'] = 'E',
            ['К'] = 'R',
            ['Е'] = 'T',
            ['Н'] = 'Y',
            ['Г'] = 'U',
            ['Ш'] = 'I',
            ['Щ'] = 'O',
            ['З'] = 'P',
            ['Х'] = '{',
            ['Ъ'] = '}',
            ['Ф'] = 'A',
            ['Ы'] = 'S',
            ['В'] = 'D',
            ['А'] = 'F',
            ['П'] = 'G',
            ['Р'] = 'H',
            ['О'] = 'J',
            ['Л'] = 'K',
            ['Д'] = 'L',
            ['Ж'] = ':',
            ['Э'] = '"',
            ['Я'] = 'Z',
            ['Ч'] = 'X',
            ['С'] = 'C',
            ['М'] = 'V',
            ['И'] = 'B',
            ['Т'] = 'N',
            ['Ь'] = 'M',
            ['Б'] = '<',
            ['Ю'] = '>',
            ['Ё'] = '~'
        };
    }
}
