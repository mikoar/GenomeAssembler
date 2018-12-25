namespace Assembly.Models
{
    internal class KMer
    {
        public KMer(string kmer)
        {
            Kmer = kmer;
        }

        public string Kmer { get; set; }
        public string LeftMer { get => Kmer.Substring(0, Kmer.Length - 1); }
        public string RigthMer { get => Kmer.Substring(1); }
    }
}