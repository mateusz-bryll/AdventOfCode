using Directory = DaySeven.Domain.Directory;

namespace DaySeven.Domain;

public class FileSystem
{
    private readonly int totalDiscSpace;

    public FileSystem(int totalDiscSpace)
    {
        this.totalDiscSpace = totalDiscSpace;
    }
    
    private RootDirectory RootDirectory { get; } = new();
    
    private Directory? CurrentDirectory { get; set; }

    public int FreeSpace => totalDiscSpace - RootDirectory.Size;

    public IEnumerable<Directory> Directories => RootDirectory.GetDirectories();

    public void ChangeDirectory(string name)
    {
        CurrentDirectory = name == "/" ? RootDirectory : CurrentDirectory?.GetDirectory(name);
    }

    public void AddFile(int size)
    {
        CurrentDirectory?.AddFile(size);
    }
    
    public void AddDirectory(string name)
    {
        CurrentDirectory?.AddSubdirectory(new Directory(name, CurrentDirectory));
    }

    public int SumDirectoriesWithTotalSizeOfAtMost(int size)
    {
        return RootDirectory.GetDirectories()
            .Where(dir => dir.Size <= size)
            .Sum(dir => dir.Size);
    }

    public int SizeOfTheSmallestDirectory(int size)
    {
        var directoryToDelete = RootDirectory.GetDirectories()
            .Where(dir => dir.Size >= size)
            .Min(dir => dir.Size);

        return directoryToDelete;
    }
}
