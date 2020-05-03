using System.Collections.Generic;

namespace _3NF.Decomposition.Core.Dtos
{
    public class RelationForCreationDto
    {
        public List<string> Members { get; set; } = new List<string>();
        public Dictionary<int, List<string>> Keys { get; set; } = new Dictionary<int, List<string>>();
        public List<FminMembersForCreationDto> Fmin { get; set; } = new List<FminMembersForCreationDto>();
    }

    public class FminMembersForCreationDto {

        public List<string> LeftSideMembers { get; set; }
        public List<string> RightSideMembers { get; set; }
    }


}
