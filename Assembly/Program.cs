using System;
using McMaster.Extensions.CommandLineUtils;

namespace Assembly
{
    public class Program
    {
        public static int Main(string[] args) =>
            CommandLineApplication.Execute<Program>(args);

        [Option(CommandOptionType.SingleValue)]
        public string ReadsPath { get; }

        [Option(CommandOptionType.SingleValue)]
        public string ContigsPath { get; }

        [Option("-k <K>", "Mer length", CommandOptionType.SingleOrNoValue)]
        public int K { get; }

        private void OnExecute()
        {

        }
    }
}