---
name: naming
description: "Suggest better naming conventions"
model: Claude Haiku 4.5
---

Suggest improved naming for the selected code (function names, variables, classes, methods, modules)... assuming that:

- naming will be more descriptive and understandable
- naming will be consistent with conventions used in the project
- naming will follow best practices for the given programming language

Focus only on significant naming issues. Tolerate minor ambiguities and ignore them.

If the code cannot be evaluated due to missing broader context, respond briefly: "❌ Cannot suggest better naming without broader context."

If any changes don't make sense, simply respond briefly: "❌ No need to change naming."