using System.Globalization;
using System.Text;

namespace FrenchNouns.AllNounRepository;

using FrenchNouns.AllConstants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public static partial class NounRepository
{
    private static readonly Dictionary<string, WordMetadata> _cache = new();
    private static readonly Random _random = new();
    private const string BaseFolderName = "Sentences";
    private const string FileExtension = ".json";

    // used for plural-only nouns like "les gens", to check against and apply custom rules
    public static readonly IReadOnlyList<string> PluralOnlyNouns = new[]
    {
        // A
        Constants.Plural_Alentours,

        // C
        Constants.Plural_Condoleances,

        // F
        Constants.Plural_Frais,
        Constants.Plural_Fesses,

        // G
        Constants.Plural_Gens,

        // J
        Constants.Plural_Jambieres,

        // L
        Constants.Plural_Lunettes,

        // S
        Constants.Plural_Soldes,

        // T
        Constants.Plural_Toilettes,

        // V
        Constants.Plural_Vacances,
        Constants.Plural_Vetements,
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
