using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG261_Project_Milestone_2
{
    public class BookingManager
    {
        
        //DELEGATE (Milestone Requirement)
        public delegate string BookingEvaluator(Student s);

      
        //EVENTS (Milestone Requirement)
        public event Action<Student> OnApproved;
        public event Action<Student> OnRejected;

     
        //INPUT VALIDATION + EXCEPTION HANDLING
        public List<Student> CaptureBookings()
        {
            List<Student> students = new List<Student>();
            string choice;

            do
            {
                Student s = new Student();

                Console.WriteLine("\n--- Enter Booking Details ---");

                Console.Write("Full Name: ");
                s.FullName = Console.ReadLine();

                Console.Write("Student Number: ");
                s.StudentNumber = Console.ReadLine();

                // Input Validation + Exception Handling
                while (true)
                {
                    try
                    {
                        Console.Write("Year of Study: ");
                        s.YearOfStudy = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input. Enter a number.");
                    }
                }

                Console.Write("Equipment Type: ");
                s.EquipmentType = Console.ReadLine();

                while (true)
                {
                    try
                    {
                        Console.Write("Duration (hours): ");
                        s.BookingDuration = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input. Enter a number.");
                    }
                }

                // YES/NO Validation
                while (true)
                {
                    Console.Write("Completed Training? (yes/no): ");
                    string input = Console.ReadLine().ToLower();

                    if (input == "yes")
                    {
                        s.HasTraining = true;
                        break;
                    }
                    else if (input == "no")
                    {
                        s.HasTraining = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter only yes or no.");
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Current Active Bookings: ");
                        s.ActiveBookings = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input. Enter a number.");
                    }
                }

                students.Add(s);

                Console.Write("\nAdd another booking? (y/n): ");
                choice = Console.ReadLine().ToLower();

            } while (choice == "y");

            return students;
        }

        // DELEGATE USED HERE
        public List<Student> EvaluateBookings(List<Student> bookings, out int rejectedCount)
        {
            rejectedCount = 0;

            // Assign delegate
            BookingEvaluator evaluator = EvaluateLogic;

            foreach (var s in bookings)
            {
                s.Status = evaluator(s);

                // EVENTS TRIGGERED HERE
                if (s.Status == "Approved" || s.Status == "Conditionally Approved")
                    OnApproved?.Invoke(s);
                else
                {
                    OnRejected?.Invoke(s);
                    rejectedCount++;
                }
            }

            
            //LINQ IMPLEMENTATION
            return bookings
                .Where(s => s.Status == "Approved" || s.Status == "Conditionally Approved")
                .ToList();
        }

        // Delegate Method Logic
        private string EvaluateLogic(Student s)
        {
            if (s.BookingDuration > 6 || s.ActiveBookings >= 3)
                return "Rejected";

            if (s.HasTraining && s.BookingDuration <= 4 && s.ActiveBookings < 2)
                return "Approved";

            if ((s.BookingDuration == 5 || s.BookingDuration == 6) && s.HasTraining && s.ActiveBookings < 2)
                return "Conditionally Approved";

            return "Rejected";
        }

        
        //LINQ: COUNT + SORT
        public void DisplayStatistics(List<Student> all, List<Student> approved, int rejected)
        {
            Console.WriteLine("\n--- Statistics ---");

            Console.WriteLine($"Total Requests: {all.Count}");

            // LINQ Count
            int approvedCount = approved.Count();
            Console.WriteLine($"Approved: {approvedCount}");

            Console.WriteLine($"Rejected: {rejected}");

            // LINQ Sorting
            var sorted = approved
                .OrderBy(s => s.YearOfStudy)
                .ThenBy(s => s.ActiveBookings)
                .ThenBy(s => s.BookingDuration);

            Console.WriteLine("\n--- Priority List ---");

            foreach (var s in sorted)
            {
                Console.WriteLine($"{s.FullName} | {s.Status} | Year {s.YearOfStudy}");
            }
        }
    }
}
    

