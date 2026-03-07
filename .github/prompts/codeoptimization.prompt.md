---
name: optimization
description: "Optimizes code for better performance"
model: Claude Haiku 4.5
---

Optimize the selected code from a performance perspective.

1. Identify bottlenecks (N+1 queries, unnecessary allocations)
2. Optimize algorithms and data structures
3. Minimize allocations and GC pressure
4. Use caching, lazy loading, parallelization where appropriate

Only optimize where it makes sense. Preserve code readability and maintainability.

💡 Start your response with a brief one-sentence assessment and add an optimization score on a scale from 0 to 10. Follow with Identified issues (⚠️) and finally suggest optimized code.

If the selected code is too large, make no changes and return a brief message "❌ Code is too large to optimize."

If the code is already sufficiently optimized, make no changes and return a brief message "❌ Code optimization is not necessary."