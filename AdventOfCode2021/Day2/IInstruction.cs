using LanguageExt;

namespace AdventOfCode2021.Day2;

[Union]
public interface IInstruction
{
    IInstruction Up(double change);

    IInstruction Down(double change);

    IInstruction Forward(double change);
}