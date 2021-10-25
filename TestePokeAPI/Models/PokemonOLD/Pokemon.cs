using System;
using System.Collections.Generic;
using System.Linq;

namespace TestePokeAPI.Models.Pokemon
{
    public class Pokemon
    {
        public ApiResource<Characteristic>[] Characteristics
        {
            get;
            internal set;
        }


        [JsonPropertyName("evolution_chain")]
        public ApiResource<EvolutionChain> EvolutionChain
        {
            get;
            internal set;
        }

        public PokemonSprites Sprites
        {
            get;
            internal set;
        }

    }

    /// <summary>
    /// NOTE: some props can be null, fall back on male, non-shiny (if all shinies are null) values!
    /// </summary>
    public struct PokemonSprites
    {
        //! NOTE: some props can be null, fall back on male, non-shiny (if all shinies are null) values!

        [JsonPropertyName("back_female")]
        public string BackFemale
        {
            get;
            internal set;
        }
        [JsonPropertyName("back_shiny_female")]
        public string BackShinyFemale
        {
            get;
            internal set;
        }
        [JsonPropertyName("back_default")]
        public string BackMale
        {
            get;
            internal set;
        }
        [JsonPropertyName("front_female")]
        public string FrontFemale
        {
            get;
            internal set;
        }
        [JsonPropertyName("front_shiny_female")]
        public string FrontShinyFemale
        {
            get;
            internal set;
        }
        [JsonPropertyName("back_shiny")]
        public string BackShinyMale
        {
            get;
            internal set;
        }
        [JsonPropertyName("front_default")]
        public string FrontMale
        {
            get;
            internal set;
        }
        [JsonPropertyName("front_shiny")]
        public string FrontShinyMale
        {
            get;
            internal set;
        }
    }


    public class Characteristic : ApiObject
    {

        [JsonPropertyName("highest_stat")]
        public NamedApiResource<Stat> HighestStat
        {
            get;
            internal set;
        }

        [JsonPropertyName("gene_modulo")]
        public int GeneModulo
        {
            get;
            internal set;
        }

        [JsonPropertyName("possible_values")]
        public int[] PossibleValues
        {
            get;
            internal set;
        }

        public Description[] Descriptions
        {
            get;
            internal set;
        }
    }

    public struct Description
    {
        /// <summary>
        /// The localized description for an <see cref="ApiResource{T}" /> in a specific langauge.
        /// </summary>
        [JsonPropertyName("description")]
        public string Text
        {
            get;
            internal set;
        }

        /// <summary>
        /// The language this description is in.
        /// </summary>
        public NamedApiResource<PokeAPI.Language> Language
        {
            get;
            internal set;
        }
    }



    public class Stat : NamedApiObject
    {
        [JsonPropertyName("game_index")]
        public int GameIndex
        {
            get;
            internal set;
        }

        [JsonPropertyName("is_battle_only")]
        public bool IsBattleOnly
        {
            get;
            internal set;
        }

        public ApiResource<Characteristic>[] Characteristics
        {
            get;
            internal set;
        }

    }

    public class EvolutionChain : ApiObject
    {
        [JsonPropertyName("baby_trigger_item")]
        public NamedApiResource<Item> BabyTriggerItem
        {
            get;
            internal set;
        }

        public ChainLink Chain
        {
            get;
            internal set;
        }
    }

    public struct ChainLink
    {
        [JsonPropertyName("is_baby")]
        public bool IsBaby
        {
            get;
            internal set;
        }

        public NamedApiResource<PokemonSpecies> Species
        {
            get;
            internal set;
        }

        [JsonPropertyName("evolution_details")]
        public EvolutionDetail[] Details
        {
            get;
            internal set;
        }

        [JsonPropertyName("evolves_to")]
        public ChainLink[] EvolvesTo
        {
            get;
            internal set;
        }
    }

}