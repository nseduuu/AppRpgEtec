namespace AppRpgEtec
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void TapMenuDisputas(object sender, TappedEventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.Disputas.ListagemView());
        }

        private async void TapMenuPersonagens(object sender, TappedEventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.Personagens.ListagemView());
        }
    }

}
