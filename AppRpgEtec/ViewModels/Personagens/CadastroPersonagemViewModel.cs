using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{

    [QueryProperty("PersonagemSelecionadoId", "pId")]

    public class CadastroPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;

        public CadastroPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            _ = ObterClasses();

            SalvarCommand = new Command(async () => { await SalvarPersonagem(); });

            CancelarCommand = new Command(async => { CancelarCadastro(); });
        }

        #region Propriedades

        private int id;
        private string nome;
        private int pontosVida;
        private int forca;
        private int defesa;
        private int inteligencia;
        private int disputas;
        private int vitorias;
        private int derrotas;
        private ObservableCollection<TipoClasse> listaTiposClasse;
        private TipoClasse tipoClasseSelecionado;
        private string personagemSelecionadoId;


        public int Id 
        { 
            get => id;
            set 
            { 
                id = value;
                OnPropertyChanged(nameof(Id));
            } 
        }
        public string Nome 
        { 
            get => nome; 
            set
            { 
                nome = value;
                OnPropertyChanged(nameof(Nome)); 
            }
        }
        public int PontosVida 
        { 
            get => pontosVida; 
            set
            { 
                pontosVida = value; 
                OnPropertyChanged(nameof(PontosVida)); 
            } 
        }
        public int Forca 
        { 
            get => forca; 
            set 
            { 
                forca = value; 
                OnPropertyChanged(nameof(Forca)); 
            } 
        }
        public int Defesa 
        { get => defesa; 
            set 
            { 
                defesa = value; 
                OnPropertyChanged(nameof(Defesa)); 
            } 
        }
        public int Inteligencia 
        { 
            get => inteligencia; 
            set 
            { 
                inteligencia = value; 
                OnPropertyChanged(nameof(Inteligencia)); 
            } 
        }
        public int Disputas 
        { 
            get => disputas; 
            set 
            { 
                disputas = value; 
                OnPropertyChanged(nameof(Disputas)); 
            } 
        }
        public int Vitorias 
        { 
            get => vitorias; 
            set 
            { 
                vitorias = value; 
                OnPropertyChanged(nameof(Vitorias)); 
            } 
        }
        public int Derrotas 
        { 
            get => derrotas; 
            set 
            { 
                derrotas = value; 
                OnPropertyChanged(nameof(Derrotas)); 
            } 
        }

        public string PersonagemSelecionadoId
        {
            get => PersonagemSelecionadoId;
            set
            {
                if (value != null)
                {
                    personagemSelecionadoId = Uri.UnescapeDataString(value);
                    CarregarPersonagem();
                }

            }
        }

        public ObservableCollection<TipoClasse> ListaTiposClasse
        {
            get { return listaTiposClasse; }
            set
            {
                if (value != null)
                {
                    listaTiposClasse = value;
                    OnPropertyChanged(nameof(ListaTiposClasse));
                }
            }
        }

        public TipoClasse TipoClasseSelecionado
        {
            get { return tipoClasseSelecionado; }
            set
            {
                if (value != null)
                {
                    tipoClasseSelecionado = value;
                    OnPropertyChanged(nameof(ListaTiposClasse));
                }
            }
        }
        #endregion

        #region methods
        public async Task ObterClasses()
        {
            try
            {
                listaTiposClasse = new ObservableCollection<TipoClasse>();
                ListaTiposClasse.Add(new TipoClasse() { Id = 1, Descricao = "Cavaleiro" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 2, Descricao = "Mago" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 3, Descricao = "Clerigo" });
                OnPropertyChanged(nameof(ListaTiposClasse));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                        .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public ICommand SalvarCommand { get; }

        public ICommand CancelarCommand { get; }

        public async Task SalvarPersonagem() 
        {
            try
            {
                Personagem model = new Personagem()
                {
                    Nome = nome,
                    PontosVida = pontosVida,
                    Defesa = defesa,
                    Derrotas = derrotas,
                    Disputas = disputas,
                    Forca = forca,
                    Inteligencia = inteligencia,
                    Vitorias = vitorias,
                    Id = id,
                    Classe = (ClasseEnum)tipoClasseSelecionado.Id
                };
                if (model.Id == 0)
                    await pService.PostPersonagemAsync(model);
                else
                    await pService.PutPersonagemAsync(model); 

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvos com sucesso!", "OK");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "OK");
            }
        }

        public async void CarregarPersonagem() 
        {
            try 
            {
                Personagem p = await pService.GetPersonagemAsync(int.Parse(PersonagemSelecionadoId));

                Nome = p.Nome;
                PontosVida = p.PontosVida;
                Defesa = p.Defesa;
                Derrotas = p.Derrotas;
                Disputas = p.Disputas;
                Forca = p.Forca;
                Inteligencia = p.Inteligencia;
                Vitorias = p.Vitorias;
                Id = p.Id;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private async void CancelarCadastro() 
        {
            await Shell.Current.GoToAsync("..");
        }
        #endregion
    }
}