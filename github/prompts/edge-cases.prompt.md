---
name: edge-cases
description: "Identifies risks and edge cases"
model: Claude Haiku 4.5
agent: 'ask'
---

Analyze potential issues in the selected code:

1. What are possible edge cases (empty inputs, null, extremes)?
2. Where is validation or error handling missing?
3. What are the security risks?
4. Where could race conditions or memory leaks occur?

Focus only on significant risks and edge cases that could lead to errors or vulnerabilities. Always:

- 🐞 Describe the problem briefly and clearly
- 💡 Suggest a concrete solution or mitigation
- 🔗 Add documentation links if relevant