namespace DVD_System
{
    class Movie
    {

        public string[] GenreList = {
            "Drama", "Adventure", "Family", "Action", "Sci-fi", "Comedy", "Animated", "Thriller", "Other"
        };
        public string[] ClassificationList = {
            "General (G)", "Parental Guidance (PG)", "Mature (M15+)", "Mature Accompanied(MA15+)"
        };

        private string genre;
        private string classification;
        private int duration;
        private int inventory; // total number of movie
        private int stock; // actual number of movie (disclude borrowed number)
        private int viewCount; // number of rented

        private string Title { get; set; }
        private string Genre
        {
            get
            {
                return genre;
            }
            set
            {
                if (!GenreList.Contains(value))
                {
                    throw new ArgumentException("Invalid Genre!");
                }
                genre = value;
            }
        }

        private string Classification
        {
            get
            {
                return classification;
            }
            set
            {
                if (!ClassificationList.Contains(value))
                {
                    throw new ArgumentException("Invalid Classification!");
                }
                classification = value;
            }
        }

        private int Duration
        {
            get
            {
                return duration;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Invalid Classification!");
                }
                duration = value;
            }
        }

        private int Inventory
        {
            get
            {
                return inventory;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Invalid Inventory!");
                }
                inventory = value;
            }
        }

        private int Stock
        {
            get
            {
                return stock;
            }
            set
            {
                if (value > inventory || value < 0)
                {
                    throw new ArgumentException("Invalid Stock!");
                }
                stock = value;
            }
        }

        private int ViewCount
        {
            get { return viewCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Invalid ViewCount!");
                }
                viewCount = value;
            }
        }

        // for user input
        public Movie(string title)
        {
            Title = title;
            SetGenre();
            SetClassification();
            SetDuration();
            SetInitialInventoryAndStock(); // inventory and stock will be set the same
            ViewCount = 0;
        }

        // for random movie generation
        public Movie(string title, int genreIndex, int classificationIndex, int duration, int inventory, int view)
        {
            Title = title;
            Genre = GenreList[genreIndex];
            Classification = ClassificationList[classificationIndex];
            Duration = duration;
            Inventory = inventory;
            Stock = inventory;
            ViewCount = view;
        }

        public void SetGenre()
        {
            Console.WriteLine("");

            bool valid = false;
            for (int i = 0; i < GenreList.Length; i++)
            {
                Console.WriteLine($"{i}. {GenreList[i]}");
            }
            while (!valid)
            {
                
                Console.Write("Enter the Genre Number: ");
                if (!int.TryParse(Console.ReadLine(), out int genreIndex))
                {
                    Console.WriteLine("Invalid! Please enter a number...");
                }
                else if (genreIndex > (GenreList.Length - 1) || genreIndex < 0)
                {
                    Console.WriteLine($"Please try again. Enter a number between 0 - {GenreList.Length - 1}");
                }
                else
                {
                    Genre = GenreList[genreIndex];
                    valid = true;
                }
            }
        }
        public void SetClassification()
        {
            Console.WriteLine("");

            bool valid = false;
            for (int i = 0; i < ClassificationList.Length; i++)
            {
                Console.WriteLine($"{i}. {ClassificationList[i]}");
            }

            while (!valid)
            {
                Console.Write("Enter the Classification Number: ");
                if (!int.TryParse(Console.ReadLine(), out int classIndex))
                {
                    Console.WriteLine("\nInvalid! Please enter a number...");
                }
                else if (classIndex > (ClassificationList.Length - 1) || classIndex < 0)
                {
                    Console.WriteLine($"\nPlease try again. Enter a number between 0 - {ClassificationList.Length - 1}");
                }
                else
                {
                    Classification = ClassificationList[classIndex];
                    valid = true;
                }
            }

        }
        public void SetDuration()
        {
            Console.WriteLine("");
            
            bool valid = false;
            while (!valid)
            {
                Console.Write("Enter the Movie Duration (min): ");
                if (!int.TryParse(Console.ReadLine(), out int duration))
                {
                    Console.WriteLine("\nInvalid! Please enter a number...");
                }
                else if (duration <= 0)
                {
                    Console.WriteLine($"\nPlease try again. Enter a number greater than 0.");
                }
                else
                {
                    Duration = duration;
                    valid = true;
                }
            }
        }
        public void SetInitialInventoryAndStock()
        {
            Console.WriteLine("");
            bool valid = false;
            while (!valid)
            {
                Console.Write("Enter Movie Inventory (integer): ");
                if (!int.TryParse(Console.ReadLine(), out int inventory))
                {
                    Console.WriteLine("\nInvalid! Please enter a number...");
                }
                else if (inventory <= 0)
                {
                    Console.WriteLine($"\nPlease try again. Enter a number greater than 0.");
                }
                else
                {
                    // set initial inventory and stock same
                    Inventory = inventory;
                    Stock = inventory;
                    valid = true;
                }
            }
        }

        public void IncreaseInventory(int count = 1)
        {
            Inventory += count;
            Stock += count;
        }

        public bool DecreaseInventory(int count = 1)
        {
            if (Stock > 0)
            {
                Inventory -= count;
                Stock -= count;
                return true;
            }
            else
            {
                Console.WriteLine("\nInventory is empty.");
                return false;
            }
        }

        // stock change based on member action(return or borrow)
        public void BeenReturn()
        {
            Stock++;
        }

        public bool CanBeRented()
        {
            if (Stock > 0)
            {
                return true;
            }
            else
            {
                Console.WriteLine("\nSorry! This movie is out of stock.");
                return false;
            }

        }

        public void BeenRented()
        {
            Stock--;
            viewCount++;
        }

        public void DisplayDetails()
        {
            Console.WriteLine($"\nTitle: {Title}");
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"Classification: {Classification}");
            Console.WriteLine($"Duration: {Duration}");
            Console.WriteLine($"Stock: {Stock}");
            Console.WriteLine($"Rented: {ViewCount}");
        }

        public string GetTitle() => Title;

        public int GetInventory() => Inventory;

        public int GetStock() => Stock;

        public int GetViewCount() => ViewCount;
    }
}

