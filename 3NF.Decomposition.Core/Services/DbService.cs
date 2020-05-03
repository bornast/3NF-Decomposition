using _3NF.Decomposition.Core.Dtos;
using _3NF.Decomposition.Core.Entities;
using _3NF.Decomposition.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3NF.Decomposition.Core.Services
{
    public class DbService : IDbService
    {
        private readonly IRepository _repo;

        public DbService(IRepository repository)
        {
            _repo = repository;
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

        // TODO: make async
        public void DecomposeToThirdNormalForm(int relationId)
        {
            var relation = _repo.GetRelation(relationId).Result;

            // (a) p: = {}
            var result = new List<List<Member>>();

            var resultMessages = new DecompositionResultDto();            
            resultMessages.Steps.Add("p := {}");

            /* 
            Create anonymous object:
            {
                 Sequence: 1,
                 LeftSideMembers: {
                    Member1, Member2... 
                },
                RightSideMembers: {
                    Member1, Member2...
                }
            },
            {
                Sequence: 2,
                ...
            }
            */
            var fmin = relation.FminMembers.ToList().GroupBy(
                x => x.Sequence,
                (key, value) => new
                {
                    Sequence = key,
                    LeftSideMembers = value.Select(x => x.LeftSideMember).Distinct().ToList(),
                    RightSideMembers = value.Select(x => x.RightSideMember).Distinct().ToList()
                }).OrderBy(x => x.Sequence);

            // (b) za svaku X->A e Fmin
            // go through each functional dependency
            foreach (var functionalDependecy in fmin)
            {
                // temp variables that we will use to print the step
                bool existingFunctionalDependency = false;
                List<Member> existingFunctionalDependencyMembers = new List<Member>();
                // go through list of members in result and check if they contain this functional dependency
                foreach (var resultMembers in result)
                {
                    // Does every member from functionalDependency exist in resultMembers
                    if (functionalDependecy.LeftSideMembers.All(x => resultMembers.Contains(x)) &&
                        functionalDependecy.RightSideMembers.All(x => resultMembers.Contains(x))
                    ) 
                    {
                        // this functional dependency already exists in the result, so we don't need to add
                        // it to the result, but store it to a temp variable so we can print this step
                        existingFunctionalDependency = true;
                        existingFunctionalDependencyMembers = resultMembers;
                        break;
                    }
                }

                // print: AB -> CD
                var decompositionStep = $"" +
                    $"{string.Join("", functionalDependecy.LeftSideMembers.Select(x => x.Name).ToArray())} " +
                    $"-> {string.Join("", functionalDependecy.RightSideMembers.Select(x => x.Name).ToArray())}\t";                    
                

                if (existingFunctionalDependency)
                {
                    // functional dependency already exists so print: AB is contained in ABC
                    decompositionStep += $"" +
                        $"{ListOfMembersToString(functionalDependecy.LeftSideMembers, parentheses: false) + ListOfMembersToString(functionalDependecy.RightSideMembers, parentheses: false) }" +
                        $" is contained in " +
                        $"{ListOfMembersToString(existingFunctionalDependencyMembers, parentheses: false)}";
                }
                else
                {
                    // functional dependecy doesn't exist, so print: {AB} Union {ABC}
                    var membersToAdd = new List<Member>();
                    membersToAdd.AddRange(functionalDependecy.LeftSideMembers);
                    membersToAdd.AddRange(functionalDependecy.RightSideMembers);

                    decompositionStep += $"p := {ListOfListOfMembersToString(result)} Union {ListOfMembersToString(membersToAdd)}";

                    result.Add(membersToAdd);
                }

                // add the print text to list of steps
                resultMessages.Steps.Add(decompositionStep);
            }

            // check if any key is in list, if no key appears in the result 
            // pick a random one and add it to the result
            // (c) Ako ključ od R nije podkljuc RI(Ri e p), tada
            var keys = relation.Keys;
            var keyExistsInResult = false;

            foreach (var members in result)
            {
                foreach (var key in keys)
                {                    
                    var keyMembers = key.KeyMembers.Select(x => x.Member);
                    // Does every member from key exist in resultMembers
                    if (keyMembers.All(x => members.Contains(x)))
                    {
                        // if the key already exists in the result, print: Key {AB} already exists in {ABC}
                        // raise the flag, keyExistsInResult to true
                        var decompositionStep = $"Key {ListOfMembersToString(keyMembers, parentheses: false)}"+
                            $" already exists in {ListOfMembersToString(members, parentheses: false)}";
                        resultMessages.Steps.Add(decompositionStep);
                        keyExistsInResult = true;
                    }
                }
            }

            // if key doesnt exist in the result, pick a random key from the list and add it to the result
            if (!keyExistsInResult)
            {
                resultMessages.Steps.Add("No key exists in the result, so we add a random key to the result");
                // add a random key to the result
                var randomIndex = new Random().Next(keys.Count);
                var randomKey = keys.ToArray()[randomIndex];
                var keyMembers = randomKey.KeyMembers.Select(x => x.Member).ToList();
                result.Add(keyMembers);

                // print: p := {AB, CD, EF} Union {ABC}
                var keyStep = $"p := " +
                    $"{ListOfListOfMembersToString(result)} Union " +
                    $"{ListOfMembersToString(randomKey.KeyMembers.Select(x => x.Member))}";
                resultMessages.Steps.Add(keyStep);
            }

            // print final result: Result: p := {AB, CD, EF}..
            resultMessages.Steps.Add($"Result: p := {ListOfListOfMembersToString(result)}");
            var test = 5;

        }

        #region ToString methods

        private string ListOfListOfMembersToString(List<List<Member>> members)
        {
            // FORM {AB, BC, D} from { {Member, Member}, {Member, Member}, {Member} }
            if (members.Count == 0)
                return "{ }";

            var result = string.Empty;

            foreach (var memberList in members)
            {
                result += $"{string.Join("", memberList.Select(x => x.Name).ToArray())}, ";
            }

            if (!string.IsNullOrEmpty(result))
                result = result.Remove(result.Length - 2);

            return $"{{{result}}}";
        }

        private string ListOfMembersToString(IEnumerable<Member> members, bool parentheses = true)
        {
            // FORM {AB} from {Member, Member}
            if (parentheses)            
                return $"{{{string.Join("", members.Select(x => x.Name).ToArray())}}}";

            // FORM AB from {Member, Member}
            return $"{string.Join("", members.Select(x => x.Name).ToArray())}";
        }

        #endregion

    }

}
