namespace VoteApplication.Services.Models
{
    public class ResultModel
    {
        public ResultModel(string candidateSurname, string candidateName, int candidateVoteCount)
        {
            CandidateFullName = string.Join(" ", candidateSurname, candidateName);
            CandidateVoteCount = candidateVoteCount;
        }

        public string CandidateFullName { get; }
        public int CandidateVoteCount { get; }
    }
}