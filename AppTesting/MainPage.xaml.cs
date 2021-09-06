using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppTesting
{
    public partial class MainPage : ContentPage
    {
        public PokemonModel pokemon { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await GetPokemon();
            pokemonImage.Source = ImageSource.FromUri(new Uri("https://cdn.cnn.com/cnnnext/dam/assets/210226041421-02-pokemon-anniversary-design.jpg"));
        }

        async Task GetPokemon()
        {
            var fallbackPolicy = Policy<PokemonModel>.Handle<ApiException>(ex => ex.StatusCode == HttpStatusCode.NotFound).FallbackAsync(HandleException);
            pokemon = await fallbackPolicy.ExecuteAsync(GetPokemonAsync);
        }

        private async Task<PokemonModel> GetPokemonAsync()
        {
            var apiResponse = RestService.For<IPokemonAPI>("https://pokeapi.co/api/v2/pokemon");
            return await apiResponse.GetPokemon(pokeNameEntry.Text);
        }

        private Task<PokemonModel> HandleException(CancellationToken arg)
        {
            Console.WriteLine("404 was returned check name of pokemon!");
            return Task.FromResult<PokemonModel>(default);
        }
    }
}
