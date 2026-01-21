namespace FrenchNouns;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public static class NounRepository
{
    private static readonly Dictionary<string, WordMetadata> _cache = new();
    private static readonly Random _random = new();
    private const string BaseFolderName = "Sentences";
    private const string FileExtension = ".json";

    public static readonly IReadOnlyList<string> A = new[]
    {
        Constants.Aide,
        Constants.Ami,
        Constants.Amie,
        Constants.Appartement,
        Constants.Arbre,
        Constants.Amour
    };

    public static readonly IReadOnlyList<string> B = new[]
    {
        Constants.Banane,
        Constants.Banque,
        Constants.Bateau,
        Constants.Bebe,
        Constants.Bureau
    };

    public static readonly IReadOnlyList<string> C = new[]
    {
        Constants.Chambre,
        Constants.Chat,
        Constants.Cheval,
        Constants.Ciel,
        Constants.Cle
    };

    public static readonly IReadOnlyList<string> D = new[]
    {
        Constants.Departement,
        Constants.Dent,
        Constants.Dimanche,
        Constants.Docteur,
        Constants.Douleur
    };

    public static readonly IReadOnlyList<string> E = new[]
    {
        Constants.Eau,
        Constants.Ecole,
        Constants.Enfant,
        Constants.Espece,
        Constants.Etoile
    };

    public static readonly IReadOnlyList<string> F = new[]
    {
        Constants.Feu,
        Constants.Femme,
        Constants.Fille,
        Constants.Foret,
        Constants.Frere
    };

    public static readonly IReadOnlyList<string> G = new[]
    {
        Constants.Gare,
        Constants.Garcon,
        Constants.Gateau,
        Constants.Glace,
        Constants.Guitare
    };

    public static readonly IReadOnlyList<string> H = new[]
    {
        Constants.Herbe,
        Constants.Heure,
        Constants.Histoire,
        Constants.Homme,
        Constants.Hopital
    };

    public static readonly IReadOnlyList<string> I = new[]
    {
        Constants.Idee,
        Constants.Image,
        Constants.Ile,
        Constants.Insecte,
        Constants.Invite
    };

    public static readonly IReadOnlyList<string> J = new[]
    {
        Constants.Jambe,
        Constants.Jardin,
        Constants.Jour,
        Constants.Jouet,
        Constants.Jupe
    };

    public static readonly IReadOnlyList<string> K = new[]
    {
        Constants.Kangourou,
        Constants.Kayak,
        Constants.Kilo,
        Constants.Kit,
        Constants.Kiosque
    };

    public static readonly IReadOnlyList<string> L = new[]
    {
        Constants.Lampe,
        Constants.Langue,
        Constants.Lit,
        Constants.Livre,
        Constants.Lune
    };

    public static readonly IReadOnlyList<string> M = new[]
    {
        Constants.Maison,
        Constants.Main,
        Constants.Marche,
        Constants.Mere,
        Constants.Musique
    };

    public static readonly IReadOnlyList<string> N = new[]
    {
        Constants.Nature,
        Constants.Neige,
        Constants.Nez,
        Constants.Nom,
        Constants.Nuit
    };

    public static readonly IReadOnlyList<string> O = new[]
    {
        Constants.Oeuf,
        Constants.Oiseau,
        Constants.Orange,
        Constants.Organisme,
        Constants.Ordinateur
    };

    public static readonly IReadOnlyList<string> P = new[]
    {
        Constants.Pain,
        Constants.Pere,
        Constants.Personne,
        Constants.Porte,
        Constants.Poulet
    };

    public static readonly IReadOnlyList<string> Q = new[]
    {
        Constants.Quartier,
        Constants.Quiche,
        Constants.Quinine,
        Constants.Queue,
        Constants.Question
    };

    public static readonly IReadOnlyList<string> R = new[]
    {
        Constants.Restaurant,
        Constants.Reve,
        Constants.Robe,
        Constants.Roi,
        Constants.Rue
    };

    public static readonly IReadOnlyList<string> S = new[]
    {
        Constants.Salle,
        Constants.Soeur,
        Constants.Soleil,
        Constants.Sport,
        Constants.Stylo
    };

    public static readonly IReadOnlyList<string> T = new[]
    {
        Constants.Table,
        Constants.Television,
        Constants.Temps,
        Constants.Train,
        Constants.Travail
    };

    public static readonly IReadOnlyList<string> U = new[]
    {
        Constants.Univers,
        Constants.Universite,
        Constants.Usage,
        Constants.Urgence,
        Constants.Usine
    };

    public static readonly IReadOnlyList<string> V = new[]
    {
        Constants.Verre,
        Constants.Vent,
        Constants.Ville,
        Constants.Voiture,
        Constants.Visage
    };

    public static readonly IReadOnlyList<string> W = new[]
    {
        Constants.Wagon,
        Constants.Web,
        Constants.Weekend,
        Constants.Whisky,
        Constants.Wifi
    };

    public static readonly IReadOnlyList<string> X = new[]
    {
        Constants.Xenon,
        Constants.Xenophobie,
        Constants.Xeres,
        Constants.Xylem,
        Constants.Xylophone
    };

    public static readonly IReadOnlyList<string> Y = new[]
    {
        Constants.Yak,
        Constants.Yaourt,
        Constants.Yacht,
        Constants.Yeux,
        Constants.Yoga
    };

    public static readonly IReadOnlyList<string> Z = new[]
    {
        Constants.Zero,
        Constants.Zebre,
        Constants.Zeste,
        Constants.Zone,
        Constants.Zoo
    };

    // Super-list that contains every letter list
    public static readonly IReadOnlyList<string> All = A
        .Concat(B)
        .Concat(C)
        .Concat(D)
        .Concat(E)
        .Concat(F)
        .Concat(G)
        .Concat(H)
        .Concat(I)
        .Concat(J)
        .Concat(K)
        .Concat(L)
        .Concat(M)
        .Concat(N)
        .Concat(O)
        .Concat(P)
        .Concat(Q)
        .Concat(R)
        .Concat(S)
        .Concat(T)
        .Concat(U)
        .Concat(V)
        .Concat(W)
        .Concat(X)
        .Concat(Y)
        .Concat(Z)
        .ToArray();

    private static IReadOnlyDictionary<char, IReadOnlyList<string>> BuildLetterMap()
    {
        return new Dictionary<char, IReadOnlyList<string>>(26)
        {
            [Constants.A[0]] = A,
            [Constants.B[0]] = B,
            [Constants.C[0]] = C,
            [Constants.D[0]] = D,
            [Constants.E[0]] = E,
            [Constants.F[0]] = F,
            [Constants.G[0]] = G,
            [Constants.H[0]] = H,
            [Constants.I[0]] = I,
            [Constants.J[0]] = J,
            [Constants.K[0]] = K,
            [Constants.L[0]] = L,
            [Constants.M[0]] = M,
            [Constants.N[0]] = N,
            [Constants.O[0]] = O,
            [Constants.P[0]] = P,
            [Constants.Q[0]] = Q,
            [Constants.R[0]] = R,
            [Constants.S[0]] = S,
            [Constants.T[0]] = T,
            [Constants.U[0]] = U,
            [Constants.V[0]] = V,
            [Constants.W[0]] = W,
            [Constants.X[0]] = X,
            [Constants.Y[0]] = Y,
            [Constants.Z[0]] = Z
        };
    }

    public static bool TryGetRandomByLetter(char letter, out string? result)
    {
        result = null;
        var key = char.ToLowerInvariant(letter);

        if (!BuildLetterMap().TryGetValue(key, out var list) || list == null || list.Count == 0)
        {
            return false;
        }

        result = list[Random.Shared.Next(list.Count)];
        return true;
    }

    public static bool TryGetRandom(out string? result)
    {
        result = null;

        if (All is null || All.Count == 0)
        {
            return false;
        }

        result = All[Random.Shared.Next(All.Count)];
        return true;
    }

    public static List<string> GetSentencesForWord(string word, int count = 3)
    {
        if (string.IsNullOrWhiteSpace(word) || count <= 0)
            return new List<string>();

        // Extract the noun part (after the article)
        var parts = word.Split(Constants.Space, 2, StringSplitOptions.None);
        var noun = parts.Length > 1 ? parts[1] : parts[0];
        noun = noun.ToLower().Trim();

        if (!_cache.ContainsKey(noun))
        {
            LoadNounJson(noun);
        }

        if (_cache.TryGetValue(noun, out var metadata) && metadata?.Sentences != null && metadata.Sentences.Any())
        {
            return metadata.Sentences
                .OrderBy(_ => _random.Next())
                .Take(count)
                .ToList();
        }

        return new List<string>();
    }

    public static string GetDescriptionForWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return string.Empty;

        var parts = word.Split(Constants.Space, 2, StringSplitOptions.None);
        var noun = parts.Length > 1 ? parts[1] : parts[0];
        noun = noun.ToLower().Trim();

        if (!_cache.ContainsKey(noun))
            LoadNounJson(noun);

        if (_cache.TryGetValue(noun, out var metadata) && !string.IsNullOrWhiteSpace(metadata.Description))
            return metadata.Description.Trim();

        return string.Empty;
    }

    private static void LoadNounJson(string noun)
    {
        // Compute the base folder path
        string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BaseFolderName);

        if (!Directory.Exists(baseDir))
        {
            // Go up from bin/... to project root
            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            baseDir = Path.Combine(projectRoot, BaseFolderName);
        }

        if (!Directory.Exists(baseDir))
        {
            _cache[noun] = new WordMetadata();
            return;
        }

        // Search recursively for the noun JSON file
        string[] files = Directory.GetFiles(baseDir, $"{noun}{FileExtension}", SearchOption.AllDirectories);

        if (files.Length == 0)
        {
            _cache[noun] = new WordMetadata();
            return;
        }

        try
        {
            // Read the first matching file and deserialize into WordMetadata
            string jsonContent = File.ReadAllText(files[0]);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var metadata = JsonSerializer.Deserialize<WordMetadata>(jsonContent, options) ?? new WordMetadata();

            // Normalize example sentences
            metadata.Sentences = (metadata.Sentences ?? new List<string>())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToList();

            metadata.Description ??= string.Empty;

            _cache[noun] = metadata;
        }
        catch
        {
            _cache[noun] = new WordMetadata();
        }
    }
}
