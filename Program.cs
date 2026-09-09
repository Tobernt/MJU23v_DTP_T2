using System.Diagnostics;

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
                if (part.Length != 5) throw new FormatException("Expected five pipe-delimited fields.");
                category = part[0];
                group = part[1];
                name = part[2];
                description = part[3];
                link = part[4];
            }
            public override string ToString() => string.Join("|", category, group, name, description, link);

            public void Print(int index)
            {
                Console.WriteLine($"|{index,-2}|{category,-10}|{group,-10}|{name,-20}|{description,-40}|");
            }

            public void OpenLink()
            {
                if (!Uri.TryCreate(link, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    Console.WriteLine("Only HTTP and HTTPS links can be opened.");
                    return;
                }
                Process application = new Process();
                application.StartInfo.UseShellExecute = true;
                application.StartInfo.FileName = link;
                application.Start();
            }
        }
        static void Main(string[] args)
        {
            string filename = @"..\..\..\links\links.lis";
            Console.WriteLine("Welcome to the link list! Type 'help' for commands.");
            LoadCommand(filename, new[] { "load" });
            do
            {
                Console.Write("> ");
                string? cmd = Console.ReadLine();
                if (cmd == null) break;
                string[] arg = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (arg.Length == 0) continue;
                string command = arg[0].ToLowerInvariant();

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
                else if (command == "remove")
                {
                    RemoveEntry(arg);
                }
                else if (command == "open")
                {
                    OpenLogic(arg);
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            } while (true);
        }

        private static void RemoveEntry(string[] arg)
        {
            if (arg.Length >= 2 && int.TryParse(arg[1], out int index) && index >= 0 && index < links.Count)
            {
                links.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Invalid index or link not found.");
            }
        }

        private static void OpenLogic(string[] arg)
        {
            if (arg.Length >= 3 && arg[1] == "link") // Check if 'link' command has enough arguments
            {
                if (int.TryParse(arg[2], out int ix) && ix >= 0 && ix < links.Count) // Validate index
                {
                    links[ix].OpenLink();
                }
                else
                {
                    Console.WriteLine("Invalid index or link not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid command or insufficient arguments.");
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
            string category = Console.ReadLine() ?? "";
            Console.Write("  enter Group: ");
            string group = Console.ReadLine() ?? "";
            Console.Write("  enter Name: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("  enter Description: ");
            string descr = Console.ReadLine() ?? "";
            Console.Write("  Enter Link: ");
            string link = Console.ReadLine() ?? "";
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
            try
            {
                if (arg.Length == 2)
                {
                    filename = $@"..\..\..\links\{arg[1]}";
                }
                var loadedLinks = new List<Link>();
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        Link Links = new Link(line);
                        loadedLinks.Add(Links);
                        line = sr.ReadLine();
                    }
                    links = loadedLinks;
                    Console.WriteLine("Successfully loaded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load: {ex.Message}");
            }
        }
    }
}