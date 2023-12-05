using LanguageExt;

namespace AdventOfCode2023.Day3;

[Union]
public interface ISchematicOption
{
    ISchematicOption Dot(int index);

    ISchematicOption PartNumber(int number, int[] indexes, int rowIndex);

    ISchematicOption Symbol(bool isGear, int index);
}