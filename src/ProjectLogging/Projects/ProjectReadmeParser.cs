
using System.Text;



namespace ProjectLogging.Projects;



public static class ProjectReadmeParser
{
    public static async Task<ProjectReadmeParseResult> ParseReadmeAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return ProjectReadmeParseResult.Failure;

        using var reader = new StreamReader(path);

        await GetContent(reader);

        var titleLine = await reader.ReadLineAsync();
        if (titleLine is null)
        {
            return ProjectReadmeParseResult.Failure;
        }

        int currentLevel = titleLine.TakeWhile(c => c == '#').Count();
        if (currentLevel != 1) return ProjectReadmeParseResult.Failure;

        var rootBuilder = new ReadmeNodeBuilder(1);
        rootBuilder.Title(titleLine[2..])
            .ParseContent(await GetContent(reader));

        var nodeStack = new Stack<ReadmeNodeBuilder>();
        nodeStack.Push(rootBuilder);

        while (!reader.EndOfStream)
        {
            titleLine = await reader.ReadLineAsync();
            if (titleLine is null)
            {
                return ProjectReadmeParseResult.Failure;
            }

            currentLevel = titleLine.TakeWhile(c => c == '#').Count();

            if (currentLevel <= 1) return ProjectReadmeParseResult.Failure;

            while (currentLevel <= nodeStack.Peek().Level)
            {
                var node = nodeStack.Pop().Build();
                nodeStack.Peek().AddChild(node);
            }

            nodeStack.Push(new ReadmeNodeBuilder(currentLevel)
                .Title(titleLine.TrimStart('#').Trim())
                .ParseContent(await GetContent(reader)));
        }

        while (nodeStack.Count > 1)
        {
            var node = nodeStack.Pop().Build();
            nodeStack.Peek().AddChild(node);
        }

        return ProjectReadmeParseResult.Success(nodeStack.Pop().Build());
    }



    private static async Task<string> GetContent(StreamReader stream)
    {
        var content = new StringBuilder();

        while (stream.Peek() != '#' && await stream.ReadLineAsync() is string line)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            line = line.Trim();

            content.AppendLine(line);
        }

        return content.ToString();
    }
}
