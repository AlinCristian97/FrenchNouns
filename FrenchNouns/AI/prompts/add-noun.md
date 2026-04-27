# Add a French Noun

Use this prompt when a user asks to add one or more nouns.
Accept both bare noun (`arbre`) and article+noun (`un arbre`, `une maison`, `des lunettes`).

## 1) Determine canonical form

- Canonical value format is always: `article + space + noun`.
- Articles:
  - `un` = masculine singular
  - `une` = feminine singular
  - `des` = plural-only noun
- If user gives only a bare noun, infer the correct gender.
- Keep accents/diacritics in string values and JSON filenames/content.

## 2) Update constants (split files)

### Singular nouns (`un` / `une`)

- Edit: `FrenchNouns\AllConstants\Constants.{LETTER}.cs`
- `{LETTER}` = uppercase ASCII first letter of the noun (`é` -> `E`).
- Add at end of file/class:

`public const string Arbre = MasculineArticle + Space + "arbre";`

Rules:
- Constant name: PascalCase, no accents/hyphens.
- Value noun: lowercase, accents preserved.
- If same spelling needs disambiguation, add suffix (example: `_Side`, `_Rib`).

### Plural-only nouns (`des`)

- Edit: `FrenchNouns\AllConstants\Constants.main.cs`
- Add under `#region Plural-only Nouns`, in the matching letter section:

`public const string Plural_Lunettes = PluralArticle + Space + "lunettes";`

## 3) Update `NounRepository.cs`

- Add the constant to the matching letter list (`A`, `B`, ...).
- Keep ordering/style consistent with nearby entries.
- For plural-only nouns:
  - add to the letter list after the `//` separator block,
  - add to `PluralOnlyNouns` under matching letter.

## 4) Add sentence JSON

Create: `FrenchNouns\Sentences\{LETTER}\{noun}.json`

- `{LETTER}` = uppercase ASCII first letter.
- `{noun}` = lowercase bare noun with accents preserved.

Content shape:

- `description`: French only.
- `sentences`: exactly 5 French sentences using the noun naturally.

Description guidelines:
- Singular: start with `Nom masculin désignant...` or `Nom féminin désignant...`.
- Plural-only: use `Nom ... pluriel uniquement (...)` and end with `Ce mot s'emploie toujours au pluriel.`
- Include French guillemets around the noun: `« noun »`.

## 5) Validate

- Build after edits and ensure no compile errors.
- If noun already exists, do not duplicate; return/keep the canonical existing constant value.
