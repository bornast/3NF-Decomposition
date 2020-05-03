using _3NF.Decomposition.Core.Dtos;
using _3NF.Decomposition.Core.Entities;
using _3NF.Decomposition.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.XPath;

namespace _3NF.Decomposition.Core.Services
{
    public class DbService : IDbService
    {
        private readonly IRepository _repository;

        public DbService(IRepository repository)
        {
            _repository = repository;
        }
        // TODO: REFACTOR THIS WHOLE METHOD
        public void DecomposeToThirdNormalForm(int relationId)
        {
            var relation = _repository.GetRelation(relationId).Result;

            var decompositionResult = new DecompositionResultDto();

            // (a) ro: = {}
            var result = new List<List<Member>>();

            decompositionResult.Steps.Add("p := {}");

            var fmin = relation.FminMembers.ToList().GroupBy(
                p => p.Sequence,
                (key, value) => new
                {
                    Sequence = key,
                    LeftSideMembers = value.Select(x => x.LeftSideMember).Distinct().ToList(),
                    RightSideMembers = value.Select(x => x.RightSideMember).Distinct().ToList()
                }).OrderBy(x => x.Sequence);

            // (b) za svaku X->A e Fmin
            foreach (var functionalDependecy in fmin)
            {
                bool existingFunctionalDependency = false;
                List<Member> existingFunctionalDependencyMembers = new List<Member>();
                foreach (var resultMembers in result)
                {
                    // Does every member from functionalDependency exist in resultMembers
                    if (functionalDependecy.LeftSideMembers.All(x => resultMembers.Contains(x)) &&
                        functionalDependecy.RightSideMembers.All(x => resultMembers.Contains(x))
                    ) 
                    {
                        existingFunctionalDependency = true;
                        existingFunctionalDependencyMembers = resultMembers;
                        break;
                    }

                    //// if any existing member contain members from functional dependecy
                    //// TODO: is this condition valid?
                    //if (existingMembers.Any(x => functionalDependecy.LeftSideMembers.Contains(x)) &&
                    //    existingMembers.Any(x => functionalDependecy.RightSideMembers.Contains(x)))
                    //{
                    //    existingFunctionalDependency = true;
                    //    existingFunctionalDependencyMembers = existingMembers;
                    //    break;
                    //}
                }

                var decompositionStep = $"" +
                    $"{string.Join("", functionalDependecy.LeftSideMembers.Select(x => x.Name).ToArray())} " +
                    $"-> {string.Join("", functionalDependecy.RightSideMembers.Select(x => x.Name).ToArray())}\t";                    
                

                // TODO: dont negate if, make it if == true
                if (!existingFunctionalDependency)
                {
                    var membersToAdd = new List<Member>();
                    membersToAdd.AddRange(functionalDependecy.LeftSideMembers);
                    membersToAdd.AddRange(functionalDependecy.RightSideMembers);

                    // TODO: one line
                    decompositionStep += $"p := {ListOfListOfMembersToString(result)}";
                    decompositionStep += $" Union {ListOfMembersToString(membersToAdd)}";

                    result.Add(membersToAdd);
                }
                else
                {
                    decompositionStep += $"" +
                        $"{ListOfMembersToString(functionalDependecy.LeftSideMembers, parentheses: false) + ListOfMembersToString(functionalDependecy.RightSideMembers, parentheses: false) }" +
                        $" is contained in " +
                        $"{ListOfMembersToString(existingFunctionalDependencyMembers, parentheses: false)}";
                }

                decompositionResult.Steps.Add(decompositionStep);
            }

            // TODO: check if any key is in list, if not add any key
            // (c) Ako ključ od R nije podkljuc RI(Ri e p), tada
            var keys = relation.Keys;
            var keyExistsInResult = false;

            foreach (var members in result)
            {
                //if (keyExistsInResult)
                //    break;

                foreach (var key in keys)
                {
                    var keyMembers = key.KeyMembers.Select(x => x.Member);
                    // Does every member from functionalDependency exist in resultMembers
                    if (keyMembers.All(x => members.Contains(x)))
                    {
                        var decompositionStep = $"Key {ListOfMembersToString(keyMembers, parentheses: false)}"+
                            $" already exists in {ListOfMembersToString(members, parentheses: false)}";
                        decompositionResult.Steps.Add(decompositionStep);
                        keyExistsInResult = true;                        
                        //break;
                    }
                }
            }

            // TODO: uncomment
            // if key doesnt exist in the result, add any of the keys to the list
            if (!keyExistsInResult)
            {
                decompositionResult.Steps.Add("No key exists in the result, so we add a random key to the result");

                var randomIndex = new Random().Next(keys.Count);
                var randomKey = keys.ToArray()[randomIndex];
                result.Add(randomKey.KeyMembers.Select(x => x.Member).ToList());

                var keyStep = $"p := " +
                    $"{ListOfListOfMembersToString(result)} Union " +
                    $"{ListOfMembersToString(randomKey.KeyMembers.Select(x => x.Member))}";
                decompositionResult.Steps.Add(keyStep);
            }

            decompositionResult.Steps.Add($"Result: p := {ListOfListOfMembersToString(result)}");
            var test = 5;

        }        

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
        

    }

}
