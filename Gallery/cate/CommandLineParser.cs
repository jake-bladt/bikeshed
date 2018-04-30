using System;
using System.Collections.Generic;
using System.Linq;

namespace cate
{
    public class CommandLineParser
    {
        public IEnumerable<String> Switches { get; protected set; }
        public IEnumerable<String> Categories { get; protected set; }
        public String SubjectName { get; protected set; } = String.Empty;

        public CommandLineParser(string[] args)
        {
            SubjectName = args[0];
            Switches = args.ToList().Where(a => a.StartsWith("-"));
            Categories = args.ToList().Where(a => !a.StartsWith("-") && a != SubjectName);
        }
    }
}
