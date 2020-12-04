# AsyncThen

Extension methods for C#'s asynchronous Tasks.


## Usage:
Include `using namespace AsyncThen;` at the top of your C# file.

Supported methods:
- `Task<B> Task<A>.Then(Func<A, B>)`
- `Task Task<A>.Then(Action<A>)`
- `Task<B> Task.Then(Func<B>)`
- `Task Task.Then(Action)`
- `Task<B> Task<A>.Then(Func<A, Task<B>>)`
- `Task Task<A>.Then(Func<A, Task>)`
- `Task<B> Task.Then(Func<Task<B>>)`
- `Task Task.Then(Func<Task>)`