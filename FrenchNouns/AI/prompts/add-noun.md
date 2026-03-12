# Add a French Noun

## When to use

Use this prompt whenever the user provides one or more French nouns to add to the project.
The user may supply a bare noun (e.g. `arbre`), or an "article + noun" form (e.g. `un arbre`).
For plural-only nouns the user will say so explicitly or use `des` (e.g. `des alentours`).

---

## Step 1 — Determine the article

| Article | Gender / Number |
|---------|-----------------|
| `un`    | Masculine singular |
| `une`   | Feminine singular |
| `des`   | Plural-only (always plural) |

If the user provides only the bare noun, you **must** determine the correct French grammatical gender yourself.
If the noun has a special diacritical form (e.g. `arbre` → `arbre`, `âge` → `âge`), use the accented version in the string value.

---

## Step 2 — Add to `Constants.cs`

### Singular nouns (`un` / `une`)

1. Open `FrenchNouns\Constants.cs`.
2. Find the `#region "X" Nouns` block that matches the **first letter** of the constant name (use the ASCII letter, not the accented character — e.g. `Élan` → region `"E"`).
3. Add a new `public const string` line **at the end** of the region, just before the `#endregion` (or before the `//` separator if plural-only entries exist for that letter).
4. Follow the naming and value pattern exactly:

```csharp
public const string Arbre = MasculineArticle + Space + "arbre";
public const string Araignee = FeminineArticle + Space + "araignée";
```

- **Constant name**: PascalCase, no accents, no hyphens. Compound words use concatenation (e.g. `GrandMere`, `BricABrac`, `PetitDejeuner`, `ApresMidi`).
- **String value**: lowercase noun with accents/diacritics preserved.
- If the same noun has distinct meanings in different genders, use a suffix: `Cote_Side` / `Cote_Rib`, `Mode_Masculine` / `Mode_Feminine`, etc.

### Plural-only nouns (`des`)

1. Add the constant inside the `#region Plural-only Nouns` block, under the matching letter comment.
2. Use the `PluralArticle` constant:

```csharp
public const string Plural_Lunettes = PluralArticle + Space + "lunettes";
```

- **Constant name**: `Plural_` prefix + PascalCase noun (no accents).

---

## Step 3 — Add to `NounRepository.cs`

1. Open `FrenchNouns\NounRepository.cs`.
2. Find the letter list that matches the noun's first letter (e.g. `public static readonly IReadOnlyList<string> A`).
3. **Singular nouns**: append `Constants.YourNoun,` at the end of the regular entries (before the `//` separator line if one exists for plural entries in that letter).
4. **Plural-only nouns**: append `Constants.Plural_YourNoun,` after the `//` separator in the letter list, **and also** add it to the `PluralOnlyNouns` list under the matching letter comment.

---

## Step 4 — Create the JSON sentence file

1. Determine the **file name**: the bare noun in lowercase with accents preserved, plus `.json`.
   - `arbre` → `arbre.json`
   - `araignée` → `araignée.json`
   - `lunettes` → `lunettes.json`
2. Determine the **folder**: `FrenchNouns\Sentences\{LETTER}\` where `{LETTER}` is the uppercase ASCII first letter (strip diacritics — `é` → `E`).
3. Create the file with this exact structure:

```json
{
  "description": "<description in French>",
  "sentences": [
    "<sentence 1>",
    "<sentence 2>",
    "<sentence 3>",
    "<sentence 4>",
    "<sentence 5>"
  ]
}
```

### Description rules

- Start with `Nom masculin désignant…` or `Nom féminin désignant…` for singular nouns.
- Start with `Nom masculin pluriel uniquement (les <noun>), désignant…` or `Nom féminin pluriel uniquement (les <noun>), désignant…` for plural-only nouns. End with `Ce mot s'emploie toujours au pluriel.`
- Include the word in French guillemets: `Le mot « <noun> » …`
- Write the entire description in French.

### Sentence rules

- Exactly **5** sentences.
- All sentences in French.
- Each sentence must use the noun naturally in context.
- Vary the sentence structures (declarative, exclamatory, questions are fine).
- Do not number the sentences.

---

## Step 5 — Build verification

After all files are created/modified, run a build to verify there are no compilation errors.

---

## Example — adding `parapluie` (masculine)

### Constants.cs (inside `#region "P" Nouns`, at the end)

```csharp
public const string Parapluie = MasculineArticle + Space + "parapluie";
```

### NounRepository.cs (inside `public static readonly IReadOnlyList<string> P`, at the end)

```csharp
Constants.Parapluie,
```

### `FrenchNouns\Sentences\P\parapluie.json`

```json
{
  "description": "Nom masculin désignant un accessoire portatif utilisé pour se protéger de la pluie. Le mot « parapluie » est très courant en français dans les contextes météorologiques et quotidiens.",
  "sentences": [
    "N'oublie pas ton parapluie, il va pleuvoir.",
    "J'ai perdu mon parapluie dans le métro.",
    "Le parapluie rouge se distinguait dans la foule.",
    "Elle a ouvert son parapluie dès les premières gouttes.",
    "Ce parapluie résiste même aux vents les plus forts."
  ]
}
```

---

## Example — adding `funérailles` (feminine, plural-only)

### Constants.cs (inside `#region Plural-only Nouns`, under `// F`)

```csharp
public const string Plural_Funerailles = PluralArticle + Space + "funérailles";
```

### NounRepository.cs

In the `F` letter list (after the `//` separator):

```csharp
Constants.Plural_Funerailles,
```

In the `PluralOnlyNouns` list (under `// F`):

```csharp
Constants.Plural_Funerailles,
```

### `FrenchNouns\Sentences\F\funérailles.json`

```json
{
  "description": "Nom féminin pluriel uniquement (les funérailles), désignant la cérémonie rendue à un défunt avant son inhumation ou sa crémation. Ce mot s'emploie toujours au pluriel.",
  "sentences": [
    "Les funérailles ont eu lieu dans la plus stricte intimité.",
    "Il a prononcé un discours émouvant lors des funérailles.",
    "Les funérailles nationales ont rassemblé des milliers de personnes.",
    "Elle n'a pas pu assister aux funérailles de son oncle.",
    "Les funérailles se sont déroulées sous une pluie battante."
  ]
}
```
