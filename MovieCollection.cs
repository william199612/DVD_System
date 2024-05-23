namespace DVD_System
{
    // the MovieCollection hashtable will be:
    // [node1, node2, ..., node1000]
    // node will be hashed with linearprobe and stored
    // each node will contain:
    // node = {Title, Movie, Occupied}
    // initial node will be empty value node
    // node = {"", null, false}

    internal class Node
    {
        private string Title;
        private Movie? MovieObj; // 
        private bool Occupied;

        // constructor
        // initialize with empty value
        public Node()
        {
            Title = "";
            MovieObj = null;
            Occupied = false;
        }

        // set node value
        public void SetNode(Movie movie)
        {
            Title = movie.GetTitle();
            MovieObj = movie;
            Occupied = true;
        }

        public void Clear()
        {
            Title = "";
            MovieObj = null;
            Occupied = false;
        }

        public string GetTitle() => Title;

        public Movie? GetMovie() => MovieObj;

        public bool IsOccupied() => Occupied;
    }

    class MovieCollection
    {
        private const int MAX_SIZE = 1000;
        private Node[] movies;
        private int movieCount;
        private const int fail = -999;

        public MovieCollection()
        {
            movies = new Node[MAX_SIZE];

            for (int i = 0; i < movies.Length; i++)
            {
                movies[i] = new Node();
            }

            movieCount = 0;
        }

        private static int GetHash(string movieTitle)
        {
            // set a random hash number
            int hash = 7;
            foreach (char c in movieTitle)
            {
                hash = (hash * 31 + c) % MAX_SIZE;
            }
            return hash;
        }

        private int LinearProbe(int hash, string movieTitle)
        {
            int index = hash;
            // make sure the movie title does not duplicate
            while (movies[index].IsOccupied() && movieTitle != movies[index].GetTitle())
            {
                index = (index + 1) % MAX_SIZE;
            }
            return index;
        }

        // for user input
        public void AddMovie()
        {
            DisplayTitle("add");

            while (true)
            {
                Console.Write("\nEnter a new movie title: ");
                string newMovieInput = Console.ReadLine();

                if (string.IsNullOrEmpty(newMovieInput))
                {
                    Console.WriteLine("\nEmpty title! Please try again...");
                    continue;
                }

                int foundIndex = Search(newMovieInput);

                if (foundIndex != fail)
                {
                    Console.WriteLine("\nMovie already exist.");
                    Console.WriteLine("\nDo you want to increase inventory instead?");
                    Console.Write("\nPress 'y/Y' to proceed, 'n/N' to abort... ");

                    string input = Console.ReadLine();
                    if (input?.ToLower() == "y")
                    {
                        movies[foundIndex]?.GetMovie()?.IncreaseInventory();
                        Console.WriteLine("Successfully increase inventory.");
                        Console.WriteLine($"Movie: {movies[foundIndex]?.GetMovie()} with Stock: {movies[foundIndex]?.GetMovie()?.GetStock()}");
                        return;
                    }
                    else if (input?.ToLower() == "n")
                    {
                        return;
                    }
                    else
                    {
                        Console.Write("\nInvalid input.");
                        continue;
                    }
                }
                else
                {
                    Add(new Movie(newMovieInput));
                    Console.WriteLine($"\nNew movie {newMovieInput} is added to the system");
                    return;
                }
            }
        }

        // for system add
        private void Add(Movie movie)
        {
            int hash = GetHash(movie.GetTitle());
            int index = LinearProbe(hash, movie.GetTitle());
            movies[index].SetNode(movie);
            movieCount++;
        }

        public void RemoveMovie()
        {
            DisplayTitle("remove");
            Console.Write("\nEnter a movie title you wish to remove: ");
            string removeMovieInput = Console.ReadLine();

            if (string.IsNullOrEmpty(removeMovieInput))
            {
                Console.WriteLine("\nEmpty title! Please try again...");
                return;
            }

            int foundIndex = Search(removeMovieInput);

            if (foundIndex == fail)
            {
                Console.WriteLine($"\nMovie: '{removeMovieInput}' does not exist in the system.");
            }
            // make sure inventory > stock => movie being rent
            else if (movies[foundIndex]?.GetMovie()?.GetInventory() > movies[foundIndex]?.GetMovie()?.GetStock())
            {
                Console.WriteLine("\nCannot remove movie already rented by member.");
            }
            else
            {
                movies[foundIndex].Clear();
                movieCount--;
                Console.WriteLine("\nMovie removed.");
            }
        }

        public void Rent(Member member)
        {
            DisplayTitle("rent");

            if (movieCount == 0)
            {
                Console.WriteLine("\nNo movies in system.");
                return;
            }

            Console.Write("\nEnter a movie title you want to rent: ");
            string rentMovie = Console.ReadLine();

            int foundIndex = Search(rentMovie);

            if (foundIndex == fail)
            {
                Console.WriteLine("\nSorry! This movie is not in the system.");
            }
            else if (member.CanRent(movies[foundIndex].GetMovie()) && movies[foundIndex].GetMovie().CanBeRented())
            {
                movies[foundIndex]?.GetMovie()?.BeenRented();
                Movie? movie = movies[foundIndex].GetMovie();
                member.Rent(movie);

                Console.WriteLine($"\nSuccessfully rented {movie?.GetTitle()}.");
            }
        }

        public void Return(Member member)
        {
            DisplayTitle("return");

            Console.Write("\nEnter a movie title you want to return: ");
            string returnMovie = Console.ReadLine();

            int foundIndex = Search(returnMovie);

            if (foundIndex == fail)
            {
                Console.WriteLine("\nSorry! This movie is not in the system.");
                return;
            }

            if (member.Return(returnMovie))
            {
                movies[foundIndex]?.GetMovie()?.BeenReturn();
                Console.WriteLine("\nThank you! Movie successfully returned.");
            }
            else
            {
                Console.WriteLine("\nThere's no such movie in your borrow list.");
            }
        }

        private int Search(string title)
        {
            int hash = GetHash(title);
            int index = LinearProbe(hash, title);
            if (movies[index].IsOccupied() && movies[index].GetTitle() == title)
            {
                return index;
            }
            return fail;
        }

        public Movie? GetSearchMovie()
        {
            DisplayTitle("search");

            Console.Write("\nEnter a movie title to view: ");
            string searchMovieInput = Console.ReadLine();

            int foundIndex = Search(searchMovieInput);

            if (foundIndex == fail)
            {
                Console.WriteLine("\nSorry! This movie is not in the system.");
                return null;
            }
            else
            {
                return movies[foundIndex].GetMovie();
            }
        }

        public void DisplaySingle()
        {
            Movie? movie = GetSearchMovie();

            if (movie != null)
            {
                movie.DisplayDetails();
            }
        }

        public void DisplayAll()
        {
            DisplayTitle("list");

            if (movieCount == 0)
            {
                Console.WriteLine("\nThere's no movie in the system yet.");
            }

            Movie[] allMovies = GetExistingMovies();
            Sort.HeapSort(allMovies, "title");

            foreach (Movie movie in allMovies)
            {
                movie.DisplayDetails();
            }
        }

        public Movie[] GetExistingMovies()
        {
            Movie[] allMovies = new Movie[MAX_SIZE];
            int count = 0;

            for (int i = 0; i < MAX_SIZE; i++)
            {
                if (movies[i].IsOccupied())
                {
                    allMovies[count] = movies[i].GetMovie();
                    count++;
                }
            }

            Array.Resize(ref allMovies, count);
            return allMovies;
        }

        // implement heap sort algorithm
        public void DisplayTopThree()
        {
            DisplayTitle("top3");

            Movie[] allMovies = GetExistingMovies();
            Sort.HeapSort(allMovies, "viewcount");

            int count = 0;
            int len = allMovies.Length;
            Movie?[] topThree = new Movie[3];

            if (movieCount < 1)
            {
                Console.WriteLine("\nSorry! There is no movie in the system.");
                return;
            }
            else if (movieCount >= 1 && movieCount < 3)
            {
                for (int i = 0; i < movieCount; i++)
                {
                    if (allMovies[len - i - 1].GetViewCount() > 0)
                    {
                        topThree[i] = allMovies[len - i - 1];
                        count++;
                        Console.WriteLine($"{count}. {topThree[i]?.GetTitle()}");
                        Console.WriteLine($"   View: {topThree[i]?.GetViewCount()}\n");
                    }
                }
            }
            else if (movieCount >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (allMovies[len - i - 1].GetViewCount() > 0)
                    {
                        topThree[i] = allMovies[len - i - 1];
                        count++;
                        Console.WriteLine($"{count}. {topThree[i]?.GetTitle()}");
                        Console.WriteLine($"   View: {topThree[i]?.GetViewCount()}\n");
                    }
                }
            }

            if (count == 0)
            {
                Console.WriteLine("\nThere is no movie been rented yet.");
                Console.WriteLine("\nBe the first customer!");
            }

        }

        private static void DisplayTitle(string title)
        {
            Console.Clear();

            switch (title)
            {
                case "add":
                    Console.WriteLine("Add a New Movie");
                    break;
                case "remove":
                    Console.WriteLine("Remove a Movie");
                    break;
                case "list":
                    Console.WriteLine("All Movies");
                    break;
                case "search":
                    Console.WriteLine("Search for a movie");
                    break;
                case "rent":
                    Console.WriteLine("Borrow a Movie");
                    break;
                case "return":
                    Console.WriteLine("Return a Movie");
                    break;
                case "top3":
                    Console.WriteLine("Top 3 Rented Movie");
                    break;
            }
            Console.WriteLine("--------------------------------------------------------\n");
        }

        public void GenerateRandomMovie(string[] titles)
        {
            var rand = new Random();
            int randomNumberMovies = rand.Next(20, 51); // generate 20 - 50 movies
            
            for (int i = 0; i < randomNumberMovies; i++)
            {
                int foundIndex = Search(titles[i]);

                if (foundIndex != fail)
                {
                    Console.WriteLine("\nMovie already exist.");
                    continue;
                }
                else
                {
                    int randGenre = rand.Next(0, 9);
                    int randClass = rand.Next(0, 4);
                    int randDuration = rand.Next(60, 200);
                    int randInventory = rand.Next(1, 10);
                    int randView = rand.Next(0, 100);
                    // Movie(string title, int genreIndex, int classificationIndex, int duration, int inventory)
                    Movie newMovie = new(titles[i], randGenre, randClass, randDuration, randInventory, randView);
                    Console.WriteLine($"Add new movie: [{titles[i]}]");
                    Add(newMovie);
                }
            }

            Console.WriteLine("\n\nRandom movies generated complete.");
            Console.WriteLine($"Generated [{randomNumberMovies}] Random movies.");
        }
    }
}



