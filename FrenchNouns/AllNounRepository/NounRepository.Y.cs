using FrenchNouns.AllConstants;

namespace FrenchNouns.AllNounRepository;

public static partial class NounRepository
{
    public static readonly IReadOnlyList<string> Y = new[]
     {
        Constants.Yak,
        Constants.Yaourt,
        Constants.Yacht,
        Constants.Yeux,
        Constants.Yoga,
        Constants.Yourte,
        Constants.Yole,
        Constants.Yucca,
        Constants.Yeti,
        Constants.Ylang,
        Constants.Yoyo,
        Constants.Youyou,
    };

    public static readonly IReadOnlyList<string> Y_Popular = new[]
    {
        Constants.Yaourt,
        Constants.Yacht,
        Constants.Yeux,
        Constants.Yoga,
        Constants.Yourte,
        Constants.Yucca,
        Constants.Yeti,
        Constants.Ylang,
        Constants.Yoyo,
        Constants.Yak,
        Constants.Yole,
    };
}
