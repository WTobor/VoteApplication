namespace VoteApplication.Services.Models
{
    public class ResultModel
    {
        public ResultModel(string candidateFullName, int candidateVoteCount)
        {
            CandidateFullName = candidateFullName;
            CandidateVoteCount = candidateVoteCount;
        }

        public string CandidateFullName { get; }
        public int CandidateVoteCount { get; }
    }
}