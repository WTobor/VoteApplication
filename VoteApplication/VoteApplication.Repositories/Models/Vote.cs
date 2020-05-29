using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoteApplication.Repositories.Models
{
    public class Vote
    {
        public Vote(string userNickname, int candidateId)
        {
            UserNickname = userNickname;
            CandidateId = candidateId;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string UserNickname { get; set; }

        [Required]
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}
