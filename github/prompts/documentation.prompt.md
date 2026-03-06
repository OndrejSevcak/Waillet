---
name: documentation
description: "Creates documentation for code"
model: Claude Sonnet 4.5
---

Based on the selected code, create documentation using markdown that includes the following sections:

- 💡 very brief description of the code's function in one sentence
- 📋 algorithm description as a concise list of steps
- 📦 usage examples, if relevant
- 🧩 if relevant, add diagrams using mermaid

If any section is not relevant, omit it.

The documentation title will always be the name of the functionality, module, class, method, or what is being documented.

Document only public API. Keep the documentation style used in the project. If some part