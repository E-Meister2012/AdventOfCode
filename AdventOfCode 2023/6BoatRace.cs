using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2023
{
    internal class BoatRace6
    {
        public static void PartTwo()
        {
            long time = FullString();
            long distance = FullString();
            Console.WriteLine(GetAmounts(time, distance));
        }

        public static long FullString()
        {
            string[] temp = Console.ReadLine().Split(' ');
            string result = "";
            foreach (string s in temp)
            {
                if (s != "" && char.IsDigit(s[0]))
                {
                    result += s;
                    Console.WriteLine(s);
                }
            }
            Console.WriteLine(result);
            return long.Parse(result);

        }

        public static void GetInput()
        {
            int finalResult = 1;
            List<int> times = GetDigits();
            List<int> records = GetDigits();
            for (int i = 0; i < times.Count; i++)
            {
                Console.WriteLine($"Currently doing problem {i}");
                finalResult *= (int)GetAmounts(times[i], records[i]);
            }
            Console.WriteLine(finalResult);
        }

        static long GetAmounts(long time, long record)
        {
            long lower = BinarySearchLow(1, time / 2, record, time);
            long higher = BinarySearchHigh(time / 2, time, record, time);
            return higher - lower + 1;

        }

        static long BinarySearchLow(long low, long high, long record, long time)
        {
            while (low <= high - 1)
            {
                long mid = low + (high - low) / 2;
                if (mid * (time - mid) > record)
                    high = mid;
                else
                    low = mid + 1;
            }
            Console.WriteLine($"Found low result {low}");   
            return low;
        }
        static long BinarySearchHigh(long low, long high, long record, long time)
        {
            while (low + 1 < high)
            {
                long mid = low + (high - low) / 2;
                if (mid * (time - mid) > record)
                    low = mid;
                else
                    high = mid;
            }
            Console.WriteLine($"Found high result {high - 1}");
            return high - 1;

        }


        static List<int> GetDigits()
        {
            string[] temp = Console.ReadLine().Split(' ');
            List<int> result = new List<int>();
            foreach (string s in temp)
            {
                if (s != "" && char.IsDigit(s[0]))
                {
                    result.Add(int.Parse(s));
                    Console.WriteLine(s);
                }
            }
            return result;

        }
    }
}
