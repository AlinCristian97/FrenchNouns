using FrenchNouns.AllConstants;

namespace FrenchNouns.AllNounRepository;

public static partial class NounRepository
{
    public static readonly IReadOnlyList<string> W = new[]
     {
        Constants.Wagon,
        Constants.Web,
        Constants.Weekend,
        Constants.Whisky,
        Constants.Wifi,
        Constants.Wok,
    };
}
