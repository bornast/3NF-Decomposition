using _3NF.Decomposition.Core.Dtos;
using _3NF.Decomposition.Core.Entities;
using _3NF.Decomposition.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _3NF.Decomposition.Core.Data
{
    public static class Seed
    {
        public static void SeedRelations(IRelationService relationService)
        {
            var relations = relationService.GetRelations().Result;
            if (relations.Count() == 0)
            {
                var relationData = File.ReadAllText("Data/RelationData.json");
                var relationsToSeed = JsonConvert.DeserializeObject<List<RelationForCreationDto>>(relationData);

                foreach (var relationToSeed in relationsToSeed)
                {
                    relationService.CreateRelation(relationToSeed).Wait();
                }
            }
        }
    }
}
