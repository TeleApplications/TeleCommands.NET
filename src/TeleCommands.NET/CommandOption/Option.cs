using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using TeleCommands.NET.CommandOption.Interfaces;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.CommandOption.Results;

namespace TeleCommands.NET.CommandOption
{
    internal class Option<T, TSource> : Factory<T> where T : Option<T, TSource>, new()
    {
        protected TSource optionResult { get; set; }

        public virtual ImmutableArray<Argument<TSource>> Arguments { get; }

        public virtual async Task<IResult<TSource>> ExecuteOptionAsync(OptionData data) 
        {
            await SetArgumentsAsync(data.Arguments);
            return new SuccesfulResult<TSource>(optionResult);
        }

        private async Task SetArgumentsAsync(ReadOnlyMemory<char> arguments)
        {
            Argument<TSource>[] currentArguments = Arguments.ToArray();
            for (int i = 0; i < currentArguments.Length; i++) 
            {
                char currentSymbol = currentArguments[i].ArgumentCharacter;
                if (TryGetArgument(out Argument<TSource> resultArgument, currentSymbol, currentArguments))
                    optionResult = await resultArgument.ArgumentFunction.Invoke(optionResult);
            }
        }

        private bool TryGetArgument([NotNullWhen(true)] out Argument<TSource> argument, char argumentSymbol, Argument<TSource>[] arguments) 
        {
            var vectorSymbol = new Vector<char>(argumentSymbol);
            var vectorSize = Vector<char>.Count;

            int difference = arguments.Length - vectorSize;
            for (int i = 0; i < difference; i+=vectorSize)
            {
                var currentArgument = arguments[i];
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
                var currentArgument = arguments[j];
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
