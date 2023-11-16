using System.Diagnostics;
using System.IO.Enumeration;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace MJU23v_DTP_T2
{
    internal class Program
    {
        static List<Link> links = new List<Link>();
        class Link
        {
            public string category, group, name, description, link;
            public Link(string category, string group, string name, string description, string link)
            {
                this.category = category;
                this.group = group;
                this.name = name;
                this.description = description;
                this.link = link;
            }

            public Link(string line)
            {
                string[] part = line.Split('|');
                category = part[0];
                group = part[1];
                name = part[2];
                description = part[3];
                link = part[4];
            }
            public void Print(int index)
            {
                Console.WriteLine($"|{index,-2}|{category,-10}|{group,-10}|{name,-20}|{description,-40}|");
            }

            public void OpenLink()
            {
                Process application = new Process();
                application.StartInfo.UseShellExecute = true;
                application.StartInfo.FileName = link;
                application.Start();
                // application.WaitForExit();
            }
            /*public string ToString()
            {
                return $"{category}|{group}|{name}|{description}|{link}";
            }*/
        }
        static void Main(string[] args)
        {
            string filename = @"..\..\..\links\links.lis";
            using (StreamReader sr = new StreamReader(filename))
            {
                Console.WriteLine("Welcome to the link list! write 'help' for help!");
                int numbering = 0;
                string line = sr.ReadLine();
                while (line != null)
                {
                    /* Console.WriteLine(line);
                    Link L = new Link(line);
                    L.Print(numbering++);
                    links.Add(L);*/
                    line = sr.ReadLine();
                }
            }
            do
            {
                Console.Write("> ");
                string cmd = Console.ReadLine().Trim();
                string[] arg = cmd.Split();
                string command = arg[0];

                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                else if (command == "help")
                {
                    PrintHelp();
                }
                else if (command == "load")
                {
                    LoadCommand(filename, arg);
                }
                else if (command == "list")
                {
                    ListEntries();
                }
                else if (command == "new")
                {
                    NewEntry();
                }
                else if (command == "save")
                {
                    filename = SaveEntry(filename, arg);
                }
                else if (command == "remove") //FIXMe Prevent from writing larger number then links length
                {
                        links.RemoveAt(Int32.Parse(arg[1]));
                }
                else if (command == "open")
                {
                    OpenLogic(arg);
                }
                else
                {
                    Console.WriteLine("Unknown Command: '{command}'");
                }
            } while (true);
        }

        private static void OpenLogic(string[] arg)
        {
            if (arg[1] == "link")
            {
                int ix = Int32.Parse(arg[2]); //FIXMe writing incorrect crashes
                links[ix].OpenLink();
            }
        }

        private static string SaveEntry(string filename, string[] arg)
        {
            if (arg.Length == 2)
            {
                filename = $@"..\..\..\links\{arg[1]}";
            }
            using (StreamWriter sr = new StreamWriter(filename))
            {
                foreach (Link Links in links)
                {
                    sr.WriteLine(Links.ToString());
                }
            }

            return filename;
        }

        private static void ListEntries()
        {
            int numbering = 0;
            foreach (Link Links in links)
                Links.Print(numbering++);
        }

        private static void NewEntry()
        {
            Console.WriteLine("Create a new link:");
            Console.Write("  enter Category: ");
            string category = Console.ReadLine();
            Console.Write("  enter Group: ");
            string group = Console.ReadLine();
            Console.Write("  enter Name: ");
            string name = Console.ReadLine();
            Console.Write("  enter Description: ");
            string descr = Console.ReadLine();
            Console.Write("  Enter Link: ");
            string link = Console.ReadLine();
            Link newLink = new Link(category, group, name, descr, link);
            links.Add(newLink);
        }

        private static void PrintHelp()
        {
            Console.WriteLine("help           - Check help");
            Console.WriteLine("quit           - Closes program");
            Console.WriteLine("load           - Loads list file");
            Console.WriteLine("list           - Displays current list");
            Console.WriteLine("new            - Add new entry to list");
            Console.WriteLine("Save           - Saves list");
            Console.WriteLine("remove x       - Removes entry based on number in list");
            Console.WriteLine("open link x    - Opens the corresponding link to number in list");
        }

        static void LoadCommand(string filename, string[] arg)
        {
            if (arg.Length == 2)
            {
                filename = $@"..\..\..\links\{arg[1]}";
            }
            links = new List<Link>();
            using (StreamReader sr = new StreamReader(filename)) //Fixme If input load x crashes
            {
                int numbering = 0;
                string line = sr.ReadLine();
                while (line != null)
                {
                    Console.WriteLine(line);
                    Link Links = new Link(line);
                    links.Add(Links);
                    line = sr.ReadLine();
                }
            }
        }
    }
}