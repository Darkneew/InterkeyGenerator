using System;
using System.Collections;
using System.Collections.Generic;

namespace InterkeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string privateKey;
            string password;
            while (true)
            {
                Console.WriteLine("Please enter your private key");
                privateKey = Console.ReadLine();
                Console.WriteLine("Please enter the private key again to make sure");
                string _privateKey = Console.ReadLine();
                if (privateKey != _privateKey)
                {
                    Console.WriteLine("You entered 2 different private keys.\n\n");
                    continue;
                }
                break;
            }
            while (true)
            {
                Console.WriteLine("Please enter your password");
                password = Console.ReadLine();
                Console.WriteLine("Please enter the password again to make sure");
                string _password = Console.ReadLine();
                if (password != _password)
                {
                    Console.WriteLine("You entered 2 different passwords.\n\n");
                    continue;
                }
                break;
            }
            string interkey = GenerateInterkey(privateKey, password);
            Console.WriteLine("\n\nHere is the interkey you need to copy and give to the bot:");
            Console.WriteLine(interkey);
            Console.WriteLine("Enter to quit");
            Console.Read();
            Environment.Exit(0);
        }

        static string GenerateInterkey(string privateKey, string password)
        {
            char[] splitupKey = privateKey.ToCharArray();
            if (password.Length <= privateKey.Length)
            {
                int[] permList = RandomUniqueList(privateKey.Length, password.Length, password);
                for (int i = 0; i < password.Length; i++)
                {
                    splitupKey[permList[i]] = (char)((int)splitupKey[permList[i]] + (int)password[i]);
                }
            }
            else
            {
                int[] permList = RandomUniqueList(password.Length, privateKey.Length, password);
                for (int i = 0; i < privateKey.Length; i++)
                {
                    splitupKey[i] = (char)((int)splitupKey[i] + (int)password[permList[i]]);
                }
            }
            return new string(splitupKey);
        }

        static int[] RandomUniqueList(int limit, int size, string seed)
        {
            BitArray isAvailable = new BitArray(limit);
            isAvailable.SetAll(true);
            int[] rawNums = GetNRandomNumbers(size, seed, limit);
            int[] nums = new int[size];
            for (int i = 0; i < size; i++)
            {
                int k = rawNums[i] % (limit - i);
                int num = 0;
                int j = 0;
                while (j < k)
                {
                    num++;
                    if (isAvailable[num]) j++;
                }
                nums[i] = num;
                isAvailable[num] = false;
            }
            return nums;
        }

        static int[] GetNRandomNumbers(int n, string seed, int lowerlimit)
        {
            int[] factors = GetNextPrimes(n * seed.Length, lowerlimit);
            int[] numbers = new int[n];
            for (int i = 0; i < n; i++)
            {
                numbers[i] = 0;
                for (int j = 0; j < seed.Length; j++) numbers[i] += factors[i * seed.Length + j] * (int)seed[j];
            }
            return numbers;
        }

        static int[] GetNextPrimes(int n, int lowerLimit)
        {
            List<int> primes = new List<int>();
            int[] answer = new int[n];
            int num = 0;
            int i = 1;
            while (num < n)
            {
                i++;
                bool isPrime = true;
                foreach (int p in primes)
                {
                    if (i % p == 0) isPrime = false;
                }
                if (!isPrime) continue;
                primes.Add(i);
                if (i > lowerLimit)
                {
                    answer[num] = i % lowerLimit;
                    num++;
                }
            }
            return answer;
        }
    }
}
