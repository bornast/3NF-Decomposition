using _3NF.Decomposition.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace _3NF.Decomposition.Core.Dtos
{
    public class RelationDto
    {
        public int Id { get; set; }
        public string Relation { get; set; }
        public Dictionary<string, string> Keys { get; set; } = new Dictionary<string, string>();
        public string Fmin { get; set; }
        public static RelationDto MapFrom(Relation relation)
        {
            var relationDto = new RelationDto
            {
                Id = relation.Id,
                Relation = $"{{{string.Join("", relation.Attributes.Select(x => x.Name))}}}"
            };

            int i = 1;
            foreach (var key in relation.Keys)
            {
                relationDto.Keys.Add($"K{i}", $"{string.Join("", key.KeyAttributes.Select(x => x.Attribute.Name))}");
                i += 1;
            }

            var fmin = relation.FminAttributes.ToList().GroupBy(
                x => x.Sequence,
                (key, value) => new
                {
                    Sequence = key,
                    LeftSideAttributes = value.Select(x => x.LeftSideAttribute).Distinct().ToList(),
                    RightSideAttributes = value.Select(x => x.RightSideAttribute).Distinct().ToList()
                }).OrderBy(x => x.Sequence);

            var functionalDependecies = new List<string>();
            foreach (var functionalDependency in fmin)
            {
                functionalDependecies.Add($"" +
                    $"{string.Join("", functionalDependency.LeftSideAttributes.Select(x => x.Name).ToArray())} " +
                    $"-> {string.Join("", functionalDependency.RightSideAttributes.Select(x => x.Name).ToArray())}");
            }

            relationDto.Fmin = $"{{{string.Join(", ", functionalDependecies)}}}";

            return relationDto;            
        }
    }
}
