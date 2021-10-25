using System;
using System.Collections.Generic;
using System.Linq;

namespace TestePokeAPI.Models.Pokemon
{
    public abstract class ApiObject
    {
        /// <summary>
        /// The identifier for this <see cref="ApiObject" />.
        /// </summary>
        public int ID
        {
            get;
            internal set;
        }
    }
    public abstract class NamedApiObject : ApiObject
    {
        /// <summary>
        /// The name for this <see cref="NamedApiObject" />.
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// The localized names for this <see cref="NamedApiObject" />.
        /// </summary>
        public ResourceName[] Names
        {
            get;
            internal set;
        }
    }

    public struct ResourceName
    {
        /// <summary>
        /// The localized name for an <see cref="ApiResource{T}" /> in a specific langauge.
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// The language this name is in.
        /// </summary>
        public NamedApiResource<Language> Language
        {
            get;
            internal set;
        }
    }
}
