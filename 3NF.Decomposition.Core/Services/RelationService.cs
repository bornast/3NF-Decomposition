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

            // Add attributes
            foreach (var attribute in relationForCreation.Attributes)
            {
                relation.Attributes.Add(new Entities.Attribute { Name = attribute });
            }

            _repo.Add(relation);

            await _repo.SaveAsync();

            // add keys
            foreach (var key in relationForCreation.Keys)
            {
                var newKey = new Key();

                // add key attributes
                foreach (var attribute in key.Value)
                {
                    var attributeId = relation.Attributes.FirstOrDefault(x => x.Name == attribute).Id;
                    newKey.KeyAttributes.Add(new KeyAttribute { AttributeId = attributeId });
                }

                relation.Keys.Add(newKey);
            }

            // add fmin
            int sequence = 1;
            foreach (var attributes in relationForCreation.Fmin)
            {                
                foreach (var leftSideAttribute in attributes.LeftSideAttributes)
                {
                    var leftAttributeId = relation.Attributes.FirstOrDefault(x => x.Name == leftSideAttribute).Id;
                    foreach (var rightSideAttribute in attributes.RightSideAttributes)
                    {
                        var rightAttributeId = relation.Attributes.FirstOrDefault(x => x.Name == rightSideAttribute).Id;
                        var fmin = new FminAttribute
                        {
                            LeftSideAttributeId = leftAttributeId,
                            RightSideAttributeId = rightAttributeId,
                            Sequence = sequence
                        };

                        relation.FminAttributes.Add(fmin);
                        await _repo.SaveAsync();
                    }                    
                }
                sequence += 1;
            }

            await _repo.SaveAsync();
        }

        public async Task DeleteRelation(int relationId)
        {
            var relation = await _repo.GetRelation(relationId);
            // keys
            foreach (var key in relation.Keys)
            {
                _repo.Remove(key);
            }

            // fmin attributes
            foreach (var fminAttribute in relation.FminAttributes)
            {
                _repo.Remove(fminAttribute);
            }

            // relation
            _repo.Remove(relation);

            await _repo.SaveAsync();
        }
    }

}
