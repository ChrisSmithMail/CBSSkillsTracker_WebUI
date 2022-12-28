using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBSSkillsTracker_WebUI.Models
{
    public class ScoreModel
    {


        public int Id { get; set; }
        public int User { get; set; }
        public int SubmissionPeriod { get; set; }
        public int Capability { get; set; }
        public byte Score { get; set; }

    }
}
