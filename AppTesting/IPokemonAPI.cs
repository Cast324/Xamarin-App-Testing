using Refit;
using System;
using System.Threading.Tasks;

namespace AppTesting
{
    public interface IPokemonAPI
    {
        [Get("/{pokemon}")]
        Task<PokemonModel> GetPokemon(string pokemon);
    }
}
