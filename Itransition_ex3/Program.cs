using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Itransition_ex3
{
    internal class Program
    {
        static string HMACHASH(string value, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                byte[] bvalue = Encoding.Default.GetBytes(value);
                var hash = hmac.ComputeHash(bvalue);
                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            }
        }

        public static int Main(string[] args)
        {
            
            if (args.Length < 3)
            {
                Console.WriteLine("The number of arguments must be more than 3!");
                Console.WriteLine("Example: rock paper scissors lizard Spock");
                return 1;
            }

            if (args.Length % 2 == 0)
            {
                Console.WriteLine("Enter an odd number of arguments!");
                Console.WriteLine("Example: rock paper scissors lizard Spock");
                return 1;
            }

            string[] array = args;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = array[i].ToLower();
            }
            if (array.Distinct().Count() != array.Length)
            {
                Console.WriteLine("Arguments must not be repeated!");
                Console.WriteLine("Example: rock paper scissors lizard Spock");
                return 1;
            }

            array = null;

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randomKey = new byte[16];
            rng.GetBytes(randomKey);

            Random randomMover = new Random();
            int movePCIndex = randomMover.Next(args.Length - 1);
            string movePC = args[movePCIndex];
            Console.WriteLine($"HMAC:\n{HMACHASH(movePC, randomKey)}");
            int input;
            while (true)
            {
                Console.WriteLine("Available moves:");
                int count = 1;
                foreach (var arg in args)
                {
                    Console.WriteLine($"{count} - {arg}");
                    count++;
                }

                Console.WriteLine("0 - Exit");
                Console.Write("Enter your move: ");
                
                if (Int32.TryParse(Console.ReadLine(), out input) && input >= 0 && input <= args.Length)
                {
                    if (input == 0) 
                    {
                        Console.WriteLine("Goodbye!");
                        return 0;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine($"\nIncorrect input! Enter a number from 0 to {args.Length}!");
                }
            }
            int movePlayerIndex =  input - 1;
            string movePlayer = args[movePlayerIndex];
            Console.WriteLine($"Your move: {movePlayer}");
            Console.WriteLine($"Computer move: {movePC}");

            
            if (movePlayerIndex == movePCIndex)
            {
                Console.WriteLine("Draw!!!!!!");
            }
            else
            {
                bool win = false;
                int i = 0;
                int current = movePlayerIndex;
                while (i < ((args.Length - 1) / 2))
                {
                    current = current + 1 < args.Length ? current + 1 : 0;
                    if (movePCIndex == current)
                    {
                        win = true;
                    }
                    i++;
                }
                Console.WriteLine(win ? "Computer win!" : "You win!");
            }
            
            Console.WriteLine($"HMAC key: {BitConverter.ToString(randomKey).Replace("-", string.Empty).ToLower()}");
            

            return 0;
        }
    }
}