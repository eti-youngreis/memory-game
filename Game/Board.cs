using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{

    public class Board
    {
        public const int MINCARDS = 4;
        public const int MAXCARDS = 40;
        public int Size { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();

        public Board()
        {
            SetSize();
        }

        public void SetSize()
        {
            do
            {
                Console.WriteLine("Enter even count of cards between 4 to 40");
                while(!int.TryParse(Console.ReadLine(), out Game.intInput))
                    Console.WriteLine("Invalid input, please enter again");
                Size = Game.intInput;
            }
            while (Size % 2 != 0 || Size < MINCARDS || Size > MAXCARDS);

        }
        public void SetBoard(CardType cardType)
        {
            Card[] cards1 = { new SymboleCard(), new LetterCard(), new ExerciseCard() };
            Card type = cards1[(int)cardType - 1];
            for (int i = 0; i < Size; i += 2)
            {
                Cards.Add(type.InitCard());
                Cards.Add(Cards[i].CopyCard());
            }
            Cards=Cards.OrderBy(x => Game.rand.Next()).ToList();
        }

        public void PrintBoard()
        {
            Console.Clear();
            for (int i = 0; i < Cards.Count; i++)
            {
                if (Cards[i].Status == Status.active)
                    Cards[i].PrintCard();
                else
                    if (Cards[i].Status == Status.discovered)
                    Console.Write("! ");
                else
                    Console.Write("? ");
            }
            Thread.Sleep(1500);
            Console.Clear();
        }
        public bool IsValidChoise(int choise)
        {
            if (Cards[choise].Status == Status.active || Cards[choise].Status == Status.discovered)
            {
                Console.Write("Your choise is active or discovered, ");
                return false;
            }
            return true;
        }
        public bool AreExistCards()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                if (Cards[i].Status == Status.covered)
                    return true;
            }
            return false;
        }
        public void ChangeStatus(Status status, params int[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                Cards[cards[i]].Status = status;
            }
        }

    }
}
