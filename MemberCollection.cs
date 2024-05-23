namespace DVD_System
{
    // the MemberCollection contains a Member[] array
    // [member1, member2, ...]
    // The collection initializes with 
    // a staff member and a default member ("john doe")
    class MemberCollection
    {
        private Member[] members;
        private int size;

        private readonly int indexNotFound = -999;
        private readonly string actionFail = "";

        private readonly string staffUserName = "staff";
        private readonly string staffPassword = "today123";

        private readonly string memberFirstName = "john";
        private readonly string memberLastName = "doe";
        private readonly string memberPhone = "0412345678";
        private readonly string memberPassword = "1234";

        // initialize with 1 staff and 1 member only
        // will keep the member collection as small as possible
        public MemberCollection(int capacity = 2)
        {
            members = new Member[capacity];
            members[0] = new Staff(staffUserName, staffPassword);
            members[1] = new RegularMember(memberFirstName, memberLastName, memberPhone, memberPassword);
            size = capacity;
        }

        // adjust members size
        // update per request on add/remove user
        // design to have no null member in array
        private void AdjustMemberSystem(string type, RegularMember? member = null, int removeIndex = -999)
        {
            if (type == "add")
            {
                Member[] newMembers = new Member[size + 1];
                Array.Copy(members, newMembers, size);
                members = newMembers;
                members[size] = member;
                size++;
            }
            else if (type == "remove")
            {
                if (removeIndex <= 0 || removeIndex >= size)
                {
                    Console.WriteLine("\nProvide a remove index to execute.");
                    return;
                }

                Member[] newMembers = new Member[size - 1];

                for (int i = 0, j = 0; i < size; i++)
                {
                    if (i == removeIndex) continue;
                    newMembers[j++] = members[i];
                }
                members = newMembers;
                size--;
            }
            else
            {
                Console.WriteLine("\nInvalid action, only add or remove action is valid.");
            }
        }

        // loop login
        public Member? Login(UserRole role)
        {
            while (true)
            {
                if (role == UserRole.Staff)
                {
                    Console.Write("Enter staff username: ");
                    string username = Console.ReadLine();

                    Console.Write("Enter staff password: ");
                    string password = Console.ReadLine();

                    if (username == members[0].GetUserName() &&
                        password == members[0].GetPassword())
                    {
                        Console.WriteLine("\nLogin successfully!");
                        return members[0];
                    }
                    Console.WriteLine("\nInvalid staff credential. Please try again...");
                }
                else
                {
                    var result = MemberNameInput();
                    string firstName = result.firstName;
                    string lastName = result.lastName;

                    Console.Write("Enter member password: ");
                    string password = Console.ReadLine();

                    int resultIndex = FindMemberByName(firstName, lastName);
                    if (resultIndex != indexNotFound)
                    {
                        if (password == members[resultIndex].GetPassword())
                        {
                            Console.WriteLine("\nLogin successfully!");
                            return members[resultIndex];
                        }
                        else
                        {
                            Console.WriteLine("\nWrong password. Please try again...");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nNo user found.");
                        return null;
                    }
                }
            }
        }

        public void AddMember()
        {
            Console.Clear();
            Console.WriteLine("Add a Member");
            Console.WriteLine("--------------------------------------------------------\n");

            var result = MemberNameInput();
            string firstName = result.firstName;
            string lastName = result.lastName;

            int foundIndex = FindMemberByName(firstName, lastName);
            if (foundIndex != indexNotFound)
            {
                Console.WriteLine("\nMember already exist, please enter another name...");
            }
            else
            {
                Console.Write("Enter member phone number (10 digits): ");
                string phone = ValidCredential("phone", Console.ReadLine());
                if (phone == actionFail) return;

                Console.Write("Enter member password (4 digits): ");
                string password = ValidCredential("password", Console.ReadLine());
                if (password == actionFail) return;

                RegularMember newMember = new(firstName, lastName, phone, password);
                AdjustMemberSystem("add", newMember);

                Console.WriteLine($"\nMember: {firstName} {lastName} is added to the system");
            }
            
        }

        public void RemoveMember()
        {
            Console.Clear();
            Console.WriteLine("Remove a Member");
            Console.WriteLine("--------------------------------------------------------\n");

            var result = MemberNameInput();
            string firstName = result.firstName;
            string lastName = result.lastName;

            int foundIndex = FindMemberByName(firstName, lastName);
            if (foundIndex == indexNotFound)
            {
                Console.WriteLine("\nCannot remove nonexistent member.");
            }
            else
            {
                // check member's movie borrow list
                // cannot remove member with unreturned movies
                int nullCount = 0;
                Movie?[] rentList = members[foundIndex].GetRentList();
                for (int i = 0; i < rentList.Length; i++)
                {
                    if (rentList[i] == null)
                        nullCount++;
                }

                if (nullCount == 0)
                {
                    AdjustMemberSystem("remove", removeIndex: foundIndex);
                    Console.WriteLine($"\nMember: {firstName} {lastName} is removed from the system");
                }
                else
                {
                    Console.WriteLine("\nCannot remove member with not returned movies.");
                }
            }
        }

        // for verify valid password/phone input
        // return fail or valid credential string
        private string ValidCredential(string type, string? credential)
        {
            if (!string.IsNullOrEmpty(credential))
            {
                if (type == "password" && (credential.Length != 4 || !int.TryParse(credential, out int pw)))
                {
                    Console.WriteLine("\nInvalid! Please enter a 4-digit number...");
                    return actionFail;
                }
                else if (type == "phone" && (credential.Length != 10 || !int.TryParse(credential, out int p)))
                {
                    Console.WriteLine("\nInvalid! Please enter a 10-digit phone number...");
                    return actionFail;

                }
                return credential;
            }
            Console.WriteLine("\nInput cannot be empty. Please try again...");
            return actionFail;
        }

        // return foundIndex or indexNotFound(-999)
        private int FindMemberByName(string firstName, string lastName)
        {
            for (int i = 1; i < size; i++)
            {
                if (firstName == members[i].GetFirstName() &&
                    lastName == members[i].GetLastName())
                {
                    return i;
                }
            }
            return indexNotFound;
        }

        // return member phone or return fail if not found
        private string FindMemberPhone(string firstName, string lastName)
        {
            int foundIndex = FindMemberByName(firstName, lastName);
            if (foundIndex != indexNotFound)
            {
                return members[foundIndex].GetPhone();
            }
            return actionFail;
        }

        public void GetMemberContact()
        {
            Console.Clear();
            Console.WriteLine($"Search For a Member's Contact");
            Console.WriteLine("--------------------------------------------------------\n");

            var result = MemberNameInput();
            string firstName = result.firstName;
            string lastName = result.lastName;

            string contactPhone = FindMemberPhone(firstName, lastName);

            if (contactPhone != actionFail)
            {
                Console.WriteLine("\nMember Information: \n");
                Console.WriteLine($"First Name: {firstName}");
                Console.WriteLine($"Last Name: {lastName}");
                Console.WriteLine($"Phone Number: {contactPhone}");
            }
            else
            {
                Console.WriteLine("\nCannot find such user.");
            }
        }

        public void FindMembersRentMovie(Movie movie)
        {
            int rentCount = 0;
            Member[]? rentMembers = new Member[movie.GetInventory()];

            Console.Clear();
            Console.WriteLine($"Members who rent {movie.GetTitle()}");
            Console.WriteLine("--------------------------------------------------------\n");

            for (int i = 1; i < size; i++)
            {
                Member member = members[i];
                Movie?[] rentList = member.GetRentList();

                foreach (Movie? m in rentList)
                {
                    if (m == movie)
                    {
                        rentMembers[rentCount] = member;
                        Console.WriteLine($"{rentCount + 1}. {member.GetFirstName() + ' ' + member.GetLastName()}");
                        rentCount++;
                    }
                }
            }

            if (rentCount == 0)
            {
                Console.WriteLine($"\nThere is no one renting {movie.GetTitle()} at the moment.");
            }
        }

        // prompt for member imput
        // firstName and lastName
        private static (string firstName, string lastName) MemberNameInput()
        {
            Console.Write("Enter member first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter member last name: ");
            string lastName = Console.ReadLine();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("\nDon't leave the name empty! Please try again...");
                MemberNameInput();
            }
            return (firstName, lastName);
        }
    }
}


