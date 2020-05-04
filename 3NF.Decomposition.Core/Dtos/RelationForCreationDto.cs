using System.Collections.Generic;

namespace _3NF.Decomposition.Core.Dtos
{
    public class RelationForCreationDto
    {
        public List<string> Attributes { get; set; } = new List<string>();
        public Dictionary<int, List<string>> Keys { get; set; } = new Dictionary<int, List<string>>();
        public List<FminAttributesForCreationDto> Fmin { get; set; } = new List<FminAttributesForCreationDto>();
    }

    public class FminAttributesForCreationDto {

        public List<string> LeftSideAttributes { get; set; }
        public List<string> RightSideAttributes { get; set; }
    }


}
