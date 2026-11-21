using System.Net.Http.Json;
using CardioPrevention.Models;

namespace CardioPrevention.Services;

public interface ICardioPredictionService
{
    Task<CardioPredictionResponse> PredictAsync(
        CardioPredictionRequest request,
        CancellationToken cancellationToken = default);
}

public class CardioPredictionService : ICardioPredictionService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CardioPredictionService> _logger;

    public CardioPredictionService(
        HttpClient httpClient,
        ILogger<CardioPredictionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CardioPredictionResponse> PredictAsync(
        CardioPredictionRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                "predict",
                request,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync(
                    cancellationToken);

                _logger.LogWarning(
                    "L'API de prédiction a renvoyé le code {StatusCode}. Détail : {ErrorContent}",
                    response.StatusCode,
                    errorContent);

                throw new HttpRequestException(
                    $"L'API de prédiction a renvoyé le code {(int)response.StatusCode}.");
            }

            CardioPredictionResponse? result =
                await response.Content.ReadFromJsonAsync<CardioPredictionResponse>(
                    cancellationToken: cancellationToken);

            if (result is null)
            {
                throw new InvalidOperationException(
                    "L'API de prédiction a renvoyé une réponse vide.");
            }

            return result;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(
                exception,
                "Impossible de joindre l'API Python de prédiction.");

            throw new InvalidOperationException(
                "Le service de prédiction est indisponible. "
                + "Vérifiez que l'API Python est démarrée sur http://127.0.0.1:8000.",
                exception);
        }
    }
}