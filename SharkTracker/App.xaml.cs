#nullable enable

using SharkTracker.Data;
using SharkTracker.Data.Models;

namespace SharkTracker;

public partial class App : Application
{

    public static Repository<Deck>? DeckRepo { get; private set; }

    public static Repository<Matchup>? MatchupRepo { get; private set; }

    public static List<Card>? AllCards { get; private set; }

    public App(Repository<Deck> deckRepo, Repository<Matchup> matchupRepo)
    {

        InitializeComponent();

        MainPage = new MainPage();
        DeckRepo = deckRepo;
        MatchupRepo = matchupRepo;
    }

    public async static Task<List<Card>> GetAllCards()
    {
        if (AllCards is null || !AllCards.Any())
        {
            AllCards = await RiotDownloader.GetAllCardsData();
        }

        return AllCards;
    }
}
