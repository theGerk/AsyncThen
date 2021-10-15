using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gerk.AsyncThen
{
	/// <summary>
	/// Extension methods for tasks
	/// </summary>
	public static class AsyncExtensions
	{
		/// <summary>
		/// Upon completion of <paramref name="task"/> executes <paramref name="func"/> with the output of <paramref name="task"/> passed in, then returns the value output by <paramref name="func"/>.
		/// </summary>
		/// <typeparam name="TInput">The type returned by <paramref name="task"/> and input into <paramref name="func"/>.</typeparam>
		/// <typeparam name="TOutput">The type returned by <paramref name="func"/>.</typeparam>
		/// <param name="task">A task.</param>
		/// <param name="func">Callback function to execute upon successful completion of the <paramref name="task"/>.</param>
		/// <returns>The output from <paramref name="func"/>.</returns>
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

		/// <summary>
		/// Upon completion of <paramref name="task"/> executes <paramref name="func"/>, then returns the value output by <paramref name="func"/>.
		/// </summary>
		/// <typeparam name="TOutput">The type returned by <paramref name="func"/>.</typeparam>
		/// <param name="task">A task.</param>
		/// <param name="func">Callback function to execute upon successful completion of the <paramref name="task"/>.</param>
		/// <returns>The output from <paramref name="func"/>.</returns>
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

		/// <summary>
		/// Upon completion of <paramref name="task"/> executes <paramref name="func"/> with the output of <paramref name="task"/> passed in.
		/// </summary>
		/// <typeparam name="TInput">The type returned by <paramref name="task"/> and input into <paramref name="func"/>.</typeparam>
		/// <param name="task">A task.</param>
		/// <param name="func">Callback function to execute upon successful completion of the <paramref name="task"/>.</param>
		/// <returns></returns>
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

		/// <summary>
		/// Upon completion of <paramref name="task"/> executes <paramref name="func"/>.
		/// </summary>
		/// <param name="task">A task.</param>
		/// <param name="func">Callback function to execute upon successful completion of the <paramref name="task"/>.</param>
		/// <returns></returns>
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

		/// <summary>
		/// Upon completion of <paramref name="task"/> executes <paramref name="func"/> with the output of <paramref name="task"/> passed in, then returns the value output by <paramref name="func"/>.
		/// </summary>
		/// <typeparam name="TInput">The type returned by <paramref name="task"/> and input into <paramref name="func"/>.</typeparam>
		/// <typeparam name="TOutput">The type returned by <paramref name="func"/>.</typeparam>
		/// <param name="task">A task.</param>
		/// <param name="func">Callback function to execute upon successful completion of the <paramref name="task"/>.</param>
		/// <returns>The output from <paramref name="func"/>.</returns>
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

		/// <summary>
		/// Upon completion of <paramref name="task"/> executes <paramref name="func"/>, then returns the value output by <paramref name="func"/>.
		/// </summary>
		/// <typeparam name="TOutput">The type returned by <paramref name="func"/>.</typeparam>
		/// <param name="task">A task.</param>
		/// <param name="func">Callback function to execute upon successful completion of the <paramref name="task"/>.</param>
		/// <returns>The output from <paramref name="func"/>.</returns>
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

		/// <summary>
		/// Upon completion of <paramref name="task"/> executes <paramref name="func"/> with the output of <paramref name="task"/> passed in.
		/// </summary>
		/// <typeparam name="TInput">The type returned by <paramref name="task"/> and input into <paramref name="func"/>.</typeparam>
		/// <param name="task">A task.</param>
		/// <param name="func">Callback function to execute upon successful completion of the <paramref name="task"/>.</param>
		/// <returns></returns>
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

		/// <summary>
		/// Upon completion of <paramref name="task"/> executes <paramref name="func"/>.
		/// </summary>
		/// <param name="task">A task.</param>
		/// <param name="func">Callback function to execute upon successful completion of the <paramref name="task"/>.</param>
		/// <returns></returns>
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