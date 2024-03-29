﻿using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.API.CommandOption.Interfaces;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.API.Interfaces;

namespace TeleCommands.NET.API.CommandOption
{
    public class Option<T, TSource> : Factory<T>, IOption<TSource>
        where T : Option<T, TSource>, new()
    {
        private static readonly string ErrorMessage =
            $"Option {nameof(T)} failed";
        protected TSource ?optionResult { get; set; }

        public virtual OptionBarrier CharacterBarrier { get; } = OptionBarrier.Empty;
        public virtual ReadOnlyMemory<Argument> Arguments { get; }

        public virtual async Task<IResult<TSource>> ExecuteOptionAsync(OptionData data) 
        {
            if(optionResult is null)
                return new ErrorResult<TSource>(optionResult, ErrorMessage);
            if(data.Arguments.Length > 0 && data.Arguments.Span[0] != Argument.UnknownArgument)
                await SetArgumentsAsync(data.Arguments);

            return new SuccesfulResult<TSource>(optionResult);
        }

        private async Task SetArgumentsAsync(ReadOnlyMemory<char> arguments)
        {
            var currentArguments = Arguments.ToArray();
            int separatorDifference = (byte)arguments.Span[0] | (byte)arguments.Span[1];

            if (separatorDifference == Argument.ArgumentSeparator) 
            {
                if (TryGetArgument(out Argument resultArgument, arguments.Span[2], currentArguments))
                    await resultArgument.ArgumentAction.Invoke();
                return;
            }

            for (int i = 1; i < arguments.Length; i++) 
            {
                char currentSymbol = arguments.Span[i];
                if (TryGetArgument(out Argument resultArgument, currentSymbol, currentArguments))
                    await resultArgument.ArgumentAction.Invoke();
            }
        }

        private bool TryGetArgument([NotNullWhen(true)] out Argument argument, char argumentName, ReadOnlyMemory<Argument> arguments) 
        {
            var vectorSymbol = new Vector<byte>((byte)argumentName);
            var vectorSize = Vector<byte>.Count;

            int difference = arguments.Length - vectorSize;
            for (int i = 0; i < difference; i+=vectorSize)
            {
                var currentArgument = arguments.Span[i];
                var currentSymbol = new Vector<byte>((byte)currentArgument.ArgumentName[0]);

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
                if (argumentName == currentArgument.ArgumentName[0])
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
