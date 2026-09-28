using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG261_Project_Milestone_2
{
    enum Menu
    {
        Capture = 1,
        Evaluate,
        Statistics,
        Exit
    }
    internal class Program
    {
       

        static void Main(string[] args)
        {
            BookingManager manager = new BookingManager();

            List<Student> allBookings = new List<Student>();
            List<Student> approvedBookings = new List<Student>();
            int rejectedCount = 0;

            
            //EVENT HANDLING (Milestone Requirement)
            
            manager.OnApproved += (s) =>
            {
                Console.WriteLine($"[EVENT] Approved: {s.FullName}");
            };

            manager.OnRejected += (s) =>
            {
                Console.WriteLine($"[EVENT] Rejected: {s.FullName}");
            };

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n--- TechnoLab System ---");
                Console.WriteLine("1. Capture Bookings");
                Console.WriteLine("2. Evaluate Bookings");
                Console.WriteLine("3. View Statistics");
                Console.WriteLine("4. Exit");

                Console.Write("Select option: ");

                if (Enum.TryParse(Console.ReadLine(), out Menu choice))
                {
                    switch (choice)
                    {
                        case Menu.Capture:
                            allBookings = manager.CaptureBookings();
                            break;

                        case Menu.Evaluate:
                            approvedBookings = manager.EvaluateBookings(allBookings, out rejectedCount);
                            break;

                        case Menu.Statistics:
                            manager.DisplayStatistics(allBookings, approvedBookings, rejectedCount);
                            break;

                        case Menu.Exit:
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
        }
    }
}
        
   



