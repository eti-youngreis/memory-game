using System.Drawing;

namespace Game
{
    public abstract class Player
    {
        public int Score { get; set; }
        public List<Card> MyCards { get; set; }
        public string? Name { get; set; }

        public Player()
        {
            Score = 0;
            MyCards = new List<Card>();
        }
        public abstract int ChooseCard(int size);
        public void DisplayCards()
        {
            for (int i = 0; i < MyCards.Count; i++)
            {
                MyCards[i].PrintCard();
            }
            Console.WriteLine();
        }
    }
    public class UserPlayer : Player
    {
        public static int ChooseCard()
        {
            Console.WriteLine("Enter your choice");
            while (!int.TryParse(Console.ReadLine(), out Game.intInput))
                Console.WriteLine("Invalid input, please enter again");
            return Game.intInput - 1;
        }
        public override int ChooseCard(int size)
        {
            int choise = ChooseCard();
            while (choise < 0 || choise > size - 1)
            {
                Console.Write("Your choice is out of the board range, ");
                choise = ChooseCard();
            }
            return choise;
        }
    }
    public class ComputerPlayer : Player
    {
        public ComputerPlayer()
        {
            Name = "Computer";
        }
        public override int ChooseCard(int size)
        {
            return Game.rand.Next(0, size);
        }
    }
}
