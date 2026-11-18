"""Checks Semgrep results against the test fixtures so CI fails on real regressions.

Usage:
    semgrep --config rules/ tests/vulnerable/ --json -o vulnerable-results.json
    semgrep --config rules/ tests/safe/ --json -o safe-results.json
    python tests/check_fixtures.py vulnerable-results.json safe-results.json

Two checks:
  1. Every rule defined in rules/ must fire at least once on tests/vulnerable/.
  2. Findings on tests/safe/ may only come from rules listed in KNOWN_BROAD.
     Any other rule firing on safe code is a false positive regression.
"""

import json
import re
import sys
from collections import Counter
from pathlib import Path

RULES_DIR = Path(__file__).resolve().parent.parent / "rules"

# Documented in the README under Known Limitations: Semgrep cannot see
# validation that happens after the call, so this rule flags every use of
# the API, including the safe fixture.
KNOWN_BROAD = {"dotnet-path-traversal-combine"}


def defined_rule_ids():
    ids = []
    for rule_file in sorted(RULES_DIR.glob("*.yml")):
        ids += re.findall(r"^\s*-\s*id:\s*(\S+)", rule_file.read_text(), re.M)
    return ids


def findings_by_rule(results_path):
    with open(results_path) as f:
        data = json.load(f)
    return Counter(r["check_id"].rsplit(".", 1)[-1] for r in data["results"])


def main(vulnerable_path, safe_path):
    rule_ids = defined_rule_ids()
    vulnerable = findings_by_rule(vulnerable_path)
    safe = findings_by_rule(safe_path)

    print(f"{'rule':42} {'vulnerable':>10} {'safe':>6}")
    for rule_id in rule_ids:
        note = "  (known broad match)" if rule_id in KNOWN_BROAD and safe[rule_id] else ""
        print(f"{rule_id:42} {vulnerable[rule_id]:>10} {safe[rule_id]:>6}{note}")

    failures = []
    for rule_id in rule_ids:
        if vulnerable[rule_id] == 0:
            failures.append(f"rule never fires on tests/vulnerable/: {rule_id}")
    for rule_id, count in safe.items():
        if rule_id not in KNOWN_BROAD:
            failures.append(f"false positive on tests/safe/: {rule_id} ({count})")

    if failures:
        print("\nFAILED")
        for failure in failures:
            print(f"  - {failure}")
        return 1
    print(f"\nOK: {len(rule_ids)} rules fire on vulnerable fixtures, no unexpected findings on safe fixtures")
    return 0


if __name__ == "__main__":
    if len(sys.argv) != 3:
        print(__doc__)
        sys.exit(2)
    sys.exit(main(sys.argv[1], sys.argv[2]))
