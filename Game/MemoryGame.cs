namespace Game
{
    public enum CardType { SymboleCard = 1, LetterCard, ExerciseCard }
    public enum Status { covered, discovered, active };

    public class Game
    {
        public static int intInput;
        public static char charInput;
        public static Random rand = new();
        readonly List<Player> players = new();
        int index;
        CardType gameType;
        readonly Board board;

        public Game()
        {
            SetPlayers();
            SetGameType();
            board = new Board();
            board.SetBoard(gameType);
            GameProgress();
        }
        public static int InputPlayers()
        {
            Console.WriteLine("Enter count of players between 2 to 4");
            while (!int.TryParse(Console.ReadLine(), out intInput))
                Console.WriteLine("Invalid input, please enter again");
            return intInput;
        }
        public void SetPlayers()
        {
            int i = 0;
            int numOfPlayers = InputPlayers();
            while (numOfPlayers < 2 || numOfPlayers > 4)
            {
                Console.Write("Invalid count of players, ");
                numOfPlayers = InputPlayers();
            }
            Console.WriteLine("Do you want one of them to be a computer player? enter Y/N");
            while (!char.TryParse(Console.ReadLine(), out charInput))
                Console.WriteLine("Invalid input, please enter again");
            char c = charInput;
            if (c == 'Y' || c == 'y')
            {
                i++;
                players.Add(new ComputerPlayer());
            }
            Console.WriteLine("Enter names of the user players");
            for (; i < numOfPlayers; i++)
            {
                players.Add(new UserPlayer());
                while ((players[i].Name = Console.ReadLine()) == null);
            }
        }
        public static int InputGameType()
        {
            Console.WriteLine("Enter type of cards:\nfor Symbole Card enter 1.\n" +
                            "for Letter Card enter 2.\nfor  Exercise Card enter 3.");
            while (!int.TryParse(Console.ReadLine(), out intInput))
                Console.WriteLine("Invalid input, please enter again");
            return intInput;
        }
        public void SetGameType()
        {
            int _gameType = InputGameType();
            while (_gameType < 1 || _gameType > 3)
            {
                _gameType = InputGameType();
            }
            gameType = (CardType)_gameType;
        }
        public void FindPair(int iCard1, int iCard2)
        {
            board.ChangeStatus(Status.discovered, iCard1, iCard2);
            players[index].MyCards.Add(board.Cards[iCard1]);
            players[index].MyCards.Add(board.Cards[iCard2]);
            players[index].Score++;
        }
        public static void DisplayWinner(List<Player> winners)
        {
            if (winners.Count > 1)
                Console.WriteLine("Draw Result!! The winners:");
            else
                Console.Write("The winner ");
            for (int i = 0; i < winners.Count; i++)
            {
                Console.WriteLine($"{winners[i].Name}:");
                winners[i].DisplayCards();
            }
        }
        public List<Player> FindWinner()
        {
            List<Player> winners = new();
            int max = -1;
            for (int i = 0; i < players.Count; i++)
                if (players[i].Score > max)
                {
                    max = players[i].Score;
                    winners.Clear();
                    winners.Add(players[i]);
                }
                else
                    if (players[i].Score == max)
                {
                    winners.Add(players[i]);
                }
            return winners;
        }
        public int ChooseCard()
        {
            int choise = players[index].ChooseCard(board.Size);
            while (!board.IsValidChoise(choise))
                choise = players[index].ChooseCard(board.Size);
            return choise;
        }
        public int PlayerTurn()
        {
            int choise = ChooseCard();
            board.ChangeStatus(Status.active, choise);
            board.PrintBoard();
            return choise;
        }
        public void DisplayPlayerName()
        {
            Console.WriteLine($"{players[index].Name}'s turn! ");
            Thread.Sleep(1000);
        }
        public void GameProgress()
        {
            int choise1, choise2;
            List<Player> winners;
            bool isGameOver = false;
            Console.Clear();
            while (!isGameOver)
            {
                for (index = 0; index < players.Count && !isGameOver; index++)
                {
                    DisplayPlayerName();
                    choise1 = PlayerTurn();
                    choise2 = PlayerTurn();
                    if (board.Cards[choise1].Equals(board.Cards[choise2]))
                    {
                        FindPair(choise1, choise2);
                        if (!board.AreExistCards())
                        {
                            winners = FindWinner();
                            DisplayWinner(winners);
                            isGameOver = true;
                        }
                    }
                    else
                        board.ChangeStatus(Status.covered, choise1, choise2);
                }
            }
        }

    }





}
