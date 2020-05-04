using _3NF.Decomposition.Core.Dtos;
using _3NF.Decomposition.Core.Entities;
using _3NF.Decomposition.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3NF.Decomposition.Core.Services
{
    public class RelationService : IRelationService
    {
        private readonly IRepository _repo;

        public RelationService(IRepository repository)
        {
            _repo = repository;
        }

        public async Task<IEnumerable<RelationDto>> GetRelations()
        {
            var relations = await _repo.GetRelations();

            var result = new List<RelationDto>();

            foreach (var relation in relations)
            {
                result.Add(RelationDto.MapFrom(relation));
            }

            return result;
        }        

        public async Task<RelationDto> GetRelation(int relationId)
        {
            var relation = await _repo.GetRelation(relationId);

            if (relation == null)
                throw new Exception($"Relation id:{relationId} not found!");

            return RelationDto.MapFrom(relation);
        }       

        public async Task CreateRelation(RelationForCreationDto relationForCreation)
        {
            // Add relation
            var relation = new Relation();

            // Add members
            foreach (var member in relationForCreation.Members)
            {
                relation.Members.Add(new Member { Name = member });
            }

            _repo.Add(relation);

            await _repo.SaveAsync();

            // add keys
            foreach (var key in relationForCreation.Keys)
            {
                var newKey = new Key();

                // add key members
                foreach (var member in key.Value)
                {
                    var memberId = relation.Members.FirstOrDefault(x => x.Name == member).Id;
                    newKey.KeyMembers.Add(new KeyMember { MemberId = memberId });
                }

                relation.Keys.Add(newKey);
            }

            // add fmin
            int sequence = 1;
            foreach (var members in relationForCreation.Fmin)
            {                
                foreach (var leftSideMember in members.LeftSideMembers)
                {
                    var leftMemberId = relation.Members.FirstOrDefault(x => x.Name == leftSideMember).Id;
                    foreach (var rightSideMember in members.RightSideMembers)
                    {
                        var rightMemberId = relation.Members.FirstOrDefault(x => x.Name == rightSideMember).Id;
                        var fmin = new FminMember
                        {
                            LeftSideMemberId = leftMemberId,
                            RightSideMemberId = rightMemberId,
                            Sequence = sequence
                        };

                        relation.FminMembers.Add(fmin);
                        await _repo.SaveAsync();
                    }                    
                }
                sequence += 1;
            }

            await _repo.SaveAsync();
        }
    }

}
