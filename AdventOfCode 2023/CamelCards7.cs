using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2023
{
    internal class CamelCards7
    {
        public static void GetInputs()
        {
            int result = 0;
            List<Hand> FiveOfAKind = new List<Hand>();
            List<Hand> FourOfAKind = new List<Hand>();
            List<Hand> FullHouse = new List<Hand>();
            List<Hand> ThreeOfAKind = new List<Hand>();
            List<Hand> TwoPair = new List<Hand>();
            List<Hand> OnePair = new List<Hand>();
            List<Hand> HighCard = new List<Hand>();
            string strength = "AKQT98765432J";

            string input = "";
            while((input = Console.ReadLine()) != "")
            {
                string[] inputs = input.Split(' ');
                Hand hand = new Hand(inputs[0], inputs[1]);
                hand.amounts = GetNewRank(hand.hand);
                switch(hand.amounts[4])
                {
                    case 5:
                        FiveOfAKind = AddHand(hand, FiveOfAKind);
                        break;
                    case 4:
                        FourOfAKind = AddHand(hand, FourOfAKind);
                        break;
                    case 3:
                        {
                            if (hand.amounts[3] == 2)
                                FullHouse = AddHand(hand, FullHouse);
                            else
                                ThreeOfAKind = AddHand(hand, ThreeOfAKind);
                        }
                        break;
                    case 2:
                        {
                            if (hand.amounts[3] == 2)
                                TwoPair = AddHand(hand, TwoPair);
                            else
                                OnePair = AddHand(hand, OnePair);
                        }
                        break;
                    default:
                        HighCard = AddHand(hand, HighCard);
                        break;
                }

            }
            int rank = 1;
            AddResult(HighCard);
            AddResult(OnePair);
            AddResult(TwoPair);
            AddResult(ThreeOfAKind);
            AddResult(FullHouse);
            AddResult(FourOfAKind);
            AddResult(FiveOfAKind);

            Console.WriteLine(result);

            void AddResult(List<Hand> list)
            {
                foreach(Hand h in list)
                {
                    result += h.prize * rank;
                    rank++;
                }
            }

            List<Hand> AddHand(Hand hand, List<Hand> toAdd)
            {
                for(int i = toAdd.Count - 1; i >= 0; i--)
                {
                    if (!CompareHands(hand, toAdd[i]))
                    {
                        toAdd.Insert(i + 1, hand);
                        return toAdd;
                    }
                    
                }
                toAdd.Insert(0, hand);
                return toAdd;
            }

            bool CompareHands (Hand currentHand, Hand otherHand)
            {
                for(int i = 0; i < 5; i++)
                {
                    int currentIndex = strength.IndexOf(currentHand.hand[i]);
                    int otherIndex = strength.IndexOf(otherHand.hand[i]);
                    if (currentIndex != otherIndex)
                        return currentIndex > otherIndex;
                }
                return false;
            }

            int[] GetRank(string hand)
            {
                int[] amounts = new int[5] { 0,0,0,0,0};
                string used = "";
                foreach(char c in hand)
                {
                    if (used.Contains(c))
                        amounts[used.IndexOf(c)]++;
                        else
                        {
                            amounts[used.Length]++;
                            used += c;
                    }
                }
                Array.Sort(amounts);
                return amounts;

            }

            int[] GetNewRank(string hand)
            {
                int[] amounts = new int[5] { 0, 0, 0, 0, 0 };
                string used = "";
                int jokers = 0;
                foreach (char c in hand)
                {
                    if (c == 'J')
                        jokers++;
                    else if (used.Contains(c))
                        amounts[used.IndexOf(c)]++;
                    else
                    {
                        amounts[used.Length]++;
                        used += c;
                    }
                }
                Array.Sort(amounts);
                amounts[4] += jokers;
                return amounts;

            }

        }
    }
    internal class Hand
    {
        public string hand;
        public int prize;
        public int[] amounts;
        public Hand(string hand, string prize)
        {
            this.hand = hand; this.prize = int.Parse(prize);
        }
    }

}
