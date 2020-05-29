namespace VoteApplication.Services.Models
{
    public class CandidateModel
    {
        public CandidateModel(int id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        public int Id { get; }
        public string FullName { get; }
    }
}
