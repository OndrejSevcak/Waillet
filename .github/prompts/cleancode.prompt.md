---
name: cleancode
description: "Suggest cleaner code"
model: Claude Sonnet 4.5
---

Suggest changes to the selected code that:

- Improve readability and comprehensibility
- Reduce duplication and improve structure
- Follow best practices for the given programming language and framework

Do not suggest technical refactoring, error handling additions, optimizations, or anything else outside of code cleanliness.

Focus only on significant code deficiencies. In the output, list the basic code issues (⚠️) and then suggest the complete refactored code.

If no changes make sense, simply respond concisely: "❌ No code refactoring needed."