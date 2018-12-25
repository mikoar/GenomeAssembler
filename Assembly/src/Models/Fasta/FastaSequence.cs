namespace Assembly.Models
{
    public class FastaSequence
    {
        public FastaSequence(string header, string sequence)
        {
            Header = header;
            Sequece = sequence;
        }
        public string Header { get; set; }
        public string Sequece { get; set; }
    }
}