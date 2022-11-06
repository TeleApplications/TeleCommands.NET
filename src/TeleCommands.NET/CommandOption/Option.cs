using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using TeleCommands.NET.CommandOption.Interfaces;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.CommandOption.Results;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET.CommandOption
{
    public class Option<T, TSource> : Factory<T>, IOption<TSource>
        where T : Option<T, TSource>, new()
    {
        private static readonly string ErrorMessage =
            $"Option {nameof(T)} failed";

        protected TSource ?optionResult { get; set; }

        public virtual ReadOnlyMemory<Argument> Arguments { get; }

        public virtual async Task<IResult<TSource>> ExecuteOptionAsync(OptionData data) 
        {
            if(optionResult is null)
                return new ErrorResult<TSource>(optionResult, ErrorMessage);

            await SetArgumentsAsync(data.Arguments);
            return new SuccesfulResult<TSource>(optionResult);
        }

        private async Task SetArgumentsAsync(ReadOnlyMemory<char> arguments)
        {
            var currentArguments = Arguments.ToArray();
            for (int i = 0; i < arguments.Length; i++) 
            {
                char currentSymbol = arguments.Span[i];
                if (TryGetArgument(out Argument resultArgument, currentSymbol, currentArguments))
                    await resultArgument.ArgumentAction.Invoke();
            }
        }

        private bool TryGetArgument([NotNullWhen(true)] out Argument argument, char argumentSymbol, ReadOnlyMemory<Argument> arguments) 
        {
            var vectorSymbol = new Vector<char>(argumentSymbol);
            var vectorSize = Vector<char>.Count;

            int difference = arguments.Length - vectorSize;
            for (int i = 0; i < difference; i+=vectorSize)
            {
                var currentArgument = arguments.Span[i];
                var currentSymbol = new Vector<char>(currentArgument.ArgumentCharacter);

                if (Vector.EqualsAll(vectorSymbol, currentSymbol)) 
                {
                    argument = currentArgument;
                    return true;
                }
            }

            int startIndex = arguments.Length / vectorSize;
            for (int j = startIndex; j < arguments.Length; j++)
            {
                var currentArgument = arguments.Span[j];
                if (argumentSymbol == currentArgument.ArgumentCharacter) 
                {
                    argument = currentArgument;
                    return true;
                }
            }

            argument = default;
            return false;
        }
    }
}
