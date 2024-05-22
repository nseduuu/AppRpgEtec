using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using Azure.Storage.Blobs;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{
    class ImagemUsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;
        private static string conexaoAzureStorage = "";
        private static string container = "arquivos";

        public ImagemUsuarioViewModel() 
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);

            FotografarCommand = new Command(Fotografar);
            SalvarImagemCommand = new Command(SalvarImagemAzure);
            AbrirGaleriaCommand = new Command(AbrirGaleria);

            CarregarUsuarioAzure();
        }

        public ICommand FotografarCommand { get; }
        public ICommand SalvarImagemCommand { get; }
        public ICommand AbrirGaleriaCommand { get; }

        private ImageSource fonteImagem;

        public ImageSource FonteImagem 
        {
            get { return fonteImagem; } 
            set 
            {  
                fonteImagem = value;
                OnPropertyChanged(nameof(FonteImagem));
            }
        }

        private byte[] foto;

        public byte[] Foto 
        { 
            get => foto;
            set
            {
                foto = value;
                OnPropertyChanged(nameof(Foto));
            } 
        }

        public async void Fotografar() 
        {
            try 
            {
                if (MediaPicker.Default.IsCaptureSupported) 
                {
                    FileResult photo =  await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        using (Stream sourceStream = await photo.OpenReadAsync())
                        {
                            using (MemoryStream ms = new MemoryStream()) 
                            {
                                await sourceStream.CopyToAsync(ms);

                                Foto = ms.ToArray();

                                FonteImagem = ImageSource.FromStream(() => new MemoryStream());
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async void SalvarImagemAzure() 
        {
            try 
            {
                Usuario u = new Usuario();
                u.Foto = foto;
                u.Id = Preferences.Get("UsuarioId", 0);

                string fileName = $"{u.Id}.jpg";

                var blobClient = new BlobClient(conexaoAzureStorage, container, fileName);

                if (blobClient.Exists())
                    blobClient.Delete();

                using (var stream = new MemoryStream(u.Foto)) 
                {
                    blobClient.Upload(stream);
                }

                await Application.Current.MainPage.DisplayAlert("Ops", " Detalhes: ", "Ok");
                await App.Current.MainPage.Navigation.PopAsync();

            }
            catch (Exception ex) 
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async void AbrirGaleria() 
        {
            if (MediaPicker.Default.IsCaptureSupported) 
            {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null) 
                {
                    using (Stream sourceStream = await photo.OpenReadAsync());
                }
            }
        }

        public async void CarregarUsuarioAzure() 
        {
            try 
            {
                int usuarioId = Preferences.Get("UsuarioId", 0);
                string filename = $"{usuarioId}.jpg";

                var blobclient = new BlobClient(conexaoAzureStorage, container, filename);
                Byte[] fileBytes;

                using (MemoryStream ms = new MemoryStream()) 
                {
                    blobclient.OpenRead().CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                Foto = fileBytes;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

    }
}
