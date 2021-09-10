using MvvmHelpers;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppTesting.ViewModels
{
    class MainPageViewModel: BaseViewModel
    {
        private PokemonModel _Pokemon;
        public PokemonModel Pokemon
        {
            get => _Pokemon;
            set => SetProperty(ref _Pokemon, value);
        }

        private string _Query;
        public string Query
        {
            get => _Query;
            set => SetProperty(ref _Query, value);
        }

        private ImageSource _PokemonImage;
        public ImageSource PokemonImage
        {
            get => _PokemonImage;
            set => SetProperty(ref _PokemonImage, value);
        }

        public ICommand GetPokemonCommand { get; }

        public MainPageViewModel()
        {
            GetPokemonCommand = new MvvmHelpers.Commands.AsyncCommand(GetPokemon);
        }
        private async Task GetPokemon()
        {
            var fallbackPolicy = Policy<PokemonModel>.Handle<ApiException>(ex => ex.StatusCode == HttpStatusCode.NotFound).FallbackAsync(HandleException);
            Pokemon = await fallbackPolicy.ExecuteAsync(GetPokemonAsync);
            PokemonImage = ImageSource.FromUri(new Uri(Pokemon.sprites.front_default));
        }

        private async Task<PokemonModel> GetPokemonAsync()
        {
            var apiResponse = RestService.For<IPokemonAPI>("https://pokeapi.co/api/v2/pokemon");
            return await apiResponse.GetPokemon(Query);
        }

        private Task<PokemonModel> HandleException(CancellationToken arg)
        {
            Console.WriteLine("404 was returned check name of pokemon!");
            return Task.FromResult<PokemonModel>(default);
        }
    }
}
