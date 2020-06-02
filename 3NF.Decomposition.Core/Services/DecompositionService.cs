using _3NF.Decomposition.Core.Dtos;
using _3NF.Decomposition.Core.Entities;
using _3NF.Decomposition.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3NF.Decomposition.Core.Services
{
    public class DecompositionService : IDecompositionService
    {
        private readonly IRepository _repo;

        public DecompositionService(IRepository repo)
        {
            _repo = repo;
        }
        
        public async Task<string> DecomposeToThirdNormalForm(int relationId)
        {
            var relation = await _repo.GetRelation(relationId);

            // (a) p: = {}
            var result = new List<List<Entities.Attribute>>();

            var resultMessages = new DecompositionResultDto();
            resultMessages.Steps.Add("p := {}");

            /* 
            Create anonymous object:
            {
                 Sequence: 1,
                 LeftSideAttributes: {
                    Attribute1, Attribute2... 
                },
                RightSideAttributes: {
                    Attribute1, Attribute2...
                }
            },
            {
                Sequence: 2,
                ...
            }
            */
            var fmin = relation.FminAttributes.ToList().GroupBy(
                x => x.Sequence,
                (key, value) => new
                {
                    Sequence = key,
                    LeftSideAttributes = value.Select(x => x.LeftSideAttribute).Distinct().ToList(),
                    RightSideAttributes = value.Select(x => x.RightSideAttribute).Distinct().ToList()
                }).OrderBy(x => x.Sequence);

            // (b) za svaku X->A e Fmin
            // go through each functional dependency
            foreach (var functionalDependency in fmin)
            {
                // temp variables that we will use to print the step
                bool existingFunctionalDependency = false;
                List<Entities.Attribute> existingFunctionalDependencyAttributes = new List<Entities.Attribute>();
                // go through list of attributes in result and check if they contain this functional dependency
                foreach (var resultAttributes in result)
                {
                    // Does every attribute from functionalDependency exist in resultAttributes
                    if (functionalDependency.LeftSideAttributes.All(x => resultAttributes.Contains(x)) &&
                        functionalDependency.RightSideAttributes.All(x => resultAttributes.Contains(x))
                    )
                    {
                        // this functional dependency already exists in the result, so we don't need to add
                        // it to the result, but store it to a temp variable so we can print this step
                        existingFunctionalDependency = true;
                        existingFunctionalDependencyAttributes = resultAttributes;
                        break;
                    }
                }

                // print: AB -> CD
                var decompositionStep = $"" +
                    $"{string.Join("", functionalDependency.LeftSideAttributes.Select(x => x.Name).ToArray())} " +
                    $"-> {string.Join("", functionalDependency.RightSideAttributes.Select(x => x.Name).ToArray())}  ";


                if (existingFunctionalDependency)
                {
                    // functional dependency already exists so print: AB is contained in ABC
                    decompositionStep += $"" +
                        $"{ListOfAttributesToString(functionalDependency.LeftSideAttributes, parentheses: false) + ListOfAttributesToString(functionalDependency.RightSideAttributes, parentheses: false) }" +
                        $" is contained in " +
                        $"{ListOfAttributesToString(existingFunctionalDependencyAttributes, parentheses: false)}";
                }
                else
                {
                    // functional dependecy doesn't exist, so print: {AB} Union {ABC}
                    var attributesToAdd = new List<Entities.Attribute>();
                    attributesToAdd.AddRange(functionalDependency.LeftSideAttributes);
                    attributesToAdd.AddRange(functionalDependency.RightSideAttributes);

                    decompositionStep += $"p := {ListOfListOfAttributesToString(result)} Union {ListOfAttributesToString(attributesToAdd)}";

                    result.Add(attributesToAdd);
                }

                // add the print text to list of steps
                resultMessages.Steps.Add(decompositionStep);
            }

            // check if any key is in list, if no key appears in the result 
            // pick a random one and add it to the result
            // (c) Ako ključ od R nije podkljuc RI(Ri e p), tada
            var keys = relation.Keys;
            var keyExistsInResult = false;

            foreach (var attributes in result)
            {
                foreach (var key in keys)
                {
                    var keyAttributes = key.KeyAttributes.Select(x => x.Attribute);
                    // Does every attribute from key exist in resultAttributes
                    if (keyAttributes.All(x => attributes.Contains(x)))
                    {
                        // if the key already exists in the result, print: Key {AB} already exists in {ABC}
                        // raise the flag, keyExistsInResult to true
                        var decompositionStep = $"Key {ListOfAttributesToString(keyAttributes, parentheses: false)}" +
                            $" already exists in {ListOfAttributesToString(attributes, parentheses: false)}";
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
                var keyAttributes = randomKey.KeyAttributes.Select(x => x.Attribute).ToList();                

                // print: p := {AB, CD, EF} Union {ABC}
                var keyStep = $"p := " +
                    $"{ListOfListOfAttributesToString(result)} Union " +
                    $"{ListOfAttributesToString(randomKey.KeyAttributes.Select(x => x.Attribute))}";                
                resultMessages.Steps.Add(keyStep);

                result.Add(keyAttributes);
            }

            // print final result: Result: p := {AB, CD, EF}..
            resultMessages.Steps.Add($"Result: p := {ListOfListOfAttributesToString(result)}");

            return string.Join("\n", resultMessages.Steps);
        }

        #region ToString methods

        private string ListOfListOfAttributesToString(List<List<Entities.Attribute>> attributes)
        {
            // FORM {AB, BC, D} from { {Attribute, Attribute}, {Attribute, Attribute}, {Attribute} }
            if (attributes.Count == 0)
                return "{ }";

            var result = string.Empty;

            foreach (var attributeList in attributes)
            {
                result += $"{string.Join("", attributeList.Select(x => x.Name).ToArray())}, ";
            }

            if (!string.IsNullOrEmpty(result))
                result = result.Remove(result.Length - 2);

            return $"{{{result}}}";
        }

        private string ListOfAttributesToString(IEnumerable<Entities.Attribute> attributes, bool parentheses = true)
        {
            // FORM {AB} from {Attribute, Attribute}
            if (parentheses)
                return $"{{{string.Join("", attributes.Select(x => x.Name).ToArray())}}}";

            // FORM AB from {Attribute, Attribute}
            return $"{string.Join("", attributes.Select(x => x.Name).ToArray())}";
        }

        #endregion
    }
}
