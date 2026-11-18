# dotnet-security-rules

Custom Semgrep rules for detecting common C#/.NET vulnerability patterns. Built to catch what generic rulesets miss in ASP.NET Core and .NET Framework applications.

## Rules

| File | Rule ID | What it catches | CWE | Severity |
|------|---------|----------------|-----|----------|
| sqli-string-concat.yml | dotnet-sqli-command | SqlCommand/SqliteCommand with variable input | CWE-89 | ERROR |
| xss-html-raw.yml | dotnet-xss-html-raw | Html.Raw() bypassing Razor encoding | CWE-79 | ERROR |
| xxe-xmlresolver.yml | dotnet-xxe-xmlresolver | XmlUrlResolver + DtdProcessing.Parse | CWE-611 | ERROR |
| deserialization.yml | dotnet-deser-typenamehandling | TypeNameHandling.All/Auto (RCE via JSON) | CWE-502 | ERROR |
| deserialization.yml | dotnet-deser-binaryformatter | BinaryFormatter.Deserialize (RCE) | CWE-502 | ERROR |
| path-traversal-combine.yml | dotnet-path-traversal-combine | Path.Combine with unsanitized input | CWE-22 | ERROR |
| command-injection-process-start.yml | dotnet-cmdi-process-start | Process.Start / StartInfo with variables | CWE-78 | ERROR |
| missing-auth-allowanonymous.yml | dotnet-auth-allowanonymous-sensitive | [AllowAnonymous] on sensitive endpoints | CWE-862 | WARNING |
| weak-crypto-hash.yml | dotnet-weak-crypto-md5 | MD5.Create() / MD5CryptoServiceProvider | CWE-328 | WARNING |
| weak-crypto-hash.yml | dotnet-weak-crypto-sha1 | SHA1.Create() / SHA1Managed | CWE-328 | WARNING |
| weak-crypto-hash.yml | dotnet-weak-crypto-des | DES / TripleDES | CWE-327 | WARNING |

## Usage

```bash
# Scan a project
semgrep --config rules/ /path/to/your/dotnet/project/

# Scan with SARIF output for CI integration
semgrep --config rules/ --sarif -o results.sarif /path/to/project/
```

## Test Results

Tested against [AspGoat](https://github.com/adamtamenne/AspGoat), an intentionally vulnerable ASP.NET Core application:

```
7 findings across 2 files (HomeController.cs, DeserializationDemo.cs)

  SQLi:              SqliteCommand with concatenated query variable
  XXE:               XmlUrlResolver enabling external entity resolution
  Path Traversal:    Path.Combine(uploads, file.FileName) with unsanitized filename
  Command Injection: Process.Start with user-controlled arguments (2 findings)
  Deserialization:   TypeNameHandling.All enabling arbitrary type instantiation
  Missing Auth:      [AllowAnonymous] on /internal/config exposing DB creds and JWT keys
```

## Known Limitations

**Razor templates (.cshtml):** Semgrep's `csharp` parser handles `.cs` files only. The `xss-html-raw` rule will not fire on `Html.Raw()` calls inside Razor views, which is where they most commonly appear in ASP.NET MVC applications. This is a Semgrep platform limitation, not a rule defect. The rule still catches `Html.Raw()` in code-behind or helper classes.

**Receiver name matching:** `dotnet-xss-html-raw` matches the receiver by name (`Html`, `html`, `htmlHelper`, `helper`, case-insensitive), not by type. An `IHtmlHelper` parameter with any other name, such as `h`, is not matched. Calls with a string literal argument, such as `Html.Raw("<hr/>")`, are not flagged.

**Broad matching:** `dotnet-path-traversal-combine` flags every use of `Path.Combine`, including ones that validate the resulting path afterwards, because Semgrep cannot see validation that happens after the call. This is intentional for code review triage: the rule surfaces locations for human review rather than claiming every match is exploitable.

**Command injection scope:** `dotnet-cmdi-process-start` skips `Process.Start` calls and `FileName` / `Arguments` assignments whose values are string literals, including variables that are only ever assigned literals. Any other value is flagged, whether or not it actually comes from user input.

## Testing

Each rule has a paired fixture in `tests/vulnerable/` (code it must flag) and `tests/safe/` (the closest safe equivalent). The fixtures carry `// ruleid:` and `// ok:` comments that document the expected result for each case.

To run the same checks as CI locally:

```bash
semgrep --config rules/ tests/vulnerable/ --json -o vulnerable-results.json
semgrep --config rules/ tests/safe/ --json -o safe-results.json
python tests/check_fixtures.py vulnerable-results.json safe-results.json
```

`tests/check_fixtures.py` fails if any rule in `rules/` produces no finding on the vulnerable fixtures, or if any rule outside the `KNOWN_BROAD` set produces a finding on the safe fixtures. `KNOWN_BROAD` currently contains only `dotnet-path-traversal-combine` (see Known Limitations). The check is per rule, not per line: it does not verify that findings land on the lines annotated `ruleid:`.

The workflow in `.github/workflows/validate-rules.yml` runs these steps on every push and pull request to `main`, with Semgrep pinned to a fixed version so results do not change between runs.
