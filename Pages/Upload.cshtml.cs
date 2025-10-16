using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using app_Teksan_ds502.Services;

namespace app_Teksan_ds502.Pages
{
    public class UploadModel : PageModel
    {
        private readonly AzureStorageService _azureStorageService;

        public UploadModel(AzureStorageService azureStorageService)
        {
            _azureStorageService = azureStorageService;
        }

        [BindProperty]
        public IFormFile? Archivo { get; set; }

        public string? Mensaje { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Archivo == null || Archivo.Length == 0)
            {
                Mensaje = "⚠️ Por favor selecciona un archivo para subir.";
                return Page();
            }

            string nombreArchivo = Archivo.FileName;
            string rutaTemporal = Path.GetTempFileName();

            using (var stream = new FileStream(rutaTemporal, FileMode.Create))
            {
                await Archivo.CopyToAsync(stream);
            }

            string urlArchivo = await _azureStorageService.UploadFileAsync(rutaTemporal, nombreArchivo);

            System.IO.File.Delete(rutaTemporal);

            Mensaje = $"✅ Archivo subido correctamente: <a href='{urlArchivo}' target='_blank'>{urlArchivo}</a>";

            return Page();
        }
    }
}
