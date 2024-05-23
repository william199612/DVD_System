namespace DVD_System
{
    enum UserRole
    {
        Staff,
        Member
    }

    class Member
    {
        private string Password { get; set; }

        private UserRole Role { get; set; }

        public Member(string password, UserRole role)
        {
            Password = password;
            Role = role;
        }

        public UserRole GetRole() => Role;

        public virtual string GetFirstName() => "";
        public virtual string GetLastName() => "";

        public virtual string GetPhone() => "";
        public virtual string GetUserName() => "";

        public string GetPassword() => Password;

        public virtual Movie?[] GetRentList() => new Movie[5];

        public virtual bool CanRent(Movie movie) => false;
        public virtual void Rent(Movie movie)
        {
            return;
        }

        public virtual bool Return(string title) => false;

        public virtual void DisplayRentList()
        {
            return;
        }
    }
    class Staff(string username, string password) : Member(password, UserRole.Staff)
    {
        private string Username { get; set; } = username;

        public override string GetUserName() => Username;
    }

    class RegularMember : Member
    {
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Phone { get; set; }
        // borrowlist may contain null object
        private Movie?[] BorrowedRecord { get; set; }

        public RegularMember(string firstName, string lastName, string phone, string password) : base(password, UserRole.Member)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            BorrowedRecord = new Movie[5];
        }

        // check if can rent the movie
        public override bool CanRent(Movie movie)
        {
            for (int i = 0; i < BorrowedRecord.Length; i++)
            {
                if (BorrowedRecord[i] == movie)
                {
                    Console.WriteLine($"\nYou already rented {movie.GetTitle()}, please rent another one...");
                    return false;
                }
                // record may contain null record in the middle
                // but duplicated after the null index
                else if (BorrowedRecord[i] == null)
                {
                    // continue loop from the first null obj
                    int firstNullIndex = i;
                    for (int j = firstNullIndex; j < BorrowedRecord.Length; j++)
                    {
                        if (BorrowedRecord[j] == movie)
                        {
                            Console.WriteLine($"\nYou already rented {movie.GetTitle()}, please rent another one...");
                            return false;
                        } // last record index
                        else if (BorrowedRecord[j] == null && j == (BorrowedRecord.Length - 1))
                        {
                            return true;
                        }
                    }
                    return true;
                }
            }
            // if no null record -> list full
            Console.WriteLine("\nYour borrow quota is full, please return at least one to borrow again...");
            return false;
        }

        // only return true to execute
        public override void Rent(Movie movie)
        {
            for (int i = 0; i < BorrowedRecord.Length; i++)
            {
                if (BorrowedRecord[i] == null)
                {
                    BorrowedRecord[i] = movie;
                    return;
                }
            }
        }

        public override bool Return(string title)
        {
            for (int i = 0; i < BorrowedRecord.Length; i++)
            {
                if (BorrowedRecord[i] != null && BorrowedRecord[i]?.GetTitle() == title)
                {
                    BorrowedRecord[i] = null;
                    return true;
                }
            }
            return false;
        }

        public override void DisplayRentList()
        {
            Console.Clear();
            string name = FirstName + " " + LastName;
            int count = 0;
            Console.WriteLine($"Member: {name}'s Rent list");
            Console.WriteLine("--------------------------------------------------------\n");

            for (int i = 0; i < BorrowedRecord.Length; i++)
            {
                if (BorrowedRecord[i] != null)
                {
                    count++;
                    Console.WriteLine($"{count}. {BorrowedRecord[i]?.GetTitle()}");
                }

            }
            if (count == 0)
            {
                Console.WriteLine("\nNo currently borrowed movie.");
            }
        }

        public override Movie?[] GetRentList() => BorrowedRecord;

        public override string GetFirstName() => FirstName;
        public override string GetLastName() => LastName;

        public override string GetPhone() => Phone;
    }
}


