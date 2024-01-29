using Assets.Scripts.Models;
using Assets.Scripts.StatSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ResourceSystem;
using UnityEngine;

namespace Assets.Scripts.ConditionalMods.ValuesConditionalScripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Conditions/Values/LoveForMuffins")]
    public class LoveForMuffinsCondition : ConditionalModifier
    {
        public ProductionBuilding bakery;
        public ProductionBuilding bakeryII;
        public ModifiersContainer container;


        protected override void AddModifiers()
        {
            //modifiers.AddModifiers(container);
            var bakeryProd = GetBakeryProductions();

            foreach (var prod in bakeryProd)
            {
                prod.recipe.resources.Where(r => r.name == ResourceName.Gold)
                    .ToList()
                    .ForEach(r => r.AddGainModifier(
                        new ResourceModifier(
                            2,
                            Helpers.Enums.ModifierType.PercentAdd,
                            TransactionType.Production,
                            source
                        )
                     ));
            }
        }

        protected override bool CheckCondition()
        {
            var bakeryProd = GetBakeryProductions();
            var workers = bakeryProd.Sum(p => p.workers);

            return workers >= 4;
        }

        protected override void DeleteModifiers()
        {
            //modifiers.RemoveModifiers(container);
        }

        public IEnumerable<Production> GetBakeryProductions()
        {
            return gameProgress.production
                .Where(p => p.buildingGuid == bakery.Guid || p.buildingGuid == bakeryII.Guid);
        }
    }
}
