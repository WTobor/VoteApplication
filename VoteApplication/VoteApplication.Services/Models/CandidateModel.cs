namespace VoteApplication.Services.Models
{
    public class CandidateModel
    {
        public CandidateModel(int id, string surname, string name)
        {
            Id = id;
            FullName = string.Join(" ", surname, name);
        }

        public int Id { get; }
        public string FullName { get; }
    }
}
