
using System;
using System.IO;
using System.Transactions;

namespace TicketingSystem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = "";
            bool run = true;

            while (run)
            {
                Console.WriteLine("Ticketing System");
                Console.WriteLine("Select an Option: ");

                int userChoice = MenuOptions.runMenu();
                switch (userChoice)
                {
                    case 1:
                        Console.Write("FileName (default filename = 'tickets.csv'): ");
                        file = file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Console.ReadLine());
                        readData(file);
                        break;
                    case 2:
                        Console.Write("New Filename (existing file will be appended to): ");
                        file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Console.ReadLine());
                        bool exists = File.Exists(file);
                        StreamWriter writer = new StreamWriter(file, append: true);
                        if (!exists)
                        {
                            writer.WriteLine("TicketID,Summary,Status,Priority,Submitter,Assigned,Watching");
                        } ;
                        writeData(writer);
                        break;
                    default:
                        run = false;
                        break;
                }
            }
        }

        private static void readData(String file)
        {
            StreamReader reader;
            if (File.Exists(file))
            {
                reader = new StreamReader(file);
                reader.ReadLine(); //Skip the header line
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    String[] parts = line.Split(',');
                    Ticket readTicket = new Ticket(parts);
                    readTicket.printTicket();
                }

                reader.Close();
            }
            else
            {
                Console.WriteLine($"{file} does not exist");
            }
        }

        private static void writeData(StreamWriter writer)
        {
            bool moreTickets = true;
            while (moreTickets)
            {


                Console.Write("Enter ID#: ");
                String iD = Console.ReadLine();

                Console.Write("Enter Summary: ");
                String summary = Console.ReadLine();

                Console.Write("Enter Status: ");
                String status = Console.ReadLine();

                Console.Write("Enter Priority: ");
                String priority = Console.ReadLine();

                Console.Write("Enter Submitter: ");
                String submitter = Console.ReadLine();

                Console.Write("Enter Assigned: ");
                String assigned = Console.ReadLine();

                Console.Write("How many are watching?: ");
                int numWatching = int.Parse(Console.ReadLine());
                String watching = "";
                if (numWatching > 0)
                {
                    Console.WriteLine("Enter Watchers: ");
                    for (int i = 0; i < numWatching; i++)
                    {
                        Console.Write($"\tWatcher#{i + 1}: ");
                        watching += Console.ReadLine();
                        if (i + 1 < numWatching)
                        {
                            watching += '|';
                        }
                    }
                }

                String[] parts = {iD, summary, status, priority, submitter, assigned, watching};
                Ticket newTicket = new Ticket(parts);
                writer.WriteLine(newTicket.ticketCSV());

                Console.Write("Add another ticket? Yes(1) or No(2): ");
                int userResponse = int.Parse(Console.ReadLine());
                switch (userResponse)
                {
                    case 1:
                        break;
                    default:
                        moreTickets = false;
                        break;
                }
            }

            writer.Close();
        }
    }

    internal static class MenuOptions
    {
        private static string[] menuStrings = {"1: Read Data from File","2: Create File from Data", "3: Exit" };

        public static int runMenu()
        {
            Console.WriteLine(menuStrings[0] + "\n" + menuStrings[1] + "\n" + menuStrings[2]);
            var userResponse = Int32.Parse(Console.ReadLine());
            return userResponse;
        }
    }

    internal class Ticket
    {
        private int ticketID;
        private string summary;
        private String status;
        private String priority;
        private String submitter;
        private String assigned;
        private String[] watching;
        private String watchingPiped;
        public Ticket(string[] ticketParts)
        {
            ticketID = int.Parse(ticketParts[0]);
            summary = ticketParts[1];
            status = ticketParts[2];
            priority = ticketParts[3];
            submitter = ticketParts[4];
            assigned = ticketParts[5];
            watchingPiped = ticketParts[6];
            watching = watchingPiped.Split('|');
        }

        public void printTicket()
        {
            Console.WriteLine($"Ticket#: {ticketID}: {summary}");
            Console.WriteLine($"\tStatus: {status}");
            Console.WriteLine($"\tPriority: {priority}");
            Console.WriteLine($"\tSubmitter: {submitter}");
            Console.WriteLine($"\tAssigned: {assigned}");
            Console.WriteLine($"\tWatching: ");
            for (int i = 0; i < watching.Length; i++)
            {
                Console.WriteLine($"\t\t{watching[i]}");
            }
        }

        public String ticketCSV()
        {
            String output = $"{ticketID},{summary},{status},{priority},{submitter},{assigned},{watchingPiped}";
            return output;
        }
    }
}