using System;

namespace Assembly
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Contigs assemlber. Usage:");
                Console.WriteLine($"{ System.Reflection.Assembly.GetExecutingAssembly().GetName().Name } input_reads.fasta output_contigs.fasta");
                return;
            }

        }
    }
}