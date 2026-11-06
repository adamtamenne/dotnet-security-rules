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
8 findings across 2 files (HomeController.cs, DeserializationDemo.cs)

  SQLi:              SqliteCommand with concatenated query variable
  XXE:               XmlUrlResolver enabling external entity resolution
  Path Traversal:    Path.Combine(uploads, file.FileName) with unsanitized filename
  Command Injection: Process.Start with user-controlled shell/args (2 findings)
  Deserialization:   TypeNameHandling.All enabling arbitrary type instantiation
  Missing Auth:      [AllowAnonymous] on /internal/config exposing DB creds and JWT keys
```

## Known Limitations

**Razor templates (.cshtml):** Semgrep's `csharp` parser handles `.cs` files only. The `xss-html-raw` rule will not fire on `Html.Raw()` calls inside Razor views, which is where they most commonly appear in ASP.NET MVC applications. This is a Semgrep platform limitation, not a rule defect. The rule still catches `Html.Raw()` in code-behind or helper classes.

**Broad matching:** Rules like `dotnet-path-traversal-combine` and `dotnet-cmdi-process-start` flag all usages of their target APIs, not just those with confirmed user input flow. This is intentional for code review triage: the rules surface locations for human review rather than claiming every match is exploitable.
