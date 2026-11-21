using CardioPrevention.Models;
using CardioPrevention.Services;
using Microsoft.AspNetCore.Mvc;

namespace CardioPrevention.Controllers;

public class CardioController : Controller
{
    private readonly ICardioPredictionService _predictionService;
    private readonly ILogger<CardioController> _logger;

    public CardioController(
        ICardioPredictionService predictionService,
        ILogger<CardioController> logger)
    {
        _predictionService = predictionService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new CardioPredictionViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        CardioPredictionViewModel viewModel,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            viewModel.Result = await _predictionService.PredictAsync(
                viewModel.Input,
                cancellationToken);

            return View(viewModel);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Erreur pendant l'appel à l'API de prédiction cardiovasculaire.");

            viewModel.ErrorMessage =
                "Impossible d'obtenir une estimation pour le moment. "
                + "Vérifiez que l'API Python est en cours d'exécution.";

            return View(viewModel);
        }
    }
}