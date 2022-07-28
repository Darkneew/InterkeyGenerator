using System;
using System.Collections;
using System.Collections.Generic;

namespace InterkeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please enter your public key");
                string publicKey = Console.ReadLine();
                Console.WriteLine("Please enter the public key again to make sure");
                string _publicKey = Console.ReadLine();
                if (publicKey != _publicKey)
                {
                    Console.WriteLine("You entered 2 different public keys.\n\n");
                    continue;
                }
                Console.WriteLine("Please enter your password");
                string password = Console.ReadLine();
                Console.WriteLine("Please enter the password again to make sure");
                string _password = Console.ReadLine();
                if (password != _password)
                {
                    Console.WriteLine("You entered 2 different passwords.\n\n");
                    continue;
                }
                Console.WriteLine("\n\nHere is the interkey you need to copy and give to the bot:");
                Console.WriteLine(GenerateInterkey(publicKey, password));
                Console.WriteLine("Enter to quit");
                Console.Read();
                Environment.Exit(0);
            }
        }

        static string GenerateInterkey(string publicKey, string password)
        {
            char[] splitupKey = publicKey.ToCharArray();
            if (password.Length <= publicKey.Length)
            {
                int[] permList = RandomUniqueList(publicKey.Length, password.Length, password);
                for (int i = 0; i < password.Length; i++)
                {
                    splitupKey[permList[i]] = (char)((int)splitupKey[permList[i]] + (int)password[i]);
                }
            }
            else
            {
                int[] permList = RandomUniqueList(password.Length, publicKey.Length, password);
                for (int i = 0; i < publicKey.Length; i++)
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