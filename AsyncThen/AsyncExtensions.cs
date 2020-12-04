using System;
using System.Threading.Tasks;

namespace AsyncThen
{
	/// <summary>
	/// Extension methods for tasks
	/// </summary>
	public static class AsyncExtensions
	{
		//TODO add XML comments
		public static Task<TOutput> Then<TInput, TOutput>(this Task<TInput> task, Func<TInput, TOutput> func)
		{
			var tcs = new TaskCompletionSource<TOutput>();
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
					tcs.TrySetException(t.Exception.InnerExceptions);
				else if (t.IsCanceled)
					tcs.TrySetCanceled();
				else
					tcs.TrySetResult(func(t.Result));
			});
			return tcs.Task;
		}
		public static Task<TOutput> Then<TOutput>(this Task task, Func<TOutput> func)
		{
			var tcs = new TaskCompletionSource<TOutput>();
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
					tcs.TrySetException(t.Exception.InnerExceptions);
				else if (t.IsCanceled)
					tcs.TrySetCanceled();
				else
					tcs.TrySetResult(func());
			});
			return tcs.Task;
		}
		public static Task Then<TInput>(this Task<TInput> task, Action<TInput> func)
		{
			var tcs = new TaskCompletionSource<bool>();
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
					tcs.TrySetException(t.Exception.InnerExceptions);
				else if (t.IsCanceled)
					tcs.TrySetCanceled();
				else
				{
					func(t.Result);
					tcs.TrySetResult(true);
				}
			});
			return tcs.Task;
		}
		public static Task Then(this Task task, Action func)
		{
			var tcs = new TaskCompletionSource<bool>();
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
					tcs.TrySetException(t.Exception.InnerExceptions);
				else if (t.IsCanceled)
					tcs.TrySetCanceled();
				else
				{
					func();
					tcs.TrySetResult(true);
				}
			});
			return tcs.Task;
		}
		public static Task<TOutput> Then<TInput, TOutput>(this Task<TInput> task, Func<TInput, Task<TOutput>> func)
		{
			var tcs = new TaskCompletionSource<TOutput>();
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
					tcs.TrySetException(t.Exception.InnerExceptions);
				else if (t.IsCanceled)
					tcs.TrySetCanceled();
				else
				{
					var resultingTask = func(t.Result);
					resultingTask.ContinueWith(u =>
					{
						if (u.IsFaulted)
							tcs.TrySetException(u.Exception.InnerExceptions);
						else if (u.IsCanceled)
							tcs.TrySetCanceled();
						else
							tcs.TrySetResult(resultingTask.Result);
					});
				}
			});
			return tcs.Task;
		}
		public static Task<TOutput> Then<TOutput>(this Task task, Func<Task<TOutput>> func)
		{
			var tcs = new TaskCompletionSource<TOutput>();
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
					tcs.TrySetException(t.Exception.InnerExceptions);
				else if (t.IsCanceled)
					tcs.TrySetCanceled();
				else
				{
					var resultingTask = func();
					resultingTask.ContinueWith(u =>
					{
						if (u.IsFaulted)
							tcs.TrySetException(u.Exception.InnerExceptions);
						else if (u.IsCanceled)
							tcs.TrySetCanceled();
						else
							tcs.TrySetResult(resultingTask.Result);
					});
				}
			});
			return tcs.Task;
		}
		public static Task Then<TInput>(this Task<TInput> task, Func<TInput, Task> func)
		{
			var tcs = new TaskCompletionSource<bool>();
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
					tcs.TrySetException(t.Exception.InnerExceptions);
				else if (t.IsCanceled)
					tcs.TrySetCanceled();
				else
				{
					var resultingTask = func(t.Result);
					resultingTask.ContinueWith(u =>
					{
						if (u.IsFaulted)
							tcs.TrySetException(u.Exception.InnerExceptions);
						else if (u.IsCanceled)
							tcs.TrySetCanceled();
						else
							tcs.TrySetResult(true);
					});
				}
			});
			return tcs.Task;
		}
		public static Task Then(this Task task, Func<Task> func)
		{
			var tcs = new TaskCompletionSource<bool>();
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
					tcs.TrySetException(t.Exception.InnerExceptions);
				else if (t.IsCanceled)
					tcs.TrySetCanceled();
				else
				{
					var resultingTask = func();
					resultingTask.ContinueWith(u =>
					{
						if (u.IsFaulted)
							tcs.TrySetException(u.Exception.InnerExceptions);
						else if (u.IsCanceled)
							tcs.TrySetCanceled();
						else
							tcs.TrySetResult(true);
					});
				}
			});
			return tcs.Task;
		}
	}
}