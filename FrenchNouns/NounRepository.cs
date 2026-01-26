using System.Globalization;
using System.Text;

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
        Constants.Amour,
        Constants.Animal,
        Constants.Avion,
        Constants.Artiste,
        Constants.Adresse,
        Constants.Annee,
        Constants.Avenir
    };

    public static readonly IReadOnlyList<string> B = new[]
    {
        Constants.Banane,
        Constants.Banque,
        Constants.Bateau,
        Constants.Bebe,
        Constants.Bruit,
        Constants.Bureau,
        Constants.Bijou,
        Constants.Blouse,
        Constants.Bougie,
        Constants.Ballon,
        Constants.Bus,
        Constants.Boite,
        Constants.Boue
    };

    public static readonly IReadOnlyList<string> C = new[]
    {
        Constants.Cafe,
        Constants.Chambre,
        Constants.Chat,
        Constants.Cheval,
        Constants.Chocolat,
        Constants.Chose,
        Constants.Cinema,
        Constants.Confiture,
        Constants.Consonne,
        Constants.Cle,
        Constants.Ciel,
        Constants.Colere,
        Constants.Concours,
        Constants.Chanteur,
        Constants.Chanteuse,
        Constants.Chemise,
        Constants.Cours,
        Constants.Chiffre,
        Constants.Chaussure,
    };

    public static readonly IReadOnlyList<string> D = new[]
    {
        Constants.Departement,
        Constants.Dent,
        Constants.Devoir,
        Constants.Dimanche,
        Constants.Docteur,
        Constants.Douleur,
        Constants.Drapeau,
        Constants.Danse,
        Constants.Dessin,
        Constants.Distance
    };

    public static readonly IReadOnlyList<string> E = new[]
    {
        Constants.Eau,
        Constants.Ecole,
        Constants.Enfant,
        Constants.Espece,
        Constants.Etoile,
        Constants.Expression,
        Constants.Etagere,
        Constants.Energie,
        Constants.Evenement,
        Constants.Erreur,
    };

    public static readonly IReadOnlyList<string> F = new[]
    {
        Constants.Famille,
        Constants.Feu,
        Constants.Femme,
        Constants.Fille,
        Constants.Film,
        Constants.Foret,
        Constants.Frere,
        Constants.Fleur,
        Constants.Fromage,
        Constants.Fenetre,
    };

    public static readonly IReadOnlyList<string> G = new[]
    {
        Constants.Gare,
        Constants.Garcon,
        Constants.Gateau,
        Constants.Glace,
        Constants.Guitare,
        Constants.Genre,
        Constants.Garage,
        Constants.Gomme,
        Constants.Graine,
    };

    public static readonly IReadOnlyList<string> H = new[]
    {
        Constants.Herbe,
        Constants.Heure,
        Constants.Histoire,
        Constants.Homme,
        Constants.Hopital,
    };

    public static readonly IReadOnlyList<string> I = new[]
    {
        Constants.Idee,
        Constants.Image,
        Constants.Information,
        Constants.Ile,
        Constants.Insecte,
        Constants.Invite,
    };

    public static readonly IReadOnlyList<string> J = new[]
    {
        Constants.Jambe,
        Constants.Jardin,
        Constants.Jour,
        Constants.Jouet,
        Constants.Journal,
        Constants.Jupe,
    };

    public static readonly IReadOnlyList<string> K = new[]
    {
        Constants.Kangourou,
        Constants.Kayak,
        Constants.Kilo,
        Constants.Kit,
        Constants.Kiosque,
    };

    public static readonly IReadOnlyList<string> L = new[]
    {
        Constants.Lampe,
        Constants.Langue,
        Constants.Lecon,
        Constants.Lettre,
        Constants.Lit,
        Constants.Livre,
        Constants.Lune,
    };

    public static readonly IReadOnlyList<string> M = new[]
    {
        Constants.Maison,
        Constants.Main,
        Constants.Marche,
        Constants.Matin,
        Constants.Matiere,
        Constants.Mere,
        Constants.Mot,
        Constants.Musique,
        Constants.Monde,
    };

    public static readonly IReadOnlyList<string> N = new[]
    {
        Constants.Nature,
        Constants.Neige,
        Constants.Nez,
        Constants.Nom,
        Constants.Nuit,
        Constants.Numero,
    };

    public static readonly IReadOnlyList<string> O = new[]
    {
        Constants.Oeuf,
        Constants.Oiseau,
        Constants.Orange,
        Constants.Organisme,
        Constants.Ordinateur,
        Constants.Outil,
    };

    public static readonly IReadOnlyList<string> P = new[]
    {
        Constants.Pain,
        Constants.Parc,
        Constants.Pere,
        Constants.Personne,
        Constants.Phrase,
        Constants.Porte,
        Constants.Poulet,
        Constants.Professeur,
        Constants.Pomme,
    };

    public static readonly IReadOnlyList<string> Q = new[]
    {
        Constants.Quartier,
        Constants.Question,
        Constants.Queue,
        Constants.Quiche,
        Constants.Quinine,
    };

    public static readonly IReadOnlyList<string> R = new[]
    {
        Constants.Restaurant,
        Constants.Reve,
        Constants.Robe,
        Constants.Roi,
        Constants.Rue,
        Constants.Raison,
    };

    public static readonly IReadOnlyList<string> S = new[]
    {
        Constants.Salle,
        Constants.Semaine,
        Constants.Soeur,
        Constants.Soleil,
        Constants.Sport,
        Constants.Stylo,
        Constants.Supermarche,
        Constants.Sac,
    };

    public static readonly IReadOnlyList<string> T = new[]
    {
        Constants.Table,
        Constants.Television,
        Constants.Temps,
        Constants.The,
        Constants.Train,
        Constants.Travail,
    };

    public static readonly IReadOnlyList<string> U = new[]
    {
        Constants.Univers,
        Constants.Universite,
        Constants.Usage,
        Constants.Urgence,
        Constants.Usine,
    };

    public static readonly IReadOnlyList<string> V = new[]
    {
        Constants.Verre,
        Constants.Vent,
        Constants.Ville,
        Constants.Voiture,
        Constants.Voyelle,
        Constants.Viande,
        Constants.Visage,
    };

    public static readonly IReadOnlyList<string> W = new[]
    {
        Constants.Wagon,
        Constants.Web,
        Constants.Weekend,
        Constants.Whisky,
        Constants.Wifi,
    };

    public static readonly IReadOnlyList<string> X = new[]
    {
        Constants.Xenon,
        Constants.Xenophobie,
        Constants.Xeres,
        Constants.Xylem,
        Constants.Xylophone,
    };

    public static readonly IReadOnlyList<string> Y = new[]
    {
        Constants.Yak,
        Constants.Yaourt,
        Constants.Yacht,
        Constants.Yeux,
        Constants.Yoga,
    };

    public static readonly IReadOnlyList<string> Z = new[]
    {
        Constants.Zebre,
        Constants.Zeste,
        Constants.Zone,
        Constants.Zoo,
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
        // Defensive default
        _cache[noun] = new WordMetadata();

        if (string.IsNullOrWhiteSpace(noun))
            return;

        // First letter folder (A, B, C, ...)
        char firstLetter = RemoveDiacritics(noun[0]).ToString().ToUpperInvariant()[0];

        // Base folder (runtime)
        string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BaseFolderName);

        if (!Directory.Exists(baseDir))
        {
            // Go up from bin/... to project root
            string projectRoot = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..")
            );

            baseDir = Path.Combine(projectRoot, BaseFolderName);
        }

        if (!Directory.Exists(baseDir))
            return;

        // Sentences/A/arbre.json
        string nounFolder = Path.Combine(baseDir, firstLetter.ToString());
        string filePath = Path.Combine(nounFolder, $"{noun}{FileExtension}");

        if (!File.Exists(filePath))
            return;

        try
        {
            string jsonContent = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var metadata = JsonSerializer.Deserialize<WordMetadata>(jsonContent, options)
                           ?? new WordMetadata();

            metadata.Description ??= string.Empty;

            metadata.Sentences = (metadata.Sentences ?? new List<string>())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToList();

            _cache[noun] = metadata;
        }
        catch
        {
            // keep default empty metadata
            _cache[noun] = new WordMetadata();
        }
    }
    
    private static char RemoveDiacritics(char c)
    {
        string normalized = c.ToString().Normalize(NormalizationForm.FormD);
        foreach (char ch in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                return ch;
        }
        return c;
    }
}
