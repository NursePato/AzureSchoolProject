using GameStore.Models;
using System.Text.Json;

namespace GameStore.Services
{
  public class GameService
  {
    private readonly string _filePath;
    private readonly string? _apiKey; //Compiler warning: CS8618 Non-nullable property 'ApiKey' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    public GameService(IConfiguration configuration)
    {
      _filePath = Path.Combine(Directory.GetCurrentDirectory(), "games.json");
      _apiKey = configuration["FakeApiKey"] ?? throw new Exception("API key not found"); // Simulating that it's coming from Key Vault & adding fallback
    }
    //private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "games.json");

    public List<Game> GetAllGames()
    {
      var jsonData = File.ReadAllText(_filePath);
      var gameList = JsonSerializer.Deserialize<List<Game>>(jsonData) ?? new List<Game>();
      return gameList;
    }
  }
}
