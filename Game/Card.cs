using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Card
    {
        public Status Status { get; set; }
        public bool IsFirst { get; set; }
        public abstract Card CopyCard();
        public abstract bool IsContained(Card card);
        public abstract void Add(Card card);
        public Card()
        {
            IsFirst = true;
            Status = Status.covered;
        }

        public void PrintCard()
        {
            PrintValue();
            Console.Write(" ");
        }

        protected abstract void PrintValue();
        public override bool Equals(object? obj)
        {
            if (obj != null && GetType() == obj.GetType())
                return Match((Card)obj);
            return false;
        }
        protected abstract bool Match(Card card);

        public Card InitCard()
        {
            Card card = Init();
            while (card.IsContained(card))
                card = Init();
            Add(card);
            return card;
        }
        protected abstract Card Init();

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    public class SymboleCard : Card
    {
        static readonly List<char> symbols = new();
        public char Symbole { get; set; }
        public ConsoleColor Color { get; set; }

        public override Card CopyCard()
        {
            SymboleCard newCard = new()
            {
                Symbole = Symbole,
                Color = Color,
                IsFirst = false
            };
            return newCard;
        }
        public override bool IsContained(Card card)
        {
            return symbols.Contains(((SymboleCard)card).Symbole);
        }
        public override void Add(Card card)
        {
            symbols.Add(((SymboleCard)card).Symbole);
        }
        protected override void PrintValue()
        {
            Console.ForegroundColor = Color;
            Console.Write(Symbole);
            Console.ForegroundColor = ConsoleColor.White;
        }

        protected override bool Match(Card card)
        {
            return Symbole == ((SymboleCard)card).Symbole && Color == ((SymboleCard)card).Color;
        }
        protected override Card Init()
        {
            SymboleCard newCard = new();
            do
                newCard.Symbole = (char)Game.rand.Next(33, 65);
            while (newCard.Symbole > 47 && newCard.Symbole < 58);
            newCard.Color = (ConsoleColor)Game.rand.Next(16);
            return newCard;
        }
    }
    public class LetterCard : Card
    {
        static readonly List<char> upLetters = new();
        public char UpLetter { get; set; }
        public char LowLetter { get; set; }
        public override Card CopyCard()
        {
            LetterCard newCard = new()
            {
                LowLetter = LowLetter,
                UpLetter = UpLetter,
                IsFirst = false
            };
            return newCard;
        }

        public override bool IsContained(Card card)
        {
            return upLetters.Contains(((LetterCard)card).UpLetter);
        }
        public override void Add(Card card)
        {
            upLetters.Add(((LetterCard)card).UpLetter);
        }
        protected override void PrintValue()
        {
            if (IsFirst)
                Console.Write(UpLetter);
            else
                Console.Write(LowLetter);
        }
        protected override bool Match(Card card)
        {
            return LowLetter - ((LetterCard)card).UpLetter == 'a' - 'A';
        }
        protected override Card Init()
        {
            LetterCard newLetter = new()
            {
                UpLetter = (char)Game.rand.Next(65, 91)
            };
            newLetter.LowLetter = (char)(newLetter.UpLetter + ('a' - 'A'));
            return newLetter;
        }
    }
    public class ExerciseCard : Card
    {
        static readonly List<int> results = new();
        public string Exercise { get; set; }

        public int Result { get; set; }
        public override bool IsContained(Card card)
        {
            return results.Contains(((ExerciseCard)card).Result);
        }
        public override void Add(Card card)
        {
            results.Add(((ExerciseCard)card).Result);
        }
        public override Card CopyCard()
        {
            ExerciseCard newCard = new()
            {
                Exercise = Exercise,
                Result = Result,
                IsFirst = false
            };
            return newCard;
        }
        protected override void PrintValue()
        {
            if (IsFirst)
                Console.Write(Exercise);
            else
                Console.Write(Result);
        }
        protected override bool Match(Card card)
        {
            return Exercise == ((ExerciseCard)card).Exercise && Result == ((ExerciseCard)card).Result;

        }
        protected override Card Init()
        {
            ExerciseCard newCard = new();
            char[] operators = new char[] { '+', '-', '*', '/' };
            int num1;
            char op;
            op = operators[Game.rand.Next(4)];
            if (op == '/')
            {
                num1 = Game.rand.Next(1, 10);
                newCard.Exercise = "" + num1 * Game.rand.Next(1, 10) + op + num1;
            }
            else
                newCard.Exercise = "" + Game.rand.Next(1, 10) + op + Game.rand.Next(1, 10);
            DataTable dataTable = new();
            newCard.Result = int.Parse(dataTable.Compute(newCard.Exercise, "").ToString());
            return newCard;
        }
    }
}
