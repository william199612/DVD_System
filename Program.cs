namespace DVD_System
{
    class Program
    {
        private static MovieCollection movies = new();
        private static MemberCollection members = new();
        private static Member? currentUser = null;

        static void Main()
        {
            // test movie data
            //movies.GenerateRandomMovie(MovieLibrary.movieTitles);
            //Console.WriteLine("\nPress any key to continue");
            //Console.ReadKey();
            MainMenu();
        }

        private static void MainMenu()
        {
            bool end = false;
            string[] options = {
                "Staff", "Member", "End the program"
            };

            while (!end)
            {
                int? option = DisplayTitle("main", options);
                switch (option)
                {
                    case 1:
                        StaffLogin();
                        break;
                    case 2:
                        MemberLogin();
                        break;
                    case 0:
                        end = true;
                        break;
                }
            }
            End();
        }

        private static void StaffMenu()
        {
            bool end = false;
            string[] options = {
                "Add DVDs to system",
                "Remove DVDs from system",
                "Register a new member to system",
                "Remove a registered member from system",
                "Find a member contact phone (given member's name)",
                "Find members who are currently renting a particular movie",
                "Return to main menu"
            };

            while (!end)
            {
                int? option = DisplayTitle("staff menu", options);
                switch (option)
                {
                    case 1:
                        movies.AddMovie();
                        break;
                    case 2:
                        movies.RemoveMovie();
                        break;
                    case 3:
                        members.AddMember();
                        break;
                    case 4:
                        members.RemoveMember();
                        break;
                    case 5:
                        members.GetMemberContact();
                        break;
                    case 6:
                        Movie? movie = movies.GetSearchMovie();
                        if (movie != null)
                            members.FindMembersRentMovie(movie);
                        break;
                    case 0:
                        end = true;
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void StaffLogin()
        {
            DisplayTitle("staff login");
            currentUser = members.Login(UserRole.Staff);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            if (currentUser != null)
            {
                StaffMenu();
            }
            else
            {
                MainMenu();
            }
        }

        private static void MemberMenu()
        {
            bool end = false;
            string[] options = {
                "Browse all movies", 
                "Display all info about a movie (given title)", 
                "Rent a movie DVD", 
                "Return a movie DVD",
                "List your movie borrow list",
                "Display top 3 rented movies",
                "Return to main menu"
            };

            while (!end)
            {
                int? option = DisplayTitle("member menu", options);
                switch (option)
                {
                    case 1:
                        movies.DisplayAll();
                        break;
                    case 2:
                        movies.DisplaySingle();
                        break;
                    case 3:
                        movies.Rent(currentUser);
                        break;
                    case 4:
                        movies.Return(currentUser);
                        break;
                    case 5:
                        currentUser.DisplayRentList();
                        break;
                    case 6:
                        movies.DisplayTopThree();
                        break;
                    case 0:
                        end = true;
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void MemberLogin()
        {
            DisplayTitle("member login");
            currentUser = members.Login(UserRole.Member);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            if (currentUser != null)
            {
                MemberMenu();
            }
            else
            {
                MainMenu();
            }
        }

        // given the display type and options to display
        private static int? DisplayTitle(string type, string[]? options=null)
        {
            bool valid = false;

            Console.Clear();

            switch (type)
            {
                case "main":
                    Console.WriteLine("========================================================\n");
                    Console.WriteLine("COMMUNITY LIBRARY MOVIE DVD MANAGEMENT SYSTEM\n");
                    Console.WriteLine("========================================================\n");
                    Console.WriteLine("Main Menu");
                    Console.WriteLine("--------------------------------------------------------\n");
                    Console.WriteLine("Select from the following:");
                    break;
                case "staff login":
                    Console.WriteLine("STAFF LOGIN");
                    Console.WriteLine("--------------------------------------------------------\n");
                    break;
                case "staff menu":
                    Console.WriteLine("STAFF MENU");
                    Console.WriteLine("--------------------------------------------------------\n");
                    Console.WriteLine("Select from the following:");
                    break;
                case "member login":
                    Console.WriteLine("MEMBER LOGIN");
                    Console.WriteLine("--------------------------------------------------------\n");
                    break;
                case "member menu":
                    Console.WriteLine("MEMBER MENU");
                    Console.WriteLine("--------------------------------------------------------\n");
                    Console.WriteLine("Select from the following:");
                    break;
            }

            if (options == null) return null;

            for (int i = 0; i < options.Length; i++)
            {
                if (i == options.Length - 1)
                    Console.WriteLine($"0. {options[i]}");
                else
                    Console.WriteLine($"{i + 1}. {options[i]}");
            }

            int option;

            while (!valid)
            {
                Console.Write("\nEnter your choice ==> ");
                if (!int.TryParse(Console.ReadLine(), out option))
                {
                    Console.WriteLine("\nInvalid input! Please enter a number.");
                }
                else if (option >= options.Length || option < 0)
                {
                    Console.WriteLine($"\nInvalid input! Enter a number between 0 - {options.Length - 1}");
                }
                else
                {
                    return option;
                }
            }
            // unlikely, but add as system require
            return null;
        }

        private static void End()
        {
            string end = @"
     ____                _   ____             
    / ___| ___   ___   __| | | __ \ _   _  ___ 
   | | __ / _ \ / _ \ / _' | | |/ /| | | |/ _ \
   | ||_ | | | | | | \ | | | |  _ \| | | | |_| |
   | |_| | |_| | |_| | |_| | | |_) | |_| |  __/
    \____|\___/ \___/ \__,_| |____/ \__, |\___|
                                    |___/      
            ";

            Console.Clear();
            Console.WriteLine("========================================================\n");
            Console.WriteLine("COMMUNITY LIBRARY MOVIE DVD MANAGEMENT SYSTEM\n");
            Console.WriteLine("========================================================\n\n");
            Console.WriteLine(end);
        }
    }
}
